using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cost.common
{
    public class ExternalEventFc : ExternalEventHandlerBase
    {
        public ExternalEventFc() { }

        public ExternalEventFc(Document doc, BimTempData tempData)
           : base(doc, tempData) { }

        public override void Execute()
        {
            UIDocument uidoc = UiApp.ActiveUIDocument;
            if (null == uidoc)
            {
                return;
            }
            Document doc = uidoc.Document;
        }
    }
}
