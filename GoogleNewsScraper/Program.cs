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
        [STAThread]
        static void Main(string[] args)
        {
            const string SECTION_CLASS = "WyeMbd";
            const string SECTION_HEADER_CLASS = "adH5zf";
            const string MAIN_COVERAGE_CLASS = "nuEeue";

            var csv = new StringBuilder();
            string filePath = "headlines.csv";

            var URL = @"https://news.google.com/";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.LoadFromBrowser(URL);

            var frontpageNewsSections = htmlDoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(SECTION_CLASS));

            List<string> headers = new List<string>();
            List<List<string>> allMainHeadlines = new List<List<string>>();

            foreach(var section in frontpageNewsSections)
            {
                var header = section.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(SECTION_HEADER_CLASS)).First().InnerText;
                var mainHeadlines = section.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(MAIN_COVERAGE_CLASS));

                headers.Add(header);
                Console.WriteLine(header);

                allMainHeadlines.Add(new List<string>());
                foreach(var headline in mainHeadlines)
                {
                    var santizedHeadline = WebUtility.HtmlDecode(headline.InnerText);
                    allMainHeadlines.Last().Add(santizedHeadline);
                    Console.WriteLine("\t" + santizedHeadline);
                }
            }

            foreach(var header in headers)
            {
                csv.Append(header + ",");
            }
            csv.Append(Environment.NewLine);

            int maxHeadLines = allMainHeadlines.Select(x => x.Count).ToList().Max();
            for(int i = 0; i < maxHeadLines; i++)
            {
                foreach(var sectionHeadlines in allMainHeadlines)
                {
                    if(i < sectionHeadlines.Count)
                    {
                        csv.Append('"' + sectionHeadlines[i] + '"' + ",");
                    }
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
