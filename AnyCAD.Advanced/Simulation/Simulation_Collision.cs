using AnyCAD.Foundation;

namespace AnyCAD.Demo.Graphics
{
    class Simulation_Collision : TestCase
    {
        BrepSceneNode mObject1;
        BrepSceneNode mObject2;
        MaterialInstance mMaterial;
        MaterialInstance mMaterialWarning;
        MaterialInstance mMaterialWarning2;
        CollisionSceneWorld _CollisionWorld;
        public override void Run(IRenderView render)
        {
            _CollisionWorld = CollisionSceneWorld.Create("Default");

            mMaterial = MeshPhongMaterial.Create("face");
            mMaterial.SetColor(ColorTable.查特酒绿);
            mMaterial.SetFaceSide(EnumFaceSide.DoubleSide);

            mMaterialWarning = MeshPhongMaterial.Create("face");
            mMaterialWarning.SetColor(ColorTable.浅粉红);
            mMaterialWarning.SetFaceSide(EnumFaceSide.DoubleSide);

            mMaterialWarning2 = MeshPhongMaterial.Create("face");
            mMaterialWarning2.SetColor(ColorTable.桃色);
            mMaterialWarning2.SetOpacity(0.5f);
            mMaterialWarning2.SetTransparent(true);
            mMaterialWarning2.SetFaceSide(EnumFaceSide.DoubleSide);

            var shape1 = ShapeBuilder.MakeCone(GP.XOY(), 10, 5, 10, System.Math.PI/2);
            mObject1 = BrepSceneNode.Create(shape1, null, null);
            mObject1.SetTransform(Matrix4d.makeTranslation(100, 100, 0));

            var shape2 = ShapeBuilder.MakeCylinder(GP.XOY(), 20, 20, 0);
            mObject2 = BrepSceneNode.Create(shape2, null, null);

            render.ShowSceneNode(mObject1);
            render.ShowSceneNode(mObject2);

            _CollisionWorld.AddObject(mObject1);
            _CollisionWorld.AddObject(mObject2);

            render.EnableAnimation(true);
        }

        public override void Exit(IRenderView render)
        {
            render.EnableAnimation(false);
        }

        float mDistance = 100;
        float mStep = -1;
        public override void Animation(IRenderView render, float time)
        {
            if(_CollisionWorld.Collide(mObject2, mObject1))
            {
                mObject1.SetFaceMaterial(mMaterialWarning);
                mObject2.SetFaceMaterial(mMaterialWarning2);
            }
            else
            {
                mObject1.SetFaceMaterial(mMaterial);
                mObject2.SetFaceMaterial(null);
            }

            if (mDistance < -100)
                mStep = 1;
            else if(mDistance > 100)
            {
                mStep = -1;
            }

            mDistance += mStep;
            mObject1.AddTransform(Matrix4d.makeTranslation(mStep, mStep, 0));
            mObject1.RequestUpdate();
            _CollisionWorld.UpdateTransform(mObject1, mObject1.GetTransform());
            render.RequestDraw(EnumUpdateFlags.Scene);
        }
     }
}
