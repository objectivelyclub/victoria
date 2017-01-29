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

namespace qr_generator
{
    public sealed partial class QRDisplayPage : Page
    {

        public string dataSizeText { get; set; }
        public string dataContentsText { get; set; }

        private System.Threading.Timer _timer;

        private string[] inputDataArray = {"ABCDEFGH22", "ABDFERGS43", "0123456789", "DEADBEEF10"};
        private int inputDataIndex = 0;


        private BarcodeWriter barcodeWriter = new BarcodeWriter
        {
            Format = ZXing.BarcodeFormat.QR_CODE,
            Options = new ZXing.Common.EncodingOptions
            {
                Width = 300,
                Height = 300,
                Margin = 30
            }
        };

        public QRDisplayPage()
        {
            this.InitializeComponent();
            _timer = new System.Threading.Timer(new System.Threading.TimerCallback((obj) => Refresh()), null, 0, 2000);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private async void Refresh()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if(inputDataIndex < 4)
                {
                    var inputData = inputDataArray[inputDataIndex];
                    var image = barcodeWriter.Write(string.Concat(Enumerable.Repeat(inputData, 50)));
                    imageBarcode.Source = image;
                    dataSizeText = (inputData.Length * sizeof(Char)).ToString();
                    dataContentsText = inputData;
                    inputDataIndex++;
                } else
                {
                    inputDataIndex = 0;
                }

            });

        }
    }
}
