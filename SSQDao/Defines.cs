using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSQDao
{
    public class SSQNumberDefine
    {
        /// <summary>
        /// 红球号码，只能是12个字符的字符串。
        /// <exception>ArgumentOutOfRangeException</exception>
        /// </summary>

        public string RedNumber { get; set; }

        /// <summary>
        /// 蓝球号码，只能是2个字符的字符串
        /// <exception>ArgumentOutOfRangeException</exception>
        /// </summary>
        public string BlueNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redNumbers">红球号码，只能是12个字符的字符串。
        /// <exception>ArgumentOutOfRangeException</exception></param>
        /// <param name="blueNumber">蓝球号码，只能是2个字符的字符串
        /// <exception>ArgumentOutOfRangeException</exception></param>
        public SSQNumberDefine(string redNumbers, string blueNumber)
        {
            RedNumber = redNumbers;
            BlueNumber = blueNumber;
        }

    }

    public class IssueDefine
    {
        /// <summary>
        /// 期号。<example>2014013</example>
        /// </summary>
        public string IssueNumber { get; set; }

        /// <summary>
        /// 期号的编码
        /// </summary>
        public string IssueCode { set; get; }

        /// <summary>
        /// 期号来源
        /// </summary>
        public SSQFrom Source { get; set; }

        public IssueDefine(string issueNumber, string issueCode, SSQFrom ssqFrom)
        {
            IssueNumber = issueNumber;
            IssueCode = issueCode;
            Source = ssqFrom;
        }
    }

    public class CloumnDefine
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public DbType DataType { get; set; }

    }

    /// <summary>
    /// 彩票来源
    /// </summary>
    public enum SSQFrom
    {
        /// <summary>
        /// 奇虎360
        /// </summary>
        QiHu360 = 1,
        /// <summary>
        /// 淘宝
        /// </summary>
        Taobao = 2,
        /// <summary>
        /// 百度
        /// </summary>
        Baidu = 3
    }

    public enum TableType
    {
        /// <summary>
        /// 原始号码表
        /// </summary>
        OriginalNum = 1,
        AnalyResult = 2,
        /// <summary>
        /// 订单主键表名
        /// </summary>
        DetailOrderKey = 3
    }
}
