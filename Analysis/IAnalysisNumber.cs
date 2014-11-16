using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis
{
    public interface IAnalysisNumber
    {
      
        /// <summary>
        /// 将原始号码解析标准的双色球投注号码，并保存
        /// </summary>
        /// <param name="issueId"></param>
        /// <param name="analysisedCount">已经分析过的号码</param>
        /// <returns></returns>
        void AnaysisNumber( int analysisedCount);

        /// <summary>
        /// 获取分页大小
        /// </summary>
        /// <returns></returns>
        int GetPageSize();
    }
}
