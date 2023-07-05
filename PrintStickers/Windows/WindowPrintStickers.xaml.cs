using PrintStickers.Models;
using PrintStickers;
using Resto.Front.Api;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;

namespace PrintStickers.Windows
{
    public partial class WindowPrintStickers : Window
    {
        private BindingList<ModelUserPrint> _userPrintData = new BindingList<ModelUserPrint>();
        private int _orderNumber = 0;
        public WindowPrintStickers(ModelIikoOrder iikoOrder)
        {
            InitializeComponent();

            _orderNumber = iikoOrder.Number;

            // Получаем имя клиента
            var ClientName = "";
            if (iikoOrder.CustomerIds.Any())
            {
                var Client = PluginContext.Operations.TryGetClientById(new Guid(iikoOrder.CustomerIds.First()));
                if (Client != null)
                {
                    ClientName = Client.Name;
                }
            }

            foreach (var item in iikoOrder.Items)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    _userPrintData.Add(new ModelUserPrint() { IsPrint = CategoryEnable(item), ProductName = item.Product.Name, ClientName = item.Guest.Name != "" ? item.Guest.Name : ClientName });
                }
            }
            dgTodoList.ItemsSource = _userPrintData;
        }

        private void PrintClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var properties = Properties.Settings.Default;
                string pathLogo = PluginContext.Integration.GetDataStorageDirectoryPath() + @"\old.bmp";
                foreach (var item in _userPrintData)
                {
                    if (item.IsPrint)
                    {
                        PluginContext.Log.Info("[PRINT]: Order: " + _orderNumber + ", Product: " + item.ProductName + ", Client: " + item.ClientName);
                        Print.PrintDoc(
                            stikerHeight: properties.StikerHeight.ToString(),
                            stikerWidth: properties.StikerWidth.ToString(),
                            spaceUpToLogo: properties.SpaceUpToLogo,
                            logoHeight: properties.LogoHeight,
                            logoWidth: properties.LogoWidth,
                            spaceLogotoProduct: properties.SpaceLogoToProduct,
                            productDivHeight: properties.ProductDivHeight,
                            productFontSize: properties.ProductFontSize,
                            productFontStyleBolt: properties.ProductFontStyleBolt,
                            productFontStyleItalic: properties.ProductFontStyleItalic,
                            clientDivHeight: properties.ClientDivHeight,
                            clientFontStyleBolt: properties.ClientFontStyleBolt,
                            clientFontStyleItalic: properties.ClientFontStyleItalic,
                            clientFontSize: properties.ClientFontSize,
                            pathImageLogo: pathLogo,
                            printerName: properties.PrinterName,
                            textProduct: item.ProductName,
                            textClient: item.ClientName
                            );

                        PluginContext.Operations.AddWarningMessage("Стрикеры отправлены на печать.", "WindowPrintStickers", TimeSpan.FromSeconds(15));
                    }
                }
            }
            catch (Exception ex)
            {
                PluginContext.Operations.AddWarningMessage("Не удалось отправить стикеры на печать: " + ex.Message, "WindowPrintStickers", TimeSpan.FromSeconds(15));
                PluginContext.Log.Error("[WindowPrintStickers]: Не удалось отправить стикеры на печать: " + ex.Message);
            }

            DialogResult = true;
        }

        public bool CategoryEnable(Items item)
        {
            string[] categories = Properties.Settings.Default.ProductCategories == "" ? new string[] { } : Properties.Settings.Default.ProductCategories.Split('\u002C');

            if (categories.Any())
            {
                foreach (var category in categories)
                {
                    if (item.Product.Category.Name == category)
                    {
                        return true;
                    }
                }
            }
            //else
            //{
            //    return true;
            //}

            return false;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
