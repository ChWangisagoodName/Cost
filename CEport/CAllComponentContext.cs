
using Autodesk.Revit.DB;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CExportscope
{
    public class CAllComponentContext : IExportContext
    {
        public Document Document { get; set; }
        List<string> lstSql = new List<string>();
        CSqlString strSql = null;
        long model_name_id = -1;
        CMysql mysql = new CMysql();
        public Dictionary<int, string> DicLevel = new Dictionary<int, string>();
        public string Modelname { get; set; }
        public string IP { get; set; }
        public short Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public void Finish()
        {

            foreach (int levelid in DicLevel.Keys)
            {
                lstSql.Add(string.Format("insert into t_level_name(level_id,level_name,model_name_id) values({0},'{1}',{2})", levelid, DicLevel[levelid], model_name_id));
            }
            mysql.ExecuteTransaction(lstSql);
        }

        public bool IsCanceled()
        {
            return false;
        }


        //public void OnDaylightPortal(DaylightPortalNode node)
        //{
        //    //Debug.WriteLine(node.NodeName);
        //}

        public string GetParameterValue(Parameter para)
        {
            string str = para.AsValueString();
            if (str == null)
            {
                switch (para.StorageType)
                {
                    case StorageType.None:
                        str = "";
                        break;
                    case StorageType.Integer:
                        str = para.AsInteger().ToString();
                        break;
                    case StorageType.Double:
                        str = para.AsDouble().ToString();
                        break;
                    case StorageType.String:
                        str = para.AsString();
                        break;
                    case StorageType.ElementId:
                        str = para.AsElementId().ToString();
                        break;
                }
            }
            if (str == null)
            {
                str = "";
            }
            return str;
        }
        public RenderNodeAction OnElementBegin(ElementId elementId)
        {
            try
            {
                strSql = new CSqlString();
                strSql.IsExistGeometry = false;
                Element ele = Document.GetElement(elementId);
                strSql.RevitID = elementId.IntegerValue;
                strSql.FamilyName = ((BuiltInCategory)(ele.Category.Id.IntegerValue)).ToString();
                Element elementType = Document.GetElement(ele.GetTypeId());
                if (elementType != null)
                {
                    Parameter para = elementType.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS);//类型注释
                    strSql.FamilyAnnotation = para != null ? GetParameterValue(para) : "";
                }
                if (ele is FamilyInstance)
                {
                    FamilyInstance instance = ele as FamilyInstance;
                    Parameter para = instance.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS);//类型注释
                    strSql.FamilyAnnotation = para != null ? GetParameterValue(para) : "";
                }
                Element group = Document.GetElement(ele.GroupId);
                Parameter para1 = group == null ? null : group.get_Parameter(BuiltInParameter.GROUP_LEVEL);//参照标高
                if (para1 != null)
                {
                    if (para1.AsElementId().IntegerValue == -1)
                    {
                        strSql.Floor = para1.AsElementId().IntegerValue;
                        goto ToContinue;
                    }
                }
                if (ele.LevelId.IntegerValue != -1)
                {
                    strSql.Floor = ele.LevelId.IntegerValue;
                    goto ToContinue;
                }
                para1 = ele.get_Parameter(BuiltInParameter.STAIRS_BASE_LEVEL_PARAM);//底部标高
                if (para1 != null)
                {
                    if (para1.AsElementId().IntegerValue == -1)
                    {
                        strSql.Floor = para1.AsElementId().IntegerValue;
                        goto ToContinue;
                    }
                }
                para1 = ele.get_Parameter(BuiltInParameter.STAIRS_RAILING_BASE_LEVEL_PARAM);//底部标高
                if (para1 != null)
                {
                    if (para1.AsElementId().IntegerValue == -1)
                    {
                        strSql.Floor = para1.AsElementId().IntegerValue;
                        goto ToContinue;
                    }
                }
                para1 = ele.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);//参照标高
                if (para1 != null)
                {
                    if (para1.AsElementId().IntegerValue == -1)
                    {
                        strSql.Floor = para1.AsElementId().IntegerValue;
                        goto ToContinue;
                    }
                }
                para1 = ele.get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM);//明细表标高
                if (para1 != null)
                {
                    if (para1.AsElementId().IntegerValue == -1)
                    {
                        strSql.Floor = para1.AsElementId().IntegerValue;
                        goto ToContinue;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(elementId.IntegerValue+"\n"+ex.Message + "\n" + ex.StackTrace);
            }
ToContinue:       
            return RenderNodeAction.Proceed;
        }

        public void OnElementEnd(ElementId elementId)
        {
           if(strSql.IsExistGeometry)
           {
               lstSql.Add(string.Format("insert into t_all_component(component_id,component_familyname,component_family_annotation,level_id,model_name_id,component_state) values({0},'{1}','{2}',{3},{4},{5})",
                   strSql.RevitID, strSql.FamilyName, strSql.FamilyAnnotation, strSql.Floor, model_name_id,0));
           }
        }

        public RenderNodeAction OnFaceBegin(FaceNode node)
        {
            return RenderNodeAction.Proceed;
        }

        public void OnFaceEnd(FaceNode node)
        {
           
        }

        public RenderNodeAction OnInstanceBegin(InstanceNode node)
        {
            return RenderNodeAction.Proceed;
        }

        public void OnInstanceEnd(InstanceNode node)
        {
            
        }

        public void OnLight(LightNode node)
        {
          
        }

        public RenderNodeAction OnLinkBegin(LinkNode node)
        {
            return RenderNodeAction.Proceed;
        }

        public void OnLinkEnd(LinkNode node)
        {
           
        }

        public void OnMaterial(MaterialNode node)
        {
            
        }

        public void OnPolymesh(PolymeshTopology node)
        {
            strSql.IsExistGeometry = true;
        }

        public void OnRPC(RPCNode node)
        {
           
        }

        public RenderNodeAction OnViewBegin(ViewNode node)
        {
            return RenderNodeAction.Proceed;
        }

        public void OnViewEnd(ElementId elementId)
        {
          
        }

        public bool Start()
        {
            string str = string.Format("select model_name_id,model_name from t_model_name where model_name='{0}'", Modelname);
            mysql.IP = IP;
            mysql.Port = Port;
            mysql.UserName = UserName;
            mysql.Password = Password;
            int ret = mysql.getmysqlcom(str);
            if (ret > 0)
            {
                MessageBox.Show("模型名已经存在！");
                return true;
            }
            str = string.Format("insert into t_model_name(model_name)  values('{0}')", Modelname);
            ret = mysql.getmysqlcom(str);
            if (ret == 0)
            {
                MessageBox.Show("模型名插入失败！");
                return true;
            }
            str = string.Format("select model_name_id,model_name from t_model_name where model_name='{0}'", Modelname);
            MySqlDataReader dr = mysql.getmysqlread(str);

            if (dr.Read())
            {
                model_name_id = dr.GetInt64("model_name_id");
                dr.Close();
            }
            return true;
        }
    }
}





