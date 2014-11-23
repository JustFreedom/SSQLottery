using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SSQDao;

namespace Statistic.StatisticOrgNumber
{
    internal class TaobaoStatisticOrgNumber : IStatisticOrgNumber
    {
        private Object lockObj = new Object();
        public void StatisticOrgNumber(string issueNumber)
        {
            List<string> orgNumbers = new List<string>();
            BaseStatistic baseStatistic = new TaobaoBaseStatistic();
            _baseDao.CreateOriginalNumTable(issueNumber, SSQFrom.Taobao);
            string orgTableName = _baseDao.GetTableName(issueNumber, TableType.OriginalNum, GetSSQFrom());
            int processedCount = _baseDao.QueryProgress(orgTableName);
            string detailOrderKeytableName = _baseDao.GetTableName(issueNumber, TableType.DetailOrderKey, GetSSQFrom());
            int totalItemCount = _baseDao.QueryItemsCount(detailOrderKeytableName);
            if (totalItemCount <= processedCount)
                return ;
            int pageSize = GetPageSize();
            TaobaoBaseStatistic taobaoBaseStatistic = new TaobaoBaseStatistic();
            string issueId = taobaoBaseStatistic.GetIssueId();


            while (totalItemCount > processedCount)
            {
                List<string> detailOrderKeys = new List<string>();
                lock (lockObj)
                {
                    totalItemCount = _baseDao.QueryItemsCount(detailOrderKeytableName);
                    processedCount = _baseDao.QueryProgress(orgTableName);
                    if (totalItemCount <= processedCount)
                        break;
                    detailOrderKeys = _baseDao.QueryDetailOrderKey(issueNumber, processedCount, pageSize);
                    _baseDao.InsertOrUpdateToProgressRate(orgTableName, detailOrderKeys.Count);

                }
              
                foreach (string orderKey in detailOrderKeys)
                {
                    string respStr =
                        baseStatistic.GetResponseByUrl(UrlConst.DetailOrderUrl +
                                                       UrlConst.ConstructUrlPara(UrlConst.Tb_United_id, orderKey) +
                                                       UrlConst.ConstructUrlPara(UrlConst.Issue_id, issueId) +
                                                       UrlConst.ConstructUrlPara(UrlConst.Page, 1.ToString()));
                    orgNumbers.AddRange(baseStatistic.GetOrgNumbers(respStr));

                    int detailPage_PageCount = baseStatistic.GetDetailOrderPage_PageCount(respStr);
                    for (int j = 1; j < detailPage_PageCount; j++)
                    {
                        respStr =
                        baseStatistic.GetResponseByUrl(UrlConst.DetailOrderUrl +
                                                       UrlConst.ConstructUrlPara(UrlConst.Tb_United_id, orderKey) +
                                                       UrlConst.ConstructUrlPara(UrlConst.Issue_id, issueId) +
                                                       UrlConst.ConstructUrlPara(UrlConst.Page, j.ToString()));
                        orgNumbers.AddRange(baseStatistic.GetOrgNumbers(respStr));
                    }
                    SaveOriginalNumbers(issueNumber, orgNumbers);
                    //InsertOrUpdateProgressCount(issueNumber, orgNumbers.Count,false);
                    StatisticMonitor.InvokeStatisticOriginalNumber(orgNumbers.Count, false);
                    orgNumbers.Clear();
                }
            }
            //通知统计完成
            StatisticMonitor.InvokeStatisticOriginalNumber(0, true);
        }

        public int GetPageSize()
        {
            return 50;
        }

        public int GetTotalOrderKey(string issueNumber)
        {
            string tableName = _baseDao.GetTableName(issueNumber, TableType.DetailOrderKey, GetSSQFrom());
            int totalItemCount =_baseDao.QueryItemsCount(tableName);
            return totalItemCount;
        }

        IBaseDao _baseDao = DaoFactory.NewBaseDaoInstance();
        
       private void SaveOriginalNumbers(string issueNum, IList<string> originalNumbers)
       {
           _baseDao.InsertOriginalNum(issueNum, originalNumbers);
       }

        public SSQFrom GetSSQFrom()
        {
            return SSQFrom.Taobao;
        }

        public void InsertOrUpdateProgressCount(string issueNumber, int newProgressedCount, bool isFinished = false)
        {
            if(string.IsNullOrEmpty(issueNumber)|| newProgressedCount<1)
                return;
            string tableName = _baseDao.GetTableName(issueNumber, TableType.OriginalNum, SSQFrom.Taobao);
            _baseDao.InsertOrUpdateToProgressRate(tableName, newProgressedCount);
            StatisticMonitor.InvokeStatisticOriginalNumber(newProgressedCount, isFinished);
        }

        public void InsertOrUpdateProgressCount(string issueNumber, int newProgressedCount)
        {
            throw new NotImplementedException();
        }
    }
}
