using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;

namespace NewXaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsFlyoutPage
    {
        public SettingsFlyoutPage()
        {
            this.InitializeComponent();
        }

        private void Register_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += (s, arg) =>
            {
                SettingsCommand defaultsCommand = new SettingsCommand("general", "General",
                    (handler) =>
                    {
                        GeneralSettingsFlyout sf = new GeneralSettingsFlyout();
                        sf.Show();
                    });
                arg.Request.ApplicationCommands.Add(defaultsCommand);
            };
        }

        private void ShowSettings_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsPane.Show();
        }
    }
}
