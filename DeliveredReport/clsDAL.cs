using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace DeliveredReport
{
    class clsDAL
    {

        #region DatabaseStruct
        /// <summary>
        /// نام پایگاه داده و جدول
        /// </summary>
        public struct DatabaseStruct
        {
            #region DataBaseName
            private string _DataBaseName;
            /// <summary>
            /// نام پایگاه اطلاعاتی
            /// </summary>
            public string DataBaseName
            {
                get { return _DataBaseName; }
                set { _DataBaseName = value; }
            }
            #endregion

            #region TableName
            private string _TableName;
            /// <summary>
            /// نام جدول
            /// </summary>
            public string TableName
            {
                get { return _TableName; }
                set { _TableName = value; }
            }
            #endregion
        }
        public int count;
        #endregion

        #region connection
        /// <summary>
        /// ساخت کانکشن به سرور مربوطه
        /// </summary>
        /// <param name="server"></param>
        /// <returns>Ip or Server Name</returns>
        public SqlConnection sqlcon(string server)
        {
            SqlConnection connection = new SqlConnection(server);
            return connection;
        }
        #endregion

        #region Connection
        /// <summary>
        /// ساخت کانکشن با نام پایگاه داده 
        /// </summary>
        /// <param name="DatabaseName"></param>
        /// <returns>SqlConnection</returns>
        public SqlConnection sqlConnection(string DBName)
        {
            SqlConnection Connection = new SqlConnection("Data Source=.;Initial Catalog=" + DBName + ";Integrated Security=True");
            return Connection;
        }

        /// <summary>
        /// ساخت کانکشن با ساختار داده ای 
        /// </summary>
        /// <param name="DBStruct"></param>
        /// <returns></returns>
        public SqlConnection sqlConnection(DatabaseStruct DBStruct)
        {
            SqlConnection Connection = new SqlConnection("Data Source=.;Initial Catalog=" + DBStruct.DataBaseName + ";Integrated Security=True");
            return Connection;
        }
        #endregion

        #region execute command
        public Boolean sqlexecutecmd(string query, string DBName = "master", string state = "execute command")
        {
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=" + DBName + ";Integrated Security=True");
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                cmd.CommandTimeout = 3600;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
        }
        #endregion

        #region sqlCommand
        /// <summary>
        /// اجرای کوئری 
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Connection"></param>
        public void sqlCommand(string Query, DatabaseStruct DBStruct, string Stat = "State")
        {
            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConnection(DBStruct));
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception e)
            {
                if (Stat != "State")
                    MessageBox.Show(Stat + Environment.NewLine + e.Message);
            }
        }

        public void sqlCommand(string Query, string DBName, string Stat = "State")
        {
            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConnection(DBName));
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception e)
            {
                if (Stat != "State")
                    MessageBox.Show(Stat + Environment.NewLine + e.Message);
            }
        }

        #endregion

        #region ComboBoxSource
        /// <summary>
        /// اتصال کمبوباکس به جدول
        /// </summary>
        /// <param name="ComboBox">نام کمبوباکس</param>
        /// <param name="Table">نام جدول</param>
        /// <param name="ColumnIndex">ایندکس ستونی که به در کمبوباکس نمایش داده می شود</param>
        /// <param name="State">نام متد استفاده کننده جهت نمابش نام متد هنگام خطا</param>
        public void ComboBoxSource(ComboBox ComboBox, DataTable Table, int ColumnIndex = 0, string State = "State")
        {
            try
            {
                ComboBox.BindingContext = new BindingContext();
                ComboBox.DataSource = Table;
                ComboBox.ValueMember = Table.Columns[ColumnIndex].ColumnName;
            }
            catch (Exception e)
            {
                if (State != "State")
                    MessageBox.Show(State + Environment.NewLine + e.Message);
            }
        }
        #endregion

        #region ComboBoxSource
        /// <summary>
        /// اتصال کمبوباکس به جدول
        /// </summary>
        /// <param name="ComboBox">نام کمبوباکس</param>
        /// <param name="Table">نام جدول</param>
        /// <param name="ColumnIndex">ایندکس ستونی که به در کمبوباکس نمایش داده می شود</param>
        /// <param name="State">نام متد استفاده کننده جهت نمابش نام متد هنگام خطا</param>
        public ComboBox ComboBoxSource2(DataTable Table, int ColumnIndex = 0, string State = "State")
        {
            ComboBox cmbx = new ComboBox();
            try
            {
                cmbx.BindingContext = new BindingContext();
                cmbx.DataSource = Table;
                cmbx.ValueMember = Table.Columns[ColumnIndex].ColumnName;
            }
            catch (Exception e)
            {
                if (State != "State")
                    MessageBox.Show(State + Environment.NewLine + e.Message);
                return cmbx;
            }

            return cmbx;
        }
        #endregion

        #region sqlDataAdapter
        /// <summary>
        /// اجرای کوئری و نمایش نتیجه در جدول
        /// </summary>
        /// <param name="Query">SQL دستور</param>
        /// <param name="Connection">کانکشن</param>
        /// <param name="Stat"> پیشفرض پیام خطا نمایش داده نمی شود - نام موقعیت برای نمایش در خطا</param>
        /// <returns>DataTable</returns>
        public DataTable sqlDataAdapter(string Query, DatabaseStruct DBStruct, string Stat = "State")
        {
            string CN;
            DataTable dt = new DataTable();
            if (clsVal.UserID == "" & clsVal.Password == "")
            {
                CN = "Data Source=farahani-pc;Initial Catalog=" + DBStruct.DataBaseName + ";Integrated Security=True";
            }
            else
            {
                CN = "Data Source=" + clsVal.ServerName + ";Initial Catalog=" + DBStruct.DataBaseName + ";User ID=" + clsVal.UserID + ";Password=" + clsVal.Password+";";
            }
            
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(Query, CN);
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                if (Stat != "State")
                    MessageBox.Show(Stat + Environment.NewLine + e.Message);
                return dt;
            }

        }
        public DataTable sqlDataAdapter(string Query, string BDName, string Stat = "State")
        {
            DataTable dt = new DataTable();
            string CN;
            if (clsVal.UserID=="" & clsVal.Password=="")
            {
                CN = "Data Source=farahani-pc;Initial Catalog="+BDName+";Integrated Security=True";
            }
            else
            {
                CN = "Data Source=" + clsVal.ServerName + ";Initial Catalog=" + BDName + ";User ID=" + clsVal.UserID + ";Password=" + clsVal.Password+";";
            }
            
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(Query, CN);
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                if (Stat != "State")
                    MessageBox.Show(Stat + Environment.NewLine + e.Message);
                return dt;
            }
        }

        /// <summary>
        /// اجرای کوئری و نمایش نتیجه در جدول با استفاده از سرور مورد نظر
        /// </summary>
        /// <param name="cn">کانکشن با نام سرور و یوزر و پسورد</param>
        /// <param name="query"></param>
        /// <param name="Stat"></param>
        /// <returns>DataTable sqldataadapter</returns>
        public DataTable sqldataadapter(string cn, string query, string Stat = "sqldataadapter")
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                if (Stat != "sqldataadapter")
                    MessageBox.Show(Stat + Environment.NewLine + e.Message);
                return dt;
            }
        }
        public DataTable sqldataadapter(string cn, string query, DatabaseStruct DBStruct, string Stat = "sqldataadapter")
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, cn);
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                if (Stat != "sqldataadapter")
                    MessageBox.Show(Stat + Environment.NewLine + e.Message);
                return dt;
            }
        }
        #endregion

        #region GetDBName
        /// <summary>
        /// بر روی سیستم SQL لیستی شامل تمام پایگاه  داده های
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable sqlGetDBName()
        {
            string Query = " SELECT * FROM sys.databases WHERE database_id>4";
            DataTable dt = sqlDataAdapter(Query, "master");
            return dt;
        }
        /// <summary>
        /// بر روی سیستم SQL لیستی شامل تمام پایگاه  داده های
        /// </summary>
        /// <param name="cn">کانکشن با نام سرور و یوزر و پسورد</param>
        /// <returns></returns>
        public DataTable sqlgetdbname(string cn)
        {
            string Query = " SELECT * FROM sys.databases WHERE database_id>4";
            //DataTable dt = sqldataadapter(cn, Query, "master");
            DataTable dt = sqlDataAdapter(Query, "master");
            return dt;
        }
        #endregion

        #region GetTableNameInDB

        #region بوسیله ساختار داده ای پایگاه داده
        /// <summary>
        /// لیستی شامل جداول موجود در پایگاه داده انتخاب شده 
        /// </summary>
        /// <param name="DBStruct">ساختار داده ای پایگاه داده</param>
        /// <returns>جدول نام جداول موجود در پایگاه داده</returns>
        public DataTable sqlGetTableNameInDB(DatabaseStruct DBStruct)
        {
            string Query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IS NOT NULL";
            DataTable dt = sqlDataAdapter(Query, DBStruct);
            return dt;
        }
        #endregion

        #region بوسیله نام پایگاه داده
        /// <summary>
        /// لیستی شامل جداول موجود در پایگاه داده انتخاب شده 
        /// </summary>
        /// <param name="DBName">نام پایگاه داده</param>
        /// <returns>جدول نام جداول موجود در پایگاه داده</returns>
        public DataTable sqlGetTableNameInDB(string DBName)
        {
            string Query = " SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IS NOT NULL";
            DataTable dt = sqlDataAdapter(Query, DBName);
            return dt;
        }
        #endregion

        #endregion

        #region sqlColumnsName
        /// <summary>
        /// لیست نام ستون های جدول
        /// </summary>
        /// <param name="Database">ساختار پایگاه داده</param>
        /// <returns>DataTable</returns>
        public DataTable sqlColumnsName(DatabaseStruct DBStruct)
        {
            string Query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " + "WHERE TABLE_NAME = '" + DBStruct.TableName + "'";
            return sqlDataAdapter(Query, DBStruct);
        }

        public DataTable sqlColumnsName(string DBName, string TblName)
        {
            string Query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " + "WHERE TABLE_NAME = '" + TblName + "'";
            return sqlDataAdapter(Query, DBName);
        }
        public DataTable sqlColumnsNameCLB(DatabaseStruct DBStruct)
        {
            string Query = "SELECT COLUMN_NAME+'  [' + DATA_TYPE + ']' FROM INFORMATION_SCHEMA.COLUMNS " + "WHERE TABLE_NAME = '" + DBStruct.TableName + "'";
            return sqlDataAdapter(Query, DBStruct);
        }

        public DataTable sqlColumnsNameCLB(string DBName, string TblName)
        {
            string Query = "SELECT COLUMN_NAME + '  [' + DATA_TYPE + ']' FROM INFORMATION_SCHEMA.COLUMNS " + "WHERE TABLE_NAME = '" + TblName + "'";
            return sqlDataAdapter(Query, DBName);
        }

        public DataTable sqlColumnsNameCLBNew(DatabaseStruct DBStruct)
        {
            string Query = "SELECT TABLE_NAME,COLUMN_NAME,IS_NULLABLE,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM [" + DBStruct + "].INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + DBStruct.TableName + "'";
            return sqlDataAdapter(Query, DBStruct);
        }
        public DataTable sqlColumnsNameCLBNew(string DBName, string TBName)
        {
            string Query = "SELECT TABLE_NAME,COLUMN_NAME,IS_NULLABLE,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM [" + DBName + "].INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + TBName + "'";
            return sqlDataAdapter(Query, DBName);
        }
        #endregion

        #region Show All Data
        public DataTable ShowAll(string dbname, string tbname)
        {
            string query = "Select * from [" + dbname + "].dbo.[" + tbname + "]";
            DataTable dt = new DataTable();
            dt = sqlDataAdapter(query, dbname);
            return dt;
        }
        #endregion

        #region FilterDB
        public DataTable FilterGetDB(ComboBox DBName,ComboBox TBName)
        {
            DataTable dt = new DataTable();
            string Query = "Select * from ["+DBName.Text+"].dbo.["+TBName.Text+"]";
            dt = sqlDataAdapter(Query, DBName.Text);
            return dt;
        }
        #endregion

        string query="";
        string query2;
        
        public DataTable OneTaTenDarand(string SaleVorodi)
        {string com = "[" + clsVal.DBName + "].dbo.[" + clsVal.TBName + "]";
            if (SaleVorodi == "")
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i == 0)
                    {
                        query = "(SELECT StudentID FROM "+com+" WHERE (Grade = " + (i + 1) + "))";
                    }
                    if (i > 0)
                    {
                        if (i < 9)
                        {
                            query = "(SELECT StudentID FROM "+com+" WHERE (Grade = " + (i + 1) + ") AND (StudentID IN " + query + "))";
                        }
                        if (i == 9)
                        {
                            query = "SELECT COUNT(*) FROM(SELECT StudentID FROM "+com+" WHERE (Grade = " + (i + 1) + ") AND (StudentID IN " + query + ")GROUP BY "+com+".StudentID)t2";
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i == 0)
                    {
                        query = "(SELECT StudentID FROM "+com+" WHERE (Grade = " + (i + 1) + "))";
                    }
                    if (i > 0)
                    {
                        if (i < 9)
                        {
                            query = "(SELECT StudentID FROM "+com+" WHERE (Grade = " + (i + 1) + ") AND (StudentID IN " + query + "))";
                        }
                        if (i == 9)
                        {
                            query = "SELECT "+com+".* FROM "+com+" join (SELECT StudentID FROM "+com+" WHERE (Grade = " + (i + 1) + ") AND (StudentID IN " + query + ")GROUP BY "+com+".StudentID)t2 ON "+com+".StudentID = t2.StudentID ";
                        }
                    }
                }                
            }
            return sqlDataAdapter(query,clsVal.DBName);
        }
    }
}
