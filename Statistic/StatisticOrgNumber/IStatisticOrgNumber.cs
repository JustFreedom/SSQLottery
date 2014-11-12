using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistic.StatisticOrgNumber
{
    public interface IStatisticOrgNumber : ISSQFrom, IProgressCount
    {
        /// <summary>
        /// 统计原始号码
        /// </summary>
        /// <param name="statisticedItemCount">已经统计了的页码</param>
        /// <returns></returns>
        List<string> StatisticOrgNumber(string issueNum, int statisticedItemCount);
        /// <summary>
        /// 每页的个数
        /// </summary>
        /// <returns></returns>
        int GetPageSize();

        /// <summary>
        /// 总的详细订单主键个数
        /// <remarks>数据库中的个数，而不是网页上的个数</remarks>
        /// </summary>
        /// <returns></returns>
        int GetTotalOrderKey(string issueNumber);
    }
}
