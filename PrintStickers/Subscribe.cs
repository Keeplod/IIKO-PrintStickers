using PrintStickers.Models;
using PrintStickers.Windows;
using Newtonsoft.Json;
using Resto.Front.Api;
using Resto.Front.Api.Data.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace PrintStickers
{
    internal sealed class Subscribe : IDisposable
    {
        private Stack<IDisposable> _subscriptions = new Stack<IDisposable>();
        private int _lastOrder = 0;

        // Подписка на события iiko
        public Subscribe()
        {
            // Заказы от стола
            _subscriptions.Push(PluginContext.Notifications.OrderChanged
                .Where(x => x.Entity.OrderType == null ? true : x.Entity.OrderType.OrderServiceType == Resto.Front.Api.Data.Organization.OrderServiceTypes.Common)
                .Where(x => x.Entity.Status == OrderStatus.Bill)
                .Subscribe(x => PrintOrder(x.Entity)));
        }

        public void PrintOrder(IOrder order)
        {
            if (Properties.Settings.Default.AutoPrint)
            {
                var JsonSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                ModelIikoOrder iikoOrder = JsonConvert.DeserializeObject<ModelIikoOrder>(JsonConvert.SerializeObject(order, JsonSettings));

                if (_lastOrder != iikoOrder.Number)
                {
                    if (Properties.Settings.Default.ProductCategories == "" || CategoryEnable(iikoOrder))
                    {
                        if (new OpenWindowYesNo(message: "Распечатать этикетку?").Result())
                        {
                            new OpenWindowPrintStickers(iikoOrder);
                        }
                    }
                }

                _lastOrder = iikoOrder.Number;
            }
        }

        public bool CategoryEnable(ModelIikoOrder iikoOrder)
        {
            string[] categories = Properties.Settings.Default.ProductCategories == "" ? new string[] { } : Properties.Settings.Default.ProductCategories.Split('\u002C');

            if (categories.Any())
            {
                foreach (var item in iikoOrder.Items)
                {
                    foreach (var category in categories)
                    {
                        if (item.Product.Category.Name == category)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
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
                    PluginContext.Log.Error("[Subscribe]: " + "Subscribe.Dispose: " + ex.Message);
                }
            }
            PluginContext.Log.Error("[Subscribe]: Subscribe stopped");
        }
    }
}
