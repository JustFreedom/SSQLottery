using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis
{
    public class AnalysisOrgNumberDefine
    {
          public string IssueNumber;

        /// <summary>
        /// 已经处理了的原始号码数
        /// </summary>
        public int OrgAnalysisedCount;

        public AnalysisOrgNumberDefine(string issueNum, int orgProcessedPageCount)
        {
            IssueNumber = issueNum;
            OrgAnalysisedCount = orgProcessedPageCount;
        }

        public void Analysis()
        {
            IAnalysisNumber analysisNumber = AnalysisFactory.TaobaoAnalysisNumberInstance(IssueNumber);
            analysisNumber.AnaysisNumber(OrgAnalysisedCount);
        }
    }
}
