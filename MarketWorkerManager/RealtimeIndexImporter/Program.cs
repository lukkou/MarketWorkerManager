using RealtimeIndexImporter.Controllers;

namespace RealtimeIndexImporter
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 0)
			{
				using (IndexImportController indexImport = new IndexImportController())
				{
					indexImport.Run(args);
				}
			}
		}
	}
}
