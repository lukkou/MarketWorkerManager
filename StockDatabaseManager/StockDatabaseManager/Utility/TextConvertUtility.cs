using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDatabaseManager.Utility
{
	public class TextConvertUtility : IFormatProvider
	{
		public object GetFormat(Type service)
		{
			if (service == typeof(ICustomFormatter))
			{
				return this;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 東証より取得したExcelの値が-かをチェック
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsHyphen(string str)
		{
			bool result = false;

			if (!string.IsNullOrEmpty(str))
			{
				if (str == "-")
				{
					result = true;
				}
			}

			return result;
		}

		/// <summary>
		/// 東証より取得したExcelの-をnullに置き換える
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int ReplaceHyphenToZero(string str)
		{
			int r = 0;
			if (!string.IsNullOrEmpty(str))
			{
				if (str != "-")
				{
					r = Convert.ToInt32(str);
				}
			}

			return r;
		}

		/// <summary>
		/// 日付型文字列が正しい日付文字列かチェックを行い正しければ指定フォーマットに変換
		/// 不正な日付の場合は空文字
		/// </summary>
		/// <param name="val">日付文字列</param>
		/// <param name="setFormat">指定フォーマット 基本はyyyy/MM/dd</param>
		/// <returns>変換した文字列</returns>
		public static string ToDateFormatString(string val,  string setFormat = "yyyy/MM/dd")
		{
			string result = string.Empty;
			DateTime valDate = new DateTime();

			if (string.IsNullOrEmpty(val) == false)
			{
				string formatString = Convert.ToInt32(val.Replace("/", string.Empty).Replace(" ", string.Empty)).ToString("0000/00/00");
				if (DateTime.TryParse(formatString, out valDate))
				{
					result = valDate.ToString(setFormat);
				}
			}

			return result;
		}

		/// <summary>
		/// 日付文字列も日付型に変換
		/// 変換出来ない場合は1970/01/01
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static DateTime DateStringToDate(string val)
		{
			DateTime result = new DateTime();

			return result;
		}
	}
}
