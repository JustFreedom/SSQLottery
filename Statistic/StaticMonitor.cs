using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistic
{
   public    class StatisticMonitor
    {
       public delegate void StatisticPage(int newStatisticPageCount,bool finished);
       public static event StatisticPage NewStatisticPageCount;

       public delegate void StatisticOriginalNumber(int newOriginalNumberCount,bool finished);

       public static event StatisticOriginalNumber NewStatisticOriginalNumberCount;

       public static void InvokeStatisticPage(int newStatisticPageCount, bool finished)
       {
           NewStatisticPageCount.Invoke(newStatisticPageCount, finished);
       }
       public static void InvokeStatisticOriginalNumber(int newStatisticOriginalNumberCount, bool finished)
       {
           NewStatisticOriginalNumberCount.Invoke(newStatisticOriginalNumberCount, finished);
       }
    }
}
