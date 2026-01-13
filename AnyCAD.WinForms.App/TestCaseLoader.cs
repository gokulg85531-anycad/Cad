using System.Reflection;

namespace AnyCAD.Demo
{
    public class TestCaseLoader : TestCaseLoaderBase
    {

        public static void Register(TreeView tv)
        {
            var node = tv.Nodes.Add("窗体示例");
            TestCaseLoader.Register(node, Assembly.GetExecutingAssembly());

            node = tv.Nodes.Add("基础功能");
            Register(node);

            node = tv.Nodes.Add("运动仿真");
            RegisterSimulate(node);

            node = tv.Nodes.Add("高级建模");
            RegisterQuick(node);
        }


        static void Register(TreeNode tv)
        {
            Dictionary<String, TreeNode> dictNodes = new Dictionary<string, TreeNode>();
            ForEachCase((Type type, string name, string groupName) =>
            {
                TreeNode groupNode = null;
                if (!dictNodes.TryGetValue(groupName, out groupNode))
                {
                    groupNode = tv.Nodes.Add(GetUIName(groupName));
                    dictNodes[groupName] = groupNode;
                }

                var node = groupNode.Nodes.Add(name);
                node.Tag = type;
            });

            tv.ExpandAll();
        }

        static void RegisterSimulate(TreeNode tv)
        {
            Dictionary<String, TreeNode> dictNodes = new Dictionary<string, TreeNode>();
            TestCaseLoaderSimulate.ForEachCase((Type type, string name, string groupName) =>
            {
                TreeNode groupNode = null;
                if (!dictNodes.TryGetValue(groupName, out groupNode))
                {
                    groupNode = tv.Nodes.Add(GetUIName(groupName));
                    dictNodes[groupName] = groupNode;
                }

                var node = groupNode.Nodes.Add(name);
                node.Tag = type;
            });

            tv.ExpandAll();
        }

        static void RegisterQuick(TreeNode tv)
        {
            Dictionary<String, TreeNode> dictNodes = new Dictionary<string, TreeNode>();
            TestCaseLoaderQuickSolid.ForEachCase((Type type, string name, string groupName) =>
            {
                TreeNode groupNode = null;
                if (!dictNodes.TryGetValue(groupName, out groupNode))
                {
                    groupNode = tv.Nodes.Add(GetUIName(groupName));
                    dictNodes[groupName] = groupNode;
                }

                var node = groupNode.Nodes.Add(name);
                node.Tag = type;
            });

            tv.ExpandAll();
        }

        static void Register(TreeNode tv, Assembly assembly)
        {
            Dictionary<String, TreeNode> dictNodes = new Dictionary<string, TreeNode>();
            TestCase.ForEachCase((Type type, string name, string groupName) =>
            {
                TreeNode groupNode = null;
                if (!dictNodes.TryGetValue(groupName, out groupNode))
                {
                    groupNode = tv.Nodes.Add(GetUIName(groupName));
                    dictNodes[groupName] = groupNode;
                }

                var node = groupNode.Nodes.Add(name);
                node.Tag = type;
            }, assembly);
            tv.ExpandAll();
        }
    }
}
