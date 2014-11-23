using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistic
{
   public    class StatisticMonitor
    {
       public delegate void StatisticPage(int newStatisticPageCount);
       public static event StatisticPage NewStatisticPageCount;

       public delegate void StatisticOriginalNumber(int newOriginalNumberCount, bool isFinished);

       public static event StatisticOriginalNumber NewStatisticOriginalNumberCount;

       public static void InvokeStatisticPage(int newStatisticPageCount)
       {
           NewStatisticPageCount.Invoke(newStatisticPageCount);
       }
       public static void InvokeStatisticOriginalNumber(int newStatisticOriginalNumberCount, bool isFinished)
       {
           NewStatisticOriginalNumberCount.Invoke(newStatisticOriginalNumberCount, isFinished);
       }
    }
}
