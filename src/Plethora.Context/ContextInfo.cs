namespace Plethora.Context
{
    /// <summary>
    /// The class which contains the information for a context.
    /// </summary>
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
        /// <param name="name">
        /// Identifies the name or "high-level type" of the context. <example>Trade</example>
        /// </param>
        /// <paramref name="rank">
        /// The score associated with this context. This is used to score and sort the actions
        /// associated with the context.
        /// </paramref>
        /// <paramref name="data">
        /// The data associated with this context. <example>TradeId=45</example>
        /// </paramref>
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
