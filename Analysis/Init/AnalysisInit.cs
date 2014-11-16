using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSQDao;

namespace Analysis.Init
{
    public abstract class AnalysisInit : IBase
    {
        IBaseDao _baseDao = DaoFactory.NewBaseDaoInstance();
        public abstract SSQFrom GetSSQFrom();
        public int GetAnalysisProcessedCount(string issueNumber)
        {
            string tableName = _baseDao.GetTableName(issueNumber, TableType.AnalyResult, GetSSQFrom());
            int count = QueryProcessCount(tableName);
            return count;
        }

        public int GetAnalysisResultItemsCount(string issueNumber)
        {
            string tableName = _baseDao.GetTableName(issueNumber, TableType.AnalyResult, GetSSQFrom());
            int count = QueryItemCount(tableName);
            return count;
        }

        public int QueryProcessCount(string tableName)
        {
            int count = 0;
            try
            {
                count = _baseDao.QueryProgress(tableName);
            }
            catch (Exception)
            {
            }
            return count;
        }

        public int QueryItemCount(string tableName)
        {
            int count = 0;
            try
            {
                count = _baseDao.QueryItemsCount(tableName);
            }
            catch (Exception)
            {
            }
            return count;
        }
    }

    public class TaobaoAnalysisInit : AnalysisInit
    {
        public override SSQFrom GetSSQFrom()
        {
            return SSQFrom.Taobao;
        }
    }
}
