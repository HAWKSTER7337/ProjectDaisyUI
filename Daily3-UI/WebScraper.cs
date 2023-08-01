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
        private static readonly HtmlWeb Web = new HtmlWeb();
        private static readonly string Daily3HyperLink = "https://www.lotterypost.com/results/mi/daily3/past";

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
            HtmlDocument doc = await Web.LoadFromWebAsync(Daily3HyperLink);
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
            var targetElement = doc.DocumentNode.SelectSingleNode($"//time[starts-with(@datetime, '{gettingDate}')]");
            if (targetElement is null)
                return new List<WinningNumber>();

            var nodeNumbers = targetElement.ParentNode.ParentNode.SelectNodes(".//li");
            return nodeNumbers
                .Select((nodeNumber, index) => new { nodeNumber, index })
                .GroupBy(pair => pair.index / 3, pair => pair.nodeNumber.InnerText)
                .Select(group => new WinningNumber(group.ToList()))
                .ToList();
        }

        /// <summary>
        /// Gets current date in the format of how the daily 3 uses it
        /// </summary>
        private static string GetCurrentDate()
        {
            var dateTime = DateTime.Today.ToString("yyyy-MM-dd");
            return dateTime;
        }

        /// <summary>
        /// Get the date of yesterday
        /// </summary>
        private static string GetYesterday()
        {
            var dateTime = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            return dateTime;
        }
    }

    
}
