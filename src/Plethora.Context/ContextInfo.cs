using JetBrains.Annotations;

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
        /// Initialises a new instance of the <see cref="ContextInfo"/> class.
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
        public ContextInfo(
            [NotNull] string name,
            int rank,
            [CanBeNull] object data)
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
        [NotNull]
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
        [CanBeNull]
        public object Data
        {
            get { return this.data; }
        }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((ContextInfo)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.rank * 397) ^ (this.name != null ? this.name.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// The equals method.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool Equals([NotNull] ContextInfo other)
        {
            return this.rank == other.rank && string.Equals(this.name, other.name);
        }
    }
}
