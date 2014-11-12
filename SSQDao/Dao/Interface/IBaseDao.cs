using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSQDao.Dao.Implement;

namespace SSQDao
{
    public interface IBaseDao
    {
        /// <summary>
        /// 初始化数据库和表
        /// </summary>
        void Init();
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <remarks>初始化数据库SSQLottery，存储表SeedNumber、ProgressRate、Issue</remarks>
        void InitDataBase();

        /// <summary>
        /// 删除数据库
        /// </summary>
        void DeleteDataBase();


        #region SeedNumber table

        /// <summary>
        /// 初始化种子号码表(SeedNumber)
        /// </summary>
        /// <remarks>如果没有这张表，则创建这张表；如果有这张表，则清空这张表中的数据</remarks>
        void InitSeedNumberTable();

        /// <summary>
        /// 插入号码到种子号码表
        /// </summary>
        /// <param name="redNum">红球号码，只能是12个字符的字符串。
        /// <exception>ArgumentOutOfRangeException</exception></param>
        /// <param name="blueNum">蓝球号码，只能是2个字符的字符串
        /// <exception>ArgumentOutOfRangeException</exception></param>
        void InsertToSeedNumber(SSQNumberDefine ssqNumber);

        void InsertToSeedNumber(IList<SSQNumberDefine> ssqNumbers);

        void DeleteSeedNunber(IList<SSQNumberDefine> ssqNumbers);

        #endregion

        #region Issue Table
        /// <summary>
        /// 初始化期表
        /// </summary>
        /// <remarks>如果没有这张表，则创建这张表；如果有这张表，则清空这张表中的数据</remarks>
        void InitIssueTable();

        void InsertToIssue(IssueDefine issueDefine);

        void InsertToIssue(IList<IssueDefine> issueDefines);

        /// <summary>
        /// 删除期号
        /// </summary>
        /// <param name="issueNumber"></param>
        /// <param name="ssqFrom"></param>
        void DeleteIssue(string issueNumber, SSQFrom ssqFrom);

        /// <summary>
        /// 删除期号
        /// </summary>
        /// <param name="issueDefines"><see cref="IssueDefine.IssueNumber"/>和
        /// <see cref="IssueDefine.Source"/>不能为空</param>
        void DeleteIssue(IList<IssueDefine> issueDefines);
        #endregion

        #region ProgressRate  Table
        /// <summary>
        /// 初始化期表
        /// </summary>
        /// <remarks>如果没有这张表，则创建这张表；如果有这张表，则清空这张表中的数据</remarks>
        void InitProgressRateTable();

        /// <summary>
        /// 插入或者更新指定表的进度数
        /// <remarks>如果没有指定表的记录，则插入这条记录；如果有指定表的记录，则在原记录上加上<para>doneCount</para></remarks>
        /// </summary>
        /// <param name="tableName">需要更新处理进度的表的表名</param>
        /// <param name="newDoneCount">新完成的数量</param>
        void InsertOrUpdateToProgressRate(string tableName, int newDoneCount);
        
        /// <summary>
        /// 删除期号
        /// </summary>
        /// <param name="issueNumber"></param>
        /// <param name="ssqFrom"></param>
        void DeleteProgressRate(string issueNumber, SSQFrom ssqFrom);
     
        #endregion

        /// <summary>
        /// 创建原始号码表
        /// <remarks>表名定义规则：数据来源_Original_期号</remarks>
        /// </summary>
        /// <param name="issueNumber">期号</param>
        /// <param name="ssqFrom">号码来源</param>
        void CreateOriginalNumTable(string issueNumber, SSQFrom ssqFrom);

        /// <summary>
        /// 新增原始号码
        /// </summary>
        /// <param name="issueNumber">彩票期号</param>
        /// <param name="originalNumbers">原始号码列表</param>
        void InsertOriginalNum(string issueNumber, IList<string> originalNumbers);

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
        void CreateDetailOrderKeyTable(string issueNumber, SSQFrom ssqFrom);

        void InsertDetailOrderKey(string issueNumber, IList<string> detailOrderKeys);

        /// <summary>
        /// 查询详细订单主键
        /// </summary>
        /// <param name="issueNumber">彩票期号</param>
        /// <param name="startIndex">开始的查询的记录序号</param>
        /// <param name="length">需要查询的记录条数</param>
        /// <returns>详细订单主键</returns>
        /// <example>
        ///  SELECT  * FROM    tableName ORDER   BY fieldName DESC OFFSET  ( 10 * ( 15 - 1 )) ROWS FETCH NEXT 10 ROWS ONLY
        /// </example>
        List<string> QueryDetailOrderKey(string issueNumber, int startIndex, int length); 

        /// <summary>
        /// 查询进度
        /// </summary>
        /// <returns>进度数</returns>
        int QueryProgress(string tableName);

        string GetTableName(string issueNumber, TableType tableType, SSQFrom ssqFrom);

        /// <summary>
        /// 查询表中的记录数
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        int QueryItemsCount(string tableName);

    }

    public class DaoFactory
    {
        public static IBaseDao NewBaseDaoInstance()
        {
            return new BaseDao();
        }
    }
}
