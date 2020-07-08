using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingFirstOrderArticleIdOnSubscription.Models
{
    public class FirstOrderArticleModel
    {
        public int CustomerNumber { get; set; }
        public string RecurringId { get; set; }
        public DateTime? SubscriptionCreatedAt { get; set; }
        public string SessionId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? Placed_Order { get; set; }
    }
}
