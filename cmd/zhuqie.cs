using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Cost.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArCost
{
    [Transaction(TransactionMode.Manual)]
    public class zhuqie : IExternalCommand
    {
        UIDocument uidoc = null;
        UIApplication uiApp = null;
        Document doc = null;
        ExternalEventFc eventFc = null;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uidoc = commandData.Application.ActiveUIDocument;
            uiApp = commandData.Application;
            doc = uidoc.Document;
            eventFc = new ExternalEventFc(doc, null);
            eventFc.RegisterEvent();

             
         try
            {
                using (var ts = new Transaction(doc, "创建模板"))
                {
                  
                    var reference = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, "选择元素");
                    List<FamilyInstance> famList = new List<FamilyInstance>();
                    foreach (var item in reference)
                    {
                        famList.Add(doc.GetElement(item) as FamilyInstance);
                    }
                    FilteredElementCollector collector = new FilteredElementCollector(doc);
                    IList<Element> levelList = collector.OfClass(typeof(Level)).ToElements();
                    foreach (var fam in famList)
                    {
                        ElementId elementId = fam.Id;
                        GetItem(elementId, levelList);

                        //foreach (Level level in levelList)
                        //{
                        //    ts.Start();
                        //    Line line = (fam.Location as LocationCurve).Curve as Line;
                        //    double p1z = line.GetEndPoint(0).Z;
                        //    double p2z = line.GetEndPoint(1).Z;
                        //    double p3 = 0;
                        //    if (p1z > p2z)
                        //    {
                        //        p3 = p1z;
                        //        p1z = p2z;
                        //        p2z = p3;
                        //    }
                        //    double h = level.ProjectElevation;

                        //    if (p1z < h || p2z > h)
                        //    {
                        //        double b =  (h - p1z)/(p2z - p1z) ;
                        //        XYZ start = XYZ.Zero;

                        //        if (b >= 0.05&& b < 1)
                        //        {


                        //         ElementId elementId=   fam.Split(b);






                        //        }
                        //    }
                        //    ts.Commit();
                        //}

                    }
                  
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("错误信息", ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" );
            }


            return Result.Succeeded;
        }






        private void GetItem(ElementId elementId, IList<Element> levelList)
        {
            ElementId elementId1=null;
            if (elementId!=null)
            {
                FamilyInstance fam = doc.GetElement(elementId) as FamilyInstance;
                using (var ts = new Transaction(doc, "创建模板"))
                {
                    foreach (Level level in levelList)
                    {
                        ts.Start();
                        if (fam.Location is LocationPoint)
                        {
                            continue;
                        }
                        if (fam.Location as LocationCurve==null)
                        {
                            continue;

                        }
                        Line line = (fam.Location as LocationCurve).Curve as Line;
                        double p1z = line.GetEndPoint(0).Z;
                        double p2z = line.GetEndPoint(1).Z;
                        double p3 = 0;
                        if (p1z > p2z)
                        {
                            p3 = p1z;
                            p1z = p2z;
                            p2z = p3;
                        }
                        double h = level.ProjectElevation;

                        if (p1z < h || p2z > h)
                        {
                            double b = (h - p1z) / (p2z - p1z);
                            XYZ start = XYZ.Zero;

                            if (b >= 0.1 && b < 0.98)
                            {

                                
                                 elementId1 = fam.Split(b);
                                #region  改参照标高

                                //fam.get_Parameter(BuiltInParameter.SCHEDULE_TOP_LEVEL_PARAM).Set(level.Id);//顶部标高

                                //Level baseLevel = null;
                                //foreach (Level levle2 in levelList)
                                //{
                                //    if (baseLevel==null)
                                //    {
                                //        baseLevel = levle2;
                                //    }
                                //    else
                                //    {
                                //        if (levle2.ProjectElevation>=level.ProjectElevation)
                                //        {
                                //            continue;
                                //        }
                                //        else if (levle2.ProjectElevation>=baseLevel.ProjectElevation)
                                //        {
                                //            baseLevel = levle2;
                                //        }


                                //    }
                                //}

                                //if (level.Id.IntegerValue != baseLevel.Id.IntegerValue && baseLevel.ProjectElevation < level.ProjectElevation&&baseLevel.Elevation<level.Elevation)
                                //{
                                //    fam.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).Set(baseLevel.Id);
                                //}



                                //if (!fam.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).IsReadOnly)
                                //{
                                //    fam.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).Set(-50 / 304.8);
                                //}
                                //fam.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).Set(-50 / 304.8);
                                #endregion

                            }
                            #region 该参照标高
                            //else if (fam.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId().IntegerValue == level.Id.IntegerValue)
                            //{

                            //    Level baseLevel = null;
                            //    foreach (Level levle2 in levelList)
                            //    {
                            //        if (baseLevel == null)
                            //        {
                            //            baseLevel = levle2;
                            //        }
                            //        else
                            //        {
                            //            if (levle2.ProjectElevation >= level.ProjectElevation)
                            //            {
                            //                continue;
                            //            }
                            //            else if (levle2.ProjectElevation >= baseLevel.ProjectElevation)
                            //            {
                            //                baseLevel = levle2;
                            //            }
                            //        }
                            //    }

                            //    if (level.Id.IntegerValue != baseLevel.Id.IntegerValue && baseLevel.ProjectElevation < level.ProjectElevation && baseLevel.Elevation < level.Elevation)
                            //    {
                            //        fam.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).Set(baseLevel.Id);
                            //    }

                            //    if (!fam.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).IsReadOnly)
                            //    {
                            //        fam.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).Set(-50 / 304.8);
                            //    }
                            //}
#endregion
                        }
                        ts.Commit();
                    }
                }
                GetItem(elementId1, levelList);
            }
          
        }

    }
}
