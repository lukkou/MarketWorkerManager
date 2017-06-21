using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDatabaseManager.Common
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
		/// 東証より取得したExcelの-を空文字に置き換える
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ReplaceHyphenToEmpty(string str)
		{
			string r = string.Empty;

			if (!string.IsNullOrEmpty(str))
			{
				if (str != "-")
				{
					r = str;
				}
			}

			return r;
		}

		/// <summary>
		/// 東証より取得したExcelの-をnullに置き換える
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int? ReplaceHyphenToNull(string str)
		{
			int? r = null;
			if (!string.IsNullOrEmpty(str))
			{
				if (str != "-")
				{
					r = Convert.ToInt32(str);
				}
			}

			return r;
		}
	}
}
