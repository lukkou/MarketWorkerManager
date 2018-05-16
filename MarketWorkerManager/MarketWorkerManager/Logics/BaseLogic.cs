using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MarketWorkerManager.Models;
using MarketWorkerManager.Context;

namespace MarketWorkerManager.Logic
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
				try
				{
					bool compatibleModelFlg = Db.Database.CompatibleWithModel(true);
					if (!compatibleModelFlg)
					{
						//現在のモデルとデータベースのハッシュモデルが違った場合のみマイグレーションを実行
						//EFの罠 https://qiita.com/Kokudori/items/8f1889d4b5a66df434de
						Db.Database.Initialize(true);
					}
				}
				catch (Exception)
				{
					//CompatibleWithModelがTrueの場合DB構造がEFモデルのメタデータがない場合例外がスローされる
					//DBの削除と再構築を実施
					//https://msdn.microsoft.com/ja-jp/library/gg679576(v=vs.113).aspx
					Db.Database.Delete();
					Db.Database.Create();
				}
			}
			else
			{
				Db.Database.Create();
			}
		}
	}
}
