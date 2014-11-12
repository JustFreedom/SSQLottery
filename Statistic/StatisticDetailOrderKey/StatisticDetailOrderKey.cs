using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSQDao;

namespace Statistic
{
   internal class TaobaoStatisticDetailOrderKey:IStatisticDetailOrderKey
    {
       IBaseDao _baseDao = DaoFactory.NewBaseDaoInstance();
       /// <summary>
       /// 统计订单主键
       /// </summary>
       /// <param name="statisticedPageCount">已经统计了的页数</param>
       /// <returns></returns>
       public List<string> StatisticDetailOrderKey(string issueNum,int statisticedPageCount)
       {
          
           List<string> detailOrderkeys = new List<string>();
           BaseStatistic baseStatistic = new TaobaoBaseStatistic();
           string respStr = baseStatistic.GetResponseByUrl(UrlConst.UnitedListByPage);
           int totalPageCount = baseStatistic.GetTotalPageCount(respStr);
           int notStatisticCount = totalPageCount - statisticedPageCount;
           _baseDao.CreateDetailOrderKeyTable(issueNum, SSQFrom.Taobao);
           for (int i = notStatisticCount; i >=1; i--)
           {
               respStr = baseStatistic.GetResponseByUrl(UrlConst.UnitedListByPage + i);
               Regex detailOrderKeyReg = new Regex(RegexConst.Taobao_DetailOrderKey);
               MatchCollection detailOrderKeys = detailOrderKeyReg.Matches(respStr);
               foreach (Match match in detailOrderKeys)
               {
                  detailOrderkeys.Add(match.Value);
               }
               SaveDetailOrderKeys(issueNum, detailOrderkeys);
               InsertOrUpdateProgressCount(issueNum, 1);
               detailOrderkeys.Clear();
           }
           
           //int itemsCount = 0;
           //int.TryParse(itemsCountStr, out itemsCount);
           return new List<string>();
       }

       private void SaveDetailOrderKeys(string issueNum ,IList<string> orderDetailKeys)
       {
           _baseDao.InsertDetailOrderKey(issueNum, orderDetailKeys);
       }

       public void InsertOrUpdateProgressCount(string issueNumber, int newProgressedCount)
       {
           string tableName = _baseDao.GetTableName(issueNumber, TableType.DetailOrderKey, SSQFrom.Taobao);
           _baseDao.InsertOrUpdateToProgressRate(tableName, 1);
           StatisticMonitor.InvokeStatisticPage(newProgressedCount);
       }
    }
}
