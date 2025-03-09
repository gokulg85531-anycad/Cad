using AnyCAD.Foundation;

namespace AnyCAD.Demo.Geometry
{
    class Geometry_CutByWires : TestCase
    {
        public override void Run(IRenderView render)
        {
            var faces = ShapeIO.Open(GetResourcePath("CutByWire/face.brep"));
            var w1 = ShapeIO.Open(GetResourcePath("CutByWire/wire.brep"));
            var w2 = ShapeIO.Open(GetResourcePath("CutByWire/wire2.brep"));

            var f0 = AdvSurfaceBuilder.AddHoles(faces, new TopoShapeList { w1, w2 });
            render.ShowShape(f0, ColorTable.AliceBlue);

            var f1 = AdvSurfaceBuilder.MakeSurface(faces, w1);
            render.ShowShape(f1, ColorTable.LightPink);

            var f2 = AdvSurfaceBuilder.MakeSurface(faces, w2);
            render.ShowShape(f2, ColorTable.Purple);
        }
    }
}
