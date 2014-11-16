using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SSQDao;

namespace Analysis
{
   public abstract class Analysis
   {
       //public abstract SSQFrom GetSSQFrom();
       //public abstract int GetTotalPageCount();
       //private readonly char SplitChar = ':';
       /// <summary>
       /// 分析原始号码
       /// </summary>
       /// <param name="orgNumbers">原始号码，如01 02 03 04 05 06:07；或者(01 02 04 05 06)07 08:03</param>
       /// <returns>标准的彩票号码，6红1蓝</returns>
       public List<SSQNumberDefine> Anaysis(IList<string> orgNumbers)
       {
           List<SSQNumberDefine> result = new List<SSQNumberDefine>();
           foreach (string orgNumber in orgNumbers)
           {
               var redAndBlueNumbers = orgNumber.Split(Const.Colon);
               if(redAndBlueNumbers.Length < 2)
                   continue;
               string redNumbers = redAndBlueNumbers[0];
               List<string> redCombines = GetRedNumbers(redNumbers);
               string[] blueNumbers = redAndBlueNumbers[1].Split(Const.WhiteSpace);
               foreach (string redCombine in redCombines)
               {
                   foreach (string blueNumber in blueNumbers)
                   {
                       result.Add(new SSQNumberDefine(redCombine, blueNumber));
                   }
               }
           }
           return result;
       }

       private List<string> GetRedNumbers(string orgRedNumbers)
       {
           List<string> redCombines = new List<string>();
           Regex regex = new Regex(RegularConst.FixedNumber);
           Match match = regex.Match(orgRedNumbers);
           string fixedNumStr = string.Empty;
           Combines combines = new Combines();
           string[] redNumbers;
           int fixedCount = 0; //定胆个数
           if (match.Success)
           {
               fixedNumStr = match.Value;
               orgRedNumbers = regex.Replace(orgRedNumbers, string.Empty);
               var fixedNumbers = fixedNumStr.Trim(new char[2] {'(', ')'}).Split(' ');
               redNumbers = orgRedNumbers.Trim().Split(' ');
               List<string> unFixedRedNumberCombines = combines.GetCombines(redNumbers, 6 - fixedNumbers.Length);
               foreach (string unFixedRedNumberCombine in unFixedRedNumberCombines)
               {
                   redCombines.Add(string.Format("{0} {1}", string.Join(" ", fixedNumbers), unFixedRedNumberCombine));
               }
               return redCombines;
           }
           redNumbers = orgRedNumbers.Trim().Split(' ');
           redCombines = combines.GetCombines(redNumbers, 6);
           return redCombines;
       }
      
   }

    public class TaobaoAnalysis : Analysis
    {

    }
}
