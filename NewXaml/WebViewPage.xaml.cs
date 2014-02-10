using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web;

namespace NewXaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebViewPage
    {
        public WebViewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Uri url = WebViewLocal.BuildLocalStreamUri("SampleScriptNotify", "WebViewPageSample2.html");
            StreamUriWinRTResolver resolver = new StreamUriWinRTResolver();

            // Pass the resolver object to the navigate call.
            WebViewLocal.NavigateToLocalStreamUri(url, resolver);
        }

        private void WebViewLocal_OnScriptNotify(object sender, NotifyEventArgs e)
        {
            ScriptNotifyResult.Text = "Message: " + e.Value;
        }
    }

    public sealed class StreamUriWinRTResolver : IUriToStreamResolver
    {
        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            if (uri == null)
            {
                throw new Exception();
            }
            string path = uri.AbsolutePath;

            // Because of the signature of the this method, it can't use await, so we 
            // call into a seperate helper method that can use the C# await pattern.
            return GetContent(path).AsAsyncOperation();
        }

        private async Task<IInputStream> GetContent(string path)
        {
            // We use a package folder as the source, but the same principle should apply
            // when supplying content from other locations
            try
            {
                Uri localUri = new Uri("ms-appx://" + path);
                StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(localUri);
                IRandomAccessStream stream = await f.OpenAsync(FileAccessMode.Read);
                return stream;
            }
            catch (Exception) { throw new Exception("Invalid path"); }
        }
    }
}
