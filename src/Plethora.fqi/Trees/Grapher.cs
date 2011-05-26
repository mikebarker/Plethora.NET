namespace Plethora.fqi.Trees
{
    public static class Grapher
    {
        public static string Graph<TKey, TValue>(AvlTree<TKey, TValue> tree)
        {
            var root = tree.Root;

            return Graph(root, "\t");
        }

        private static string Graph<TKey, TValue>(AvlTree<TKey, TValue>.AvlNode node, string tab)
        {
            var leftGraph = (node.Left == null)
                                ? ""
                                : tab + " L:" + Graph(node.Left, tab + "\t") + "\r\n";

            var rightGraph = (node.Left == null)
                                 ? ""
                                 : tab + " R:" + Graph(node.Left, tab + "\t") + "\r\n";

            return
                node.Key.ToString() + "\t" + node.BalanceFactor + "\r\n" +
                leftGraph +
                rightGraph;
        }

    }
}
