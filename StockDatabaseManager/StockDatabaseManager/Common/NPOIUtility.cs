using System;
using System.IO;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

using StockDatabaseManager.DataModels;


namespace StockDatabaseManager.Common
{
	public class NPOIUtility : IDisposable
	{
		private IWorkbook _workbook;
		private ISheet _sheet;

		public IWorkbook WookBook { get; private set; }
		public ISheet Sheet { get; private set; }

		private const string EXTENSIONXLS = ".xls";
		private const string EXTENSIONXLSX = ".xlsx";

		#region IDisposable メンバー
		private bool disposedValue = false; // 重複する呼び出しを検出するには

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: マネージ状態を破棄します (マネージ オブジェクト)。
				}

				// TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
				// TODO: 大きなフィールドを null に設定します。

				disposedValue = true;
			}
		}

		// TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
		// ~NPOIUtility() {
		//   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
		//   Dispose(false);
		// }

		// このコードは、破棄可能なパターンを正しく実装できるように追加されました。
		public void Dispose()
		{
			// このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
			Dispose(true);
			// TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
			// GC.SuppressFinalize(this);
		}
		#endregion

		/// <summary>
		/// コンストラクター
		/// </summary>
		/// <param name="extention">拡張子</param>
		/// <param name="path">ファイルパス(取り込み時のみ使用)</param>
		public NPOIUtility(string extention, string path)
		{
			//パスがある場合取り込みと判断
			if (string.IsNullOrEmpty(path))
			{
				_workbook = WorkbookFactory.Create(path);
			}
			else
			{
				if (extention == EXTENSIONXLS)
				{
					_workbook = new HSSFWorkbook();
				}
				else
				{
					_workbook = new XSSFWorkbook();
				}
				_sheet = _workbook.CreateSheet();
			}
		}

		public string GetExtension()
		{
			string r = string.Empty;
			
			if (_workbook.GetType() == GetType())
			{
				r = EXTENSIONXLS;
			}
			else
			{
				r = EXTENSIONXLSX;
			}

			return r;
		}

		/// <summary>
		/// 作業シートを指定
		/// </summary>
		public void SheetDesignation(string sheetName)
		{
			_sheet = _workbook.GetSheet(sheetName);
		}
	}
}
