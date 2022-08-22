using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cost.common
{
    public class BimTempData
    {
        private ExternalCommandData _commandData;
        private UIDocument m_revitDoc;
        private Document m_Doc;
        private ElementId m_elemId;
        /// <summary>
        /// 自定义事件处理器
        /// </summary>
        public ExternalEventHandlerBase ExternalEventHandler { get; set; }

        /// <summary>
        /// 自定义事件
        /// </summary>
        public ExternalEvent ExternalEvent { get; set; }

        public ElementId ElemId
        {
            get
            {
                return m_elemId;
            }

            set
            {
                m_elemId = value;
            }
        }

        public BimTempData(ExternalCommandData commandData)
        {
            m_revitDoc = commandData.Application.ActiveUIDocument;
            m_Doc = commandData.Application.ActiveUIDocument.Document;
            this._commandData = commandData;

        }
        /// <summary>
        /// 选择元素
        /// </summary>
        public void PickObject()
        {
            Reference reference = m_revitDoc.Selection.PickObject(ObjectType.Element);
            m_elemId = m_Doc.GetElement(reference).Id;

        }


    }
}
