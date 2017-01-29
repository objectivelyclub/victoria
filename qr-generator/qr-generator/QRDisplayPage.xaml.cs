using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public QRDisplayPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var barcodeWriter = new BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300,
                    Margin = 30
                }
            };

            var image = barcodeWriter.Write("The Return of the King. Abridged. dfkdhjdfhsadfhjdfhhfkjhdsakjfhkjdshfjhfkjafjkdjjjjjjjjjjjjjjjjjjjjjjjjjjjThe Return of the King. Abridged. dfkdhjdfhsadfhjdfhhfkjhdsakjfhkjdshfjhfkjafjkdjjjjjjjjjjjjjjjjjjjjjjjjjjjThe Return of the King. Abridged. dfkdhjdfhsadfhjdfhhfkjhdsakjfhkjdshfjhfkjafjkdjjjjjjjjjjjjjjjjjjjjjjjjjjjThe Return of the King. Abridged. dfkdhjdfhsadfhjdfhhfkjhdsakjfhkjdshfjhfkjafjkdjjjjjjjjjjjjjjjjjjjjjjjjjjj");

            imageBarcode.Source = image;
        }
    }
}
