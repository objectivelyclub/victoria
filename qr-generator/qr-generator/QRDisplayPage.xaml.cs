using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZXing.Mobile;
using Windows.Storage;

namespace qr_generator
{
    public sealed partial class QRDisplayPage : Page
    {

        public string dataSizeText { get; private set; }
        public string dataContentsText { get; private set; }
        public object fileNameText { get; private set; }

        private System.Threading.Timer _timer;

        private string[] inputDataArray = {"ABCDEFGH22", "ABDFERGS43", "0123456789", "DEADBEEF10"};
        private int inputDataIndex = 0;

        private List<Windows.UI.Xaml.Media.Imaging.WriteableBitmap> imageQueue = new List<Windows.UI.Xaml.Media.Imaging.WriteableBitmap>();
        private List<int> dataSizeInfoQueue = new List<int>();
        private List<string> fakeInputDataList = new List<string>();

        private BarcodeWriter barcodeWriter = new BarcodeWriter
        {
            Format = ZXing.BarcodeFormat.QR_CODE,
            Options = new ZXing.Common.EncodingOptions
            {
                Width = 750,
                Height = 750,
                Margin = 30
            }
        };

        public QRDisplayPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is StorageFile)
            {
                StorageFile file = e.Parameter as StorageFile;
                fileNameText = file.Name;
                Bindings.Update();
            }
            base.OnNavigatedTo(e);
            GenerateBitmap();
            _timer = new System.Threading.Timer(new System.Threading.TimerCallback((obj) => Refresh()), null, 0, 2000);
        }

        private void GenerateBitmap()
        {
            foreach (var inputData in inputDataArray)
            {
                string fakeInputData = string.Concat(Enumerable.Repeat(inputData, 50));
                var fakeInputDataSize = fakeInputData.Length * sizeof(char);
                var image = barcodeWriter.Write(fakeInputData);

                dataSizeInfoQueue.Add(fakeInputDataSize);
                imageQueue.Add(image);
            }
        }

        private async void Refresh()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (inputDataIndex >= imageQueue.Count)
                {
                    inputDataIndex = 0;
                }
                dataSizeText = dataSizeInfoQueue[inputDataIndex].ToString();
                var image = imageQueue[inputDataIndex];
                if(image != null)
                {
                    imageBarcode.Source = image;
                }
                inputDataIndex++;
                Bindings.Update();
            });

        }
    }
}
