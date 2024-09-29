using AnyCAD.Foundation;

namespace AnyCAD.Demo.Geometry
{
    class Geometry_SweepByPath : TestCase
    {
        public override void Run(IRenderView render)
        {
            var shape = ShapeIO.Open(GetResourcePath("sweep/2.igs"));
            if(shape == null)
            {
                return;
            }

            var edges = shape.GetChildren(EnumTopoShapeType.Topo_EDGE);


            int pathIdx = 18;
            var path = edges[pathIdx];
            var path2 = edges[pathIdx+1];
            edges.RemoveAt(pathIdx);
            edges.RemoveAt(pathIdx);
            //foreach (var edge in edges)
            //{
            //    render.ShowShape(edge, ColorTable.RandomColor());
            //}
            render.ShowShape(path, ColorTable.RandomColor());
            render.ShowShape(path2, ColorTable.RandomColor());

            TopoShapeList sections = new TopoShapeList();
            for(int ii=0; ii<3; ii++)
            {
                sections.Add(edges[ii]);
                render.ShowShape(edges[ii], ColorTable.RandomColor());
            }
            sections.Add(edges[edges.Count-1]);
            render.ShowShape(edges[edges.Count - 1], ColorTable.RandomColor());
            //var shell = FeatureTool.Loft(edges, false, true);
            var shell = FeatureTool.SweepBySections(sections, path, EnumSweepTransitionMode.RoundCorner, false, true, true);
            render.ShowShape(shell, ColorTable.RandomColor());
        }
    }
}
