using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingFirstOrderArticleIdOnSubscription.Utils
{
    public class LogUtil
    {
        ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void ConsoleWriteLineWithInfoLog(object message)
        {
            logger.Info(message);
            Console.WriteLine(message);
        }

        public void ConsoleWriteLineWithErrorLog(object message, Exception exception)
        {
            logger.Error(message, exception);
            Console.WriteLine(message);
            if (exception != null)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
