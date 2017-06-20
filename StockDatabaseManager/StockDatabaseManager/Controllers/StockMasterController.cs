using System;
using Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StockDatabaseManager.Common;

namespace StockDatabaseManager.Controllers
{
	class StockMasterController: BaseController
	{
		private const string DOWNLOADEXCEL_FILENAME = "\\data.xls";

		public void StockMasterLoad()
		{

		}


		public void ExcelDownload()
		{
			//保存先を作成
			string saveDirectory = Properties.Settings.Default.SaveDirectory;
			string monthDirectory ="\\" +  DateTime.Now.ToString("yyyyMM");
			DirectoryUtility.CreateDirectory(saveDirectory + monthDirectory);

			Uri StockListUrl = new Uri("http://www.jpx.co.jp/markets/statistics-equities/misc/tvdivq0000001vg2-att/data_j.xls");

			using (System.Net.WebClient webClient = new System.Net.WebClient())
			{
				webClient.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);

				//非同期ダウンロード
				Console.WriteLine(MessageUtility.CreateConsoleMessage("東証上場銘柄一覧ダウンロード開始"));
				webClient.DownloadFileAsync(StockListUrl, monthDirectory + DOWNLOADEXCEL_FILENAME);
			}
		}

		private void Client_DownloadProgressChanged(object sender,System.Net.DownloadProgressChangedEventArgs e)
		{
			Console.WriteLine(MessageUtility.CreateConsoleMessage("東証上場銘柄一覧ダウンロード完了"));
		}


		private void AddStockMaster()
		{

		}

	}
}
