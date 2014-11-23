using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statistic.Init;
using Statistic.StatisticOrgNumber;

namespace Statistic
{
   public class StatisticFactory
    {
       public static IStatisticDetailOrderKey StatisticDetailOrderKeyInstance()
       {
           return new TaobaoStatisticDetailOrderKey();
       }

       public static TaobaoBaseStatistic TaobaoBaseStatisticInstance()
       {
           return new TaobaoBaseStatistic();
       }

       public static TaobaoStatisticInit TaobaoStatisticInitInstance()
       {
           return new TaobaoStatisticInit();
       }

       private static IStatisticOrgNumber _taobaoStatisticOrgNumberInstance;
       public static IStatisticOrgNumber TaobaoStatisticOrgNumberInstance()
       {
           return _taobaoStatisticOrgNumberInstance ??
                  (_taobaoStatisticOrgNumberInstance = new TaobaoStatisticOrgNumber());
       }
    }
}
