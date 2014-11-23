using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
       public List<string> StatisticDetailOrderKey(string issueNum)
       {
           int statisticedPageCount = GetStatisticedPageCount(issueNum);
           List<string> detailOrderkeys = new List<string>();
           BaseStatistic baseStatistic = new TaobaoBaseStatistic();
           string respStr = baseStatistic.GetResponseByUrl(UrlConst.UnitedListByPage);
           int totalPageCount = baseStatistic.GetTotalPageCount(respStr);
           _baseDao.CreateDetailOrderKeyTable(issueNum, SSQFrom.Taobao);
           var serializer = new JavaScriptSerializer();

           if (totalPageCount > 0 && totalPageCount >= statisticedPageCount)
           {
               respStr = baseStatistic.GetResponseByUrl(UrlConst.UnitedListByPage + statisticedPageCount);
               string tableName = _baseDao.GetTableName(issueNum, TableType.DetailOrderKey, SSQFrom.Taobao);
               int statisticedOrderKeyCount = _baseDao.QueryItemsCount(tableName);
               TaobaoPageShcemeDefine shcemeDefine = serializer.Deserialize<TaobaoPageShcemeDefine>(respStr);
               int notStatisticCountInPage = shcemeDefine.schemes.Count - statisticedOrderKeyCount;
               for (int i = 0; i < notStatisticCountInPage; i++)
               {
                   detailOrderkeys.Add(shcemeDefine.schemes[i].id);
               }
               SaveDetailOrderKeys(issueNum, detailOrderkeys);
               InsertOrUpdateProgressCount(issueNum, 1);
               detailOrderkeys.Clear();
               return new List<string>();
           }
           for (int i = statisticedPageCount + 1; i <= totalPageCount; i++)
           {
               respStr = baseStatistic.GetResponseByUrl(UrlConst.UnitedListByPage + i);
               Regex detailOrderKeyReg = new Regex(RegexConst.Taobao_DetailOrderKey);
               MatchCollection detailOrderKeys = detailOrderKeyReg.Matches(respStr);
            
               foreach (Match match in detailOrderKeys)
               {
                  detailOrderkeys.Add(match.Value);
               }
               if (detailOrderKeys.Count < 1)
                   break;
               SaveDetailOrderKeys(issueNum, detailOrderkeys);
               InsertOrUpdateProgressCount(issueNum, 1);
               detailOrderkeys.Clear();
           }
           
           //int itemsCount = 0;
           //int.TryParse(itemsCountStr, out itemsCount);
           return new List<string>();
       }

       private List<string> GetNotStatisticOrderKeyInLastpage(int lastPageIndex)
       {
           respStr = baseStatistic.GetResponseByUrl(UrlConst.UnitedListByPage + lastPageIndex);
           string tableName = _baseDao.GetTableName(issueNum, TableType.DetailOrderKey, SSQFrom.Taobao);
           int statisticedOrderKeyCount = _baseDao.QueryItemsCount(tableName);
           TaobaoPageShcemeDefine shcemeDefine = serializer.Deserialize<TaobaoPageShcemeDefine>(respStr);
           int notStatisticCountInPage = shcemeDefine.totalItem - statisticedOrderKeyCount;
           for (int i = 0; i < notStatisticCountInPage; i++)
           {
               detailOrderkeys.Add(shcemeDefine.schemes[i].id);
           }
           SaveDetailOrderKeys(issueNum, detailOrderkeys);
           InsertOrUpdateProgressCount(issueNum, 1);
           detailOrderkeys.Clear();
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

       /// <summary>
       /// 获取原始号码进度数
       /// </summary>
       /// <param name="issueNumber"></param>
       /// <returns></returns>
       private int GetStatisticedPageCount(string issueNumber)
       {
           string tableName = _baseDao.GetTableName(issueNumber, TableType.OriginalNum, SSQFrom.Taobao);
           int count = _baseDao.QueryProgress(tableName);
           return count;
       }

    }
}
