using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NewXaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationPage
    {
        private Frame _rootFrame;

        public NavigationPage()
        {
            this.InitializeComponent();
            _rootFrame = ((App) Application.Current).RootFrame;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Depth.Text = _rootFrame.BackStackDepth.ToString();
        }

        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            _rootFrame.Navigate(typeof (NavigationPage));
        }

        private void Unwind_Click(object sender, RoutedEventArgs e)
        {
            while (_rootFrame.BackStackDepth > 2)
            {
                _rootFrame.BackStack.Remove(_rootFrame.BackStack.Last());
            }
            _rootFrame.GoBack();
        }
    }
}
