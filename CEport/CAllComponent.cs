
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
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

    public class CAllComponent : IExternalCommand
    {
        //CMysql mysql = new CMysql();
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiApp.Application;
            Document document = uiDoc.Document;
            FormRecordData formRecordData = new FormRecordData();
            //formRecordData.ConnectDBEvent += formRecordData_ConnectDBEvent;
            //formRecordData.InitiateDBEvent += formRecordData_InitiateDBEvent;
            //formRecordData.UpdateDBEvent += formRecordData_UpdateDBEvent;
            formRecordData.DeleteAllDataDBEvent+=formRecordData_DeleteAllDataDBEvent;
            Dictionary<int, string> dicLevel = new Dictionary<int, string>();
            formRecordData.ShowDialog();
            if(formRecordData.DBstate==State.Initiate)
            {
                //ElementSet elemSet1 = new ElementSet();

                FilteredElementCollector fec = new FilteredElementCollector(document);
                ElementClassFilter levelFilter = new ElementClassFilter(typeof(Level));
                fec.WherePasses(levelFilter);
                List<Element> eles = fec.ToElements() as List<Element>;

                //foreach (Element element in eles)
                //{
                //    elemSet1.Insert(element);
                //}
                foreach (Element item in eles)
                {
                    dicLevel.Add(item.Id.IntegerValue,item.Name);
                }

                CAllComponentContext allComponentContext = new CAllComponentContext();
                allComponentContext.Document = document;
                allComponentContext.DicLevel = dicLevel;
                allComponentContext.IP = formRecordData.IP;
                allComponentContext.Port = formRecordData.Port;
                allComponentContext.UserName = formRecordData.UserName;
                allComponentContext.Password = formRecordData.Password;
                allComponentContext.Modelname = formRecordData.Modelname;
                CustomExporter exporter = new CustomExporter(document, allComponentContext);
                exporter.Export(document.ActiveView as View3D);
            }
            else if(formRecordData.DBstate==State.UpdateToComplete)
            {
                IList<Reference> lstReference1 = uiDoc.Selection.PickObjects(ObjectType.Element, "请选择所要更新的构件");
                //IList<Element> lstReference2 = uiDoc.Selection.PickElementsByRectangle("请选择所要更新的构件");
                List<int> lstID = new List<int>();
                foreach (Reference reference in lstReference1)
                {
                    lstID.Add(reference.ElementId.IntegerValue);
                }
                //foreach (Element element in lstReference2)
                //{
                //    lstID.Add(element.Id.IntegerValue);
                //}
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
                    return Result.Succeeded;
                }
                List<string> lstSql = new List<string>();
                foreach (int id in lstID)
                {
                    lstSql.Add(string.Format("update t_all_component set component_state=1 where component_id={0} and model_name_id={1}", id, model_name_id));
                }
                mysql.ExecuteTransaction(lstSql);
            }
            else if (formRecordData.DBstate == State.UpdateToInitiate)
            {
                IList<Reference> lstReference1 = uiDoc.Selection.PickObjects(ObjectType.Element, "请选择所要更新的构件");
                //IList<Element> lstReference2 = uiDoc.Selection.PickElementsByRectangle("请选择所要更新的构件");
                List<int> lstID = new List<int>();
                foreach (Reference reference in lstReference1)
                {
                    lstID.Add(reference.ElementId.IntegerValue);
                }
                //foreach (Element element in lstReference2)
                //{
                //    lstID.Add(element.Id.IntegerValue);
                //}
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
                    return Result.Succeeded;
                }
                List<string> lstSql = new List<string>();
                foreach (int id in lstID)
                {
                    lstSql.Add(string.Format("update t_all_component set component_state=0 where component_id={0} and model_name_id={1}", id, model_name_id));
                }
                mysql.ExecuteTransaction(lstSql);
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

        public void formRecordData_UpdateDBEvent(object sender, EventArgs e)
        {
            FormRecordData formRecordData = sender as FormRecordData;
            //mysql.IP = formRecordData.IP;
            //mysql.Port = formRecordData.Port;
            //mysql.UserName = formRecordData.UserName;
            //mysql.Password = formRecordData.Password;
        }

        public void formRecordData_InitiateDBEvent(object sender, EventArgs e)
        {
            FormRecordData formRecordData = sender as FormRecordData;
            //mysql.IP = formRecordData.IP;
            //mysql.Port = formRecordData.Port;
            //mysql.UserName = formRecordData.UserName;
            //mysql.Password = formRecordData.Password;
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
