using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using BusinessLogicLayer;

class Program
{

    static void Main(string[] args)
    {
        string directoryPath = @"D:\download\HTMLCoursesPages";

        Console.Write("Press 'c' to continue scraping...");
        if (Console.ReadLine() == "c")
        {
            if (clsScraping.StartScraping(directoryPath))
            {
                Console.WriteLine("Done Succesfully!");

            }
            else
            {
                Console.WriteLine("Failed Succesfully!");
            }


        }

        Console.ReadLine();
    }
}
