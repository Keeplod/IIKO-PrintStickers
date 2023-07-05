using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Image = System.Drawing.Image;

namespace PrintStickers
{
    internal class Print
    {
        public static void PrintDoc(
            string stikerHeight,
            string stikerWidth,

            int spaceUpToLogo,
            int logoHeight,
            int logoWidth,
            int spaceLogotoProduct,

            int productDivHeight,
            int productFontSize,
            bool productFontStyleBolt,
            bool productFontStyleItalic,

            int clientDivHeight,
            int clientFontSize,
            bool clientFontStyleBolt,
            bool clientFontStyleItalic,

            string printerName,
            string pathImageLogo,

            string textProduct = "",
            string textClient = "")
        {
            if (stikerHeight == "")
            {
                stikerHeight = "5.0";
            }
            if (stikerWidth == "")
            {
                stikerWidth = "2.5";
            }

            // преобразование указанных миллиметров в псевдо сотые дюйма (это не так, но документация говорит обратное :) )
            int widthDouble = Convert.ToInt32(Math.Round(((Convert.ToDouble(stikerWidth.Replace(".", ",")) - 2.6) / 0.256) + 20));
            if (widthDouble < 20)
                widthDouble = 20;

            int heightDouble = Convert.ToInt32(Math.Round((Convert.ToDouble(stikerHeight.Replace(".", ",")) - 5.0) / 0.256) + 20);
            if (heightDouble < 20)
                heightDouble = 20;

            // Получение логотипа
            Image newImage = Image.FromFile(pathImageLogo);

            // создание документа печати
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPageHandler;
            printDocument.DocumentName = "PrintStickers";
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("First custom size", widthDouble, heightDouble);
            printDocument.PrinterSettings.PrinterName = printerName;
            printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); ;

            // диалог настройки печати
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;
            printDialog.Document.Print(); 

            // создание страницы документа
            void PrintPageHandler(object sender, PrintPageEventArgs e)
            {
                e.Graphics.DrawImage(newImage, (Convert.ToInt32(widthDouble - (logoWidth * 4))) / 2, Convert.ToInt32(spaceUpToLogo * 4.16), logoWidth * 4, logoHeight * 4);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;

                Rectangle DivProduct = new Rectangle(
                        0,
                        Convert.ToInt32((logoHeight * 4) + (spaceLogotoProduct * 4.16) + (spaceUpToLogo * 4.16)),
                        Convert.ToInt32(Convert.ToDouble(stikerWidth) * 4),
                        Convert.ToInt32(productDivHeight * 3.5));
                var fontProduct = new Font("Arial", Convert.ToInt32(productFontSize * 2.5), (productFontStyleBolt ? FontStyle.Bold : FontStyle.Regular) | (productFontStyleItalic ? FontStyle.Italic : FontStyle.Regular));
                e.Graphics.DrawString(textProduct, fontProduct, Brushes.Black, DivProduct, stringFormat);

                Rectangle DivClient = new Rectangle(
                    0, 
                    Convert.ToInt32(logoHeight * 4 + productDivHeight * 4 + spaceLogotoProduct * 4.16 + (spaceUpToLogo * 4.16)),
                    Convert.ToInt32(Convert.ToDouble(stikerWidth) * 4),
                    Convert.ToInt32(clientDivHeight * 3.5));
                var fontClientName = new Font("Arial", Convert.ToInt32(clientFontSize * 2.5), (clientFontStyleBolt ? FontStyle.Bold : FontStyle.Regular) | (clientFontStyleItalic ? FontStyle.Italic : FontStyle.Regular));
                e.Graphics.DrawString(textClient, fontClientName, Brushes.Black, DivClient, stringFormat);
            }
        }
    }
}
