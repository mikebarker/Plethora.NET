using System;

using JetBrains.Annotations;

namespace Plethora
{
    /// <summary>
    /// A class which calls an <see cref="Action"/> delegate when disposed.
    /// </summary>
    /// <remarks>
    /// This is useful when defining a class to make using of the "using" symantics to provide some kind of logic.
    /// <example>
    /// The class:
    /// <code><![CDATA[
    ///     class WriteOnExit
    ///     {
    ///         private int entryCount = 0;
    /// 
    ///         public IDisposable Enter()
    ///         {
    ///             if (this.entryCount == 0)
    ///                 Console.WriteLine("Enter");
    /// 
    ///             this.entryCount++;
    ///             Console.WriteLine("Thread Enter");
    /// 
    ///             return new Disposable(delegate
    ///             {
    ///                 this.entryCount--;
    ///                 Console.WriteLine("Thread Exit");
    /// 
    ///                 if (this.entryCount == 0)
    ///                     Console.WriteLine("All Exit");
    ///             });
    ///         }
    ///     }
    /// 
    ///     // ...
    /// 
    ///     using (WriteOnExit.Enter()
    ///     {
    ///         using (WriteOnExit.Enter()
    ///         {
    ///             Console.WriteLine("Inside");
    ///         }
    ///     }
    /// ]]></code>
    /// 
    /// Results in the following output:
    /// 
    /// <![CDATA[
    /// Enter
    /// Thread Enter
    /// Thread Enter
    /// Inside
    /// Thread Exit
    /// Thread Exit
    /// All Exit
    /// ]]>
    /// </example>
    /// </remarks>
    public class ActionOnDispose : IDisposable
    {
        #region Field

        private readonly Action onDispose;

        #endregion

        #region Constructors

        public ActionOnDispose([NotNull] Action onDispose)
        {
            if (onDispose == null)
                throw new ArgumentNullException(nameof(onDispose));

            this.onDispose = onDispose;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            this.onDispose();
        }

        #endregion
    }
}
