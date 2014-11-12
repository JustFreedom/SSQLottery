using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistic
{
    public interface IStatisticDetailOrderKey:IProgressCount
    {
        /// <summary>
        /// 统计详细订单的主键
        /// </summary>
        /// <param name="statisticedPageCount">已经统计了的页码</param>
        /// <returns></returns>
        List<string> StatisticDetailOrderKey(string issueNum, int statisticedPageCount);

      
    }
}
