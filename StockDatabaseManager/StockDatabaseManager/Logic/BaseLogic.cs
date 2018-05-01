using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockDatabaseManager.Models;
using StockDatabaseManager.Context;

namespace StockDatabaseManager.Logic
{
	class BaseLogic
	{
		public DatabaseContext Db { get; set; }

		/// <summary>
		/// DatabaseとTableが存在しない場合新規に作成
		/// </summary>
		public void CreateDatacase()
		{
			bool createFlg = Db.Database.Exists();

			if (createFlg)
			{
				bool compatibleModelFlg = Db.Database.CompatibleWithModel(true);
				if (!compatibleModelFlg)
				{
					//現在のモデルとデータベースのハッシュモデルが違った場合のみマイグレーションを実行
					//EFの罠 https://qiita.com/Kokudori/items/8f1889d4b5a66df434de
					Db.Database.Initialize(false);
				}
			}
			else
			{
				Db.Database.Create();
			}
		}
	}
}
