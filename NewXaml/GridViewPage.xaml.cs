using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

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

        private void GridView_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            args.Handled = true;

            if (args.Phase == 0)
            {
                var templateRoot = args.ItemContainer.ContentTemplateRoot as Grid;
                var placeholder = templateRoot.FindName("placeholderRect") as Rectangle;
                var itemTitle = templateRoot.FindName("itemTitle") as TextBlock;
                var itemImage = templateRoot.FindName("itemImage") as Image;

                placeholder.Opacity = 1;
                itemTitle.Opacity = 0;
                itemImage.Opacity = 0;

                args.RegisterUpdateCallback(ShowTitle);
            }
        }

        private void ShowTitle(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase == 1)
            {
                var item = args.Item as Item;

                var templateRoot = args.ItemContainer.ContentTemplateRoot as Grid;
                var itemTitle = templateRoot.FindName("itemTitle") as TextBlock;

                Task.Delay(1).Wait();
                itemTitle.Text = item.Title;
                itemTitle.Opacity = 1;

                args.RegisterUpdateCallback(ShowImage);
            }
        }

        private void ShowImage(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase == 2)
            {
                var item = args.Item as Item;

                var templateRoot = args.ItemContainer.ContentTemplateRoot as Grid;
                var itemImage = templateRoot.FindName("itemImage") as Image;

                Task.Delay(1).Wait();
                itemImage.Source = new BitmapImage(new Uri(item.Image));
                itemImage.Opacity = 1;

                // no further
            }
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
