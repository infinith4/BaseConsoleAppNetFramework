using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettingFirstOrderArticleIdOnSubscription.Models;

namespace SettingFirstOrderArticleIdOnSubscription.Utils
{
    /// <summary>
    /// 初月度注文時の注文を取得する
    /// </summary>
    internal class FirstOrderOnSubscriptionUtil
    {
        private static LogUtil _logUtil = new LogUtil();
        //初月度注文時のSessionIdのリストを取得する
        internal static List<FirstOrderArticleModel> GetFirstOrder(int status)
        {
            try
            {
                using (var _db = new crtEntities())
                {
                    //var t_subscriptionList = _db.t_subscription.Where(c => c.status == status).OrderBy(c => c.created_at)
                    //    .Select(c => new FirstOrderArticleModel()
                    //{
                    //    CustomerNumber = c.customer_number,
                    //    SubscriptionCreatedAt = c.created_at
                    //});

                    var t_subscriptionList = from subsc in _db.t_subscription
                                             where subsc.status == status
                                             orderby subsc.created_at
                                             select new FirstOrderArticleModel()
                                             {
                                                 CustomerNumber = subsc.customer_number,
                                                 SubscriptionCreatedAt = subsc.created_at
                                             };

                    return new List<FirstOrderArticleModel>();
                }
            }
            catch(Exception ex)
            {
                _logUtil.ConsoleWriteLineWithErrorLog("Exception on GetFirstOrder.", ex);
                _logUtil.ConsoleWriteLineWithErrorLog("Exception on GetFirstOrder.", ex.InnerException);
                throw;
            }
        }

        internal static List<FirstOrderArticleModel> GetFirstOrderArticleId(List<FirstOrderArticleModel> firstOrderArticleListForSubscription)
        {
            var firstOrderArticleList = new List<FirstOrderArticleModel>();
            using (var _db = new crtEntities())
            {
                foreach (var item in firstOrderArticleListForSubscription)
                {
                    var firstOrderArticle = ( from um in _db.UserManages
                                      join om in _db.OrderManages on um.SessionId equals om.SessionId
                                      join cart in _db.Carts on um.SessionId equals cart.SessionId
                                      where um.customer == item.CustomerNumber
                                      orderby um.PlacedOrder descending
                                      select new FirstOrderArticleModel()
                                      {
                                          SessionId = om.SessionId,
                                          CustomerNumber = item.CustomerNumber,
                                          SubscriptionCreatedAt = item.SubscriptionCreatedAt,
                                          ProductId = (int)cart.ProductId,
                                          Placed_Order = (DateTime)um.PlacedOrder,
                                      } ).FirstOrDefault();
                    firstOrderArticleList.Add(firstOrderArticle);

                }
            }

            return firstOrderArticleList;
        }
    }
}
