using Microsoft.Win32;
using PrintStickers;
using Resto.Front.Api;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PrintStickers.Windows
{
    public partial class WindowSettingsPrintStickers : Window
    {
        private static Bitmap _bitmap;
        private string _pathStorage = PluginContext.Integration.GetDataStorageDirectoryPath();

        public WindowSettingsPrintStickers()
        {
            InitializeComponent();

            var parameters = Properties.Settings.Default;

            ValueStikerHeight.Text = parameters.StikerHeight.ToString();
            ValueStikerWidth.Text = parameters.StikerWidth.ToString();
            ValueSpaceUpToLogo.Text = parameters.SpaceUpToLogo.ToString();
            ValueLogoHeight.Text = parameters.LogoHeight.ToString();
            ValueLogoWidth.Text = parameters.LogoWidth.ToString();
            ValueSpaceLogoToProduct.Text = parameters.SpaceLogoToProduct.ToString();
            ValueProductDivHeight.Text = parameters.ProductDivHeight.ToString();
            ValueProductFontSize.Text = parameters.ProductFontSize.ToString();
            CheckProductFontStyleBolt.IsChecked = parameters.ProductFontStyleBolt;
            CheckProductFontStyleItalic.IsChecked = parameters.ProductFontStyleItalic;
            ValueClientDivHeight.Text = parameters.ClientDivHeight.ToString();
            ValueClientFontSize.Text = parameters.ClientFontSize.ToString();
            CheckClientFontStyleBolt.IsChecked = parameters.ClientFontStyleBolt;
            CheckClientFontStyleItalic.IsChecked = parameters.ClientFontStyleItalic;
            SelectPrinter.ItemsSource = PrinterSettings.InstalledPrinters;
            SelectPrinter.Text = parameters.PrinterName;
            CheckAutoPrint.IsChecked = parameters.AutoPrint;
            ValueProductCategories.Text = parameters.ProductCategories;

            if (File.Exists(_pathStorage + @"\old.bmp"))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri(_pathStorage + @"\old.bmp", UriKind.RelativeOrAbsolute);
                image.EndInit();

                StickerImageLogo.Source = image;
            }

            EditScenneSticker();
        }

        private void PrintClick(object sender, RoutedEventArgs e)
        {
            PluginContext.Log.Info("[PRINT TEST]: Product: " + ValueProductText.Text + ", Client: " + ValueClientText.Text);

            Print.PrintDoc(
                stikerHeight: ValueStikerHeight.Text,
                stikerWidth: ValueStikerWidth.Text,
                spaceUpToLogo: Int32.Parse(ValueSpaceUpToLogo.Text),
                logoHeight: Int32.Parse(ValueLogoHeight.Text),
                logoWidth: Int32.Parse(ValueLogoWidth.Text),
                spaceLogotoProduct: Int32.Parse(ValueSpaceLogoToProduct.Text),
                productDivHeight: Int32.Parse(ValueProductDivHeight.Text),
                productFontSize: Int32.Parse(ValueProductFontSize.Text),
                productFontStyleBolt: CheckProductFontStyleBolt.IsChecked.HasValue ? CheckProductFontStyleBolt.IsChecked.Value : false,
                productFontStyleItalic: CheckProductFontStyleItalic.IsChecked.HasValue ? CheckProductFontStyleItalic.IsChecked.Value : false,
                clientDivHeight: Int32.Parse(ValueClientDivHeight.Text),
                clientFontSize: Int32.Parse(ValueClientFontSize.Text),
                clientFontStyleBolt: CheckClientFontStyleBolt.IsChecked.HasValue ? CheckClientFontStyleBolt.IsChecked.Value : false,
                clientFontStyleItalic: CheckClientFontStyleItalic.IsChecked.HasValue ? CheckClientFontStyleItalic.IsChecked.Value : false,
                pathImageLogo: File.Exists(_pathStorage + @"\new.bmp") ? _pathStorage + @"\new.bmp" : _pathStorage + @"\old.bmp",
                printerName: SelectPrinter.Text,
                textProduct: ValueProductText.Text,
                textClient: ValueClientText.Text);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Config.SetConfig(
                Convert.ToInt32(ValueStikerHeight.Text),
                Convert.ToInt32(ValueStikerWidth.Text),
                Convert.ToInt32(ValueSpaceUpToLogo.Text),
                Convert.ToInt32(ValueLogoHeight.Text),
                Convert.ToInt32(ValueLogoWidth.Text),
                Convert.ToInt32(ValueSpaceLogoToProduct.Text),
                Convert.ToInt32(ValueProductDivHeight.Text),
                Convert.ToInt32(ValueProductFontSize.Text),
                (bool)CheckProductFontStyleBolt.IsChecked,
                (bool)CheckProductFontStyleItalic.IsChecked,
                Convert.ToInt32(ValueClientDivHeight.Text),
                Convert.ToInt32(ValueClientFontSize.Text),
                (bool)CheckClientFontStyleBolt.IsChecked,
                (bool)CheckClientFontStyleItalic.IsChecked,
                SelectPrinter.Text,
                (bool)CheckAutoPrint.IsChecked,
                ValueProductCategories.Text);

            if (_bitmap != null)
            {
                if (File.Exists(_pathStorage + @"\old.bmp"))
                {
                    File.Delete(_pathStorage + @"\old.bmp");
                }
                _bitmap.Save(_pathStorage + @"\old.bmp");
            }

            DialogResult = true;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var filePath = string.Empty;
            var pathStorage = PluginContext.Integration.GetDataStorageDirectoryPath();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Image Files (*.bmp)|*.bmp";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;

                var path = new Uri(filePath);
                var image = new BitmapImage(path);

                StickerImageLogo.Source = image;

                _bitmap = new Bitmap(filePath);
                _bitmap.Save(_pathStorage + @"\new.bmp");
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EditScenneSticker();
        }

        private void CheckStyleTextProductBoltChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckProductFontStyleBolt.IsChecked)
            {
                StickerTextProduct.FontWeight = FontWeights.Bold;
            }
            else
            {
                StickerTextProduct.FontWeight = FontWeights.Normal;
            }
        }

        private void CheckStyleTextProductItalicChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckProductFontStyleItalic.IsChecked)
            {
                StickerTextProduct.FontStyle = FontStyles.Italic;
            }
            else
            {
                StickerTextProduct.FontStyle = FontStyles.Normal;
            }
        }

        private void CheckStyleTextClientBoltChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckClientFontStyleBolt.IsChecked)
            {
                StickerTextClient.FontWeight = FontWeights.Bold;
            }
            else
            {
                StickerTextClient.FontWeight = FontWeights.Normal;
            }
        }

        private void CheckStyleTextClientItalicChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckClientFontStyleItalic.IsChecked)
            {
                StickerTextClient.FontStyle = FontStyles.Italic;
            }
            else
            {
                StickerTextClient.FontStyle = FontStyles.Normal;
            }
        }

        private void ValueHeightStikerChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueStikerHeight.Text, "[^0-9]"))
                ValueStikerHeight.Text = ValueStikerHeight.Text.Remove(ValueStikerHeight.Text.Length - 1);
            if (ValueStikerHeight.Text == "")
                ValueStikerHeight.Text = "0";

            EditScenneSticker();
        }

        private void ValueWidthStikerChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueStikerWidth.Text, "[^0-9]"))
                ValueStikerWidth.Text = ValueStikerWidth.Text.Remove(ValueStikerWidth.Text.Length - 1);
            if (ValueStikerWidth.Text == "")
                ValueStikerWidth.Text = "0";

            EditScenneSticker();
        }

        private void ValueSpaceUpToLogoChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueSpaceUpToLogo.Text, "[^0-9]"))
                ValueSpaceUpToLogo.Text = ValueSpaceUpToLogo.Text.Remove(ValueSpaceUpToLogo.Text.Length - 1);
            if (ValueSpaceUpToLogo.Text == "")
                ValueSpaceUpToLogo.Text = "0";

            EditScenneSticker();
        }

        private void ValueHeightwLogoChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueLogoHeight.Text, "[^0-9]"))
                ValueLogoHeight.Text = ValueLogoHeight.Text.Remove(ValueLogoHeight.Text.Length - 1);
            if (ValueLogoHeight.Text == "")
                ValueLogoHeight.Text = "0";

            EditScenneSticker();
        }

        private void ValueWidthLogoChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueLogoWidth.Text, "[^0-9]"))
                ValueLogoWidth.Text = ValueLogoWidth.Text.Remove(ValueLogoWidth.Text.Length - 1);
            if (ValueLogoWidth.Text == "")
                ValueLogoWidth.Text = "0";

            EditScenneSticker();
        }

        private void ValueSpaceLogoToProductChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueSpaceLogoToProduct.Text, "[^0-9]"))
                ValueSpaceLogoToProduct.Text = ValueSpaceLogoToProduct.Text.Remove(ValueSpaceLogoToProduct.Text.Length - 1);
            if (ValueSpaceLogoToProduct.Text == "")
                ValueSpaceLogoToProduct.Text = "0";

            EditScenneSticker();
        }

        private void ValueHeightBlockProductChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueProductDivHeight.Text, "[^0-9]"))
                ValueProductDivHeight.Text = ValueProductDivHeight.Text.Remove(ValueProductDivHeight.Text.Length - 1);
            if (ValueProductDivHeight.Text == "")
                ValueProductDivHeight.Text = "0";

            EditScenneSticker();
        }

        private void ValueSizeFontProductChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueProductFontSize.Text, "[^0-9]"))
                ValueProductFontSize.Text = ValueProductFontSize.Text.Remove(ValueProductFontSize.Text.Length - 1);
            if (ValueProductFontSize.Text == "")
                ValueProductFontSize.Text = "0";

            EditScenneSticker();
        }

        private void ValueHeightClientChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueClientDivHeight.Text, "[^0-9]"))
                ValueClientDivHeight.Text = ValueClientDivHeight.Text.Remove(ValueClientDivHeight.Text.Length - 1);
            if (ValueClientDivHeight.Text == "")
                ValueClientDivHeight.Text = "0";

            EditScenneSticker();
        }

        private void ValueSizeFontClientChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ValueClientFontSize.Text, "[^0-9]"))
                ValueClientFontSize.Text = ValueClientFontSize.Text.Remove(ValueClientFontSize.Text.Length - 1);
            if (ValueClientFontSize.Text == "")
                ValueClientFontSize.Text = "0";

            EditScenneSticker();
        }

        private void EditScenneSticker()
        {
            try
            {
                StickerBorder.Height = Int32.Parse(ValueStikerHeight.Text) * StickerSliderZoom.Value;
                StickerBorder.Width = Int32.Parse(ValueStikerWidth.Text) * StickerSliderZoom.Value;
                StickerStackPanel.Height = Int32.Parse(ValueStikerHeight.Text) * StickerSliderZoom.Value;
                StickerStackPanel.Width = Int32.Parse(ValueStikerWidth.Text) * StickerSliderZoom.Value;

                SpaceUpToLogo.Height = Int32.Parse(ValueSpaceUpToLogo.Text) * StickerSliderZoom.Value;
                StickerImageLogo.Height = Int32.Parse(ValueLogoHeight.Text) * StickerSliderZoom.Value;
                StickerImageLogo.Width = Int32.Parse(ValueLogoWidth.Text) * StickerSliderZoom.Value;
                SpaceLogoToProduct.Height = Int32.Parse(ValueSpaceLogoToProduct.Text) * StickerSliderZoom.Value;

                StickerTextProduct.Height = Int32.Parse(ValueProductDivHeight.Text) * StickerSliderZoom.Value;
                StickerTextProduct.FontSize = Int32.Parse(ValueProductFontSize.Text) * StickerSliderZoom.Value;
                StickerTextProduct.Width = Int32.Parse(ValueStikerWidth.Text) * StickerSliderZoom.Value;

                StickerTextClient.Height = Int32.Parse(ValueClientDivHeight.Text) * StickerSliderZoom.Value;
                StickerTextClient.FontSize = Int32.Parse(ValueClientFontSize.Text) * StickerSliderZoom.Value;
                StickerTextClient.Width = Int32.Parse(ValueStikerWidth.Text) * StickerSliderZoom.Value;
            }
            catch (Exception)
            {

            }
        }
    }
}
