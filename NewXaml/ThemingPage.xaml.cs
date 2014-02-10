using Windows.UI.Xaml;

namespace NewXaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThemingPage
    {
        public ThemingPage()
        {
            this.InitializeComponent();
        }

        private void SwitchTheme_OnClick(object sender, RoutedEventArgs e)
        {
            if (DynamicThemeContainer.RequestedTheme == ElementTheme.Light)
            {
                DynamicThemeContainer.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                DynamicThemeContainer.RequestedTheme = ElementTheme.Light;
            }
        }
    }
}
