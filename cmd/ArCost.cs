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
    public class arcost : IExternalCommand
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
                Reference reference = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "选择元素");
                FamilyInstance element = doc.GetElement(reference) as FamilyInstance;



                //柱子的代码和方法，目前未写高支模
                // double perimeter = GetRectangularColumnPerimeter(element);//获取柱子边长  单位米
                // double n=  element.GetSweptProfile().GetDrivingCurve().Length*304.8;//获取柱子高度  单位毫米
                // double volume = GetRectangularVolume(element);//获取柱子体积  单位立方米
                //double areaOfScaffold=GetRectangularColumnAreaOfScaffold(element);  //获取柱子的脚手架面积单位平方米
                //double columnTemplateArea = GetColumnTemplateArea(element);//获取柱子的模板面积 单位平方米
                // double columnCrossSectionArea= GetColumnCrossSectionArea(element);//获取柱子的截面面积 单位平方米


                //梁的代码
                //double beamPerimeter = GetbeamPerimeter(element);//获取梁的边长 单位米
                // double beamAxisLength = UnitUtils.ConvertFromInternalUnits(element.GetSweptProfile().GetDrivingCurve().Length, DisplayUnitType.DUT_METERS);//获取梁的轴线长度  单位 米 ;
                // double BeamLength = GetBeamLength(element);//获取梁的净长度  单位米
                // double BeamTemplateArea = GetBeamTemplateArea(element);//获取梁的模板面积 单位平方米
                // double BeamFlankArea =GetBeamFlankArea(element);



                TaskDialog.Show("r", "");


                //墙的代码
                //double wallLength = GetWallLength(element);//获取墙的长度 单位米
                // double wallHigh = GetWallHigh(element);//获取墙的高度 单位米
                //double wallWidth = GetWallWidth(element);//获取墙的宽度 单位米
                // double wallVolum = GetWallVolum(element);//获取墙的体积 单位立方米
                //double wallScaffoldArea = GetWallScaffoldArea(element); //获取墙的脚手架面积 单位平方米
                // double wallTemplateArea = GetWallTemplateArea(element);//获取墙的模板面积 单位平方米
                //TaskDialog.Show("r", wallScaffoldArea.ToString());






            }
            catch (Exception ex)
            {
                TaskDialog.Show("错误信息", ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace + "\n" );
            }


            return Result.Succeeded;
        }



        #region  计算柱子的方法，目前缺个高支模

        /// <summary>
        /// 获取柱子的周长 单位毫米
        /// </summary>
        /// <param 获取的柱子="element"></param>
        /// <returns>考虑柱子都是直的，且不会扣减，所以按照找到柱子最底层的面的周长加起来就是周长</returns>
        public double GetRectangularColumnPerimeter(FamilyInstance element )
        {

            Options options = new Options() { ComputeReferences = false, IncludeNonVisibleObjects = false };
            GeometryElement eleGeo = element.get_Geometry(options);

            List<Face> listFace = new List<Face>();//面的集合
            foreach (GeometryObject geomObj in eleGeo)
            {
                GeometryInstance geomInstance = geomObj as GeometryInstance;
                if (geomInstance != null)
                {
                    foreach (GeometryObject instObj in geomInstance.GetInstanceGeometry())
                    {

                        Solid instsolid = instObj as Solid;
                        if (instsolid == null || instsolid.Faces.Size == 0 || instsolid.Edges.Size == 0)
                        {
                            continue;
                        }
                        foreach (Face face in instsolid.Faces)
                        {
                            listFace.Add(face);
                        }
                    }
                }
                Solid solid = geomObj as Solid;
                if (solid != null)
                {
                    if (solid.Faces.Size == 0 || solid.Edges.Size == 0)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (Face face in solid.Faces)
                        {
                            listFace.Add(face);
                        }
                    }
                }
            }//获取所有面 网上抄的

            double n = 0;
            foreach (PlanarFace face in listFace)
            {
                
                if (face.FaceNormal.Z == 1)
                {
                    face.FaceNormal.Normalize();
                    foreach (var item in face.GetEdgesAsCurveLoops().First())
                    {
                        n += item.Length;

                    }
                    break;
                }
            }
            n = UnitUtils.ConvertFromInternalUnits(n, DisplayUnitType.DUT_METERS);
            n = double.Parse(Math.Round(n, 2).ToString());
            return n;
        }

        /// <summary>
        /// 获取族的体积 单位立方米
        /// </summary>
        /// <param 获取的族="element"></param>
        /// <returns>直接获取的族的体积，然后换算</returns>
        public double GetRectangularVolume(FamilyInstance element)
        {
            double n = 0;
            Options options = new Options() { ComputeReferences = false, IncludeNonVisibleObjects = false };
            GeometryElement eleGeo = element.get_Geometry(options);
            foreach (GeometryObject geomObj in eleGeo)
            {
                GeometryInstance geomInstance = geomObj as GeometryInstance;
                if (geomInstance != null)
                {
                    foreach (GeometryObject instObj in geomInstance.GetInstanceGeometry())
                    {

                        Solid instsolid = instObj as Solid;
                        n += instsolid.Volume;
                    }
                }
                Solid solid = geomObj as Solid;
                if (solid != null)
                {
                    n += solid.Volume;

                }
            }//获取所有体积
            n = n * 304.8 * 304.8 * 304.8/1000000000;
            n = double.Parse(Math.Round(n, 2).ToString());

            return n;
        }

     
        /// <summary>
        /// 获取柱子的脚手架面积 单位平方米
        /// </summary>
        /// <param 获取的柱子="element"></param>
        /// <returns>按照表格给的公式及前面的功能做了一个简单的数学运算</returns>
        public double GetRectangularColumnAreaOfScaffold(FamilyInstance element)
        {
            
            double h = element.GetSweptProfile().GetDrivingCurve().Length * 304.8;
            double l = GetRectangularColumnPerimeter(element);
            double n = h / 1000 * (l / 1000 + 3.6);
            n = double.Parse(Math.Round(n, 2).ToString());
            return n;
        }



        /// <summary>
        /// 获取元素的solid
        /// </summary>
        /// <param 获取soild的构建="element"></param>
        /// <returns>这个方法仅限于有一个soild的</returns>
        private Solid GetElementSolid(Element element)
        {
            Solid solid = null;
            Options option = new Options() { ComputeReferences = true, IncludeNonVisibleObjects = true };
            GeometryElement eleGeo = element.get_Geometry(option);
            var geometryElement = element.get_Geometry(option);

            foreach (var geoOb in geometryElement)
            {
                if (geoOb is Solid)
                {
                    var s = geoOb as Solid;
                    if (s != null && s.Volume > 0)
                    {
                        solid = s;
                    }
                }
                else if (geoOb is GeometryInstance)
                {
                    var gIn = geoOb as GeometryInstance;
                    if (gIn != null)
                    {
                        //获取当前实例的几何信息
                        var ge = gIn.GetInstanceGeometry();
                        //获取族类型的几何信息
                        //var ge = gIn.GetSymbolGeometry();
                        foreach (var go in ge)
                        {
                            var s = go as Solid;
                            if (s != null && s.Volume > 0)
                            {
                                solid = s;
                            }
                        }
                    }
                }
            }
            return solid;
        }

        /// <summary>
        /// 获取solid的面faces
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        private List<Face> GetFacesFromSolid(Solid solid)
        {
            var faces = new List<Face>();
            foreach (Face face in solid.Faces)
            {
                faces.Add(face);
            }
            return faces;

        }

        /// <summary>
        /// 计算柱子模板面积 单位平方米
        /// </summary>
        /// <param 计算模板面积的柱子="element"></param>
        /// <returns>获取柱子的模板面积</returns>
        /// 现有思路是，在柱子外围将模板创建出去直接读取模板的面积，增加起来便是木板的面积
        private double GetColumnTemplateArea(Element element)
        {
            Solid solidElem = GetElementSolid(element);//获取构件的Solid
            List<Face> faceList = GetFacesFromSolid(solidElem);//获取Solid的所有的面
            double TemplateArea = 0;//柱子模板的面积
            using (var ts = new Transaction(doc, "创建模板"))
            {
                ts.Start();
                //循环四个面对每个面都进行操作
                foreach (var face in faceList)
                {
                    var planarFace = face as PlanarFace;
                    //忽略掉顶面和底面
                    if (planarFace.FaceNormal.IsAlmostEqualTo(new XYZ(0, 0, 1))
                        || planarFace.FaceNormal.IsAlmostEqualTo(new XYZ(0, 0, -1)))
                    {
                        continue;
                    }

                    //由face转变为拉伸所需要的截面profile
                    var profiles = planarFace.GetEdgesAsCurveLoops();

                    //生成拉伸体solid
                    var solid = GeometryCreationUtilities
                    .CreateExtrusionGeometry(profiles, planarFace.FaceNormal, 10 / 304.8);
                    //创建拉伸构件的类型
                    var ds = DirectShape.CreateElement(doc, new ElementId(-2000011));


                    //收集族和楼板和创建模板是否有接触
                    var collector = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)) ;
                    var collector2 = new FilteredElementCollector(doc);
                    var solidFilter = new ElementIntersectsSolidFilter(solid);
                    collector.WherePasses(solidFilter);
                    collector2.WherePasses(solidFilter).OfClass(typeof(Floor)).Where(x => x.LookupParameter("结构").AsInteger() == 1);

                    //创建几个Soild周转和剪切出最后的Soild
                    Solid solid2 = null;
                    Solid solid3 = null;
                    if (collector.Count() > 0)
                    {
                        Solid solid1 = GetElementSolid((collector.First() as FamilyInstance));

                        solid2 = BooleanOperationsUtils.ExecuteBooleanOperation(solid, solid1, BooleanOperationsType.Difference);

                    }
                    if (collector2.Count() > 0)
                    {
                        List<Floor> floorList = collector2.OfClass(typeof(Floor)).Cast<Floor>().ToList();
                        foreach (var item in floorList)
                        {
                            if (item is Floor)
                            {
                                Solid solid1 = GetElementSolid((item as Floor));
                                if (solid2!=null)
                                {
                                   
                                    solid3 = BooleanOperationsUtils.ExecuteBooleanOperation(solid2, solid1, BooleanOperationsType.Difference);
                                    solid2 = solid3;
                                }
                                else
                                {
                                    solid2 = BooleanOperationsUtils.ExecuteBooleanOperation(solid, solid1, BooleanOperationsType.Difference);
                                    //这里原为solid3，不知为何觉得solid2更为合适所以改成了solid2。如果以后有bug在改回solid3
                                    
                                }
                            }
                              
                            else
                            {
                                continue;
                            }
                         
                        }
                    }

                   
                    List<Face> solid2FaceList = new List<Face>();
                    //获取新的Solid的所有面
                    if (solid2 != null)
                    {
                        //创建出模板，如果不创建屏蔽掉这一行或者不提交事件即可
                        ds.AppendShape(new List<GeometryObject> { solid2 });
                        solid2FaceList = GetFacesFromSolid(solid2);
                        //选出和柱子循环中同样向量的面把面积记录下来
                        foreach (PlanarFace S2face in solid2FaceList)
                        {
                            if (S2face.FaceNormal.IsAlmostEqualTo(planarFace.FaceNormal))
                            {
                                TemplateArea += S2face.Area;
                             
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        //创建出模板，如果不创建屏蔽掉这一行或者不提交事件即可
                        ds.AppendShape(new List<GeometryObject> { solid });
                        solid2FaceList = GetFacesFromSolid(solid);
                        //选出和柱子循环中同样向量的面把面积记录下来
                        foreach (PlanarFace S2face in solid2FaceList)
                        {
                            if (S2face.FaceNormal.IsAlmostEqualTo(planarFace.FaceNormal))
                            {
                                TemplateArea += S2face.Area;
                                
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                   

                   
                    


                }
                ts.Commit();
            }

            TemplateArea = TemplateArea / 1000000*304.8*304.8;
            TemplateArea = double.Parse(Math.Round(TemplateArea, 2).ToString());

            return TemplateArea;
        }
     
        /// <summary>
        /// 计算柱子的截面面积 单位平方米
        /// </summary>
        /// <param 计算的柱子="element"></param>
        /// <returns></returns>
        private double GetColumnCrossSectionArea(Element element)
        {
            Options options = new Options() { ComputeReferences = false, IncludeNonVisibleObjects = false };
            GeometryElement eleGeo = element.get_Geometry(options);

            List<Face> listFace = new List<Face>();//面的集合
            foreach (GeometryObject geomObj in eleGeo)
            {
                GeometryInstance geomInstance = geomObj as GeometryInstance;
                if (geomInstance != null)
                {
                    foreach (GeometryObject instObj in geomInstance.GetInstanceGeometry())
                    {

                        Solid instsolid = instObj as Solid;
                        if (instsolid == null || instsolid.Faces.Size == 0 || instsolid.Edges.Size == 0)
                        {
                            continue;
                        }
                        foreach (Face face in instsolid.Faces)
                        {
                            listFace.Add(face);
                        }
                    }
                }
                Solid solid = geomObj as Solid;
                if (solid != null)
                {
                    if (solid.Faces.Size == 0 || solid.Edges.Size == 0)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (Face face in solid.Faces)
                        {
                            listFace.Add(face);
                        }
                    }
                }
            }//获取所有面 网上抄的

            double n = 0;
            foreach (PlanarFace face in listFace)
            {
                if (face.FaceNormal.Z == 1)
                {
                    n = face.Area;
                }
            }
            n = n * 304.8*304.8/ 1000000;
            n = double.Parse(Math.Round(n, 2).ToString());
            return n;
        }

        #endregion
       

        #region  计算梁的方法
        /// <summary>
        /// 计算梁的边长
        /// </summary>
        /// <param 梁="element"></param>
        /// <returns>单位米</returns>
        private double GetbeamPerimeter(Element element)
        {
            double n = 0;
            FamilySymbol symbol = (element as FamilyInstance).Symbol;
            List<Face> faceList = GetFacesFromSolid(GetElementSolid(symbol));
            foreach (PlanarFace  face in faceList)
            {
                if (face.FaceNormal.X!=0)
                {
                    foreach (var item in face.GetEdgesAsCurveLoops().First())
                    {
                        n += item.Length;
                    }
                    break;
                }
            }
            n = UnitUtils.ConvertFromInternalUnits(n,DisplayUnitType.DUT_METERS);
            n = double.Parse(Math.Round(n, 2).ToString());
            return n;
        }
        /// <summary>
        /// 计算梁的净长度  单位米
        /// </summary>
        /// <param 梁="element"></param>
        /// <returns>单位米</returns>
        private double GetBeamLength(Element element)
        {
            double n = 0;
            XYZ dir = (((element as FamilyInstance).Location as LocationCurve).Curve as Line).Direction;//梁的location的向量
           Solid solid= GetElementSolid(element);
            List<Face> faceList = GetFacesFromSolid(solid);
           
            foreach (PlanarFace face in faceList)
            {

              
               foreach (var item in face.GetEdgesAsCurveLoops().First())
                {
                    if (item is Line)
                    {

                    }
                    else
                    {
                        continue;
                    }
                    Line line = item as Line;
                    if (Math.Abs(line.Direction.DotProduct(dir))<0.00001)//判断是否location和线是否垂直 垂直的全部剔除，不垂直的留下
                    {
                        continue;
                    }
                    else
                    {
                        n += item.Length;
                    }
                }
            }
            n = UnitUtils.ConvertFromInternalUnits(n, DisplayUnitType.DUT_METERS)/8;//一共四根线，每根线两个面所以计算了两次是八根线，只需计算一根即可，所以除以8
            
            n = double.Parse(Math.Round(n, 2).ToString());

            return n;
        }


        private double GetBeamVolume(Element element)
        {
            double n = 0;
            Solid solidElem = GetElementSolid(element);
            List<Face> faceList = GetFacesFromSolid(solidElem);
            foreach (Face face in faceList)
            {
                XYZ normal = face.ComputeNormal(UV.Zero).Normalize();
                if (normal.Z<=1&&normal.Z>=0.9)//获取梁上面的那一个面，然后获取   思路就是弄个最大面积的，然后反向拉伸区并集。
                {
                  
                }
            }
            return n;
        }


        /// <summary>
        /// 计算梁的模板面积  单位平方米
        /// </summary>
        /// <param name="element"></param>
        /// <returns>单位平方米</returns>
        private double GetBeamTemplateArea(FamilyInstance element)
        {

            Solid solidElem = GetElementSolid(element);//获取构件的Solid
            List<Face> faceList = GetFacesFromSolid(solidElem);//获取Solid的所有的面
            double n = 0;
            using (Transaction tran = new Transaction(doc, "创建模板"))
            {
                tran.Start();
                foreach (var face in faceList)
                {
                    var planarFace = face as PlanarFace;
                    XYZ normal = face.ComputeNormal(UV.Zero).Normalize();
                    if ((normal.Z <= 1 && normal.Z >= 0.9))//获取梁上面的那一个面
                    {
                        continue;
                    }
                    //由face转变为拉伸所需要的截面profile
                    var profiles = planarFace.GetEdgesAsCurveLoops();
                    Solid solid = GeometryCreationUtilities
                    .CreateExtrusionGeometry(profiles, planarFace.FaceNormal, 10 / 304.8);//创建出来solid
                    Solid solid2 = solid;//复制一个solid用于后面的剪切
                                         //创建拉伸构件的类型
                    DirectShape ds = DirectShape.CreateElement(doc, new ElementId(-2000011));
                    var collector = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance));
                    var collector2 = new FilteredElementCollector(doc);
                    var collector3 = new FilteredElementCollector(doc).OfClass(typeof(Wall));//收集需要剪切的东西
                    var solidFilter = new ElementIntersectsSolidFilter(solid2);//搜集和solid相交的集合
                    collector.WherePasses(solidFilter);
                    collector2.WherePasses(solidFilter).OfClass(typeof(Floor)).Where(x => x.LookupParameter("结构").AsInteger() == 1);
                    collector3.WherePasses(solidFilter);
                    List<Element> eleList = new List<Element>();//创建集合然后把所有东西聚集到一块

                    eleList.AddRange(collector.ToList());
                    eleList.AddRange(collector2.ToList());
                    eleList.AddRange(collector3.ToList());

                    if (eleList.Count() > 0)
                    {
                        bool bl = false;
                        foreach (var item in eleList)
                        {
                            if (item.Category.Id.IntegerValue == -2001330)
                            {
                                bl = true;
                                break;
                            }
                            else
                            {
                                Solid solid1 = GetElementSolid(item);

                                solid2 = BooleanOperationsUtils.ExecuteBooleanOperation(solid2, solid1, BooleanOperationsType.Difference);
                            }
                        }

                        if (bl)
                        {
                            continue;
                        }

                    }
                    List<Face> solid2FaceList = new List<Face>();
                    //获取新的Solid的所有面

                    if (solid2 != null)
                    {
                        //创建出模板，如果不创建屏蔽掉这一行或者不提交事件即可
                        ds.AppendShape(new List<GeometryObject> { solid2 });
                        solid2FaceList = GetFacesFromSolid(solid2);
                        //选出和柱子循环中同样向量的面把面积记录下来
                        foreach (PlanarFace S2face in solid2FaceList)
                        {
                            if (S2face.FaceNormal.IsAlmostEqualTo(planarFace.FaceNormal))
                            {
                                n += S2face.Area;

                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                tran.Commit();
            }
            n = n * 304.8 * 304.8 / 1000000;
            n = double.Parse(Math.Round(n, 2).ToString());
            return n;
        }

        /// <summary>
        /// 计算梁的侧面面积 单位平方米
        /// </summary>
        /// <param name="element"></param>
        /// <returns>这个公式太不明确指示和模板面积目前是同样的算法，需要以后明确算法来做</returns>
        private double GetBeamFlankArea(FamilyInstance element)
        {
            Solid solidElem = GetElementSolid(element);//获取构件的Solid
            List<Face> faceList = GetFacesFromSolid(solidElem);//获取Solid的所有的面
            double n = 0;
            using (Transaction tran = new Transaction(doc, "创建模板"))
            {
                tran.Start();
                foreach (var face in faceList)
            {
                var planarFace = face as PlanarFace;
                XYZ normal = face.ComputeNormal(UV.Zero).Normalize();
                    if ((normal.Z <= 1 && normal.Z >= 0.9))//获取梁上面的那一个面
                    {
                        continue;
                    }
                    //由face转变为拉伸所需要的截面profile
                    var profiles = planarFace.GetEdgesAsCurveLoops();
                Solid solid = GeometryCreationUtilities
                .CreateExtrusionGeometry(profiles, planarFace.FaceNormal, 10 / 304.8);//创建出来solid
                Solid solid2 = solid;//复制一个solid用于后面的剪切
                                     //创建拉伸构件的类型
                DirectShape ds = DirectShape.CreateElement(doc, new ElementId(-2000011));
                var collector = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance));
                var collector2 = new FilteredElementCollector(doc);
                var collector3 = new FilteredElementCollector(doc).OfClass(typeof(Wall)) ;//收集需要剪切的东西
                var solidFilter = new ElementIntersectsSolidFilter(solid2);//搜集和solid相交的集合
                collector.WherePasses(solidFilter);
                collector2.WherePasses(solidFilter).OfClass(typeof(Floor)).Where(x => x.LookupParameter("结构").AsInteger() == 1);
                collector3.WherePasses(solidFilter);
                List<Element> eleList =new List<Element>();//创建集合然后把所有东西聚集到一块

                eleList.AddRange(collector.ToList());
                eleList.AddRange(collector2.ToList());
                eleList.AddRange(collector3.ToList());

                if (eleList.Count()>0)
                {
                    bool bl = false;
                    foreach (var item in eleList)
                    {
                        if (item.Category.Id.IntegerValue == -2001330)
                        {
                            bl = true;
                            break;
                        }
                        else
                        {
                            Solid solid1 = GetElementSolid(item);

                            solid2 = BooleanOperationsUtils.ExecuteBooleanOperation(solid2, solid1, BooleanOperationsType.Difference);
                        }
                    }

                    if (bl)
                    {
                        continue;
                    }

                }



                List<Face> solid2FaceList = new List<Face>();
                //获取新的Solid的所有面

                if (solid2 != null)
                {
                    //创建出模板，如果不创建屏蔽掉这一行或者不提交事件即可
                    ds.AppendShape(new List<GeometryObject> { solid2 });
                    solid2FaceList = GetFacesFromSolid(solid2);
                    //选出和柱子循环中同样向量的面把面积记录下来
                    foreach (PlanarFace S2face in solid2FaceList)
                    {
                        if (S2face.FaceNormal.IsAlmostEqualTo(planarFace.FaceNormal))
                        {
                            n += S2face.Area;

                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }






            }

                tran.Commit();
            }

            n = n * 304.8 * 304.8 / 1000000;
            n = double.Parse(Math.Round(n, 2).ToString());
            return n;
        }


        #endregion

        #region 计算墙的方法
        /// <summary>
        /// 获取墙的长度  单位米
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private  double GetWallLength(Wall element)
        {
          
            double n=  (element.Location as LocationCurve).Curve.Length;

            n = UnitUtils.ConvertFromInternalUnits(n, DisplayUnitType.DUT_METERS);
            n = double.Parse(Math.Round(n, 2).ToString());


            return n;
        }
        /// <summary>
        /// 获取墙的高度  单位米
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private double GetWallHigh(Wall element)
        {
          double n=  element.LookupParameter("无连接高度").AsDouble();
            n = UnitUtils.ConvertFromInternalUnits(n, DisplayUnitType.DUT_METERS);
            n = double.Parse(Math.Round(n, 2).ToString());

            return n;
        }
        /// <summary>
        /// 获取墙的宽度  单位米
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private double GetWallWidth(Wall element) 
        {
            double n = (element as Wall).Width;
            n = UnitUtils.ConvertFromInternalUnits(n, DisplayUnitType.DUT_METERS);
            n = double.Parse(Math.Round(n, 2).ToString());

            return n;
        }
        /// <summary>
        /// 获取墙的体积 单位立方米
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private double GetWallVolum(Wall element)
        {
            double n = double.Parse(element.LookupParameter("体积").AsValueString().Replace("m³",""))  ;
       
            return n;
        }
        /// <summary>
        /// 获取墙的脚手架面积 单位平方米
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private  double GetWallScaffoldArea(Wall element)
        {
          double n=  GetWallLength(element) * GetWallHigh(element);
            n = double.Parse(Math.Round(n, 2).ToString());

            return n;
        }

        /// <summary>
        /// 获取墙的模板面积 单位平方米
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private double GetWallTemplateArea(Wall element)
        {
            double n = GetWallLength(element) * 2 * GetWallHigh(element);
            n = double.Parse(Math.Round(n, 2).ToString());

            return n;
        }
        #endregion


    }
}
