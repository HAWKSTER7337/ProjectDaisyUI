using CsvHelper;
using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace Daily3_UI
{

    /// <summary>
    /// Class for Getting the Daily numbers 
    /// off of the website
    /// </summary>
    public static class WebScraper
    {
        /// <summary>
        /// Web reader for collecting information from the site
        /// </summary>
        private static HtmlWeb web = new HtmlWeb();
        private readonly static string daily3HyperLink = "https://www.lotterypost.com/results/mi/daily3/past";
        private readonly static string middayId = "daily-3-wn-";
        private readonly static string eveningId = "daily-3-eve-";

        /// <summary>
        /// The 2 times that you can get a ticket
        /// for a day 
        /// </summary>
        enum TOD
        {
            Midday,
            Evening
        }


        /// <summary>
        /// Pulling all winning numbers from today and 
        /// yesterday
        /// </summary>
        public static async Task<List<WinningNumber>> GetWinningNumbers()
        {
            HtmlDocument doc = await web.LoadFromWebAsync(daily3HyperLink);
            List<WinningNumber> winningNumbers = Scraping(doc);
            return winningNumbers;
        }

        /// <summary>
        /// Actually scraping the data
        /// </summary>
        private static List<WinningNumber> Scraping(HtmlDocument doc)
        {
            List<WinningNumber> winningNumbers = new();
            winningNumbers.AddRange(ScarpingGivenADate(doc, GetYesterday()));
            winningNumbers.AddRange(ScarpingGivenADate(doc, GetCurrentDate()));
            return winningNumbers;
        }

        /// <summary>
        /// Scraping the numbers based on the time of day
        /// </summary>
        private static List<WinningNumber> ScarpingGivenADate(HtmlDocument doc, string gettingDate)
        {
            List<WinningNumber> winningNumbers = new();

            HtmlNode targetElement = doc.DocumentNode.SelectSingleNode($"//time[starts-with(@datetime, '{gettingDate}')]");
            HtmlNode parentElement = targetElement.ParentNode.ParentNode;
            HtmlNodeCollection nodeNumbers = parentElement.SelectNodes(".//li");
            List<string> stringsOfNumbers = new();
            foreach (var nodeNumber in nodeNumbers)
            {
                stringsOfNumbers.Add(nodeNumber.InnerText);
            }
            
            for(int i = 0; i < stringsOfNumbers.Count / 3; i++)
            {
                winningNumbers.Add(new WinningNumber(stringsOfNumbers.GetRange(i * 3, 3)));
            }
            return winningNumbers;
        }

        /// <summary>
        /// Gets current date in the format of how the daily 3 uses it
        /// </summary>
        private static string GetCurrentDate()
        {
            string dateTime = DateTime.Today.ToString("yyyy-MM-dd");
            return dateTime;
        }

        /// <summary>
        /// Get the date of yesterday
        /// </summary>
        private static string GetYesterday()
        {
            string dateTime = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            return dateTime;
        }
    }

    
}
