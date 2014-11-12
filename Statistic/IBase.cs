using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSQDao;

namespace Statistic
{
    public interface ISSQFrom
    {
        SSQFrom GetSSQFrom();
    }

    public interface IProgressCount
    {
        /// <summary>
        /// 更新或者插入处理进度  
        /// </summary>
        /// <param name="issueNumber"></param>
        /// <param name="newProgressedCount"></param>
        void InsertOrUpdateProgressCount(string issueNumber, int newProgressedCount);
    }
}
