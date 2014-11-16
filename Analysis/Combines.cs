using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis
{
   public class Combines
    {

        /// <summary>
        /// 获取全组合
        /// </summary>
        /// <remarks>算法思路：
        /// 1、先将声明一个长度为<para>numberList</para>的数组，将前<para>combinationCount</para>置为1，其余的置为0<br/>
        /// 2、循环<c>i < n - combinationCount</c>次:如果<c>i=n - combinationCount</c>，则证明<c>i</c>后面的全是1<br/>
        /// 3、如果遇到“10”，则记录组合，并将“10”置为“10”：即<c>flag[i] == 1 && flag[i+1] == 0</c>;左边“1”出现的次数（countOfLeftOnet）加1<br/>
        /// 4、将<c>i</c>左边的1全部移到最左端<br/>
        /// 4、将countOfLeftOnet减一赋值给countOfLeftOnet，并将countOfLeftOnet赋值给i：前面肯定有<c>countOfLeftOnet-1</c>个1,<br/>
        /// 因为刚刚将<c>countOfLeftOnet</c>个1向左移动了，下次开始匹配的时候，就从<c>countOfLeftOnet-1</c>开始，出现的次数也相应滴减1：<br/>
        /// 如上次匹配后为“11110",<c>i</c>为3，<c>countOfLeftOnet</c>为3，那么匹配将“10”变为“01”后，是“11101”，左移后为“11101”，<br/>
        /// 则下次开始的index就是3-1，从111开始<br/>
        /// </remarks>
        /// <param name="numberList">需要进行组合的数组</param>
        /// <param name="combinationCount">取的个数（n中的m）</param>
        /// <returns>组合的字符串</returns>
        public List<string> GetCombines(IList<string> numberList, int combinationCount)
        {
            List<string> combines = new List<string>();
            int n = numberList.Count;
            if (combinationCount > n)
                throw new ArgumentException("组合n取m中，m比n大！参数不合法");
            byte selectFlag = 1;
            byte unSelectFlag = 0;
            var flagsList = InitFlags(combinationCount, selectFlag);
            flagsList.AddRange(InitFlags(n - combinationCount, unSelectFlag));
            int countOfLeftOne = 0;
            for (int i = 0; i < n - 1; )
            {
                if (flagsList[i] == 1)
                {

                    if (flagsList[i + 1] == 0)
                    {
                        combines.Add(GetCombineItem(numberList, flagsList, i - countOfLeftOne, combinationCount, selectFlag));
                        flagsList[i] = 0;
                        flagsList[i + 1] = 1;
                        for (int j = 0; j < i; j++)
                        {
                            flagsList[j] = j < countOfLeftOne ? selectFlag : unSelectFlag;
                        }
                        countOfLeftOne = countOfLeftOne - 1 < 0 ? 0 : countOfLeftOne - 1;
                        i = countOfLeftOne;
                        continue;
                    }
                    countOfLeftOne++;
                }
                i++;
            }
            //combinationCount - 1 ,由于会匹配i+1，所以最后一个1一定匹配不到
            if (countOfLeftOne == combinationCount - 1)
                combines.Add(GetCombineItem(numberList, flagsList, n -combinationCount, combinationCount, selectFlag));
            return combines;
        }

        private List<T> InitFlags<T>(int count, T flag)
        {
            var flags = new List<T>(count);
            for (var i = 0; i < count; i++)
            {
                flags.Add(flag);
            }
            return flags;
        }

        /// <summary>
        /// 根据标志，获取一条组合
        /// </summary>
        /// <param name="numberList">需要进行排列的数组</param>
        /// <param name="flagsList">数组的标准类表</param>
        /// <param name="startIndex">从<param name="flagsList">标志数组</param>中开始的索引</param>
        /// <param name="matchTimes">需要匹配标志的次数</param>
        /// <param name="selectFlag">需要匹配的标志，需要和<param name="flagsList">标志数组</param>中的类型一样</param>
        /// <returns></returns>
        private string GetCombineItem(IList<string> numberList, IList<byte> flagsList, int startIndex, int matchTimes, byte selectFlag)
        {
            startIndex = startIndex < 0 ? 0 : startIndex;
            int numberOfOne = 0;
            int count = flagsList.Count;
            var selectNumbers = new  List<string>(6);
            for (int i = startIndex; i < count; i++)
            {
                if (flagsList[i] == selectFlag)
                {
                    selectNumbers.Add(numberList[i]);
                    numberOfOne++;
                    continue;
                }
                //如果已经找到了指定个数的标志，则不需要再继续找了
                //不放在上个判断中：如果放在上个判断中，则每次不管flag是不是1都会去判断；如果不上在上面，则只有不是1的时候采取判断
                if (numberOfOne == matchTimes)
                    break;
            }
            return string.Join(" ", selectNumbers);
        }

       public List<string> GetCombines(string needCombineStr, int combinationCount)
       {
           List<string> combines = new List<string>();
           if (string.IsNullOrWhiteSpace(needCombineStr))
               return combines;
           var redNumbers = needCombineStr.Trim(Const.WhiteSpace).Split(Const.WhiteSpace);
           combines = GetCombines(redNumbers, combinationCount);
           return combines;
       }

       
    }

}
