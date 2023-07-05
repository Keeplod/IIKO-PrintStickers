using PrintStickers.Models;
using PrintStickers.Windows;
using Newtonsoft.Json;
using Resto.Front.Api;
using Resto.Front.Api.Data.Orders;
using System;
using System.Reactive.Disposables;

namespace PrintStickers
{
    using static PluginContext;

    class IikoButtons : IDisposable
    {
        private readonly CompositeDisposable subscriptions;
        private const string IikoIcon = "M247 2553 c-4 -3 -7 -477 -7 -1053 0 -800 3 -1049 12 -1058 17 -17\r\n2389 -17 2406 0 9 9 12 258 12 1060 0 948 -2 1049 -16 1055 -11 4 -38 -17 -88\r\n-66 l-72 -71 -59 60 c-33 33 -64 60 -70 60 -6 0 -37 -27 -70 -60 l-59 -60 -61\r\n60 c-33 33 -65 60 -71 60 -5 0 -36 -27 -69 -60 l-59 -60 -61 60 c-33 33 -65\r\n60 -71 60 -5 0 -36 -27 -69 -60 l-59 -60 -61 60 c-33 33 -65 60 -71 60 -5 0\r\n-36 -27 -69 -60 l-59 -60 -61 60 c-33 33 -65 60 -71 60 -5 0 -36 -27 -69 -60\r\nl-59 -60 -61 60 c-33 33 -65 60 -70 60 -5 0 -37 -27 -70 -60 l-61 -60 -59 60\r\nc-33 33 -64 60 -70 60 -6 0 -37 -27 -70 -60 l-59 -60 -61 60 c-33 33 -65 60\r\n-71 60 -5 0 -36 -27 -69 -60 l-59 -60 -71 70 c-68 67 -84 78 -98 63z m429\r\n-193 c5 0 36 27 69 60 l59 60 61 -60 c33 -33 65 -60 70 -60 5 0 37 27 70 60\r\nl61 60 59 -60 c33 -33 64 -60 69 -60 6 0 38 27 71 60 l61 60 59 -60 c33 -33\r\n64 -60 69 -60 6 0 38 27 71 60 l61 60 59 -60 c33 -33 64 -60 70 -60 6 0 37 27\r\n70 60 l59 60 61 -60 c33 -33 65 -60 70 -60 5 0 37 27 70 60 l61 60 59 -60 c33\r\n-33 64 -60 69 -60 6 0 38 27 71 60 l61 60 59 -60 c33 -33 64 -60 69 -60 6 0\r\n36 26 68 57 l58 57 0 -997 0 -997 -40 0 c-37 0 -40 2 -40 27 0 44 -38 109 -82\r\n139 l-41 29 -962 0 -962 0 -41 -29 c-44 -30 -82 -95 -82 -139 0 -25 -3 -27\r\n-40 -27 l-40 0 0 997 0 997 58 -57 c32 -31 62 -57 68 -57 5 0 36 27 69 60 l59\r\n60 61 -60 c33 -33 65 -60 71 -60z m1732 -1744 c42 -22 72 -60 79 -100 l6 -36\r\n-1038 0 -1038 0 6 36 c7 41 27 68 72 95 29 18 72 19 958 19 778 0 932 -2 955\r\n-14z M485 2136 c-45 -19 -93 -72 -105 -114 -8 -25 -10 -189 -8 -498 l3\r\n-460 24 -36 c13 -19 40 -46 59 -59 l36 -24 961 0 961 0 36 24 c19 13 46 40 59\r\n59 l24 36 3 469 c3 457 2 471 -18 512 -12 25 -38 55 -62 71 l-41 29 -951 2\r\nc-773 2 -957 0 -981 -11z m1923 -50 c15 -8 38 -26 52 -41 l25 -27 3 -461 c2\r\n-447 2 -463 -18 -495 -11 -18 -34 -41 -52 -52 -32 -20 -53 -20 -963 -20 -910\r\n0 -931 0 -963 20 -18 11 -41 34 -52 52 -20 32 -20 48 -18 494 3 446 4 463 23\r\n484 11 13 34 31 50 41 29 18 72 19 958 19 778 0 932 -2 955 -14z M984 1723 c-48 -81 -125 -212 -171 -291 -82 -137 -95 -178 -57 -170\r\n19 4 355 566 351 588 -1 8 -10 16 -19 18 -12 2 -40 -37 -104 -145z M1334 1703 c-48 -81 -125 -212 -171 -291 -82 -137 -95 -178 -57 -170\r\n19 4 355 566 351 588 -1 8 -10 16 -19 18 -12 2 -40 -37 -104 -145z M1684 1703 c-48 -81 -125 -212 -171 -291 -82 -137 -95 -178 -57 -170\r\n19 4 355 566 351 588 -1 8 -10 16 -19 18 -12 2 -40 -37 -104 -145z M2034 1683 c-48 -81 -125 -212 -171 -291 -82 -137 -95 -178 -57 -170\r\n19 4 355 566 351 588 -1 8 -10 16 -19 18 -12 2 -40 -37 -104 -145z";
        public static int count = 10;

        public IikoButtons()
        {
            subscriptions = new CompositeDisposable
            {
                Operations.AddButtonToOrderEditScreen("Стикеры", x => ButtonPrintStickers(x.order), IikoIcon),
                Operations.AddButtonToClosedOrderScreen("Стикеры", x => ButtonPrintStickers(x.closedOrder), IikoIcon),
                Operations.AddButtonToPluginsMenu("Настройка стикеров", x => ButtonSettingPrintSticker()),
            };
        }

        private static void ButtonPrintStickers(IOrder order)
        {
            var JsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            ModelIikoOrder iikoOrder = JsonConvert.DeserializeObject<ModelIikoOrder>(JsonConvert.SerializeObject(order, JsonSettings));

            try
            {
                new OpenWindowPrintStickers(iikoOrder);
            }
            catch (Exception ex)
            {
                PluginContext.Operations.AddErrorMessage("[ButtonToOrderEditScreen]: " + ex.Message, "PrintStickers", TimeSpan.FromSeconds(10));
                PluginContext.Log.Error("[ButtonToOrderEditScreen]: " + ex.Message);
            }
        }

        private static void ButtonSettingPrintSticker()
        {
            try
            {
                new OpenWindowSettingsPrintStickers();
            }
            catch (Exception ex)
            {
                PluginContext.Operations.AddErrorMessage("[ButtonSettingPrintSticker]: " + ex.Message, "PrintStickers", TimeSpan.FromSeconds(10));
                PluginContext.Log.Error("[ButtonSettingPrintSticker]: " + ex.Message);
            }
        }

        public void Dispose()
        {
            subscriptions.Dispose();
        }
    }
}
