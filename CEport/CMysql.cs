using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExportscope
{
    public class CMysql
    {
        public string IP { get; set; }
        public short Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        private string dbName = "rvtdateex";

        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回MySqlConnection对象</returns>
        public MySqlConnection getmysqlcon()
        {
            //[Description("Information used to connect to a DataSource, such as 'Server=xxx;UserId=yyy;Password=zzz;Database=dbdb'.")]
            //("tcp://192.168.1.214:3306", "root", "root", "model_data");
            string M_str_sqlcon = string.Format("Server={0};port={1};UserId={2};Password={3};Database={4};pooling=false;CharSet=utf8", IP, Port, UserName, Password, dbName); //根据自己的设置
            MySqlConnection myCon = new MySqlConnection(M_str_sqlcon);
            return myCon;
        }
       
        /// <summary>
        /// 执行MySqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public int getmysqlcom(string M_str_sqlstr)
        {
            MySqlConnection mysqlcon = this.getmysqlcon();
            mysqlcon.Open();
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
            int ret = mysqlcom.ExecuteNonQuery();
            //mysqlcon.Close();
            mysqlcom.Dispose();
            mysqlcon.Close();
            mysqlcon.Dispose();
            return ret;
        }
     
        /// <summary>
        /// 创建一个MySqlDataReader对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <returns>返回MySqlDataReader对象</returns>
        public MySqlDataReader getmysqlread(string M_str_sqlstr)
        {
            MySqlConnection mysqlcon = this.getmysqlcon();
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
            mysqlcon.Open();
            MySqlDataReader mysqlread = mysqlcom.ExecuteReader();
            return mysqlread;
        }
        public void ExecuteTransaction(List<string> lstSql)
        {
            string M_str_sqlcon = string.Format("Server={0};port={1};UserId={2};Password={3};Database={4};pooling=false;CharSet=utf8", IP, Port, UserName, Password, dbName);
            using (MySqlConnection conn = new MySqlConnection(M_str_sqlcon))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                //MySqlCommand cmd = conn.CreateCommand();
                //cmd.Transaction = transaction;

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;
                try
                {
                    foreach (string item in lstSql)
                    {
                        cmd.CommandText = item;
                        int x = cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
