using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleNewsScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            const string SECTION_CLASS = "WyeMbd";
            const string SECTION_HEADER_CLASS = "adH5zf";
            const string MAIN_COVERAGE_CLASS = "nuEeue";

            var csv = new StringBuilder();
            string filePath = "headlines.csv";

            var URL = @"https://news.google.com/";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(URL);

            var frontpageNewsSections = htmlDoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(SECTION_CLASS));

            foreach(var section in frontpageNewsSections)
            {
                var header = section.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(SECTION_HEADER_CLASS)).First().InnerText;
                var mainCoverageHeaders = section.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(MAIN_COVERAGE_CLASS));

                Console.WriteLine(header);
                csv.Append(header + Environment.NewLine);
                foreach(var mainCoverageHeader in mainCoverageHeaders)
                {
                    var mainCoverageHeaderText = WebUtility.HtmlDecode(mainCoverageHeader.InnerText);
                    Console.WriteLine("\t" + mainCoverageHeaderText);
                    csv.Append("\"" + mainCoverageHeaderText + "\"" + Environment.NewLine);
                }
                csv.Append(Environment.NewLine);
            }

            File.WriteAllText(filePath, csv.ToString());
            Console.WriteLine();
            Console.WriteLine("Output exported to " + filePath);
            Console.ReadKey();
        }
    }
}
