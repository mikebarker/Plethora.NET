namespace Plethora.Context
{
    public class ContextInfo
    {
        #region Fields

        private readonly string name;
        private readonly int rank;
        private readonly object data;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Context"/> class.
        /// </summary>
        public ContextInfo(string name, int rank, object data)
        {
            this.name = name;
            this.rank = rank;
            this.data = data;
        }
        #endregion

        #region Properties

        public string Name
        {
            get { return this.name; }
        }

        public int Rank
        {
            get { return this.rank; }
        }

        public object Data
        {
            get { return this.data; }
        }
        #endregion
    }
}
