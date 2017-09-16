namespace Plethora.SearchBar.Definitions
{
    public abstract class Definition
    {
        private readonly string name;

        protected Definition(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
