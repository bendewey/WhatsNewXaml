using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace NewXaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchBoxPage
    {
        private List<string> Fruits = new List<string>()
        {
            "Apple",
            "Apricot",
            "Avocado",
            "Banana",
            "Breadfruit",
            "Bilberry",
            "Blackberry",
            "Blackcurrant",
            "Blueberry",
            "Boysenberry",
            "Cantaloupe",
            "Currant",
            "Cherry",
            "Cherimoya",
            "Cloudberry",
            "Coconut",
            "Cranberry",
            "Cucumber",
            "Damson",
            "Date",
            "Dragonfruit",
            "Durian",
            "Eggplant",
            "Elderberry",
            "Feijoa",
            "Fig",
            "Goji berry",
            "Gooseberry",
            "Grape",
            "Grapefruit",
            "Guava",
            "Huckleberry",
            "Honeydew",
            "Jackfruit",
            "Jambul",
            "Jujube",
            "Kiwi fruit",
            "Kumquat",
            "Lemon",
            "Lime",
            "Loquat",
            "Lychee",
            "Mango",
            "Melon",
            "Canary melon",
            "Cantaloupe",
            "Honeydew",
            "Watermelon",
            "Rock melon",
            "Mulberry",
            "Nectarine",
            "Nut",
            "Olive",
            "Orange",
            "Clementine",
            "Mandarine",
            "Blood Orange",
            "Tangerine",
            "Pamelo",
            "Papaya",
            "Passionfruit",
            "Peach",
            "Pepper",
            "Chili Pepper",
            "Bell Pepper",
            "Pear",
            "Persimmon",
            "Physalis",
            "Plum",
            "Pineapple",
            "Pomegranate",
            "Pomelo",
            "Prune",
            "Purple Mangosteen",
            "Quince",
            "Raspberry",
            "Western raspberry",
            "Rambutan",
            "Redcurrant",
            "Salal berry",
            "Satsuma",
            "Star fruit",
            "Strawberry",
            "Tamarillo",
            "Ugli fruit",
            "Watermelon"
        };

        public SearchBoxPage()
        {
            this.InitializeComponent();
        }

        private void SearchBox_OnQuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs e)
        {
            var dialog = new MessageDialog("Search Submitted for " + e.QueryText);
            dialog.ShowAsync();
        }

        private void SearchBox_OnSuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs e)
        {
            var upperQuery = e.QueryText.ToUpperInvariant();
            if (upperQuery.Length > 0)
            {
                if (upperQuery[0] == 'B')
                {
                    var stream = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/40Banana.png"));
                    e.Request.SearchSuggestionCollection.AppendResultSuggestion("Banana", "See full details on bananas", "banana", stream, "banana icon");
                    e.Request.SearchSuggestionCollection.AppendSearchSeparator("");    
                }

                e.Request.SearchSuggestionCollection.AppendQuerySuggestions(Fruits.Where(s => s.ToUpperInvariant().StartsWith(upperQuery)));    
            }
        }
    }
}
