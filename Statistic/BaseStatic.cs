using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SSQDao;

namespace Statistic
{
   public abstract class BaseStatistic
   {
       /// <summary>
       /// 获取总的页数
       /// </summary>
       /// <returns></returns>
       public abstract int GetTotalPageCount( string responseStr = "");
       /// <summary>
       /// 获取期号
       /// </summary>
       /// <returns></returns>
       public abstract string GetCurrentIssueNumber(string responseStr = "");

       /// <summary>
       /// 获取总的记录数
       /// </summary>
       /// <returns></returns>
       public abstract int GetTotalItemsCount(string responseStr = "");

       public abstract List<string> GetOrgNumbers(string responseStr = ""); 

       public string GetResponseByUrl(string url)
       {
           if (string.IsNullOrEmpty(url)) 
               return string.Empty;
           HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
           request.Method = "POST";
           request.ContentType = "text/html;charset=GBK2312";
           request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
               ;
           WebResponse response = request.GetResponse();
           Stream dataStream = response.GetResponseStream();
           if (dataStream == null)
               return string.Empty;
           var reader = new StreamReader(dataStream);
           string responseFromServer = reader.ReadToEnd();
           reader.Close();
           response.Close();
           return responseFromServer;
       }

       /// <summary>
       /// 订单详情页面的页数
       /// </summary>
       /// <param name="responseStr"></param>
       /// <returns></returns>
       public abstract int GetDetailOrderPage_PageCount(string responseStr);
       //public abstract List<string> GetDetailOrders();
   }

   public class TaobaoBaseStatistic : BaseStatistic
    {
       public override int GetTotalPageCount(string responseStr = "")
       {
           if (string.IsNullOrEmpty(responseStr))
           {
               responseStr = GetResponseByUrl(UrlConst.UnitedList);
           }
           Regex totalPageCountReg = new Regex(RegexConst.Taobao_PageCount);
           if (responseStr.Length > 50)
               responseStr = responseStr.Substring(responseStr.Length - 50, 50);
           var pageCountStr = totalPageCountReg.Match(responseStr).Value;
           int pageCount = 0;
           int.TryParse(pageCountStr, out pageCount);
           return pageCount;
       }

       /// <summary>
       /// 获取期号。
       /// <example>2014133</example>
       /// </summary>
       /// <param name="responseStr"></param>
       /// <returns></returns>
       public override string GetCurrentIssueNumber(string responseStr = "")
       {
           if (string.IsNullOrEmpty(responseStr))
           {
               responseStr = GetResponseByUrl(UrlConst.UnitedList);
           }
           Regex totalPageCountReg = new Regex(RegexConst.Taobao_CurIssueNum);
           if (responseStr.Length > 500)
               responseStr = responseStr.Substring(responseStr.Length - 500, 500);
           var curIssueNum = totalPageCountReg.Match(responseStr).Value;
           return curIssueNum;
       }

       /// <summary>
       /// 获取期号的id。为guid
       /// </summary>
       /// <param name="responseStr"></param>
       /// <returns></returns>
       public string GetIssueId(string responseStr = "")
       {
           if (string.IsNullOrEmpty(responseStr))
           {
               responseStr = GetResponseByUrl(UrlConst.UnitedListByPage);
               Regex regex = new Regex(RegexConst.Taobao_DetailOrderKey);
               string detailOrderKey = regex.Match(responseStr, 150, 200).Value;
               responseStr =
                   GetResponseByUrl(UrlConst.DetailOrderPageUrl +
                                    UrlConst.ConstructUrlPara(UrlConst.United_id, detailOrderKey));
           }
           Regex regex1 = new Regex(RegexConst.Taobao_IssueId);
           return regex1.Match(responseStr,54700,500).Value;
       }

       public override int GetTotalItemsCount(string responseStr = "")
       {
           if (string.IsNullOrEmpty(responseStr))
           {
               responseStr = GetResponseByUrl(UrlConst.UnitedList);
           }
           Regex totalPageCountReg = new Regex(RegexConst.Taobao_TotalItemsCount);
           if (responseStr.Length > 500)
               responseStr = responseStr.Substring(responseStr.Length - 500, 500);
           var itemsCountStr = totalPageCountReg.Match(responseStr).Value;
           int itemsCount = 0;
           int.TryParse(itemsCountStr, out itemsCount);
           return itemsCount;
       }

       /// <summary>
       /// 订单详情页面的页数
       /// </summary>
       /// <param name="responseStr"></param>
       /// <returns></returns>
       public override int GetDetailOrderPage_PageCount(string responseStr )
       {
           if (string.IsNullOrEmpty(responseStr))
               return 0;
           Regex detailOrderPage_PageCountReg= new Regex(RegexConst.Taobao_DetailOrderPage_PageCount);
           string pageCountStr = detailOrderPage_PageCountReg.Match(responseStr).Value;
           int pageCount;
           int.TryParse(pageCountStr, out pageCount);
           return pageCount;
       }


       /// <summary>
       /// 获取原始号码
       /// <remarks>获取原始号码的正则可能有问题</remarks>
       /// </summary>
       /// <param name="responseStr"></param>
       /// <returns></returns>
       public override List<string> GetOrgNumbers(string responseStr = "")
       {
           if (string.IsNullOrEmpty(responseStr))
               return new List<string>();
           Regex regex = new Regex(RegexConst.OriginalNumber);
           var orgNumbersMatches = regex.Matches(responseStr);
           List<string> orgNumbers =
               (from Match orgNumbersMatch in orgNumbersMatches select orgNumbersMatch.Value.Trim()).ToList();
           return orgNumbers;
       }
    }
}
