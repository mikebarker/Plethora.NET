namespace Plethora.Context
{
    /// <summary>
    /// The class which contains the information for a context.
    /// </summary>
    /// <remarks>
    /// Instance of this <see cref="ContextInfo"/> class are passed through the system to describe the current
    /// context of the application.
    /// </remarks>
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
        /// <param name="rank">
        /// The score associated with this context. This is used to score and sort the actions
        /// associated with the context.
        /// </param>
        /// <param name="data">
        /// The data associated with this context. <example>TradeId=45</example>
        /// </param>
        public ContextInfo(string name, int rank, object data)
        {
            this.name = name;
            this.rank = rank;
            this.data = data;
        }
        #endregion

        #region Properties

        /// <summary>
        /// The name of the business entity of this context.
        /// </summary>
        /// <remarks>
        ///  <para>
        ///   The <see cref="Name"/> of the context describes at a high level what type of context the
        ///   application is currently in.
        ///  </para>
        ///  <para>
        ///   The specifics which identify the exact entity under context is described by <see cref="Data"/>.
        ///  </para>
        /// </remarks>
        /// <example>
        /// The name should describe the entity selected. e.g. "Trade", "Stock", or "Grid"
        /// </example>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// The rank of the context.
        /// </summary>
        /// <remarks>
        /// The rank is used as a scoring mechanism. UI elements may limited the number of
        /// actions presented to the user based on the rank of the context and action.
        /// </remarks>
        public int Rank
        {
            get { return this.rank; }
        }

        /// <summary>
        /// The data which defines the current context.
        /// </summary>
        /// <remarks>
        /// It is the responsibility of the provider and consumers of the <see cref="ContextInfo"/> to ensure
        /// that the data is passed with the context is well understood.
        /// </remarks>
        /// <example>
        /// In the context of a "Trade" this may be trade identifier as an integer, or in the context
        /// of "Stock" this may be the "StockMnemonic" as a string.
        /// </example>
        public object Data
        {
            get { return this.data; }
        }

        #endregion
    }
}
