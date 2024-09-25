using AnyCAD.Foundation;
using System;


namespace AnyCAD.Demo.Geometry
{
    class Geometry_VesselHead : TestCase
    {
        public VesselHead myVessel;
        public override void Run(IRenderView render)
        {
            myVessel = new VesselHead(500, 10, 150, VesselHead.HeadSetType.EHA);
            TopoShape origin = myVessel.CreateTopo();

            double centerX = Math.Cos(270 * Math.PI / 180) * 90;
            double centerY = Math.Sin(270 * Math.PI / 180) * 90;
            GPnt center = new GPnt(centerX, centerY, 0);
            GAx2 center2 = new GAx2(center, GP.DZ());
            TopoShape nozzelInner = ShapeBuilder.MakeCylinder(center2, 30, 435, 0);
            TopoShape nozzelOuter = ShapeBuilder.MakeCylinder(center2, 40, 335, 0);
            TopoShape downWire = BooleanTool.Section(nozzelInner, myVessel.InnerFace);
            downWire  = SketchBuilder.MakeBSplineByWire(downWire, 1, EnumConvertParameterisationType.RationalC1);
            TopoShape upWire = BooleanTool.Section(nozzelOuter, myVessel.OuterFace);
            upWire = SketchBuilder.MakeBSplineByWire(upWire, 1, EnumConvertParameterisationType.RationalC1);
            TopoShape cutFace = FeatureTool.Loft(downWire, upWire, false);
            render.ShowShape(cutFace, ColorTable.SaddleBrown);
        }
    }
}