using System.Diagnostics;

using Plethora.SearchBar.Definitions;

namespace Plethora.SearchBar.ParseTree
{
    [DebuggerDisplay("{GetType().Name} [Id = {" + nameof(Definition) + ".Name}, Text = {" + nameof(Text) + "}]")]
    public abstract class Node
    {
        protected Node(Definition definition, string text)
        {
            this.Definition = definition;
            this.Text = text;
        }

        public Definition Definition { get; }
        internal string Text { get; }
    }
}
