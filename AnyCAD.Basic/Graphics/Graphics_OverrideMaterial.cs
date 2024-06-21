using AnyCAD.Foundation;
using System;

namespace AnyCAD.Demo.Graphics
{
    /// <summary>
    /// 透明物体绘制顺序
    /// </summary>
    class Graphics_OverrideMaterial : TestCase
    {
        public override void Run(IRenderView render)
        {
            Run2(render);
            //var face = MeshPhongMaterial.Create("stl");
            //face.SetColor(ColorTable.LightYellow);
            //face.SetOpacity(0.8f);
            //face.SetDepthTest(false);
            //face.SetTransparent(true);

            //var node = SceneIO.Load(GetResourcePath("stl/1234.stl"));
            //if (node == null)
            //    return;

            //node.SetOverrideFaceMaterial(face);
            //render.ShowSceneNode(node);
        }

        public void Run2(IRenderView render)
        {
            var topoShape = SketchBuilder.MakeCircle(new GPnt(0, 0, 0), 50, new GDir(0, 1, 0));
            var topoShapeInner = SketchBuilder.MakeCircle(new GPnt(0, 0, 0), 30, new GDir(0, 1, 0));
            topoShapeInner = SketchBuilder.MakePlanarFace(topoShapeInner);
            topoShape = SketchBuilder.MakePlanarFace(topoShape);
            topoShape = BooleanTool.Cut(topoShape, topoShapeInner);
            topoShape = FeatureTool.Extrude(topoShape, 200, new GDir(0, 1, 0));
            var node = BrepSceneNode.Create(topoShape, null, null, 1e-3, false);
            node.SetDisplayFilter(EnumShapeFilter.Face);
            node.SetPickOrder(100);
            render.ShowSceneNode(node);


            var buffer = new Float32Buffer(0);
            for (int i = 1; i <= 180; i++)
            {
                double angle = i * 2 * Math.PI / 180;
                buffer.Append(50 * (float)Math.Sin(angle), 0, 50 * (float)Math.Cos(angle));
                if (i != 1)
                {
                    buffer.Append(50 * (float)Math.Sin(angle), 0, 50 * (float)Math.Cos(angle));
                }
            }
            var primitive = GeometryBuilder.CreateLines(new Float32Array(buffer), null, null);
            var node2 = new PrimitiveSceneNode(primitive, null);
            render.ShowSceneNode(node2);
        }
    }
}
