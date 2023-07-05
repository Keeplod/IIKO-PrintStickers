using Resto.Front.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using PrintStickers.Models;

namespace PrintStickers
{
    internal class Config
    {
        private static string _pathXml = PluginContext.Integration.GetConfigsDirectoryPath() + @"\Config.xml";

        /// <summary>
        ///  Создание конфига %appdata%\Roaming\iiko\CashServer\PluginConfigs\PrintStickers\Config.xml.
        /// </summary>
        public static void CreateXML()
        {
            XDocument configAuthorization = new XDocument();
            XElement config = new XElement("config");

            XElement setting = new XElement("setting");

            XElement stikerHeight = new XElement("stikerHeight", 38);
            XElement stikerWidth = new XElement("stikerWidth", 58);
            XElement spaceUpToLogo = new XElement("spaceUpToLogo", 0);
            XElement logoHeight = new XElement("logoHeight", 5);
            XElement logoWidth = new XElement("logoWidth", 25);
            XElement spaceLogoToProduct = new XElement("spaceLogoToProduct", 5);
            XElement productDivHeight = new XElement("productDivHeight", 18);
            XElement productFontSize = new XElement("productFontSize", 7);
            XElement productFontStyleBolt = new XElement("productFontStyleBolt", "true");
            XElement productFontStyleItalic = new XElement("productFontStyleItalic", "false");
            XElement clientDivHeight = new XElement("clientDivHeight", 20);
            XElement clientFontSize = new XElement("clientFontSize", 5);
            XElement clientFontStyleBolt = new XElement("clientFontStyleBolt", "false");
            XElement clientFontStyleItalic = new XElement("clientFontStyleItalic", "true");
            XElement printerName = new XElement("printerName", "TSC TE200");
            XElement autoPrint = new XElement("autoPrint", "false");
            XElement productCategories = new XElement("productCategories", "");

            setting.Add(stikerHeight);
            setting.Add(stikerWidth);
            setting.Add(spaceUpToLogo);
            setting.Add(logoHeight);
            setting.Add(logoWidth);
            setting.Add(spaceLogoToProduct);
            setting.Add(productDivHeight);
            setting.Add(productFontSize);
            setting.Add(productFontStyleBolt);
            setting.Add(productFontStyleItalic);
            setting.Add(clientDivHeight);
            setting.Add(clientFontSize);
            setting.Add(clientFontStyleBolt);
            setting.Add(clientFontStyleItalic);
            setting.Add(printerName);
            setting.Add(autoPrint);
            setting.Add(productCategories);

            config.Add(setting);
            configAuthorization.Add(config);

            string pathString = PluginContext.Integration.GetConfigsDirectoryPath();
            Directory.CreateDirectory(pathString);

            configAuthorization.Save(_pathXml);
        }

        /// <summary>  
        ///  Проверка наличия конфига
        /// </summary> 
        public static bool CheckConfig()
        {
            return File.Exists(_pathXml) ? true : false;
        }

        /// <summary>
        ///  Возвращает данные из Config.xml
        /// </summary>
        public static ModelConfig GetConfig()
        {
            if (File.Exists(_pathXml))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(_pathXml);

                return new ModelConfig()
                {
                    StikerHeight = Convert.ToInt32(xDoc.GetElementsByTagName("stikerHeight").Item(0).InnerText),
                    StikerWidth = Convert.ToInt32(xDoc.GetElementsByTagName("stikerWidth").Item(0).InnerText),
                    SpaceUpToLogo = Convert.ToInt32(xDoc.GetElementsByTagName("spaceUpToLogo").Item(0).InnerText),
                    LogoHeight = Convert.ToInt32(xDoc.GetElementsByTagName("logoHeight").Item(0).InnerText),
                    LogoWidth = Convert.ToInt32(xDoc.GetElementsByTagName("logoWidth").Item(0).InnerText),
                    SpaceLogoToProduct = Convert.ToInt32(xDoc.GetElementsByTagName("spaceLogoToProduct").Item(0).InnerText),
                    ProductDivHeight = Convert.ToInt32(xDoc.GetElementsByTagName("productDivHeight").Item(0).InnerText),
                    ProductFontSize = Convert.ToInt32(xDoc.GetElementsByTagName("productFontSize").Item(0).InnerText),
                    ProductFontStyleBolt = Convert.ToBoolean(xDoc.GetElementsByTagName("productFontStyleBolt").Item(0).InnerText),
                    ProductFontStyleItalic = Convert.ToBoolean(xDoc.GetElementsByTagName("productFontStyleItalic").Item(0).InnerText),
                    ClientDivHeight = Convert.ToInt32(xDoc.GetElementsByTagName("clientDivHeight").Item(0).InnerText),
                    ClientFontSize = Convert.ToInt32(xDoc.GetElementsByTagName("clientFontSize").Item(0).InnerText),
                    ClientFontStyleBolt = Convert.ToBoolean(xDoc.GetElementsByTagName("clientFontStyleBolt").Item(0).InnerText),
                    ClientFontStyleItalic = Convert.ToBoolean(xDoc.GetElementsByTagName("clientFontStyleItalic").Item(0).InnerText),
                    PrinterName = xDoc.GetElementsByTagName("printerName").Item(0).InnerText,
                    AutoPrint = Convert.ToBoolean(xDoc.GetElementsByTagName("autoPrint").Item(0).InnerText),
                    ProductCategories = xDoc.GetElementsByTagName("productCategories").Item(0).InnerText,
                };
            }
            else
            {
                var config = new ModelConfig()
                {
                    StikerHeight = 50,
                    StikerWidth = 50,
                    SpaceUpToLogo = 0,
                    LogoHeight = 50,
                    LogoWidth = 10,
                    SpaceLogoToProduct = 0,
                    ProductDivHeight = 20,
                    ProductFontSize = 8,
                    ProductFontStyleBolt = false,
                    ProductFontStyleItalic = false,
                    ClientDivHeight = 20,
                    ClientFontSize = 8,
                    ClientFontStyleBolt = false,
                    ClientFontStyleItalic = false,
                    PrinterName = "",
                    AutoPrint = false,
                    ProductCategories = ""
                };
                return config;
            }
        }

        /// <summary>  
        ///  Запись данных в конфиг
        /// </summary> 
        public static void SetConfig(
            int stikerHeight,
            int stikerWidth,
            int spaceUpToLogo,
            int logoHeight,
            int logoWidth,
            int spaceLogoToProduct,
            int productDivHeight,
            int productFontSize,
            bool productFontStyleBolt,
            bool productFontStyleItalic,
            int clientDivHeight,
            int clientFontSize,
            bool clientFontStyleBolt,
            bool clientFontStyleItalic,
            string printerName,
            bool autoPrint,
            string productCategories
            )
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_pathXml);

            xDoc.GetElementsByTagName("stikerHeight").Item(0).InnerText = stikerHeight.ToString();
            xDoc.GetElementsByTagName("stikerWidth").Item(0).InnerText = stikerWidth.ToString();
            xDoc.GetElementsByTagName("spaceUpToLogo").Item(0).InnerText = spaceUpToLogo.ToString();
            xDoc.GetElementsByTagName("logoHeight").Item(0).InnerText = logoHeight.ToString();
            xDoc.GetElementsByTagName("logoWidth").Item(0).InnerText = logoWidth.ToString();
            xDoc.GetElementsByTagName("spaceLogoToProduct").Item(0).InnerText = spaceLogoToProduct.ToString();
            xDoc.GetElementsByTagName("productDivHeight").Item(0).InnerText = productDivHeight.ToString();
            xDoc.GetElementsByTagName("productFontSize").Item(0).InnerText = productFontSize.ToString();
            xDoc.GetElementsByTagName("productFontStyleBolt").Item(0).InnerText = productFontStyleBolt.ToString();
            xDoc.GetElementsByTagName("productFontStyleItalic").Item(0).InnerText = productFontStyleItalic.ToString();
            xDoc.GetElementsByTagName("clientDivHeight").Item(0).InnerText = clientDivHeight.ToString();
            xDoc.GetElementsByTagName("clientFontSize").Item(0).InnerText = clientFontSize.ToString();
            xDoc.GetElementsByTagName("clientFontStyleBolt").Item(0).InnerText = clientFontStyleBolt.ToString();
            xDoc.GetElementsByTagName("clientFontStyleItalic").Item(0).InnerText = clientFontStyleItalic.ToString();
            xDoc.GetElementsByTagName("printerName").Item(0).InnerText = printerName.ToString();
            xDoc.GetElementsByTagName("autoPrint").Item(0).InnerText = autoPrint.ToString();
            xDoc.GetElementsByTagName("productCategories").Item(0).InnerText = productCategories.ToString();

            xDoc.Save(_pathXml);
            SetParametrs(stikerHeight, stikerWidth, spaceUpToLogo, logoHeight, logoWidth, spaceLogoToProduct, productDivHeight, productFontSize, productFontStyleBolt, productFontStyleItalic, clientDivHeight, clientFontSize, clientFontStyleBolt, clientFontStyleItalic, printerName, autoPrint, productCategories);
        }

        /// <summary>  
        ///  Запись данных в параметры плагина
        /// </summary> 
        public static void SetParametrs(
            int stikerHeight,
            int stikerWidth,
            int spaceUpToLogo,
            int logoHeight,
            int logoWidth,
            int spaceLogoToProduct,
            int productDivHeight,
            int productFontSize,
            bool productFontStyleBolt,
            bool productFontStyleItalic,
            int clientDivHeight,
            int clientFontSize,
            bool clientFontStyleBolt,
            bool clientFontStyleItalic,
            string printerName,
            bool autoPrint,
            string productCategories)
        {
            var Parametrs = Properties.Settings.Default;

            Parametrs.StikerHeight = stikerHeight;
            Parametrs.StikerWidth = stikerWidth;
            Parametrs.SpaceUpToLogo = spaceUpToLogo;
            Parametrs.LogoHeight = logoHeight;
            Parametrs.LogoWidth = logoWidth;
            Parametrs.SpaceLogoToProduct = spaceLogoToProduct;
            Parametrs.ProductDivHeight = productDivHeight;
            Parametrs.ProductFontSize = productFontSize;
            Parametrs.ProductFontStyleBolt = productFontStyleBolt;
            Parametrs.ProductFontStyleItalic = productFontStyleItalic;
            Parametrs.ClientDivHeight = clientDivHeight;
            Parametrs.ClientFontSize = clientFontSize;
            Parametrs.ClientFontStyleBolt = clientFontStyleBolt;
            Parametrs.ClientFontStyleItalic = clientFontStyleItalic;
            Parametrs.PrinterName = printerName;
            Parametrs.AutoPrint = autoPrint;
            Parametrs.ProductCategories = productCategories;
        }

        /// <summary>  
        /// Обновление всех параметров плагина
        /// </summary> 
        public static void RefreshParametrs()
        {
            var fullConfig = GetConfig();
            var Parametrs = Properties.Settings.Default;

            Parametrs.StikerHeight = fullConfig.StikerHeight;
            Parametrs.StikerWidth = fullConfig.StikerWidth;
            Parametrs.SpaceUpToLogo = fullConfig.SpaceUpToLogo;
            Parametrs.LogoHeight = fullConfig.LogoHeight;
            Parametrs.LogoWidth = fullConfig.LogoWidth;
            Parametrs.SpaceLogoToProduct = fullConfig.SpaceLogoToProduct;
            Parametrs.ProductDivHeight = fullConfig.ProductDivHeight;
            Parametrs.ProductFontSize = fullConfig.ProductFontSize;
            Parametrs.ProductFontStyleBolt = fullConfig.ProductFontStyleBolt;
            Parametrs.ProductFontStyleItalic = fullConfig.ProductFontStyleItalic;
            Parametrs.ClientDivHeight = fullConfig.ClientDivHeight;
            Parametrs.ClientFontSize = fullConfig.ClientFontSize;
            Parametrs.ClientFontStyleBolt = fullConfig.ClientFontStyleBolt;
            Parametrs.ClientFontStyleItalic = fullConfig.ClientFontStyleItalic;
            Parametrs.PrinterName = fullConfig.PrinterName;
            Parametrs.AutoPrint = fullConfig.AutoPrint;
            Parametrs.ProductCategories = fullConfig.ProductCategories;
        }
    }
}
