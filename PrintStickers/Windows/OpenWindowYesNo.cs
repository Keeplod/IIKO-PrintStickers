using Resto.Front.Api;
using System;
using System.Threading;

namespace PrintStickers.Windows
{
    public class OpenWindowYesNo
    {
        private bool _result = false;
        private string _title;
        private string _message;

        public OpenWindowYesNo(string message = null, string title = "Print Stikers")
        {
            _title = title;
            _message = message;
        }

        public Boolean Result()
        {
            var windowThread = new Thread(StartWindowYesNo);
            windowThread.SetApartmentState(ApartmentState.STA);
            windowThread.Start();
            windowThread.Join();

            return _result;
        }

        private void StartWindowYesNo()
        {
            try
            {
                WindowYesNo windowYesNo = new WindowYesNo(_title, _message);
                _result = (Boolean)windowYesNo.ShowDialog();
                windowYesNo.Dispatcher.InvokeShutdown();
            }
            catch (Exception ex)
            {
                _result = false;
                PluginContext.Log.Error("[StartWindowYesNo]: " + ex.Message);
                PluginContext.Operations.AddErrorMessage("[StartWindowYesNo]: " + ex.Message, "PrintStickers", TimeSpan.FromSeconds(15));
            }
        }
    }
}
