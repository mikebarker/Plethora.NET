using JetBrains.Annotations;
using System;
using System.Linq;

namespace Plethora.Mvvm.Binding
{
    /// <summary>
    /// A binding observer.
    /// </summary>
    /// <remarks>
    /// The <see cref="ValueChanged"/> property is raised when the value of the evaluated binding changes.
    /// </remarks>
    public class BindingObserver : IBindingObserver
    {
        private readonly IBindingObserver root;
        private readonly IBindingObserver leaf;

        /// <summary>
        /// Initialise a new instance of the <see cref="BindingObserver"/> class.
        /// </summary>
        /// <param name="root">The root element of the binding.</param>
        /// <param name="leaf">The leaf element of the binding.</param>
        /// <remarks>
        /// It is expected that the binding elements are chained from the <paramref name="root"/> to the <paramref name="leaf"/>.
        /// </remarks>
        public BindingObserver(
            [NotNull] IBindingObserver root,
            [NotNull] IBindingObserver leaf)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            if (leaf == null)
                throw new ArgumentNullException(nameof(leaf));

            this.root = root;
            this.leaf = leaf;
        }

        /// <summary>
        /// Sets the object to be observed by this observer.
        /// </summary>
        public void SetObserved(object observed)
        {
            this.root.SetObserved(observed);
        }

        /// <summary>
        /// Raised when the value of the evaluated binding changes.
        /// </summary>
        public event EventHandler ValueChanged
        {
            add { this.leaf.ValueChanged += value; }
            remove { this.leaf.ValueChanged += value; }
        }

        /// <summary>
        /// Gets the value of the evaluated binding changes. 
        /// </summary>
        /// <param name="value">The evaluated bindings value.</param>
        /// <returns>
        /// 'true' if the binding evaluation was successful; otherwise false.
        /// </returns>
        public bool TryGetValue(out object value)
        {
            return this.leaf.TryGetValue(out value);
        }
    }
}
