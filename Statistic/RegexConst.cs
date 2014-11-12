using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Statistic
{
    internal class RegexConst
    {
        #region string const
        /// <summary>
        /// 淘宝总页数
        /// </summary>
       public readonly static string Taobao_PageCount = "(?<=\"totalPage\":)\\d+(?=,)";

        /// <summary>
        /// 淘宝当前期号
        /// </summary>
       public static readonly string Taobao_CurIssueNum = "(?<=\"onsale\":true,\"issue\":\")\\d+(?=\")";

        /// <summary>
        /// 淘宝总记录数
        /// </summary>
       public static readonly string Taobao_TotalItemsCount = "(?<=\"totalItem\":)\\d+(?=,)";
        /// <summary>
        /// 订单主键
        /// </summary>
       public static readonly string Taobao_DetailOrderKey = "(?<=united_id=)\\w+(?=\")";

        public static readonly string Taobao_IssueId =
            @"(?<=united_order_detail_szc.htm\Wtb_united_id=\w+&issue_id=)\w+(?=\W)";

        /// <summary>
        /// 淘宝订单页中的页数
        /// <example> var max_page     = 3;</example>
        /// </summary>
        public static readonly string Taobao_DetailOrderPage_PageCount = @"(?<=var\s+max_page\s*=\s*)\d+(?=;)";

        public static readonly string OriginalNumber =
            @"(\((((0[1-9]\s)|(1[0-9]\s)|(2[0-9]\s)|(3[0-3]\s)(?!\1))){1,}((0[1-9])|(1[0-9])|(2[0-9])|(3[0-3]))\)){0,1}\s{0,1}((0[1-9]\s)|(1[0-9]\s)|(2[0-9]\s)|(3[0-3]\s)(?!\1)){1,}((0[1-9])|(1[0-9])|(2[0-9])|(3[0-3])):((0[1-9]\s)|(1[0-9]\s)|(2[0-9]\s)|(3[0-3]\s)(?!\1)){1,}";//"(\d\d\s*)+:(\d\d\s*)+";//(?<=<td.*>.*)(\b\d\d\s?)+:(\d\d\s)+";
        public static readonly string OriginalNumber2 =
            "(?<=<td.*>\\s*)(\\b\\d\\d\\s?)+:(\\d\\d\\s)+(?=\\b[\\u4e00-\\u9fa5])";

        #endregion

    }
}
