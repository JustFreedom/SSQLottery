using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSQDao
{
    public static class Const
    {
        public const string DataBaseName = "SSQLottery";

        public const string Id = "Id";
        public const string SeedNumberTableName = "SeedNumber";

        public const string ProgressRateTableName = "ProgressRate";

        public const string IssueTableName = "Issue";

        //public const string ConnectionString =System.Configuration.ConfigurationManager.ConnectionStrings.AppSettings["ConnectionString"]
        //    "server=127.0.0.1\\MSSQLSERVER;uid=lidan;pwd=P@ssw0rd;database=" + DataBaseName + ";";


        public const string RedNumber = "RedNumber";

        public const string BlueNumber = "BlueNumber";

        public const string DataTypeToken = "@";

        public const string IssueNo = "IssueNo";

        public const string IssueCode = "Code";

        public const string IssueFrom = "IssueFrom";

        /// <summary>
        /// 处理进度表中的存储表名
        /// </summary>
        public const string PR_TableName = "TableName";

        /// <summary>
        /// 进度数
        /// </summary>
        public const string PR_Count = "PR_Count";

       

        public const string Org_OriginalNumber = "OriginalNumber"; 

        

        /// <summary>
        /// 订单主键表_订单主键字段
        /// </summary>
        public const string DOK_DetailOrderKey = "OrderKey";
    }
}
