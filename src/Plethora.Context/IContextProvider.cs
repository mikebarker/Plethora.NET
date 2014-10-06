using System;
using System.Collections.Generic;

namespace Plethora.Context
{
    /// <summary>
    /// Interface which provides the methods and events required to created a 
    /// context provider.
    /// </summary>
    /// <remarks>
    /// An instance of <see cref="IContextProvider"/> registered with a <see cref="ContextManager"/>
    /// informs the manager as items pass in and out of context, and returns a list of the contexts
    /// currently in scope.
    /// </remarks>
    public interface IContextProvider
    {
        /// <summary>
        /// Notifies the listener that the contexts which this class provides are longer in scope.
        /// </summary>
        event EventHandler EnterContext;

        /// <summary>
        /// Notifies the listener that the contexts which this class provides are no longer in scope.
        /// </summary>
        event EventHandler LeaveContext;

        /// <summary>
        /// Notifies the listener that the context list has change.
        /// </summary>
        event EventHandler ContextChanged;


        /// <summary>
        /// Gets the list of contexts currently in scope.
        /// </summary>
        IEnumerable<ContextInfo> Contexts { get; }
    }
}
