using System.IO;


namespace StockDatabaseManager.Utility
{
	class DirectoryUtility
	{
		/// <summary>
		/// 指定パスにディレクトリを作成する
		/// </summary>
		/// <param name="path">作成パス</param>
		public static void CreateDirectory(string path)
		{
			if (!IsDirectoryExists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		/// <summary>
		/// ディレクトリの存在を確認する
		/// </summary>
		/// <param name="path">対象パス</param>
		/// <returns></returns>
		public static bool IsDirectoryExists(string path)
		{
			if (Directory.Exists(path))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
