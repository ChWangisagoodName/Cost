using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Cost
{
    [Transaction(TransactionMode.Manual)]
    public class ExdataDemo : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            //创建一个ribbontab
            //application.CreateRibbonTab("");
            //创建tab下面的panel
            RibbonPanel rp = application.CreateRibbonPanel("DataEx");
            //指定程序集命令
            //string asspath = @"D:\wc\开发\revit二次开发\毕慕造价开发\项目文件\cost\bin\Debug\Cost.dll";
            string asspath = Assembly.GetExecutingAssembly().Location;
            string className = "ArCost.DateIdentify";
            // 创建pushbutton
            PushButtonData pushButtonData = new PushButtonData("firstblood","日期导出", asspath, className);
            //将pushbutton添加到面板中
            PushButton pushButton = rp.AddItem(pushButtonData) as PushButton;
            //给按钮设置个图片
            //string imgpath = @"D:\wc\开发\revit二次开发\日期导出\存钱罐 - 01.png";
           // pushButton.LargeImage = new BitmapImage(new Uri("pack://application:,,,/Cost;component/pic/存钱罐 - 01.png",UriKind.Absolute));
            //给按钮设置信息
            pushButton.ToolTip = "将日期信息写入模型或将信息导出倒数据库";

            return Result.Succeeded;

        }
    }
}
