using AnyCAD.Foundation;
using System.Collections.Generic;


namespace AnyCAD.Demo.Geometry
{
    class Geometry_Wire : TestCase
    {
        public override void Run(IRenderView render)
        {
            string fileName = GetResourcePath("wire/封头1.STEP");
            var shape = ShapeIO.Open(fileName);
            if (shape == null)
                return;

            List<int> edgeIds = new List<int>() { 40, 8, 7, 47};
            TopoShapeList edges = new TopoShapeList();
            foreach(int edgeId in edgeIds)
            {
                var edge = shape.FindChild(EnumTopoShapeType.Topo_EDGE, edgeId);
                edges.Add(edge);
            }
           
            var wire = SketchBuilder.MakeWire(edges);
            
            var wireNode = render.ShowShape(wire, ColorTable.Red);
            wireNode.SetChildrenPickable(false);

            render.ShowShape(shape, ColorTable.Gray);

        }
    }
}
