using AnyCAD.Foundation;

namespace AnyCAD.Demo.Geometry
{
    class Geometry_BiArcs : TestCase
    {
        public override void Run(IRenderView render)
        {
            var face = ShapeIO.Open(GetResourcePath("Split/face.brep"));
            var edges = face.GetChildren(EnumTopoShapeType.Topo_EDGE);
            var plane = new GPln();
            int ii = 0;
            foreach(var edge in edges)
            {
                var node = render.ShowShape(edge, ColorTable.Green);
                node.SetTransform(Matrix4.makeTranslate(0, 0, 1));

                var simpleCurve = QuickCurveTool.BiArcFitCurve2D(edge, plane, 0.1);
                foreach (var curve in simpleCurve)
                {
                    render.ShowShape(curve, ColorTable.Red);
                }
            }
        }
    }
}
