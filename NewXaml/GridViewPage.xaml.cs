using System.Collections.Generic;
using Windows.UI.Xaml.Controls.Primitives;

namespace NewXaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GridViewPage
    {
        public GridViewPage()
        {
            this.InitializeComponent();

            DataContext = new GridViewPageViewModel();
        }
    }

    public class GridViewPageViewModel
    {
        public GridViewPageViewModel()
        {
            Groups = GenerateSampleData();
        }

        public List<Group> Groups { get; set; }

        private List<Group> GenerateSampleData()
        {
            var list = new List<Group>();
            for (int i = 0; i < 50; i++)
            {
                list.Add(new Group(i)
                {
                    Title = "Group" + i
                });
            }

            return list;
        }
    }

    public class Group
    {
        public Group(int key)
        {
            Items = GenerateSampleData(key);
        }

        public string Title { get; set; }
        public List<Item> Items { get; set; }

        private List<Item> GenerateSampleData(int key)
        {
            var list = new List<Item>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(new Item()
                {
                    Image = "http://lorempixel.com/80/80/sports/?key=" + key + "-" + i,
                    Title = "Item " + i
                });
            }

            return list;
        }
    }

    public class Item
    {
        public string Image { get; set; }
        public string Title { get; set; }
    }
}
