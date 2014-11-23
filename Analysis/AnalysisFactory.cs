using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analysis.Init;

namespace Analysis
{
   public class AnalysisFactory
   {
       /// <summary>
       /// 淘宝分析实例
       /// <remarks>多线程必须是一个实例，才能在执行分析的时候，在查询时锁住</remarks>
       /// </summary>
       private static IAnalysisNumber _taobaoAnalysisNumberInstance;

       public static IAnalysisNumber TaobaoAnalysisNumberInstance(string issueNumber)
       {
           return _taobaoAnalysisNumberInstance ?? (_taobaoAnalysisNumberInstance = new AnalysisNumber(issueNumber));
       }

       public static AnalysisInit TaobaoAnalysisInitInstance()
       {
           return new TaobaoAnalysisInit();
       }
    }
}
