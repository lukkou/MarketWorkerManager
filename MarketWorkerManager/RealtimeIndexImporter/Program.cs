using System;
using RealtimeIndexImporter.Controller;

namespace RealtimeIndexImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("重要指標通知exe\r\nEをクリックすると止まります");
            while (true)
            {
                using (IndexImportController indexImport = new IndexImportController())
                {
                    indexImport.Run();

                    if (Console.KeyAvailable)
                    {
                        string inputKey = Console.ReadKey().Key.ToString();
                        if (inputKey == "E")
                        {
                            return;
                        }
                    }
                }
            }
            
        }
    }
}

