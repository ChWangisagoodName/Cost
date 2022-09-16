using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Cost.common;
using Cost.Entity;
using Cost.Form;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArCost
{
    [Transaction(TransactionMode.Manual)]
    public class DateIdentify : IExternalCommand
    {
        public ExternalCommandData commandData = null;
        UIDocument uidoc = null;
        UIApplication uiApp = null;
        Document doc = null;
        ExternalEventFc eventFc = null;
        DateIdentityForm dateIdentityForm = null;//窗体
        public FilteredElementCollector paraEle = null;
        public ObservableCollection<DateIdentifyEntity> entities = new ObservableCollection<DateIdentifyEntity>();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {

                this.commandData = commandData;
                uidoc = commandData.Application.ActiveUIDocument;
                uiApp = commandData.Application;
                doc = uidoc.Document;
                eventFc = new ExternalEventFc(doc, null);
                eventFc.RegisterEvent();

               
                
                View view = doc.ActiveView;
                if (!(view is View3D))
                {
                    TaskDialog.Show("阿灿提示", "您现在未在3D视图下，请切换至3D视图在进行后续操作");
                    return Result.Succeeded;
                }
                //收集带有几何图形的族
                List<Element> eleList = new FilteredElementCollector(doc).WherePasses(new ElementIsElementTypeFilter(true)).
                Where(x => x.get_Geometry(new Options { IncludeNonVisibleObjects = false, ComputeReferences = false }) != null).ToList();
                List<ElementId> categories = new List<ElementId>();

                foreach (var item in eleList)
                {
                    if (categories.Contains(item.Category.Id)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_FabricationContainmentCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_FabricationPipeworkCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_FabricationDuctworkCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_ConduitFittingCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_CableTrayFittingCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_ConduitCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_CableTrayCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_PipeFittingCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_DuctFittingCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_FlexPipeCurvesCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_PipeCurvesCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_FlexDuctCurvesCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_DuctCurvesCenterLine)||
                        item.Category == Category.GetCategory(doc, BuiltInCategory.OST_CenterLines)||
                        item.Category.Name=="中心线"
                        ||item.Category.Name.Contains("线")||
                        item.Category.Name.Contains("dwg"))
                    {
                        continue;
                    }
                    else
                    {
                        if (item.Category.Id != null)
                        {
                            categories.Add(item.Category.Id);
                        }
                      
                    }
                }
                StringBuilder sb = new StringBuilder();
                foreach (var item in categories)
                {
                    if (Category.GetCategory(doc, item)!=null)
                    {
                        sb.AppendLine(Category.GetCategory(doc, item).Name );
                    }
                 
                }
                TaskDialog.Show("r",sb.ToString());

               
                






                   paraEle = new FilteredElementCollector(doc, view.Id).WherePasses(new ElementIsElementTypeFilter(true));//收集视图里的族
                List<Element> elemList = paraEle.Where(x => x.LookupParameter("起始日期") != null || x.LookupParameter("终止日期") != null).ToList();



               

                if (elemList.Count() == 0)//先看项目里有没有合适的参数，如果没有就提示然后退出程序
                {
                    using (Transaction tran = new Transaction(doc))
                    {
                        tran.Start("日期参数导入");
                        CreateSharedParm(doc, "data", "起始日期", ParameterType.Text, categories, BuiltInParameterGroup.PG_TEXT);
                        CreateSharedParm(doc, "data", "终止日期", ParameterType.Text, categories, BuiltInParameterGroup.PG_TEXT);
                        tran.Commit();
                    }
                }


                



                dateIdentityForm = new DateIdentityForm(eventFc, commandData, this);
                dateIdentityForm.Show();

            }
            catch (Exception ex)
            {
                TaskDialog.Show("错误信息", ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" );
            }


            return Result.Succeeded;
        }




        public void EXinformation()
        {
            try
            {
                  entities = dateIdentityForm.eleDG.ItemsSource as ObservableCollection<DateIdentifyEntity>;
            
            using (Transaction tran = new Transaction(doc, "录入信息"))
            {
                tran.Start();
              
                foreach (var item in entities)
                {
                  
                    if (item.EleList != null && item.EleList.Count > 0)
                    {
                        foreach (var item2 in item.EleList)
                        {
                            if (/*!string.IsNullOrEmpty(item.StartDate) &&*/ !item2.LookupParameter("起始日期").IsReadOnly)
                            {
                                item2.LookupParameter("起始日期").Set(item.StartDate);
                            }
                            if (/*!string.IsNullOrEmpty(item.EndDate)&&*/!item2.LookupParameter("终止日期").IsReadOnly)
                            {
                                item2.LookupParameter("终止日期").Set(item.EndDate);
                            }
                        }
                    }
                }
                TaskDialog.Show("r","信息录入完毕");
               

                tran.Commit();
            }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("错误信息", ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n");

            }

          
              
        }



        public void updateInfor()
        {
            //连接数据库
            string connectStr = string.Format("Server={0};port={1};UserId={2};Password={3};Database={4};pooling=false;CharSet=utf8", "192.168.10.193", "3306", "root", "root", "rvtdateex");

            using (MySqlConnection conn=new MySqlConnection(connectStr))
            {
                try
                {
                    conn.Open();//数据库打开
                    if (conn.State.ToString() == "Open")
                    {
                     
                    string prName = doc.Title;
                    string del = string.Format("delete from dateex where prName = '{0}';", prName); 
                   
                  

                    //收集所有的元素
                    FilteredElementCollector collertor = new FilteredElementCollector(doc).WherePasses(new ElementIsElementTypeFilter(true));
                    List<Element> elements = collertor.Where(x => (x.LookupParameter("起始日期") != null &&
                    !string.IsNullOrEmpty(x.LookupParameter("起始日期").AsString())) || (x.LookupParameter("终止日期") != null &&
                    !string.IsNullOrEmpty(x.LookupParameter("终止日期").AsString()))).ToList();   //收集需要上传的族



                    //TaskDialog.Show("r",elements.Count().ToString());
                  
             
                  
                    
                        MySqlCommand delcmd = new MySqlCommand(del, conn);
                        int n = delcmd.ExecuteNonQuery();

                        //TaskDialog.Show("r","1111");
                        //添加数据

                        foreach (var item in elements)//循环所有的族
                        {
                            
                            int id = item.Id.IntegerValue;
                            string startDate = item.LookupParameter("起始日期").AsString();
                            string endDate = item.LookupParameter("终止日期").AsString();
                            string a = string.Format("insert into dateex(prName,ID,startDate,endDate) values('{0}','{1}','{2}','{3}')", prName,id,startDate,endDate);  ;
                            MySqlCommand cmd1 = new MySqlCommand(a, conn);

                            int i = cmd1.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        TaskDialog.Show("阿灿提示", "连接数据库失败"+ conn.State.ToString());
                        return;
                    }

                    conn.Close();
                    // 执行事务
                    dateIdentityForm.Close();
                    TaskDialog.Show("阿灿提示","信息录入成功");
                }
                catch (Exception ex)
                {
                    conn.Close();
                    TaskDialog.Show("错误信息", ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n");
                }
                finally
                {
                   
                    conn.Close();
                    dateIdentityForm.Close();
                }
            }
        
           
        }

        public void delInfor()
        {
            //连接数据库
            string connectStr = string.Format("Server={0};port={1};UserId={2};Password={3};Database={4};pooling=false;CharSet=utf8", "192.168.10.193", "3306", "root", "root", "rvtdateex");
            using (MySqlConnection conn = new MySqlConnection(connectStr))
            {
                try
                {
                    conn.Open();//数据库打开
                    if (conn.State.ToString()=="Open")
                    {
                        string prName = doc.Title;
                        string del = string.Format("delete from dateex where prName = '{0}';", prName);
                        MySqlCommand delcmd = new MySqlCommand(del, conn);
                        int n = delcmd.ExecuteNonQuery();
                    }
                    else
                    {
                        TaskDialog.Show("阿灿提示", "连接数据库失败");
                        return;
                    }
                  
                    conn.Close();
                    dateIdentityForm.Close();
                    TaskDialog.Show("阿灿提示", "信息删除完毕");

                }
                catch (Exception ex)
                {
                    conn.Close();
                    TaskDialog.Show("错误信息", ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n");

                }
                finally
                {

                    conn.Close();
                    dateIdentityForm.Close();
                }

            }
            }



        /// <summary>
        /// 增加项目参数
        /// </summary>
        /// <param name="doc"></param>
        /// <param 参数组="paramGroup"></param>
        /// <param 参数名="paramName"></param>
        /// <param 参数类型="paramType"></param>
        /// <param 参数分组方式="listCategoryId"></param>
        /// <param 绑定实例类别="type"></param>
        public static void CreateSharedParm(Document doc, string paramGroup, string paramName, ParameterType paramType, List<ElementId> listCategoryId, BuiltInParameterGroup type)
        {
            // 获取创建共享参数的txt路径
            string txtFile = doc.Application.SharedParametersFilename;
            // 判断路径是否有效，如果为空，创建txt文件将路径赋值给app.SharedParametersFilename
            if (!string.IsNullOrEmpty(txtFile))
            {
                if (!File.Exists(txtFile))
                {
                    System.IO.StreamWriter sw = System.IO.File.CreateText(txtFile);
                    sw.Close();
                }
                DefinitionFile dfile = doc.Application.OpenSharedParameterFile();
                DefinitionGroup dg = dfile.Groups.get_Item(paramGroup);
                if (dg == null)
                    dg = dfile.Groups.Create(paramGroup);// 创建一个共享参数分组
                Definition df = dg.Definitions.get_Item(paramName);
                if (df == null)
                {
                    // 参数创建的选项，包括参数名字，参数类型，用户是不是可以修改。。
                    ExternalDefinitionCreationOptions edco = new ExternalDefinitionCreationOptions(paramName, paramType);
                    // 创建参数
                    df = dg.Definitions.Create(edco);
                }
                BindingMap bmap = doc.ParameterBindings;
                if (!bmap.Contains(df))
                {
                    CategorySet cateSet = doc.Application.Create.NewCategorySet();
                    // 在Category集合中加入所有的category
                    foreach (ElementId bit in listCategoryId)
                    {

                        // 在Category集合中加入线荷载的category
                        if (Category.GetCategory(doc, bit)!=null)
                        {
                            bool flag = cateSet.Insert(Category.GetCategory(doc, bit));
                        }
                      
                    }
                    InstanceBinding loadInsBd = doc.Application.Create.NewInstanceBinding(cateSet);
                    bmap.Insert(df, loadInsBd, type);
                }
            }
        }


    }
}
