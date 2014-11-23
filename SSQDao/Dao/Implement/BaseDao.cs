using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSQDao.Dao.Implement
{
    internal class BaseDao : IBaseDao
    {
        
        private IDataParameter GetDataParameter(string parameterName, DbType dbType, object value)
        {
            IDataParameter parameter = new SqlParameter(parameterName, dbType);
            parameter.Value = value;
            return parameter;
        }

        private string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["SSQ"].ConnectionString;
        }
     

        private void ExcuteNoQuery(string sql, params IDataParameter[] parameters)
        {
            string conString = GetConnectionString();
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();
                var command = new SqlCommand(sql, conn);
                //int paraMaxCount = 2000;
                //int count = parameters.Length/paraMaxCount;
                //for (int i = 0; i < count; i++)
                //{
                //    Array tempPara = Array.CreateInstance(typeof(IDataParameter), paraMaxCount);
                //    Array.Copy(parameters, i * paraMaxCount, tempPara, 0, paraMaxCount);
                //    command.Parameters.AddRange(tempPara);
                //    command.ExecuteNonQuery();   
                //}
                //int paraCount = parameters.Length % paraMaxCount;
                //Array tempPara1 = Array.CreateInstance(typeof(IDataParameter), paraCount);
                //Array.Copy(parameters, count * paraMaxCount, tempPara1, 0, paraCount);
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }
        private object ExecuteScalary(string sql, params IDataParameter[] parameters)
        {
            string conString = GetConnectionString();
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();
                var command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(parameters);
                return command.ExecuteScalar();
            }
        }

        private List<object> ExecuteReader(string sql, params IDataParameter[] parameters)
        {
            var values = new List<object>();
            string conString = GetConnectionString();
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();
                var command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(parameters);
                SqlDataReader reader =command.ExecuteReader();
                while (reader.Read())
                {
                    values.Add(reader.GetValue(0));
                }
            }
            return values;
        }

        public void Init()
        {
            InitDataBase();
            InitSeedNumberTable();
            InitIssueTable();
            InitProgressRateTable();
        }

        public void InitDataBase()
        {
            var sql = string.Format(
                    "begin use master if not exists (select * from sysdatabases  where name='{0}')	create database {0} end ", Const.DataBaseName);
            ExcuteNoQuery(sql);
        }

        public void DeleteDataBase()
        {
            var sql = string.Format(
                   "begin use master if  exists (select * from sysdatabases  where name='{0}')	delete database {0} end ", Const.DataBaseName);
            ExcuteNoQuery(sql);
        }
        #region SeedNumberTable
        public void InitSeedNumberTable()
        {
            var sql = string.Format(
                "begin use {0} if not exists (select * from sysobjects  where name = '{1}') create table {1} ({2} char(12) not null,{3} char (2) not null ) end ", Const.DataBaseName, Const.SeedNumberTableName, Const.RedNumber, Const.BlueNumber);
            ExcuteNoQuery(sql);
        }

        public void InsertToSeedNumber(SSQNumberDefine ssqNumber)
        {
            var sql = string.Format(
                "begin use {0} insert into {1} ({2},{3}) values ({4}{5},{4}{6}) end  ", Const.DataBaseName, Const.SeedNumberTableName, Const.RedNumber, Const.BlueNumber, Const.DataTypeToken, Const.RedNumber, Const.BlueNumber);
            IDataParameter redPara = GetDataParameter(Const.RedNumber, DbType.String, ssqNumber.RedNumber);
            IDataParameter bluePara = GetDataParameter(Const.BlueNumber, DbType.String, ssqNumber.BlueNumber);
            IDataParameter[] parameters = new[] { redPara, bluePara };
            ExcuteNoQuery(sql, parameters);
        }

        public void InsertToSeedNumber(IList<SSQNumberDefine> ssqNumbers)
        {
            var valueBuilder = new StringBuilder();
            IList<IDataParameter> parameters = new List<IDataParameter>();
            int i = 1;
            foreach (var define in ssqNumbers)
            {
                valueBuilder.AppendFormat("({0}{1}{3},{0}{2}{3})", Const.DataTypeToken, Const.RedNumber,
                    Const.BlueNumber, i);
                parameters.Add(GetDataParameter(Const.RedNumber + i, DbType.String, define.RedNumber));
                parameters.Add(GetDataParameter(Const.BlueNumber + i, DbType.String, define.BlueNumber));
                i++;
            }
            var sql = string.Format(
                "begin use {0} insert into {1} ({2},{3}) values {4} end  ", Const.DataBaseName,
                Const.SeedNumberTableName, Const.RedNumber, Const.BlueNumber, string.Join(",", valueBuilder));
            ExcuteNoQuery(sql, parameters.ToArray());
        }

        public void DeleteSeedNunber(IList<SSQNumberDefine> ssqNumbers)
        {
            var redNumbers = new List<string>();
            var blueNumbers = new List<string>();
            IList<IDataParameter> parameters = new List<IDataParameter>();
            foreach (var define in ssqNumbers)
            {
                redNumbers.Add(define.RedNumber);
                blueNumbers.Add(define.BlueNumber);
                parameters.Add(GetDataParameter(Const.RedNumber, DbType.String, define.RedNumber));
            }
            var sql = string.Format(
                "begin use {0} delete from {1} where {2} in ({3}) and {4} in ({5}) end  ", Const.DataBaseName,
                Const.SeedNumberTableName, Const.RedNumber, string.Join(",", redNumbers), Const.BlueNumber,
                string.Join(",", redNumbers));
            ExcuteNoQuery(sql, parameters.ToArray());
        }
        #endregion

        #region
        public void InitIssueTable()
        {
            var sql = string.Format(
                 "begin use {0} if not exists (select * from sysobjects  where name = '{1}') create table {1} ({2} char(7) not null ,{3} nvarchar(30) not null,{4} nvarchar(15) not null,primary key ({2},{3},{4})) end ", Const.DataBaseName, Const.IssueTableName, Const.IssueNo, Const.IssueCode, Const.IssueFrom);
            ExcuteNoQuery(sql);
        }

        public void InsertToIssue(IssueDefine issueDefine)
        {
            var sql = string.Format(
                "begin use {0} insert into {1} ({2},{3},{4}) values ({5}{6},{5}{7},{5}{8}) end  ", Const.DataBaseName, Const.IssueTableName, Const.IssueNo, Const.IssueCode, Const.IssueFrom, Const.DataTypeToken, Const.IssueNo, Const.IssueCode, Const.IssueFrom);
            IDataParameter numberPara = GetDataParameter(Const.IssueNo, DbType.String, issueDefine.IssueNumber);
            IDataParameter codePara = GetDataParameter(Const.IssueCode, DbType.String, issueDefine.IssueCode);
            IDataParameter fromPara = GetDataParameter(Const.IssueFrom, DbType.String, issueDefine.Source);
            IDataParameter[] parameters = new[] { numberPara, codePara, fromPara };
            ExcuteNoQuery(sql, parameters);
        }

        public void InsertToIssue(IList<IssueDefine> issueDefines)
        {

            var valueBuilder = new StringBuilder();
            IList<IDataParameter> parameters = new List<IDataParameter>();
            int i = 1;
            foreach (var define in issueDefines)
            {
                valueBuilder.AppendFormat("({0}{1}{4},{0}{2}{4},{0}{3}{4})", Const.DataTypeToken, Const.IssueNo,
                   Const.IssueCode, Const.IssueFrom, i);
                parameters.Add(GetDataParameter(Const.IssueNo + i, DbType.String, define.IssueNumber));
                parameters.Add(GetDataParameter(Const.IssueCode + i, DbType.String, define.IssueCode));
                parameters.Add(GetDataParameter(Const.IssueFrom + i, DbType.String, define.Source));
                i++;
            }
            var sql = string.Format(
                "begin use {0} insert into {1} ({2},{3},{4}) values {5} end  ", Const.DataBaseName,
                Const.IssueTableName, Const.IssueNo, Const.IssueCode, Const.IssueFrom, string.Join(",", valueBuilder));
            ExcuteNoQuery(sql, parameters.ToArray());
        }

        public void DeleteIssue(string issueNumber, SSQFrom ssqFrom)
        {
            var sql = string.Format(
                "begin use {0} delete from {1} where {2} in ({3}) and {4} in ({5}) end  ", Const.DataBaseName,
                Const.IssueTableName, Const.IssueNo, issueNumber, Const.IssueFrom,
                Enum.GetName(typeof(SSQFrom), ssqFrom));
            ExcuteNoQuery(sql);
        }

        public void DeleteIssue(IList<IssueDefine> issueDefines)
        {
            var issueNumbers = new List<string>();
            var issueCodes = new List<string>();
            var issueFroms = new List<string>();
            foreach (var define in issueDefines)
            {
                issueNumbers.Add(define.IssueNumber);
                issueCodes.Add(define.IssueCode);
                issueFroms.Add(Enum.GetName(typeof(SSQFrom), define.Source));
            }
            var sql = string.Format(
                "begin use {0} delete from {1} where {2} in ({3}) and {4} in ({5}) and {6} in ({7}) end  ", Const.DataBaseName, Const.IssueTableName, Const.IssueNo, string.Join(",", issueNumbers), Const.IssueCode,
                string.Join(",", issueCodes), Const.IssueFrom, string.Join(",", issueFroms));
            ExcuteNoQuery(sql);
        }


        #endregion

        #region ProgressRate
        public void InitProgressRateTable()
        {
            var sql = string.Format(
                 "begin use {0} if not exists (select * from sysobjects  where name = '{1}') create table {1} ({2} nvarchar(30) not null ,{3} int not null) end ", Const.DataBaseName, Const.ProgressRateTableName, Const.PR_TableName, Const.PR_Count);
            ExcuteNoQuery(sql);
        }

        public void DeleteProgressRate(string issueNumber, SSQFrom ssqFrom)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateToProgressRate(string tableName, int doneCount)
        {
            var sql = string.Format(
                " use {0} if not exists (select * from {1} where {2}={4}{2})  insert into {1} ({2},{3}) values ({4}{2},{4}{3})  else begin update {1} set {3}={3} + {4}{3} where {2} = {4}{2} end  ", Const.DataBaseName,
                Const.ProgressRateTableName, Const.PR_TableName, Const.PR_Count, Const.DataTypeToken);
            IDataParameter tableNamePara = GetDataParameter(Const.PR_TableName, DbType.String, tableName);
            IDataParameter doneCountPara = GetDataParameter(Const.PR_Count, DbType.Int32, doneCount);
            IDataParameter[] parameters = new[] { tableNamePara, doneCountPara };
            ExcuteNoQuery(sql, parameters);
        }

        public void DeleteProgressRate(string tableName)
        {
            var sql = string.Format(
                "begin use {0} delete from {1} where {2} in ({3}) end  ", Const.DataBaseName,
                Const.ProgressRateTableName, Const.PR_TableName, tableName);
            ExcuteNoQuery(sql);
        }

        #endregion

        public void CreateOriginalNumTable(string issueNumber, SSQFrom ssqFrom)
        {
            string tableName = GetTableName(issueNumber, TableType.OriginalNum, ssqFrom);
            string sql =
                string.Format(
                    "begin use {0} if not exists (select * from sysobjects  where name = '{1}') create table {1} ({2} int PRIMARY KEY IDENTITY,{3} varchar(100) not null ) end",
                    Const.DataBaseName, tableName, Const.Id,Const.Org_OriginalNumber);
            ExcuteNoQuery(sql);
        }

        public void InsertOriginalNum(string issueNumber, IList<string> originalNumbers)
        {
            string tableName = GetTableName(issueNumber, TableType.OriginalNum, GetSSQFrom());
            if (originalNumbers.Count < 1)
                return;
            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.Add(Const.Org_OriginalNumber, typeof(string));
                foreach (string originalNumber in originalNumbers)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[Const.Org_OriginalNumber] = originalNumber;
                    dataTable.Rows.Add(dataRow);
                }
                using (SqlBulkCopy bulk = new SqlBulkCopy(GetConnectionString()))
                {
                    bulk.BatchSize = 5000;
                    bulk.DestinationTableName = tableName;
                    bulk.ColumnMappings.Add(Const.Org_OriginalNumber, Const.Org_OriginalNumber);
                    bulk.WriteToServer(dataTable);
                }
            }
        }

        public List<string> QueryOrgNumbers(string issueNumber, int startIndex, int length)
        {
            string tableName = GetTableName(issueNumber, TableType.OriginalNum, GetSSQFrom());
            var sql = string.Format(
                "begin use {0} select {1} from {2} order by {3} offset {4} rows fetch next {5} rows only end ",
                Const.DataBaseName, Const.Org_OriginalNumber,
                tableName, Const.Id, startIndex, length);
            var result = new List<object>();
            try
            {
                result = ExecuteReader(sql);
            }
            catch (Exception)
            {
            }
            return result.Select(x => x.ToString()).ToList();
        }

        public void CreateAnalyResultTable(string issueNumber, SSQFrom ssqFrom)
        {
            string tableName = GetTableName(issueNumber, TableType.AnalyResult, ssqFrom);
            string sql = string.Format("begin use {0} if not exists (select * from sysobjects  where name = '{1}') create table {1} ({2} int PRIMARY KEY IDENTITY,{3} varchar(19) not null,{4} varchar(2) not null) end", Const.DataBaseName, tableName,Const.Id, Const.RedNumber, Const.BlueNumber);
            ExcuteNoQuery(sql);
        }

        public void InsertAnalysisNumbers(string issueNumber, IList<SSQNumberDefine> ssqNumberDefines)
        {
            string tableName = GetTableName(issueNumber, TableType.AnalyResult, GetSSQFrom());
            if (ssqNumberDefines.Count < 1)
                return;

            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.Add(Const.RedNumber, typeof(string));
                dataTable.Columns.Add(Const.BlueNumber, typeof(string));
                //dataTable.Columns.Add(Const.Id, typeof(int));
                int i = 1;
                foreach (SSQNumberDefine ssqNumberDefine in ssqNumberDefines)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[Const.RedNumber] = ssqNumberDefine.RedNumber;
                    dataRow[Const.BlueNumber] = ssqNumberDefine.BlueNumber;
                    //dataRow[Const.Id] = i++;
                    dataTable.Rows.Add(dataRow);
                }
                using (SqlBulkCopy bulk = new SqlBulkCopy(GetConnectionString()))
                {
                    bulk.BatchSize = 5000;
                    bulk.DestinationTableName = tableName;
                    bulk.ColumnMappings.Add(Const.RedNumber, Const.RedNumber);
                    bulk.ColumnMappings.Add(Const.BlueNumber, Const.BlueNumber);
                    //bulk.ColumnMappings.Add(Const.Id, Const.Id);
                    bulk.WriteToServer(dataTable);
                }
            }
        }


        public void CreateDetailOrderKeyTable(string issueNumber, SSQFrom ssqFrom)
        {
            string tableName = GetTableName(issueNumber, TableType.DetailOrderKey, ssqFrom);
            string sql =
                string.Format(
                    "begin use {0} if not exists (select * from sysobjects  where name = '{1}') create table {1} ({2} int PRIMARY KEY IDENTITY,{3} varchar(30) not null ) end",
                    Const.DataBaseName, tableName, Const.Id, Const.DOK_DetailOrderKey);
            ExcuteNoQuery(sql);
        }

        public void InsertDetailOrderKey(string issueNumber,IList<string > detailOrderKeys)
        {
            if (detailOrderKeys.Count < 1)
                return;
            //var valueBuilder = new StringBuilder();
            List<string> valueList = new List<string>();
            IList<IDataParameter> parameters = new List<IDataParameter>();
            int i = 1;
            foreach (string detailOrderkey in detailOrderKeys)
            {
                //valueBuilder.AppendFormat("({0}{1}{2})", Const.DataTypeToken, Const.DOK_DetailOrderKey, i);
                valueList.Add(string.Format("{0}{1}{2}", Const.DataTypeToken, Const.DOK_DetailOrderKey, i));
                parameters.Add(GetDataParameter(Const.DOK_DetailOrderKey + i, DbType.String, detailOrderkey));
                i++;
            }
            string tableName = GetTableName(issueNumber, TableType.DetailOrderKey, GetSSQFrom());
            var sql = string.Format(
                "begin use {0} insert into {1} ({2}) values ({3}) end  ", Const.DataBaseName,
                tableName, Const.DOK_DetailOrderKey, string.Join("),(", valueList));
            ExcuteNoQuery(sql, parameters.ToArray());
        }


        public List<string> QueryDetailOrderKey(string issueNumber, int startIndex, int length)
        {
            string tableName = GetTableName(issueNumber, TableType.DetailOrderKey, GetSSQFrom());
            var sql = string.Format(
                "begin use {0} select {1} from {2} order by {3} offset {4} rows fetch next {5} rows only end ",
                Const.DataBaseName, Const.DOK_DetailOrderKey,
                tableName, Const.Id, startIndex, length);
            var result = new List<object>();
            try
            {
                result = ExecuteReader(sql);
            }
            catch (Exception)
            {
            }
            return result.Select(x => x.ToString()).ToList();
        }

        public int QueryProgress(string tableName)
        {
            var sql = string.Format(
                "begin use {0} select {1} from {2} where {3} = '{4}' end ", Const.DataBaseName, Const.PR_Count,
                Const.ProgressRateTableName, Const.PR_TableName, tableName);
            int count = 0;
            try
            {
                count = int.Parse(ExecuteScalary(sql).ToString());
            }
            catch (Exception)
            {

            }
            return count;
        }

       

        public string GetTableName(string issueNumber, TableType tableType, SSQFrom ssqFrom)
        {
            string tableName = string.Format("{0}_{1}_{2}", Enum.GetName(typeof(SSQFrom), ssqFrom), Enum.GetName(typeof(TableType), tableType),
                issueNumber);
            return tableName;
        }

        public int QueryItemsCount(string tableName)
        {
            var sql = string.Format(
                "begin use {0} select count(*) from {1}  end ", Const.DataBaseName, tableName);
            int count = 0;
            try
            {
                count = int.Parse(ExecuteScalary(sql).ToString());
            }
            catch (Exception )
            {

            }
            return count;
        }

        public bool IsExistDetailOrderKey(string issueNumber, string detailOrderKey)
        {
            string tableName = GetTableName(issueNumber, TableType.DetailOrderKey, GetSSQFrom());
            var sql = string.Format(
                "begin use {0} select count(*) from {1} where {2} = {3}{2} end ", Const.DataBaseName, tableName,
                Const.DOK_DetailOrderKey, Const.DataTypeToken);
            var dataPara = GetDataParameter(Const.DOK_DetailOrderKey, DbType.String, detailOrderKey);
            bool exist = false;
            try
            {
                exist = int.Parse(ExecuteScalary(sql, dataPara).ToString()) > 0;
            }
            catch (Exception)
            {

            }
            return exist;
        }

        public SSQFrom GetSSQFrom()
        {
            return SSQFrom.Taobao;
        }
    }
}
