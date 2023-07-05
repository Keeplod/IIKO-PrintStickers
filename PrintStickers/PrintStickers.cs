using Resto.Front.Api.Attributes.JetBrains;
using Resto.Front.Api.Attributes;
using Resto.Front.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStickers
{
    [UsedImplicitly]
    [PluginLicenseModuleId(21016318)]
    public class PrintStickers : IFrontPlugin
    {
        private static readonly Stack<IDisposable> _subscriptions = new Stack<IDisposable>();
        public PrintStickers()
        {
            // Приветсвие пользователя
            PluginContext.Operations.AddNotificationMessage("[Print Stickers]: Started", "Print Stickers", TimeSpan.FromSeconds(10));

            // Создание конфига
            if (!Config.CheckConfig())
                Config.CreateXML();

            // Обновление параметров
            Config.RefreshParametrs();

            // Инициализирование кнопок в доп меню iiko
            _subscriptions.Push(new IikoButtons());

            // Инициализирование подписки на события iiko
            _subscriptions.Push(new Subscribe());
        }

        public void Dispose()
        {
            while (_subscriptions.Any())
            {
                var subscription = _subscriptions.Pop();
                try
                {
                    subscription.Dispose();
                }
                catch (Exception ex)
                {
                    PluginContext.Log.Error("[PrintStickers]: " + "Subscribe.Dispose: " + ex.Message);
                }
            }
            PluginContext.Log.Error("[PrintStickers]: PrintStickers stopped");
        }
    }
}
