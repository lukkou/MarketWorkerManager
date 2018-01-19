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

						list.UpdatedDate = npoi.Row.GetCell(0).StringCellValue;
						list.Code = npoi.Row.GetCell(1).StringCellValue;
						list.Name = npoi.Row.GetCell(2).StringCellValue;
						list.MarketName = npoi.Row.GetCell(3).StringCellValue;
						list.Category33Code = TextConvertUtility.IsReplaceHyphen(npoi.Row.GetCell(4).StringCellValue) ? string.Empty : npoi.Row.GetCell(4).StringCellValue;
						list.Category33Name = TextConvertUtility.IsReplaceHyphen(npoi.Row.GetCell(5).StringCellValue) ? string.Empty : npoi.Row.GetCell(5).StringCellValue;
						list.Category17Code = TextConvertUtility.IsReplaceHyphen(npoi.Row.GetCell(6).StringCellValue) ? string.Empty : npoi.Row.GetCell(6).StringCellValue;
						list.Category17Name = TextConvertUtility.IsReplaceHyphen(npoi.Row.GetCell(7).StringCellValue) ? string.Empty : npoi.Row.GetCell(7).StringCellValue;
						list.ClassCode = TextConvertUtility.IsReplaceHyphen(npoi.Row.GetCell(8).StringCellValue) ? string.Empty : npoi.Row.GetCell(8).StringCellValue;
						list.ClassName = TextConvertUtility.IsReplaceHyphen(npoi.Row.GetCell(9).StringCellValue) ? string.Empty : npoi.Row.GetCell(9).StringCellValue;

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
		/// 新規上場分と上場廃止分のマスタモデルを作成
		/// </summary>
		/// <param name="excelList">Excelのデータ</param>
		/// <returns></returns>
		public Tuple<List<StockMaster>, List<StockMaster>> DeffStockMaster(List<TokyoStockExchangeExcelModel> excelList)
		{
			List<StockMaster> newResult = new List<StockMaster>();
			List<StockMaster> delistResult = new List<StockMaster>();

			List<StockMaster> masterList = Db.StockMasters.ToList();
			List<StockMaster> convertExcelList = ExcelModelToStockMasterModel(excelList);

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
		/// 銘柄マスターの登録
		/// </summary>
		/// <param name="lists"></param>
		/// <returns></returns>
		public async Task AddStockMaster(List<TokyoStockExchangeExcelModel> excelList)
		{
			var stockMasters = ExcelModelToStockMasterModel(excelList);

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

			Db.IndustryCode17Master.AddRange(masterList);
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

			Db.IndustryCode33Master.AddRange(masterList);
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

			Db.ClassMaster.AddRange(masterList);
			await Db.SaveChangesAsync();
		}

		/// <summary>
		/// 新規上場分のマスターデータの登録
		/// </summary>
		/// <param name="masterList"></param>
		/// <returns></returns>
		public async Task AddNewStockMaster(List<StockMaster> newMasterList)
		{
			Db.StockMasters.AddRange(newMasterList);
			await Db.SaveChangesAsync();
		}

		/// <summary>
		/// 上場廃止分のマスターデータをOldに移動
		/// </summary>
		/// <param name="deleteMasterList"></param>
		/// <returns>Oldに移動した際に採番したGuid</returns>
		public Guid DeleteStockMaster(List<StockMaster> deleteMasterList)
		{
			List<string> deleteCodeList = deleteMasterList.Select(x => x.StockCode).ToList();

			//上場廃止分のデータを削除
			Db.StockMasters.RemoveRange(deleteMasterList);
			Db.SaveChanges();

			//Oldテーブル用のGuid
			Guid guid = Guid.NewGuid();
			List<OldStockMaster> moveList = deleteMasterList.Select(x => new OldStockMaster
															{
																GuidKey = guid,
																StockCode = x.StockCode,
																DeleteDate = DateTime.Now.ToString("yyyyMM"),
																StockName = x.StockName,
																MarketCode = x.MarketCode,
																IndustryCode33 = x.IndustryCode33,
																IndustryCode17 = x.IndustryCode17,
																ClassCode = x.ClassCode
															}).ToList();

			Db.OldStockMaster.AddRange(moveList);
			Db.SaveChanges();

			return guid;
		}

		/// <summary>
		/// ExcelモデルをDBのマスタモデルに変換
		/// </summary>
		/// <param name="excelList"></param>
		/// <returns></returns>
		private List<StockMaster> ExcelModelToStockMasterModel(List<TokyoStockExchangeExcelModel> excelList)
		{
			return excelList.Select(x => new StockMaster
			{
				StockCode = x.Code,
				StockName = x.Name,
				MarketCode = x.MarketName,
				IndustryCode33 = x.Category33Code,
				IndustryCode17 = x.Category17Code,
				ClassCode = x.ClassCode
			}).ToList();
		}
	}
}
