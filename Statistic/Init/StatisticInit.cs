using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSQDao;

namespace Statistic.Init
{
    public abstract class StatisticInit 
    {
        IBaseDao _baseDao = DaoFactory.NewBaseDaoInstance();
        public abstract SSQFrom GetSSQFrom();

        public int GetProgressCount(string tableName)
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

        /// <summary>
        /// 获取原始号码进度数
        /// </summary>
        /// <param name="issueNumber"></param>
        /// <returns></returns>
        public int GetOriginalProgressCount(string issueNumber)
        {
            string tableName = _baseDao.GetTableName(issueNumber, TableType.OriginalNum, GetSSQFrom());
            int count = GetProgressCount(tableName);
            return count;
        }

        public void InitDataBaseAndTable()
        {
            _baseDao.Init();
        }
        
        public int GetDetailOrderKeyProgressCount(string issueNumber)
        {
            string tableName = _baseDao.GetTableName(issueNumber,TableType.DetailOrderKey, GetSSQFrom());
            int count = GetProgressCount(tableName);
            return count;
        }

        public int GetOrgItemsCount(string issueNumber)
        {
            string tableName = _baseDao.GetTableName(issueNumber, TableType.OriginalNum, GetSSQFrom());
            int count = _baseDao.QueryItemsCount(tableName);
            return count;
        }

    }

    public class TaobaoStatisticInit : StatisticInit
    {
        public override SSQFrom GetSSQFrom()
        {
            return SSQFrom.Taobao;
        }
    }

    public class Qihu360StaticInit : StatisticInit
    {
        public override SSQFrom GetSSQFrom()
        {
            return SSQFrom.QiHu360;
        }
    }

    public class BaiduStaticInit : StatisticInit
    {
        public override SSQFrom GetSSQFrom()
        {
            return SSQFrom.Baidu;
        }
    }
}
