﻿using System;
using RealtimeIndexImporter.Controller;

namespace RealtimeIndexImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IndexImportController indexImport = new IndexImportController())
            {
                Console.WriteLine("Eをクリックすると止まります");
                while (true)
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

