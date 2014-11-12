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
        public List<string> StatisticOrgNumber(string issueNumber, int statisticedItemCount)
        {
            List<string> orgNumbers = new List<string>();
            BaseStatistic baseStatistic = new TaobaoBaseStatistic();
            _baseDao.CreateOriginalNumTable(issueNumber, SSQFrom.Taobao);
            string orgTableName = _baseDao.GetTableName(issueNumber, TableType.OriginalNum, GetSSQFrom());
            int processedCount = _baseDao.QueryProgress(orgTableName);
            string detailOrderKeytableName = _baseDao.GetTableName(issueNumber, TableType.DetailOrderKey, GetSSQFrom());
            int totalItemCount = _baseDao.QueryItemsCount(detailOrderKeytableName);
            if (totalItemCount <= processedCount)
                return orgNumbers;
            int pageSize = GetPageSize();
            TaobaoBaseStatistic taobaoBaseStatistic = new TaobaoBaseStatistic();
            string issueId = taobaoBaseStatistic.GetIssueId();
            
            for (int i = processedCount; i < totalItemCount;i = i + pageSize)
            {
                Stopwatch wa = new Stopwatch();
                wa.Start();
                List<string> detailOrderKeys = _baseDao.QueryDetailOrderKey(issueNumber, processedCount, pageSize);
                _baseDao.InsertOrUpdateToProgressRate(orgTableName, detailOrderKeys.Count);
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
                    int sqlMaxCount = 1000;
                    int modle = orgNumbers.Count / sqlMaxCount;
                    int complemente = orgNumbers.Count % sqlMaxCount;
                    for (int k = 0; k <= modle; k++)
                    {
                        int endIndex = k == modle ? Math.Min(sqlMaxCount, complemente) : sqlMaxCount;
                        var tempList = orgNumbers.GetRange(k * sqlMaxCount, endIndex);
                        SaveOriginalNumbers(issueNumber, tempList);
                        InsertOrUpdateProgressCount(issueNumber, tempList.Count);
                    }
                    orgNumbers.Clear();
                }
                wa.Stop();
                var s = wa.ElapsedMilliseconds;              
                
            }
            return orgNumbers;
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

        public void InsertOrUpdateProgressCount(string issueNumber, int newProgressedCount)
        {
            if(string.IsNullOrEmpty(issueNumber)|| newProgressedCount<1)
                return;
            string tableName = _baseDao.GetTableName(issueNumber, TableType.OriginalNum, SSQFrom.Taobao);
            _baseDao.InsertOrUpdateToProgressRate(tableName, newProgressedCount);
            StatisticMonitor.InvokeStatisticOriginalNumber(newProgressedCount);
        }
    }
}
