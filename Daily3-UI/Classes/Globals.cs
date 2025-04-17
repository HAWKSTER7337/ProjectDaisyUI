using Daily3_UI.Enums;
using Daily3_UI.Pages;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace Daily3_UI.Classes
{
    /// <summary>
    ///     All Global values to the application
    /// </summary>
    public static class Globals
    {
        private static Guid? _userid = null;
        private static Status? _status = null;

        public static Guid? UserId
        {
            get => _userid;
            set => _userid ??= value;
        }

        public static Status? Status
        {
            get => _status;
            set => _status ??= value;
        }

        /// <summary>
        ///     Tries to get color from static resources. Returns black if not found.
        /// </summary>
        public static Color GetColor(string colorName)
        {
            try
            {
                object value = null; // ✅ initialize to avoid CS0165
                var found = Application.Current?.Resources?.TryGetValue(colorName, out value) ?? false;
                return found && value is Color color ? color : Color.FromRgb(0, 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetColor('{colorName}'): {ex.Message}");
                return Color.FromRgb(0, 0, 0);
            }
        }



        // Lazy loading avoids static init exceptions
        private static List<KeyValuePair<string, ContentPage>>? _daily3ContentPages;
        public static List<KeyValuePair<string, ContentPage>> Daily3ContentPages =>
            _daily3ContentPages ??= new List<KeyValuePair<string, ContentPage>>
            {
                new("Daily Numbers", new WinningNumbersPage()),
                new("Buy Tickets", new BuyTickets()),
                new("View History", new TicketHistory()),
                new("Entrants Tickets", new HousePage())
            };

        private static List<KeyValuePair<string, ContentPage>>? _daily4ContentPages;
        public static List<KeyValuePair<string, ContentPage>> Daily4ContentPages =>
            _daily4ContentPages ??= new List<KeyValuePair<string, ContentPage>>
            {
                new("Daily Numbers", new WinningNumbersPageDaily4()),
                new("Buy Tickets", new BuyTicketsDaily4()),
                new("View History", new TicketHistoryDaily4()),
                new("Entrants Tickets", new HousePageDaily4())
            };
    }
}
