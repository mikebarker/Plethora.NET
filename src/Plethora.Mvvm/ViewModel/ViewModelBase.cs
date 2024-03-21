using System;

namespace Plethora.Mvvm.ViewModel
{
    /// <summary>
    /// A view-model base class which supports a view-model-first paradigm.
    /// </summary>
    public abstract class ViewModelBase : NotifyPropertyChanged, IViewModel, INavigationState
    {
        private readonly object viewLock = new();
        private readonly WeakReference<object?> weakView = new(null);

        /// <inheritdoc />
        IViewModel INavigationState.GetViewModel() => this;

        /// <inheritdoc />
        INavigationState IViewModel.NavigationState => this.NavigationState;

        /// <summary>
        /// Gets the <see cref="INavigationState"/> of this view-model.
        /// </summary>
        /// <remarks>
        /// By default returns an instance to itself. Override this member to provide
        /// a light weight <see cref="INavigationState"/> which represents this view-model.
        /// </remarks>
        protected virtual INavigationState NavigationState => this;

        /// <inheritdoc />
        object IViewModel.View
        {
            get
            {
                object? view;
                if (this.weakView.TryGetTarget(out view))
                {
                    return view;
                }

                lock (this.viewLock)
                {
                    if (this.weakView.TryGetTarget(out view))
                    {
                        return view;
                    }

                    view = CreateView();
                    this.weakView.SetTarget(view);
                    return view;
                }
            }
        }

        /// <summary>
        /// Creates the view associated this view-model.
        /// </summary>
        /// <returns>
        /// The view associated with this view-model.
        /// </returns>
        /// <remarks>
        /// This method is guaranteed to be called only once during the view-model's lifetime.
        /// </remarks>
        protected abstract object CreateView();
    }
}
