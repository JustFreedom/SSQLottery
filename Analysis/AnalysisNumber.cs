using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSQDao;

namespace Analysis
{
    internal class AnalysisNumber : IAnalysisNumber, IBase
    {
        private readonly string _issueNumber;
        public AnalysisNumber( string issueNumber)
        {
            _issueNumber = issueNumber;
        }

        IBaseDao _baseDao = DaoFactory.NewBaseDaoInstance();
        private List<SSQNumberDefine> AnaysisNumber(IList<string> orgNumberList)
        {
            Analysis analysis = new TaobaoAnalysis();
            List<SSQNumberDefine> numbers = analysis.Anaysis(orgNumberList);
            return numbers;
        }
       
        public void AnaysisNumber( int analysisedCount)
        {
            _baseDao.CreateAnalyResultTable(_issueNumber,GetSSQFrom());
            string analysisTableName = _baseDao.GetTableName(_issueNumber, TableType.AnalyResult, GetSSQFrom());
            int processedCount = _baseDao.QueryProgress(analysisTableName);
            string orgNumberTableName = _baseDao.GetTableName(_issueNumber, TableType.OriginalNum, GetSSQFrom());
            int totalItemCount = _baseDao.QueryItemsCount(orgNumberTableName);
            if (totalItemCount <= processedCount)
                return ;
            int pageSize = GetPageSize();
            
            for (int i = processedCount; i < totalItemCount; i = i + pageSize)
            {
                List<string> orgNumbers = _baseDao.QueryOrgNumbers(_issueNumber, processedCount, pageSize);
                _baseDao.InsertOrUpdateToProgressRate(analysisTableName, orgNumbers.Count);
                List<SSQNumberDefine> ssqNumberDefines = AnaysisNumber(orgNumbers);
                SaveAnalysisNumber(_issueNumber,ssqNumberDefines);
                InsertOrUpdateProgressCount(_issueNumber, orgNumbers.Count);
            }
            return ;
        }

        public int GetPageSize()
        {
            return 100;
        }

        private void SaveAnalysisNumber(string issueNumber,IList<SSQNumberDefine> ssqNumberDefines)
        {
            _baseDao.InsertAnalysisNumbers(issueNumber,ssqNumberDefines);
        }
        public  SSQFrom GetSSQFrom()
        {
            return SSQFrom.Taobao;
        }
        public void InsertOrUpdateProgressCount(string issueNumber, int newProgressedCount)
        {
            if (string.IsNullOrEmpty(issueNumber) || newProgressedCount < 1)
                return;
            string tableName = _baseDao.GetTableName(issueNumber, TableType.OriginalNum, SSQFrom.Taobao);
            _baseDao.InsertOrUpdateToProgressRate(tableName, newProgressedCount);
            AnalysisMonitor.InvokeAnalysisOrgNumber(newProgressedCount);
        }
    }
}
