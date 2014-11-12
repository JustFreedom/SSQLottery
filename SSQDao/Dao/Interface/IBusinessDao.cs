namespace SSQDao.Dao.Interface
{
   public interface IBusinessDao
    {
        /// <summary>
        /// 创建原始号码表
        /// <remarks>表名定义规则：数据来源_Original_期号</remarks>
        /// </summary>
        /// <param name="issueNumber">期号</param>
        /// <param name="ssqFrom">号码来源</param>
        void CreateOriginalNumTable(string issueNumber, SSQFrom ssqFrom);

        /// <summary>
        /// 创建分析结果表
        /// <remarks>表名定义规则：数据来源_Analy_期号</remarks>
        /// </summary>
        /// <param name="issueNumber"></param>
        /// <param name="ssqFrom"></param>
        void CreateAnalyResultTable(string issueNumber, SSQFrom ssqFrom);

        /// <summary>
        /// 创建 “详细订单键值”表
        /// <example>淘宝：用户主键；奇虎360：方案id</example>
        /// </summary>
        /// <param name="issueNumber"></param>
        /// <param name="ssqFrom"></param>
        void CreateDetailOrderKey(string issueNumber, SSQFrom ssqFrom);
    }
}
