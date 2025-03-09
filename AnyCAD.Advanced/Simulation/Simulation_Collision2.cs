using AnyCAD.Foundation;

namespace AnyCAD.Demo.Graphics
{
    class Simulation_Collision2 : TestCase
    {
        GroupSceneNode mObject1;
        BrepSceneNode mObject2;
        MaterialInstance mMaterial;
        MaterialInstance mMaterialWarning;
        CollisionSceneWorld _CollisionWorld;
        public override void Run(IRenderView render)
        {
            mMaterial = MeshPhongMaterial.Create("face");
            mMaterial.SetColor(ColorTable.PaleGodenrod);
            mMaterial.SetFaceSide(EnumFaceSide.DoubleSide);

            mMaterialWarning = MeshPhongMaterial.Create("face");
            mMaterialWarning.SetColor(ColorTable.浅粉红);
            mMaterialWarning.SetFaceSide(EnumFaceSide.DoubleSide);

            _CollisionWorld = CollisionSceneWorld.Create("Default");

            var fileName = GetResourcePath("JMS.step");
            var shape1 = ShapeIO.Open(fileName);

            var rot = Matrix4d.makeRotationAxis(Vector3d.UNIT_X, System.Math.PI / 2);

            var solids = shape1.GetChildren(EnumTopoShapeType.Topo_SOLID);
            mObject1 = new GroupSceneNode();
            foreach (var solid in solids)
            {
                var node = BrepSceneNode.Create(solid, mMaterial, null, 0.1, true);
                node.SetTransform(rot);
                mObject1.AddNode(node);
            }

            var shape2 = ShapeBuilder.MakeCylinder(GP.XOY(), 20, 200, 0);
            mObject2 = BrepSceneNode.Create(shape2, null, null, 1, true);
            mObject2.SetTransform(Matrix4d.makeTranslation(-400, -500, 0));

            render.ShowSceneNode(mObject1);
            render.ShowSceneNode(mObject2);
            _CollisionWorld.AddObject(mObject2);
            render.EnableAnimation(true);
        }

        public override void Exit(IRenderView render)
        {
            render.EnableAnimation(false);
        }

        float mDistance = -200;
        float mStep = -5;
        public override void Animation(IRenderView render, float time)
        {

            for (var itr = mObject1.CreateIterator(); itr.More(); itr.Next())
            {
                var current = BrepSceneNode.Cast(itr.Current());
                if (_CollisionWorld.Collide(current, mObject2))
                {
                    current.SetFaceMaterial(mMaterialWarning);
                }
                else
                {
                    current.SetFaceMaterial(mMaterial);
                }
            }

            if (mDistance < -200)
            {
                mStep = 5;
            }
            else if (mDistance > 200)
            {
                mStep = -5;
            }

            mDistance += mStep;
            mObject2.AddTransform(Matrix4d.makeTranslation(mStep, mStep, 0));
            mObject2.RequestUpdate();
            _CollisionWorld.UpdateTransform(mObject2, mObject2.GetTransform());
            render.RequestDraw(EnumUpdateFlags.Scene);
        }
    }
}
