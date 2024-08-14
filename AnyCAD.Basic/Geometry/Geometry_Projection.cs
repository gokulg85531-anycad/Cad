using AnyCAD.Foundation;

namespace AnyCAD.Demo.Geometry
{
    class Geometry_Projection : TestCase
    {

        public override void Run(IRenderView render)
        {
            var wire = ShapeIO.Open(GetResourcePath("proj/wire.brep"));
            if(wire == null)
            {
                return;
            }
            render.ShowShape(wire, ColorTable.Red);
            var box = wire.GetBBox();
            var minB = box.CornerMin();
            var maxB = box.CornerMax();
            var pln = SketchBuilder.MakePlanarFace(new GPln(new GPnt(0, 0, 10), new GDir(0, 0, 1)), minB.x - 100, maxB.x + 100, minB.y - 100, maxB.y + 100);

            var newWire =  ProjectionTool.ProjectOnSurface(wire, pln);
            render.ShowShape(newWire, ColorTable.Green);

        }
    }
}
