using SettingFirstOrderArticleIdOnSubscription.Utils;
using log4net;
using System;
using System.IO;

namespace SettingFirstOrderArticleIdOnSubscription
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //UnhandledExceptionイベントハンドラを登録
            System.Threading.Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(Application_UnhandledException);

            var logUtil = new LogUtil();
            if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                //すでに起動していると判断する
                logUtil.ConsoleWriteLineWithErrorLog("二重起動されました。", new Exception("Multi Executed Application"));
            }
            else
            {
                try
                {
                    logUtil.ConsoleWriteLineWithInfoLog("Start App.");
                    #region Write Your Program
                    //Phase5 の対応のためアルバムあり・なしを判別するためt_subscription.article_id に初月度注文時のArticleId(Cartテーブル)を設定する
                    var firstOrderList = FirstOrderOnSubscriptionUtil.GetFirstOrder((int)Constants.Subscription.Status.UnderContract);
                    var firstOrderArticleIdList = FirstOrderOnSubscriptionUtil.GetFirstOrderArticleId(firstOrderList);
                    foreach (var item in firstOrderArticleIdList)
                    {
                        logUtil.ConsoleWriteLineWithInfoLog($"{item.CustomerNumber};{item.RecurringId};{item.SubscriptionCreatedAt};{item.SessionId};{item.ProductId};{item.Placed_Order};");
                        FirstOrderOnSubscriptionUtil.UpdateSubscriptionArticleId(item);
                    }

                    #endregion
                    logUtil.ConsoleWriteLineWithInfoLog("End App.");
                }
                catch (Exception ex)
                {
                    logUtil.ConsoleWriteLineWithErrorLog("Failed SettingFirstOrderArticleIdOnSubscription", ex);
                }
            }
        }

        private static void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //UIスレッド以外での予期しない例外発生時にプロセスを落とす（これをやっておかないとWindowsのアラートが出てプロセスが生きたままになってしまい死活監視での再起動ができない）
            try
            {
                var logUtil = new LogUtil();
                logUtil.ConsoleWriteLineWithErrorLog("Failed CollectLogs.Application_UnhandledException: ", (Exception)e.ExceptionObject);
            }
            finally
            {
                Environment.Exit(1);
            }
        }
    }
}
