using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using StockDatabaseManager.Common;
using StockDatabaseManager.Context;
using StockDatabaseManager.Utility;
using StockDatabaseManager.Models;
using StockDatabaseManager.DataModels;

namespace StockDatabaseManager.Logic
{
	class StockMasterLogic
	{
		public DatabaseContext Db { get; set; }

		public HttpClient Client { get; set; }

		/// <summary>
		/// 東証の銘柄マスタのExcelをダウンロードする
		/// </summary>
		/// <param name="filePash">保存先パス</param>
		/// <returns></returns>
		public async Task DownloadTokyoExchangeExcelAsync(string filePash)
		{
			using (HttpResponseMessage response = await Client.GetAsync(Define.Stock.TokyoExchangeUrl))
			{
				if (response.StatusCode == HttpStatusCode.OK)
				{
					using (var fileStream = File.Create(filePash))
					{
						var httpStream = await response.Content.ReadAsStreamAsync();
						httpStream.CopyTo(fileStream);
						fileStream.Flush();
					}
				}
				else
				{
					//THHPステータス 200以外は404とする
					throw new HttpResponseException(HttpStatusCode.NotFound);
				}
			}
		}

		/// <summary>
		/// 東証から取得したExcelをデータモデルに変換
		/// </summary>
		/// <param name="filePash"></param>
		/// <returns></returns>
		public List<TokyoStockExchangeExcelModel> ExcelToDataModel(string filePash)
		{
			List<TokyoStockExchangeExcelModel> result = new List<TokyoStockExchangeExcelModel>();

			if (File.Exists(filePash))
			{
				using (NPOIUtility npoi = new NPOIUtility(string.Empty, filePash))
				{
					npoi.SetWorkSheet(Define.Stock.ExcelSheetName);
					int lastRow = npoi.Sheet.LastRowNum;

					for (int i = 0; i <= lastRow - 1; i++)
					{
						if (i == 0)
						{
							//１行目はヘッダーなので無視
							continue;
						}
						npoi.SetWorkRow(i);
						TokyoStockExchangeExcelModel list = new TokyoStockExchangeExcelModel();

						list.UpdatedDate = npoi.GetCellValue(npoi.Row.GetCell(0));
						list.Code = npoi.GetCellValue(npoi.Row.GetCell(1));
						list.Name = npoi.GetCellValue(npoi.Row.GetCell(2));
						list.MarketName = npoi.GetCellValue(npoi.Row.GetCell(3));
						list.Category33Code = TextConvertUtility.IsHyphen(npoi.GetCellValue(npoi.Row.GetCell(4))) ? string.Empty : npoi.GetCellValue(npoi.Row.GetCell(4));
						list.Category33Name = TextConvertUtility.IsHyphen(npoi.GetCellValue(npoi.Row.GetCell(5))) ? string.Empty : npoi.GetCellValue(npoi.Row.GetCell(5));
						list.Category17Code = TextConvertUtility.IsHyphen(npoi.GetCellValue(npoi.Row.GetCell(6))) ? string.Empty : npoi.GetCellValue(npoi.Row.GetCell(6));
						list.Category17Name = TextConvertUtility.IsHyphen(npoi.GetCellValue(npoi.Row.GetCell(7))) ? string.Empty : npoi.GetCellValue(npoi.Row.GetCell(7));
						list.ClassCode = TextConvertUtility.IsHyphen(npoi.GetCellValue(npoi.Row.GetCell(8))) ? string.Empty : npoi.GetCellValue(npoi.Row.GetCell(8));
						list.ClassName = TextConvertUtility.IsHyphen(npoi.GetCellValue(npoi.Row.GetCell(9))) ? string.Empty : npoi.GetCellValue(npoi.Row.GetCell(9));

						result.Add(list);
					}
				}
			}
			else
			{
				throw new FileNotFoundException();
			}

			return result;
		}

		/// <summary>
		/// エクセルモデルから市場マスタを作成
		/// </summary>
		/// <returns></returns>
		public List<MarketMaster> ExcelModelToMarketModel(List<TokyoStockExchangeExcelModel> excelList)
		{
			List<MarketMaster> result = new List<MarketMaster>();

			var marketGroup = excelList.GroupBy(x => x.MarketName).ToList();

			for (int i = 0; i <= marketGroup.Count - 1; i++)
			{
				MarketMaster list = new MarketMaster();
				list.MarketId = i.ToString();
				list.MarketName = marketGroup[i].Key;

				result.Add(list);
			}

			return result;
		}

		/// <summary>
		/// 新規上場分と上場廃止分のマスタモデルを作成
		/// </summary>
		/// <param name="excelList">Excelのデータ</param>
		/// <returns></returns>
		public Tuple<List<StockMaster>, List<StockMaster>> DeffStockMaster(List<TokyoStockExchangeExcelModel> excelList)
		{
			List<StockMaster> newResult = new List<StockMaster>();
			List<StockMaster> delistResult = new List<StockMaster>();

			List<StockMaster> masterList = Db.StockMasters.ToList();
			List<MarketMaster> marketMasterList = GetMarketMaster();
			List<StockMaster> convertExcelList = ExcelModelToStockMasterModel(excelList, marketMasterList);

			//新規上場分の差分を取得
			HashSet<StockMaster> newHashSet = new HashSet<StockMaster>(convertExcelList);
			newHashSet.ExceptWith(masterList);
			newResult = newHashSet.ToList();

			//上場廃止分の差分を取得
			HashSet<StockMaster> deleteHashSet = new HashSet<StockMaster>(masterList);
			deleteHashSet.ExceptWith(convertExcelList);
			delistResult = deleteHashSet.ToList();

			return new Tuple<List<StockMaster>, List<StockMaster>>(newResult, delistResult);
		}

		/// <summary>
		/// 市場マスターのデータを取得
		/// </summary>
		/// <returns></returns>
		public List<MarketMaster> GetMarketMaster()
		{
			return Db.MarketMasters.ToList();
		}

		/// <summary>
		/// 市場マスターの登録
		/// </summary>
		/// <param name="marketList"></param>
		/// <returns></returns>
		public async Task AddMarketMaster(List<MarketMaster> marketList)
		{
			Db.MarketMasters.AddRange(marketList);
			await Db.SaveChangesAsync();
		}

		/// <summary>
		/// 銘柄マスターの登録
		/// </summary>
		/// <param name="lists"></param>
		/// <returns></returns>
		public async Task AddStockMaster(List<TokyoStockExchangeExcelModel> excelList, List<MarketMaster> marketList)
		{
			var stockMasters = ExcelModelToStockMasterModel(excelList, marketList);

			Db.StockMasters.AddRange(stockMasters);
			await Db.SaveChangesAsync();
		}

		/// <summary>
		/// 17業種コードマスタの登録
		/// </summary>
		/// <param name="lists"></param>
		public async Task AddIndustryCode17(List<TokyoStockExchangeExcelModel> excelList)
		{
			var masterList = excelList.GroupBy(x => x.Category17Code).Select(x => new IndustryCode17Master { Code = x.Key, Name = x.First().Category17Name });
			masterList = masterList.Where(x => !string.IsNullOrEmpty(x.Code)).Where(x => !string.IsNullOrEmpty(x.Name)).ToList();

			Db.IndustryCode17Masters.AddRange(masterList);
			await Db.SaveChangesAsync();
		}

		/// <summary>
		/// 33業種コードマスタの登録
		/// </summary>
		/// <param name="lists"></param>
		/// <returns></returns>
		public async Task AddIndustryCode33(List<TokyoStockExchangeExcelModel> excelList)
		{
			var masterList = excelList.GroupBy(x => x.Category33Code).Select(x => new IndustryCode33Master { Code = x.Key, Name = x.First().Category33Name });
			masterList = masterList.Where(x => !string.IsNullOrEmpty(x.Code)).Where(x => !string.IsNullOrEmpty(x.Name)).ToList();

			Db.IndustryCode33Masters.AddRange(masterList);
			await Db.SaveChangesAsync();
		}

		/// <summary>
		/// 上場規模コードの登録
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public async Task AddClass(List<TokyoStockExchangeExcelModel> excelList)
		{
			var masterList = excelList.GroupBy(x => x.ClassCode).Select(x => new ClassMaster { Code = x.Key, Name = x.First().ClassName });
			masterList = masterList.Where(x => !string.IsNullOrEmpty(x.Code)).Where(x => !string.IsNullOrEmpty(x.Name)).ToList();

			Db.ClassMasters.AddRange(masterList);
			await Db.SaveChangesAsync();
		}

		/// <summary>
		/// 新規上場分のマスターデータの登録
		/// </summary>
		/// <param name="masterList"></param>
		/// <returns></returns>
		public void AddNewStockMaster(List<StockMaster> newMasterList)
		{
			Db.StockMasters.AddRange(newMasterList);
			 Db.SaveChanges();
		}

		/// <summary>
		/// 上場廃止分のマスターデータをOldに移動
		/// </summary>
		/// <param name="deleteMasterList"></param>
		/// <returns>Oldに移動したデータのモデル</returns>
		public List<DeleteStockModel> DeleteStockMaster(List<StockMaster> deleteMasterList)
		{
			List<string> deleteCodeList = deleteMasterList.Select(x => x.StockCode).ToList();

			//上場廃止分のデータを削除
			Db.StockMasters.RemoveRange(deleteMasterList);
			Db.SaveChanges();

			//Oldテーブル用のGuid
			List<OldStockMaster> moveList = deleteMasterList.Select(x => new OldStockMaster
			{
				GuidKey = Guid.NewGuid(),
				StockCode = x.StockCode,
				DeleteDate = DateTime.Now.ToString("yyyyMM"),
				StockName = x.StockName,
				MarketCode = x.MarketCode,
				IndustryCode33 = x.IndustryCode33,
				IndustryCode17 = x.IndustryCode17,
				ClassCode = x.ClassCode
			}).ToList();

			Db.OldStockMasters.AddRange(moveList);
			Db.SaveChanges();

			List<DeleteStockModel> result = moveList.Select(x => new DeleteStockModel { GuidKey = x.GuidKey,StockCode = x.StockCode}).ToList();
			return result;
		}

		/// <summary>
		/// ExcelモデルをDBのマスタモデルに変換
		/// </summary>
		/// <param name="excelList"></param>
		/// <returns></returns>
		private List<StockMaster> ExcelModelToStockMasterModel(List<TokyoStockExchangeExcelModel> excelList, List<MarketMaster> marketList)
		{
			return excelList.Select(x => new StockMaster
			{
				StockCode = x.Code,
				StockName = x.Name,
				MarketCode = marketList.Where(m => m.MarketName == x.MarketName).FirstOrDefault().MarketId ,
				IndustryCode33 = x.Category33Code,
				IndustryCode17 = x.Category17Code,
				ClassCode = x.ClassCode
			}).ToList();
		}
	}
}
