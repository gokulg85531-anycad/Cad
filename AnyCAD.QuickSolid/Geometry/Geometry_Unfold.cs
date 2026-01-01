using AnyCAD.Foundation;
using AnyCAD.QuickSolid;
namespace AnyCAD.Demo.Geometry
{
    class Geometry_Unfold : TestCase
    {
        public override void Run(IRenderView render)
        {
            var solid = ShapeIO.Open(GetResourcePath("adv/sheet.STEP"));

            var plan = SheetMetalTool.Unfold(solid, 0.5, 0.1);

            render.ShowShape(solid, ColorTable.AliceBlue);
            var node = render.ShowShape(plan, ColorTable.LightCoral);
            node.SetTransform(Matrix4d.makeTranslate(0, 0, -100));
        }
    }
}
