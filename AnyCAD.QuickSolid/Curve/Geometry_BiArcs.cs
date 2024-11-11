using AnyCAD.Foundation;

namespace AnyCAD.Demo.Geometry
{
    class Geometry_BiArcs : TestCase
    {
        public override void Run(IRenderView render)
        {
            QuickSolidEngineManager.Instance().Initialize();

            var face = ShapeIO.Open(GetResourcePath("Split/face.brep"));
            var edges = face.GetChildren(EnumTopoShapeType.Topo_EDGE);
            var plane = new GPln();
            foreach(var edge in edges)
            {
                var node = render.ShowShape(edge, ColorTable.Green);
                node.SetTransform(Matrix4d.makeTranslate(0, 0, 1));

                render.ShowShape(edge, ColorTable.Red);
            }
        }
    }
}
