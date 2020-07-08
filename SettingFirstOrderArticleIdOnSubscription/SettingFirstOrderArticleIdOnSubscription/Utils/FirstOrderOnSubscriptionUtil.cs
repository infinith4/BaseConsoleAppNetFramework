using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettingFirstOrderArticleIdOnSubscription.Models;
using SettingFirstOrderArticleIdOnSubscription.Models.EF;

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
                                             where subsc.status == status && subsc.article_id == null
                                             orderby subsc.created_at
                                             select new FirstOrderArticleModel()
                                             {
                                                 CustomerNumber = subsc.customer_number,
                                                 SubscriptionCreatedAt = subsc.created_at,
                                                 RecurringId = subsc.recurring_id
                                             };
                    return t_subscriptionList.ToList();
                }
            }
            catch(Exception ex)
            {
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
                    var firstOrderArticle = ( from um in _db.UserManage
                                              join om in _db.OrderManage on um.SessionId equals om.SessionId
                                              join cart in _db.Cart on um.SessionId equals cart.SessionId
                                              where um.customer == item.CustomerNumber
                                              orderby um.PlacedOrder descending
                                              select new FirstOrderArticleModel()
                                              {
                                                  SessionId = om.SessionId,
                                                  CustomerNumber = item.CustomerNumber,
                                                  RecurringId = item.RecurringId,
                                                  SubscriptionCreatedAt = item.SubscriptionCreatedAt,
                                                  ProductId = (int)cart.ProductId,
                                                  Placed_Order = (DateTime)um.PlacedOrder,
                                              } ).FirstOrDefault();
                    firstOrderArticleList.Add(firstOrderArticle);
                }
            }

            return firstOrderArticleList;
        }

        internal static bool UpdateSubscriptionArticleId(FirstOrderArticleModel firstOrderArticle)
        {
            using (var _db = new crtEntities())
            {
                var t_subscription = _db.t_subscription.SingleOrDefault(
                    c => c.customer_number == firstOrderArticle.CustomerNumber && c.recurring_id == firstOrderArticle.RecurringId);
                t_subscription.article_id = firstOrderArticle.ProductId;

                return _db.SaveChanges() > 0;
            }
        }
    }
}
