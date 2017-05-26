using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDatabaseManager
{
	class Program
	{
		static void Main(string[] args)
		{
			if (Properties.Settings.Default.FirstInsert)
			{
				//データベースへテーブルを作成
			}
			else
			{
				//初回取り込みの実行
			}

		}
	}
}
