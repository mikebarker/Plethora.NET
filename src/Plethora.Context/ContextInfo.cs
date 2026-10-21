namespace Plethora.Context;

/// <summary>
/// The class which contains the information for a context.
/// </summary>
/// <remarks>
/// Instance of this <see cref="ContextInfo"/> class are passed through the system to describe the current
/// context of the application.
/// </remarks>
/// <remarks>
/// Initialises a new instance of the <see cref="ContextInfo"/> class.
/// </remarks>
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
public class ContextInfo(
    string name,
    int rank,
    object? data)
{
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
    public string Name => name;

    /// <summary>
    /// The rank of the context.
    /// </summary>
    /// <remarks>
    /// The rank is used as a scoring mechanism. UI elements may limited the number of
    /// actions presented to the user based on the rank of the context and action.
    /// </remarks>
    public int Rank => rank;

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
    public object? Data => data;

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
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

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCodeHelper.GetHashCode(this.Name, this.Rank, this.Data);
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
    protected bool Equals(ContextInfo? other)
    {
        if (other is null)
            return false;

        return 
            string.Equals(this.Name, other.Name) &&
            this.Rank == other.Rank &&
            object.Equals(this.Data, other.Data);
    }
}
