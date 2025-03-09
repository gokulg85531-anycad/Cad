using AnyCAD.Foundation;

namespace AnyCAD.Demo.Geometry
{
    class Geometry_Wrap : TestCase
    {
        public override void Run(IRenderView render)
        {           
            var dxf = ShapeIO.Open(GetResourcePath("dxf/tiger.dxf"));

            render.ShowShape(dxf, ColorTable.LightGreen);

            var circle = CurveBuilder.MakeCircle(new GPnt(), 50, new GDir(0, 0, 1));
            var face = FeatureTool.Extrude(circle, 500, new GDir(0, 0, -1));

            foreach (var edge in dxf.GetChildren(EnumTopoShapeType.Topo_EDGE))
            {
                var curves = AdvFeatureTool.Wrapping(edge, face);

                render.ShowShape(curves, ColorTable.Red);
            }
            render.ShowShape(face, ColorTable.LightGray);
        }
    }
}
