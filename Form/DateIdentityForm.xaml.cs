using ArCost;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Cost.common;
using Cost.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cost.Form
{
    /// <summary>
    /// DateIdentityForm.xaml 的交互逻辑
    /// </summary>
    public partial class DateIdentityForm : Window
    {

        ExternalCommandData commandData = null;
        Document doc = null;
        ExternalEventFc eventFc = null;
        DateIdentify cmd = null;

        public ObservableCollection<DateIdentifyEntity> entities = new ObservableCollection<DateIdentifyEntity>();
        public DateIdentityForm()
        {
            InitializeComponent();
        }

        public DateIdentityForm(ExternalEventFc _eventFc, ExternalCommandData _commandData, DateIdentify _cmd)
        {
            InitializeComponent();
            this.commandData = _commandData;
            this.doc = _commandData.Application.ActiveUIDocument.Document;
            this.eventFc = _eventFc;
            this.cmd = _cmd;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                form.Height = 450;//设定窗口高度

                //收集的结构柱
                List<Element> columnList = cmd.paraEle.Where(x => (x is FamilyInstance) && x.Category.Id.IntegerValue == -2001330).ToList();
                //收集的结构梁
                List<Element> BeamList = cmd.paraEle.Where(x => (x is FamilyInstance) && x.Category.Id.IntegerValue == -2001320).ToList();
                //收集的结构板
                List<Element> FloorList = cmd.paraEle.Where(x => (x is Floor) && x.LookupParameter("结构").AsInteger() == 1).ToList();
                //收集结构墙
                List<Element> strWall = cmd.paraEle.Where(x => (x is Wall) && x.LookupParameter("结构").AsInteger() == 1).ToList();
                //收集结构墙
                List<Element> ArWall = cmd.paraEle.Where(x => (x is Wall) && x.LookupParameter("结构").AsInteger() == 0).ToList();


                if (columnList.Count > 0)
                {
                    List<Element> columnList1 = columnList.Where(x => x.LookupParameter("起始日期").AsString() != null
                    || x.LookupParameter("终止日期").AsString() != null).ToList();//有数据的直接体现在窗体上
                    if (columnList1.Count()>0)
                    {
                        entities.Add(new DateIdentifyEntity("结构柱", columnList, 
                      columnList1.FirstOrDefault().LookupParameter("起始日期").AsString(), 
                      columnList1.FirstOrDefault().LookupParameter("终止日期").AsString()));
                    }
                    else
                    {
                        entities.Add(new DateIdentifyEntity("结构柱", columnList, null, null));
                    }
                    
                }
                if (BeamList.Count > 0)
                {
                    List<Element> columnList1 = BeamList.Where(x => x.LookupParameter("起始日期").AsString() != null
                   || x.LookupParameter("终止日期").AsString() != null).ToList();
                    if (columnList1.Count() > 0)
                    {
                        entities.Add(new DateIdentifyEntity("结构梁", BeamList,
                      columnList1.FirstOrDefault().LookupParameter("起始日期").AsString(),
                      columnList1.FirstOrDefault().LookupParameter("终止日期").AsString()));
                    }
                    else
                    {
                        entities.Add(new DateIdentifyEntity("结构梁", BeamList, null, null));
                    }
                    
                }
                if (FloorList.Count > 0)
                {
                    List<Element> columnList1 = FloorList.Where(x => x.LookupParameter("起始日期").AsString() != null
                  || x.LookupParameter("终止日期").AsString() != null).ToList();
                    if (columnList1.Count>0)
                    {
                        entities.Add(new DateIdentifyEntity("结构板", FloorList,
                         columnList1.FirstOrDefault().LookupParameter("起始日期").AsString(),
                         columnList1.FirstOrDefault().LookupParameter("终止日期").AsString()));

                    }
                    else
                    {
                        entities.Add(new DateIdentifyEntity("结构板", FloorList, null, null));
                    }
                   
                }
                if (strWall.Count > 0)
                {
                    List<Element> columnList1 = strWall.Where(x => x.LookupParameter("起始日期").AsString() != null
               || x.LookupParameter("终止日期").AsString() != null).ToList();
                    if (columnList1.Count > 0)
                    {
                        entities.Add(new DateIdentifyEntity("结构墙", strWall,
                         columnList1.FirstOrDefault().LookupParameter("起始日期").AsString(),
                         columnList1.FirstOrDefault().LookupParameter("终止日期").AsString()));

                    }
                    else
                    {
                        entities.Add(new DateIdentifyEntity("结构墙", strWall, null, null));
                    }
                  
                }
                if (ArWall.Count > 0)
                {
                    List<Element> columnList1 = ArWall.Where(x => x.LookupParameter("起始日期").AsString() != null
                || x.LookupParameter("终止日期").AsString() != null).ToList();
                    if (columnList1.Count > 0)
                    {
                        entities.Add(new DateIdentifyEntity("建筑墙", ArWall,
                         columnList1.FirstOrDefault().LookupParameter("起始日期").AsString(),
                         columnList1.FirstOrDefault().LookupParameter("终止日期").AsString()));

                    }
                    else
                    {
                        entities.Add(new DateIdentifyEntity("建筑墙", ArWall, null, null));
                    }
                   
                }
                //收集管道和风管
                List<string> piepStrList = new List<string>();
                List<Element> pipeList = cmd.paraEle.Where(x => x.LookupParameter("系统类型") != null).ToList();//把管道的收集起来
                foreach (var item in pipeList)
                {

                    if (piepStrList.Contains(item.LookupParameter("系统类型").AsValueString()))
                    {
                        continue;
                    }
                    else
                    {
                        piepStrList.Add(item.LookupParameter("系统类型").AsValueString());
                    }

                }

                foreach (var item in piepStrList)
                {
                    List<Element> Pipe2List = cmd.paraEle.Where(x => x.LookupParameter("系统类型") != null
                    && x.LookupParameter("系统类型").AsValueString().Equals(item)).ToList();
                    List<Element> columnList1 = Pipe2List.Where(x => x.LookupParameter("起始日期").AsString() != null
                     || x.LookupParameter("终止日期").AsString() != null).ToList();
                    if (columnList1.Count > 0)
                    {
                        entities.Add(new DateIdentifyEntity(item, Pipe2List,
                         columnList1.FirstOrDefault().LookupParameter("起始日期").AsString(),
                         columnList1.FirstOrDefault().LookupParameter("终止日期").AsString()));

                    }
                    else
                    {
                        entities.Add(new DateIdentifyEntity(item, Pipe2List, null, null));
                    }
                    
                }


                //收集桥架类型
                List<string> trayStrList = new List<string>();
                List<Element> trayList = cmd.paraEle.Where(x => x.LookupParameter("设备类型") != null).ToList();

                foreach (var item in trayList)//手机桥架名称
                {

                    if (trayStrList.Contains(item.LookupParameter("设备类型").AsString()))
                    {
                        continue;
                    }
                    else
                    {
                        trayStrList.Add(item.LookupParameter("设备类型").AsString());
                    }

                }

                foreach (var item in trayStrList)
                {
                    List<Element> tray2List = cmd.paraEle.Where(x => x.LookupParameter("设备类型") != null
                    && x.LookupParameter("设备类型").AsString().Equals(item)).ToList();

                    List<Element> columnList1 = tray2List.Where(x => x.LookupParameter("起始日期").AsString() != null
                    || x.LookupParameter("终止日期").AsString() != null).ToList();
                    if (columnList1.Count > 0)
                    {
                        entities.Add(new DateIdentifyEntity(item, tray2List,
                         columnList1.FirstOrDefault().LookupParameter("起始日期").AsString(),
                         columnList1.FirstOrDefault().LookupParameter("终止日期").AsString()));

                    }
                    else
                    {
                        entities.Add(new DateIdentifyEntity(item, tray2List, null, null));
                    }
                    
                }




                if (entities.Count>0)
                {
                    eleDG.ItemsSource = null;
                    eleDG.ItemsSource = entities;
                }
                else
                {
                    TaskDialog.Show("阿灿提示", "未收集到构件，将正确的构建隔离在重新操作");
                    form.Close();
                }


            }
            catch (Exception ex)
            {

                TaskDialog.Show("错误信息", ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n");
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            eventFc.ClearExecutedDelegate();
            eventFc.ExecutedDelegate = new ExecutedDelegate(cmd.EXinformation);
            eventFc.TriggerExecute();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            eventFc.ClearExecutedDelegate();
            eventFc.ExecutedDelegate = new ExecutedDelegate(cmd.updateInfor);
            eventFc.TriggerExecute();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            eventFc.ClearExecutedDelegate();
            eventFc.ExecutedDelegate = new ExecutedDelegate(cmd.delInfor);
            eventFc.TriggerExecute();
        }
    }
}
