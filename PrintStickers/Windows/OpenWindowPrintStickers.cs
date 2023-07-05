using PrintStickers.Models;
using PrintStickers.Windows;
using Resto.Front.Api;
using System;
using System.Threading;

namespace PrintStickers.Windows
{
    internal class OpenWindowPrintStickers
    {
        public ModelIikoOrder _iikoOrder;
        public OpenWindowPrintStickers(ModelIikoOrder iikoOrder)
        {
            _iikoOrder = iikoOrder;
            var windowThread = new Thread(StartWindowPrintStickers);
            windowThread.SetApartmentState(ApartmentState.STA);
            windowThread.Start();
            windowThread.Join();
        }
        private void StartWindowPrintStickers()
        {
            try
            {
                WindowPrintStickers windowPrintStickers = new WindowPrintStickers(_iikoOrder);
                windowPrintStickers.ShowDialog();
                windowPrintStickers.Dispatcher.InvokeShutdown();
            }
            catch (Exception ex)
            {
                PluginContext.Operations.AddErrorMessage("[StartWindowPrintStickers]: " + ex.Message, "PrintStickers", TimeSpan.FromSeconds(30));
            }
        }
    }
}