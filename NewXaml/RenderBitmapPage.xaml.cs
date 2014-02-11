using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace NewXaml
{
    public sealed partial class RenderBitmapPage
    {
        public RenderBitmapPage()
        {
            this.InitializeComponent();
            DataTransferManager.GetForCurrentView().DataRequested += RenderBitmapPage_DataRequested;
        }

        void RenderBitmapPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.Properties.Title = "RenderToBitmap";
            e.Request.Data.Properties.Description = "Render to Bitmap Meme";

            e.Request.Data.SetDataProvider(StandardDataFormats.Bitmap, OnDeferredImageRequestedHandler);
        }

        /// <summary>
        /// Handles image requests from the Share charm.
        /// </summary>
        /// <param name="request"></param>
        private async void OnDeferredImageRequestedHandler(DataProviderRequest request)
        {
            // Request deferral to wait for async calls
            DataProviderDeferral deferral = request.GetDeferral();

            // XAML objects can only be accessed on the UI thread, and the call may come in on a background thread
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
                    InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
                    // Render to an image at the current system scale and retrieve pixel contents
                    await renderTargetBitmap.RenderAsync(Meme);
                    var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

                    // Encode image to an in-memory stream
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Ignore,
                        (uint)renderTargetBitmap.PixelWidth,
                        (uint)renderTargetBitmap.PixelHeight,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        pixelBuffer.ToArray());

                    await encoder.FlushAsync();

                    // Set content of the DataProviderRequest to the encoded image in memory
                    request.SetData(RandomAccessStreamReference.CreateFromStream(stream));
                }
                finally
                {
                    deferral.Complete();
                }
            });
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }
    }
}
