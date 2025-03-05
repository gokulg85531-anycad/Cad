using AnyCAD.Foundation;
using System;

namespace AnyCAD.Demo.Geometry
{
    class Analysis_Solid : TestCase
    {
        public override void Run(IRenderView renderer)
        {
            var cyl = ShapeBuilder.MakeCylinder(new GAx2(), 10, 20, 0);

            var node = renderer.ShowShape(cyl, ColorTable.Gray);

            SolidExplor se = new SolidExplor();
            se.Initialize(cyl);

            var faceIds = se.GetSharedFaceIDsByIndex(2);
            foreach(var id in faceIds)
            {
                node.SetFaceColor(id, ColorTable.HotPink);
            }
        }
    }
 
}
