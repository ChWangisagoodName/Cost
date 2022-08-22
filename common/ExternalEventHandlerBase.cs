using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cost.common
{
    public delegate void ExecutedDelegate();

   public abstract class ExternalEventHandlerBase : IExternalEventHandler
    {

        #region 构造方法
        public ExternalEventHandlerBase()
        {
        }
        public ExternalEventHandlerBase(Document doc, BimTempData tempdata)
        {
            this.BimTempData = tempdata;
            if (doc != null)
                this.document = doc;

        }
        #endregion

        

        private ExecutedDelegate executedDelegate;//委托回调
        private ExecutedDelegate executedDelegate2;//委托回调
        private UIApplication uiApp = null;//
        private Document document;
        private string eventName;//事件名称
        private BimTempData bimTempData = null;//临时数据
        private bool isExecuted = false;//执行状态
        /// <summary>
        /// 回调列表
        /// </summary>
        List<ExecutedDelegate> ExecutedDelegateList = new List<ExecutedDelegate>();
        private ExternalEvent _exEvent = null;
        /// <summary>
        /// 注册的事件
        /// </summary>
        public ExternalEvent ExternalEvent
        {
            get { return _exEvent; }
        }

       


        /// <summary>
        /// 注册事件--不可用
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        private ExternalEvent RegisterEvent(IExternalEventHandler handler)
        {
            ExternalEvent exEvent = ExternalEvent.Create(handler);
            this._exEvent = exEvent;
            return exEvent;
        }
        /// <summary>
        /// 提交事件
        /// </summary>
        /// <returns></returns>
        public ExternalEventRequest Raise()
        {
            ExternalEventRequest eventReq = _exEvent.Raise();
            return eventReq;
        }

        public ExternalEventRequest TriggerExecute()
        {
            return Raise();
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <returns></returns>
        public ExternalEvent RegisterEvent()
        {
            ExternalEvent exEvent = ExternalEvent.Create(this);
            this._exEvent = exEvent;
            return exEvent;
        }


        /// <summary>
        /// 是否执行完毕
        /// </summary>
        public bool IsExecuted
        {
            get { return isExecuted; }
        }

        /// <summary>
        /// 实现接口方法
        /// </summary>
        /// <param name="uiapp"></param>
        public void Execute(UIApplication uiapp)
        {
            UiApp = uiapp;
            UIDocument uidoc = uiapp.ActiveUIDocument;//.ActiveDocument;
            if (null == uidoc)
            {
                return; // no document, nothing to do
            }
            document = uidoc.Document;
            //执行前
            bool flag = BeforExecute();
            if (!flag)
            {
                return;
            }
            //执行
            Execute();

            //执行完毕之后,委托回调
            AfterExecute();
        }

        /// <summary>
        /// 执行前，子类可重写 overwrite
        /// </summary>
        /// <returns></returns>
        public virtual bool BeforExecute()
        {
            isExecuted = false;
            return true;
        }
        /// <summary>
        /// 子类必须实现
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// 执行完毕之后执行操作,委托回调
        /// </summary>
        /// <returns></returns>
        public virtual bool AfterExecute()
        {

            isExecuted = true;

            if (ExecutedDelegate != null)
            {
                ExecutedDelegate.Invoke();
            }
            if (executedDelegate2 != null)
            {
                executedDelegate2.Invoke();
            }
            if (ExecutedDelegateList != null && ExecutedDelegateList.Count > 0)
            {
                foreach (var item in ExecutedDelegateList)
                {
                    item.Invoke();
                }
            }
            return true;
        }


       
        public UIApplication UiApp
        {
            get { return uiApp; }
            set { uiApp = value; }
        }

        public Document Document
        {
            get { return document; }
            set { document = value; }
        }

        public string EventName
        {
            set { eventName = value; }
        }


        public BimTempData BimTempData
        {
            get { return bimTempData; }
            set { bimTempData = value; }
        }

        public void ClearExecutedDelegate()
        {
            executedDelegate = null;
            executedDelegate2 = null;
            ExecutedDelegateList.Clear();
        }

        public void AddExecutedDelegate(ExecutedDelegate e)
        {
            ExecutedDelegateList.Add(e);
        }
        /// <summary>
        /// 委托，回调
        /// </summary>
        public ExecutedDelegate ExecutedDelegate
        {
            get { return executedDelegate; }
            set { executedDelegate = value; }
        }
        /// <summary>
        /// 委托，回调
        /// </summary>
        public ExecutedDelegate ExecutedDelegate2
        {
            get { return executedDelegate2; }
            set { executedDelegate2 = value; }
        }
        public string GetName()
        {
            return eventName;
        }
    }
   
}
