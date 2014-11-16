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
       public static IAnalysisNumber TaobaoAnalysisNumberInstance(string issueNumber)
       {
           return new AnalysisNumber(issueNumber);
       }

       public static AnalysisInit TaobaoAnalysisInitInstance()
       {
           return new TaobaoAnalysisInit();
       }
    }
}
