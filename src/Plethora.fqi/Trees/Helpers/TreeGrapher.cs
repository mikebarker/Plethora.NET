namespace Plethora.fqi.Trees.Helpers
{
    public static class TreeGrapher
    {
        public static string Graph<TKey, TValue>(BinaryTree<TKey, TValue> tree)
        {
            var root = tree.Root;

            return Graph(root, "\t");
        }

        private static string Graph<TKey, TValue>(BinaryTree<TKey, TValue>.Node node, string tab)
        {
            var leftGraph = (node.Left == null)
                                ? ""
                                : tab + " L:" + Graph(node.Left, tab + "\t") + "\r\n";

            var rightGraph = (node.Right == null)
                                 ? ""
                                 : tab + " R:" + Graph(node.Right, tab + "\t") + "\r\n";

            return
                node.Key.ToString() + "\t" + AdditionalInfo(node) + "\r\n" +
                leftGraph +
                rightGraph;
        }

        private static string AdditionalInfo<TKey, TValue>(BinaryTree<TKey, TValue>.Node node)
        {
            if(node is AvlTree<TKey, TValue>.AvlNode)
            {
                var avlNode = (AvlTree<TKey, TValue>.AvlNode)node;
                return avlNode.BalanceFactor.ToString();
            }

            return string.Empty;
        }
    }
}
