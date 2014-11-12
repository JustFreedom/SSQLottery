using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statistic.StatisticOrgNumber;

namespace Statistic
{
    public class StatisticDetailOrderKeyDefine
    {
        public string IssueNumber;

        /// <summary>
        /// 已经处理了的页数
        /// </summary>
        public int OrgProcessedPageCount;

        public StatisticDetailOrderKeyDefine(string issueNum, int orgProcessedPageCount)
        {
            IssueNumber = issueNum;
            OrgProcessedPageCount = orgProcessedPageCount;
        }

        public void Statistic()
        {
            IStatisticDetailOrderKey statisticDetailOrderKey = StatisticFactory.StatisticDetailOrderKeyInstance();
            statisticDetailOrderKey.StatisticDetailOrderKey(IssueNumber, OrgProcessedPageCount);
        }
    }

    public class StatisticOriginalNumDefine
    {
        public string IssueNumber;

        /// <summary>
        /// 已经处理了的主键数
        /// </summary>
        public int OrgProcessedDetailKeyCount;

        public StatisticOriginalNumDefine(string issueNum, int orgProcessedDetailKeyCount)
        {
            IssueNumber = issueNum;
            OrgProcessedDetailKeyCount = orgProcessedDetailKeyCount;
        }

        public void Statistic()
        {
            IStatisticOrgNumber statisticOrgNumber = StatisticFactory.TaobaoStatisticOrgNumberInstance();
            statisticOrgNumber.StatisticOrgNumber(IssueNumber,OrgProcessedDetailKeyCount);
        }
    }
}
