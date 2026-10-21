using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Plethora.Context.Help;

/// <summary>
/// The pricipal manager of help related to contexts. 
/// </summary>
public class HelpManager
{
    #region Singleton Instance

    private static readonly HelpManager globalInstance = new();

    /// <summary>
    /// The global instance of the <see cref="HelpManager"/>.
    /// </summary>
    /// <remarks>
    /// Implementations can utilise the global instance or choose to define a local instance if preferred.
    /// </remarks>
    public static HelpManager GlobalInstance
    {
        get { return globalInstance; }
    }

    #endregion

    #region Fields

    private readonly ReaderWriterLockSlim rwLock = new();
    private readonly ICollection<IHelpFactory> helpFactories = new List<IHelpFactory>(0);

    #endregion

    /// <summary>
    /// Registers an action factory with this <see cref="HelpManager"/>.
    /// </summary>
    /// <param name="factory">The help factory to be registered.</param>
    /// <remarks>
    ///  <para>
    ///   The help factory is used to provide a list of available help documents for the
    ///   contexts in-scope.
    ///  </para>
    /// </remarks>
    /// <seealso cref="GetHelpDocuments"/>
    public void RegisterFactory(IHelpFactory factory)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(factory);

        this.rwLock.EnterWriteLock();
        try
        {
            this.helpFactories.Add(factory);
        }
        finally
        {
            this.rwLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Deregisters an action factory.
    /// </summary>
    public void DeregisterFactory(IHelpFactory factory)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(factory);

        this.rwLock.EnterWriteLock();
        try
        {
            this.helpFactories.Remove(factory);
        }
        finally
        {
            this.rwLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Gets the actions available for a list of contexts.
    /// </summary>
    /// <param name="contexts">The list of contexts</param>
    /// <returns>
    /// A list of available actions for the given contexts.
    /// </returns>
    public IEnumerable<IHelpDocument> GetHelpDocuments(IEnumerable<ContextInfo> contexts)
    {
        IEnumerable<IHelpFactory> factories;
        this.rwLock.EnterReadLock();
        try
        {
            //Copy the helpFactories to a new list to minimise the time required
            // within the lock.
            factories = this.helpFactories.ToList();
        }
        finally
        {
            this.rwLock.ExitReadLock();
        }

        var documents = factories
            .Select(factory => factory.GetHelpDocuments(contexts))
            .Where(docList => docList != null)
            .SelectMany(doc => doc!)
            .Where(doc => doc != null)
            .ToList();

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return documents;
#pragma warning restore CS8619
    }
}
