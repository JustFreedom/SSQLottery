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

        public StatisticDetailOrderKeyDefine(string issueNum)
        {
            IssueNumber = issueNum;
        }

        public void Statistic()
        {
            IStatisticDetailOrderKey statisticDetailOrderKey = StatisticFactory.StatisticDetailOrderKeyInstance();
            statisticDetailOrderKey.StatisticDetailOrderKey(IssueNumber);
        }
    }

    public class StatisticOriginalNumDefine
    {
        public string IssueNumber;


        public StatisticOriginalNumDefine(string issueNum)
        {
            IssueNumber = issueNum;
        }

        public void Statistic()
        {
            IStatisticOrgNumber statisticOrgNumber = StatisticFactory.TaobaoStatisticOrgNumberInstance();
            statisticOrgNumber.StatisticOrgNumber(IssueNumber);
        }
    }

    #region 淘宝方案定义，便于反序列化
    [Serializable] 
    public class TaobaoSchemeDefine
    {
        public int progress;
        public string amountMoney;
        public int status;
        public string minBuy;
        public int issueId;
        public float bonusRatio;
        public int playType;
        public string lotteryTypeCode;
        public int remainCount;

        public int commision;
        public string id;
        public string detailLink;
        public string title;
        public string userLink;
        public string userId;
        public int seq;
        public int userScore;
        public string userName;
        public string bonus;
        public string unitMoney;
        public int confidential;
    }
    [Serializable] 
    public class IssueDefine
    {
        public bool onsale;
        public string Issue;
    }
    [Serializable] 
    public class TaobaoPageShcemeDefine
    {
        public List<TaobaoSchemeDefine> schemes;
        public int totalItem;
        public string lotteryType;
        public List<IssueDefine> issueList;

        public int totalPage;
        public bool sale;
    }
    #endregion
}
