using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.SS.UserModel;

using StockDatabaseManager.Common;
using StockDatabaseManager.Utility;
using StockDatabaseManager.DataModels;

namespace StockDatabaseManager.Controllers
{
	class StockMasterController: BaseController
	{
		private const string DOWNLOADEXCEL_FILENAME = "\\data.xls";
		private const string SHEETNAME_DATA_J = "Sheet1";

		public void StockMasterLoad()
		{

		}


		public bool ExcelDownload()
		{
			bool r = true;

			//保存先を作成
			string saveDirectory = Properties.Settings.Default.SaveDirectory;
			string monthDirectory ="\\" +  DateTime.Now.ToString("yyyyMM");
			DirectoryUtility.CreateDirectory(saveDirectory + monthDirectory);

			Uri StockListUrl = new Uri("http://www.jpx.co.jp/markets/statistics-equities/misc/tvdivq0000001vg2-att/data_j.xls");


			try
			{
				using (System.Net.WebClient webClient = new System.Net.WebClient())
				{
					//非同期イベントハンドラを設定
					webClient.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
					webClient.DownloadDataCompleted += new System.Net.DownloadDataCompletedEventHandler(Client_DownloadProgressCompleted);

					//非同期ダウンロード
					webClient.DownloadFileAsync(StockListUrl, monthDirectory + DOWNLOADEXCEL_FILENAME);
				}
			}
			catch(Exception)
			{
				r = false;
			}

			return r;
		}


		private bool AddStockMaster()
		{
			bool r = true;
			var excelModel = new List<TokyoStockExchangeExcelModel> ();

			string excelPass = Properties.Settings.Default.SaveDirectory + "\\" + DateTime.Now.ToString("yyyyMM") + DOWNLOADEXCEL_FILENAME;
			if (System.IO.File.Exists(excelPass))
			{
				using (NPOIUtility npoi = new NPOIUtility(string.Empty, excelPass))
				{
					npoi.SheetDesignation(SHEETNAME_DATA_J);
					ISheet sheet = npoi.Sheet;
					int lastRow = sheet.LastRowNum;

					//Excelをモデルに置き換える
					for (int i = 1; i <= lastRow; i++)
					{
						TokyoStockExchangeExcelModel list = new TokyoStockExchangeExcelModel();
						IRow row = sheet.GetRow(i);
						list.UpdatedDate = row.GetCell(0).StringCellValue;
						list.Code = Convert.ToInt32(row.GetCell(1).StringCellValue);
						list.Name = row.GetCell(2).StringCellValue;
						list.MarketName = row.GetCell(3).StringCellValue;
						list.Category33Code = TextConvertUtility.ReplaceHyphenToZero(row.GetCell(4).StringCellValue);
						list.Category33Name = TextConvertUtility.ReplaceHyphenToEmpty(row.GetCell(5).StringCellValue);
						list.Category17Code = TextConvertUtility.ReplaceHyphenToZero(row.GetCell(6).StringCellValue);
						list.Category17Name = TextConvertUtility.ReplaceHyphenToEmpty(row.GetCell(7).StringCellValue);
						list.ClassCode = TextConvertUtility.ReplaceHyphenToZero(row.GetCell(8).StringCellValue);
						list.ClassName = TextConvertUtility.ReplaceHyphenToEmpty(row.GetCell(9).StringCellValue);

						excelModel.Add(list);
					}
				} 


			}
			else
			{
				r = false;
			}

			return r;
		}

		#region イベントハンドラ
		/// <summary>
		/// ファイルダウンロードの完了時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
		{
			Console.WriteLine("・");
			System.Threading.Thread.Sleep(1500);
		}

		/// <summary>
		/// ファイルダウンロードの完了時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Client_DownloadProgressCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
		{

		}
		#endregion

	}
}
