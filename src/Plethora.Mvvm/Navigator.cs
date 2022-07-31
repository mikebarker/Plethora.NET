using Plethora.Collections;
using Plethora.Mvvm.ViewModel;
using System;

namespace Plethora.Mvvm
{
    /// <summary>
    /// A class which is used to navigate between view-models, providing back and forward functionality.
    /// </summary>
    public class Navigator : NotifyPropertyChanged
    {
        /// <summary>
        /// The default number of back navigations stored in a <see cref="Navigator"/>.
        /// </summary>
        public const int DefaultBackCapacity = 20;

        /// <summary>
        /// The default number of forward navigations stored in a <see cref="Navigator"/>.
        /// </summary>
        public const int DefaultForwardCapacity = 20;

        private readonly DropoutStack<INavigationState> backStack;
        private readonly DropoutStack<INavigationState> forwardStack;
        private IViewModel current;

        /// <summary>
        /// Initialise a new instance of the <see cref="Navigator"/> class, with the default capacity.
        /// </summary>
        /// <seealso cref="DefaultBackCapacity"/>
        /// <seealso cref="DefaultForwardCapacity"/>
        public Navigator()
            : this(DefaultBackCapacity, DefaultForwardCapacity)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="Navigator"/> class.
        /// </summary>
        /// <param name="backCapacity">The number of back navigations stored in this <see cref="Navigator"/>.</param>
        /// <param name="forwardCapacity">The number of forward navigations stored in this <see cref="Navigator"/>.</param>
        public Navigator(int backCapacity, int forwardCapacity)
        {
            this.backStack = new DropoutStack<INavigationState>(backCapacity);
            this.forwardStack = new DropoutStack<INavigationState>(forwardCapacity);
        }

        /// <summary>
        /// Gets the current view-model.
        /// </summary>
        public IViewModel Current
        {
            get { return this.current; }
            private set
            {
                this.current = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sets the <see cref="Current"/> view-model.
        /// </summary>
        /// <param name="viewModel">The <see cref="IViewModel"/> to make the current view-model.</param>
        /// <remarks>
        /// The <see cref="Current"/> view-model state is moved to the 'back' stack.
        /// </remarks>
        public void NavigatorTo(IViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            if (this.Current != null)
            {
                var currentNavigationState = this.Current.NavigationState;
                this.PushBack(currentNavigationState);
            }

            if (this.forwardStack.Count != 0)
            {
                this.forwardStack.Clear();
                this.OnPropertyChanged(nameof(this.CanNavigateForeward));
            }

            this.Current = viewModel;
        }

        /// <summary>
        /// Gets a flag indicating whether it is possible to navigate back.
        /// </summary>
        public bool CanNavigateBack => this.backStack.Count > 0;

        /// <summary>
        /// Navigates to the previous view-model.
        /// </summary>
        public void NavigateBack()
        {
            if (!this.CanNavigateBack)
                throw new InvalidOperationException();

            if (this.Current != null)
            {
                var currentNavigationState = this.Current.NavigationState;
                this.PushForward(currentNavigationState);
            }

            var prevNavigationState = this.PopBack();

            this.Current = prevNavigationState.GetViewModel();
        }

        /// <summary>
        /// Gets a flag indicating whether it is possible to navigate forward.
        /// </summary>
        public bool CanNavigateForeward => this.forwardStack.Count > 0;

        /// <summary>
        /// Navigates to the next view-model.
        /// </summary>
        public void NavigateForeward()
        {
            if (!this.CanNavigateForeward)
                throw new InvalidOperationException();

            if (this.Current != null)
            {
                var currentNavigationState = this.Current.NavigationState;
                this.PushBack(currentNavigationState);
            }

            var nextNavigationState = this.PopForward();

            this.Current = nextNavigationState.GetViewModel();
        }

        private void PushBack(INavigationState navigationState)
        {
            this.Push(navigationState, this.backStack, nameof(CanNavigateBack));
        }

        private INavigationState PopBack()
        {
            return this.Pop(this.backStack, nameof(CanNavigateBack));
        }

        private void PushForward(INavigationState navigationState)
        {
            this.Push(navigationState, this.forwardStack, nameof(CanNavigateForeward));
        }

        private INavigationState PopForward()
        {
            return this.Pop(this.forwardStack, nameof(CanNavigateForeward));
        }

        private void Push(INavigationState navigationState, DropoutStack<INavigationState> stack, string canNavigatePropertyName)
        {
            stack.Push(navigationState);
            if (stack.Count == 1)
                this.OnPropertyChanged(canNavigatePropertyName);
        }

        private INavigationState Pop(DropoutStack<INavigationState> stack, string canNavigatePropertyName)
        {
            var navigationState = stack.Pop();
            if (stack.Count == 0)
                this.OnPropertyChanged(canNavigatePropertyName);

            return navigationState;
        }
    }
}
