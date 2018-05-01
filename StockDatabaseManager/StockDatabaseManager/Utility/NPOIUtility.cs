using System;
using System.IO;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;


namespace StockDatabaseManager.Utility
{
	public class NPOIUtility : IDisposable
	{
		public IWorkbook WookBook { get; private set; }
		public ISheet Sheet { get; private set; }
		public IRow Row { get; private set; }

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
					WookBook.Close();
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
			if (!string.IsNullOrEmpty(path))
			{
				WookBook = WorkbookFactory.Create(path);
			}
			else
			{
				if (extention == EXTENSIONXLS)
				{
					WookBook = new HSSFWorkbook();
				}
				else
				{
					WookBook = new XSSFWorkbook();
				}
				Sheet = WookBook.CreateSheet();
			}
		}

		/// <summary>
		/// 作業Excelの拡張子を取得
		/// </summary>
		/// <returns></returns>
		public string GetExtension()
		{
			string r = string.Empty;
			
			if (WookBook.GetType() == GetType())
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
		public void SetWorkSheet(string sheetName)
		{
			if (Sheet != null)
			{
				Sheet = null;
			}

			if (WookBook == null)
			{
				throw new NullReferenceException("作業ファイルを指定してください。");
			}

			Sheet = WookBook.GetSheet(sheetName);
		}

		/// <summary>
		/// 作業行を指定
		/// </summary>
		/// <param name="rowNum">作業行</param>
		public void SetWorkRow(int rowNum)
		{
			if(Row != null)
			{
				Row = null;
			}

			if(Sheet == null)
			{
				throw new NullReferenceException("作業シートを指定してください。");
			}

			Row = Sheet.GetRow(rowNum);
		}

		/// <summary>
		/// セルの型に対応した値を取得
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		public string GetCellValue(ICell cell)
		{
			string result = string.Empty;

			switch (cell.CellType)
			{
				case CellType.String:
					result = cell.StringCellValue;
					break;

				case CellType.Numeric:
					if (DateUtil.IsCellDateFormatted(cell))
					{
						result = cell.DateCellValue.ToString();
					}
					else
					{
						result = cell.NumericCellValue.ToString();
					}
					break;

				case CellType.Boolean:
					result = cell.BooleanCellValue.ToString();
					break;

				case CellType.Formula:
					result = cell.CellFormula;
					break;

				default:
					result = cell.StringCellValue;
					break;

			}

			return result;
		}
	}
}
