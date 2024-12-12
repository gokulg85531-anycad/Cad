using AnyCAD.Foundation;
using System;

namespace AnyCAD.Demo.Graphics
{
    class Geometry_HeartCurve : TestCase
    {
        public override void Run(IRenderView render)
        {
            var curve = CurveBuilder.MakeBezierCurve(new GPntList()
            {
                new GPnt(0, 0, 0),
                new GPnt(-8, 20, 0),
                new GPnt(10, 15, 0),
                new GPnt(20, 0, 0)
            });

            var curve1 = TransformTool.Rotation(curve, GP.OZ(), -0.5 * Math.PI);
            var curve2 = TransformTool.Mirror(curve1, GP.OY());

            var spline = CurveBuilder.MakeBSplineByCurves(new TopoShapeList { curve1, curve2 }, EnumConvertParameterisationType.Polynomial);
          

            render.ShowShape(spline, ColorTable.AliceBlue);
        }
    }
}
