using Plethora.SearchBar.Definitions;

namespace Plethora.SearchBar.ParseTree
{
    public class ValueNode : Node
    {
        internal ValueNode(DataTypeDefinition definition, string text)
            : base(definition, text)
        {
        }

        public new DataTypeDefinition Definition
        {
            get { return (DataTypeDefinition)base.Definition; }
        }

        public object Value
        {
            get
            {
                try
                {
                    if (this.Definition.TryParse(this.Text, out object value))
                        return value;

                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
