using AnyCAD.Foundation;

namespace AnyCAD.Demo.Geometry
{
    class Interop_ImportDXF : TestCase
    {
        public override void Run(IRenderView render)
        {
           var fileName = DialogUtil.OpenFileDialog("DXF", new StringList { "DXF Files(.dxf)", "*.dxf"});
            if (fileName.IsEmpty())
                return;

           var shapes = ShapeIO.Open(fileName.GetString());
           foreach(var edge in shapes.GetChildren(EnumTopoShapeType.Topo_EDGE))
            {
                render.ShowShape(edge, ColorTable.Red);

            }
        }
    }
}
