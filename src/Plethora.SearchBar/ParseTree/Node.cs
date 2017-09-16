using System.Diagnostics;

using Plethora.SearchBar.Definitions;

namespace Plethora.SearchBar.ParseTree
{
    [DebuggerDisplay("{GetType().Name} [Id = {" + nameof(Definition) + ".Name}, Text = {" + nameof(Text) + "}]")]
    public abstract class Node
    {
        private readonly Definition definition;
        private readonly string text;

        protected Node(Definition definition, string text)
        {
            this.definition = definition;
            this.text = text;
        }

        public Definition Definition
        {
            get { return this.definition; }
        }

        internal string Text
        {
            get { return this.text; }
        }
    }
}
