using Resto.Front.Api;
using System;
using System.Threading;

namespace PrintStickers.Windows
{
    internal class OpenWindowSettingsPrintStickers
    {
        public OpenWindowSettingsPrintStickers()
        {
            var windowThread = new Thread(StartWindowSettingsPrintStickers);
            windowThread.SetApartmentState(ApartmentState.STA);
            windowThread.Start();
            windowThread.Join();
        }

        private void StartWindowSettingsPrintStickers()
        {
            try
            {
                WindowSettingsPrintStickers windowSettingsPrintStickers = new WindowSettingsPrintStickers();
                windowSettingsPrintStickers.ShowDialog();
                windowSettingsPrintStickers.Dispatcher.InvokeShutdown();
            }
            catch (Exception ex)
            {
                PluginContext.Operations.AddErrorMessage("[StartWindowSettingsPrintStickers]: " + ex.Message, "PrintStickers", TimeSpan.FromSeconds(30));
            }
        }
    }
}