
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Cost.common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CExportscope
{

    [Transaction(TransactionMode.Manual)]

    public class RvtDateEx : IExternalCommand
    {
        //CMysql mysql = new CMysql();
        ExternalEventFc eventFc = null;
        
        Document doc = null;
        
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
         
            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uiDoc = uiApp.ActiveUIDocument;
                Autodesk.Revit.ApplicationServices.Application app = uiApp.Application;
                Document document = uiDoc.Document;
                doc = document;
                
                FormRecordData formRecordData = new FormRecordData();
                //formRecordData.ConnectDBEvent += formRecordData_ConnectDBEvent;
                //formRecordData.InitiateDBEvent += formRecordData_InitiateDBEvent;
                //formRecordData.UpdateDBEvent += formRecordData_UpdateDBEvent;
                // formRecordData.UpdateDBEvent += formRecordData_UpdateDBEvent;
                formRecordData.UpdateDBEvent += formRecordData_UpdateDBEvent;
                formRecordData.DeleteAllDataDBEvent += formRecordData_DeleteAllDataDBEvent;
                Dictionary<int, string> dicLevel = new Dictionary<int, string>();
                formRecordData.ShowDialog();
               
            }
            catch (Exception ex)
            {
                TaskDialog.Show("错误信息", ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n");

            }
            return Result.Succeeded;
        }

        public void formRecordData_DeleteAllDataDBEvent(object sender, EventArgs e)
        {
            FormRecordData formRecordData = sender as FormRecordData;
            CMysql mysql = new CMysql();
            mysql.IP = formRecordData.IP;
            mysql.Port = formRecordData.Port;
            mysql.UserName = formRecordData.UserName;
            mysql.Password = formRecordData.Password;
            // mysql.Modelname = formRecordData.Modelname;
            string str = string.Format("select model_name_id,model_name from t_model_name where model_name='{0}'", formRecordData.Modelname);
            MySqlDataReader dr = mysql.getmysqlread(str);
            long model_name_id = -1;
            if (dr.Read())
            {
                model_name_id = dr.GetInt64("model_name_id");
                dr.Close();
            }
            else
            {
                MessageBox.Show("模型不存在！");
            }
            List<string> lstSql = new List<string>();
            lstSql.Add(string.Format("delete from t_all_component where model_name_id={0}",model_name_id));
            lstSql.Add(string.Format("delete from t_level_name where model_name_id={0}", model_name_id));
            lstSql.Add(string.Format("delete from t_model_name where model_name_id={0}", model_name_id));
            mysql.ExecuteTransaction(lstSql);
        }
        /// <summary>
        /// 点击确定的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="doc"></param>
        public void formRecordData_UpdateDBEvent(object sender, EventArgs e)
        {
            FormRecordData formRecordData = sender as FormRecordData;
            //mysql.IP = formRecordData.IP;
            //mysql.Port = formRecordData.Port;
            //mysql.UserName = formRecordData.UserName;
            //mysql.Password = formRecordData.Password;
            FilteredElementCollector elements = new FilteredElementCollector(doc);
            TaskDialog.Show("r", elements.Count().ToString());
        }

        public void formRecordData_InitiateDBEvent(object sender, EventArgs e)
        {
            FormRecordData formRecordData = sender as FormRecordData;
            //mysql.IP = formRecordData.IP;
            //mysql.Port = formRecordData.Port;
            //mysql.UserName = formRecordData.UserName;
            //mysql.Password = formRecordData.Password
        }

        public void formRecordData_ConnectDBEvent(object sender, EventArgs e)
        {
            FormRecordData formRecordData = sender as FormRecordData;
            //mysql.IP = formRecordData.IP;
            //mysql.Port = formRecordData.Port;
            //mysql.UserName = formRecordData.UserName;
            //mysql.Password = formRecordData.Password;

        }
    }
}
