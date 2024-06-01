using AnyCAD.Drawing;
using AnyCAD.Foundation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace AnyCAD.WPF
{
    internal partial class MainViewModel : ObservableObject
    {
        IRenderView mRenderView;
        [ObservableProperty]
        ObservableCollection<TreeViewItem> _BasicSamples;

        [ObservableProperty]
        ObservableCollection<TreeViewItem> _AdvSamples;
        [ObservableProperty]
        ObservableCollection<TreeViewItem> _QuickSolidSamples;

        [ObservableProperty]
        string _MousePosition = string.Empty;

        [ObservableProperty]
        string _SelectionInfo = string.Empty;
        public MainViewModel(IRenderView view)
        {
            mRenderView = view;
            _BasicSamples = TestCaseLoader.LoadBasic();
            _AdvSamples = TestCaseLoader.LoadAdv();
            _QuickSolidSamples = TestCaseLoader.LoadQuickSolid();
        }

        public void ViewReady()
        {
            mRenderView.SetHilightingCallback((PickedResult pr) =>
            {
                var pt = pr.GetItem().GetPosition();
                MousePosition = $"{pt.x} {pt.y} {pt.z}";
                return true;
            });

            mRenderView.SetAnimationCallback((ViewerListener.AnimationHandler)((float timer) =>
            {
                Demo.TestCase.RunAnimation(mRenderView, timer);
            }));

        }


        public void UpdateSelectionInfo(PickedResult pr)
        {
            var ss = mRenderView.SelectionManager.GetSelection();
            var count = ss.GetCount();
            var msg = $"Count: {count}";
            if(count == 1)
            {
                for(var itr = ss.CreateIterator();itr.More(); itr.Next())
                {
                    var item = itr.Current();
                    msg += $"\nNodeId: {item.GetNodeId()}";
                    msg += $"\nObjectId: {item.GetId().GetId().Value}";
                    msg += $"\nUserId: {item.GetUserId()}";
                    msg += $"\nShapeId: {item.GetShapeIndex()}";
                    msg += $"\nPrimitiveId: {item.GetPrimitiveIndex()}";
                    msg += $"\nType: {item.GetShapeType().ToString()}";
                    msg += $"\nTopoShapeId: {item.GetTopoShapeId().ToString()}";
                    var pt = item.GetPosition();
                    msg += $"\nPosition: {pt.x} {pt.y} {pt.z}";

                    var shapeNode = BrepSceneNode.Cast(item.GetNode());
                    if(shapeNode != null)
                    {
                        var trf = shapeNode.GetWorldTransform().ToTrsf();
                        trf.Invert();
                        var pointOnShape = pt.ToPnt().Transformed(trf);
                        switch (item.GetShapeType())
                        {
                            case EnumShapeFilter.Edge:
                                {
                                   var edge = shapeNode.GetTopoShape().FindChild(EnumTopoShapeType.Topo_EDGE, (int)item.GetTopoShapeId());
                                    if(edge != null)
                                    {
                                        var pc = new ParametricCurve(edge);
                                        msg += $"\nEdge: {pc.GetCurveType().ToString()}";

                                        var epc = new ExtremaPointCurve();
                                        if(epc.Initialize(edge, pointOnShape) && epc.GetPointCount() > 0)
                                        {
                                            var param = epc.GetParameter(0);
                                            msg += $"\nU: {param}";
                                        }
                                    }
                                }
                                break;
                            case EnumShapeFilter.Face:
                                {
                                    var face = shapeNode.GetTopoShape().FindChild(EnumTopoShapeType.Topo_FACE, (int)item.GetTopoShapeId());
                                    if(face != null)
                                    {
                                        var ps = new ParametricSurface(face);
                                        msg += $"\nFace: {ps.GetSurfaceType().ToString()}";

                                        var uv = ps.ComputeClosestPoint(pointOnShape, GP.Resolution(), GP.Resolution());
                                        msg += $"\nUV: {uv.x} {uv.y}";
                                    }
                                }
                                break;
                            default:
                                {
                                    msg += $"\n^_^";
                                }
                                break;
                        }
                        
                    }
                }                
            }
            
            SelectionInfo= msg ;
        }


        [RelayCommand]
        void OnNewScene()
        {
            mRenderView.ClearAll();
        }

        [RelayCommand]
        void OnOpenModel()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".stp",
                Filter = "Models (*.igs;*.iges;*.stp;*.step;*.brep;*.stl)|*.igs;*.iges;*.stp;*.step;*.brep;*.stl"
            };
            if (dlg.ShowDialog() != true)
                return;

            SceneNode? node = null;
            var shape = ShapeIO.Open(dlg.FileName);
            if (shape == null)
                return;
            node = BrepSceneNode.Create(shape, null, null, 0, false);
            if (node == null)
                return;

            mRenderView.ShowSceneNode(node);
            mRenderView.ZoomAll();
        }

        [RelayCommand]
        void OnOpenMesh()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".obj",
                Filter = SceneIO.FormatFilters()
            };
            if (dlg.ShowDialog() != true)
                return;

            SceneNode? node = null;
            node = SceneIO.Load(dlg.FileName);

            if (node == null)
                return;

            mRenderView.ShowSceneNode(node);
            mRenderView.ZoomAll();
        }

        [RelayCommand]
        void OnOpenDrawing()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "Drawing (*.json)|*.json"
            };
            if (dlg.ShowDialog() != true)
                return;

            var drawing = DrawingDb.Load(dlg.FileName);
            if(drawing != null)
            {
                drawing.Show(mRenderView);
            }
        }

        [RelayCommand]
        void OnPickEdge()
        {
            mRenderView.ViewContext.SetPickFilter((uint)EnumShapeFilter.Edge);
        }
    }
}
