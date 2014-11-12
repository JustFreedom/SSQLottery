using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistic
{
   public class UrlConst
    {

        #region 参数

        /// <summary>
        /// 是否满员
        /// <value>1、未满员</value><value>2、已满员</value><value>3、已撤单</value>
        /// </summary>
        public static readonly string Is_not_full = "is_not_full";

        /// <summary>
        /// 方案是否公开
        /// <value>1、内容公开</value>
        /// <value>2、内容保密</value>
        /// <value>3、不限保密</value>
        /// <value>0、参与后公开</value>
        /// </summary>
        public static readonly string Confidential = "confidential";

       public static readonly string United_id = "united_id";

       public static readonly string Tb_United_id = "tb_united_id";

       public static readonly string Issue_id = "issue_id";

       public static readonly string Page = "page";

        #endregion
        /// <summary>
        /// 基础url
        /// </summary>
        public static readonly string BaseUrl = "http://caipiao.taobao.com/";

        /// <summary>
        /// 双色球合买中心列表
        /// <remarks>包含信息：双色球总页数、双色球总条数、第一页订单主键</remarks>
        /// </summary>
        public static readonly string UnitedList = BaseUrl + "lottery/ajax/get_united_list.htm?lotteryType=SSQ";

       /// <summary>
       /// 合买中心列表
       /// <remarks>需要添加页号</remarks>
       /// </summary>
        public static readonly string UnitedListByPage = string.Format("{0}&{1}={2}&{3}={4}&page=", UnitedList, Is_not_full, 2, Confidential, 1);

       public static readonly string DetailOrderPageUrl = BaseUrl + "lottery/order/united_detail.htm?";
       public static readonly string DetailOrderUrl = BaseUrl + "lottery/order/united_order_detail_szc.htm?lottery_type_id=1";

       public static string ConstructUrlPara(string paraName, string paraValue)
       {
           return String.Format("&{0}={1}", paraName, paraValue);
       }
    }
}
