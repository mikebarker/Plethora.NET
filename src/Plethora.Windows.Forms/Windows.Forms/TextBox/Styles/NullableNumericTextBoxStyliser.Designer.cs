﻿using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Plethora.Windows.Forms.Styles
{
    public partial class NullableNumericTextBoxStyliserComponent
    {
        /// <summary>
        /// Designer for the <see cref="NullableNumericTextBoxStyliserComponent"/>
        /// </summary>
        protected class Designer : IDesigner
        {
            #region Fields

            private NullableNumericTextBoxStyliserComponent styliser;
            #endregion

            #region IDisposable Members

            private bool disposed = false;

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing,
            /// or resetting unmanaged resources
            /// </summary>
            public void Dispose()
            {
                Dispose(true);

                // Call GC.SupressFinalize to take this object off the finalization queue
                // and prevent finalization code for this object from executing a second
                // time.
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing,
            /// or resetting unmanaged resources
            /// </summary>
            /// <param name="disposing">
            /// true if called directly form the user's code.
            /// </param>
            /// <remarks>
            /// <para>
            /// Dispose(bool disposing) executes in two distinct scenarios:
            /// </para>
            /// <para>
            /// If disposing equals true, the method has been called directly
            /// or indirectly by a user's code. Managed and unmanaged resources
            /// can be disposed.
            /// </para>
            /// <para>
            /// If disposing equals false, the method has been called by the
            /// runtime from inside the finalizer and you should not reference
            /// other objects. Only unmanaged resources can be disposed.
            /// </para>
            /// </remarks>
            protected virtual void Dispose(bool disposing)
            {
                // Check to see if Dispose has already been called
                if (!this.disposed)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources
                    if (disposing)
                    {
                        // Dispose managed resources

                    }

                    // Dispose unmanaged resources

                    // Disposing has been done
                    disposed = true;
                }
            }

            #region Finalizer

            /// <summary>
            /// Destructor for the <see cref="Designer"/> class.
            /// </summary>
            /// <remarks>
            /// This destructor will run only if the Dispose method does not get
            /// called.
            /// </remarks>
            ~Designer()
            {
                Dispose(false);
            }

            #endregion

            #endregion

            #region IDesigner Implementation

            ///<summary>
            ///Performs the default action for this designer.
            ///</summary>
            void IDesigner.DoDefaultAction()
            {
                //Do nothing
            }

            ///<summary>
            ///Initializes the designer with the specified component.
            ///</summary>
            ///<param name="component">The component to associate with this designer. </param>
            public virtual void Initialize(IComponent component)
            {
                if (component == null)
                    throw new ArgumentNullException("component");

                NullableNumericTextBoxStyliserComponent styliser = component as NullableNumericTextBoxStyliserComponent;
                if (styliser == null)
                    throw new ArgumentException("component must be of type NullableNumericTextBoxStyliserComponent");

                this.styliser = styliser;
            }

            /// <summary>
            /// Gets the base component that this designer is designing.
            /// </summary>
            /// <returns>
            /// An <see cref="IComponent"/> indicating the base component that this designer is designing.
            /// </returns>
            IComponent IDesigner.Component
            {
                get { return this.Styliser; }
            }

            /// <summary>
            /// Gets or sets the design-time verbs supported by the designer.
            /// </summary>
            /// <returns>
            /// An array of <see cref="DesignerVerb"/> objects supported by the designer,
            /// or null if the component has no verbs.
            /// </returns>
            public virtual DesignerVerbCollection Verbs
            {
                get
                {
                    DesignerVerbCollection verbs = new DesignerVerbCollection();
                    verbs.Add(new DesignerVerb("Add positive style", new EventHandler(AddPositiveStyle)));
                    verbs.Add(new DesignerVerb("Add negative style", new EventHandler(AddNegativeStyle)));
                    verbs.Add(new DesignerVerb("Add zero style", new EventHandler(AddZeroStyle)));
                    verbs.Add(new DesignerVerb("Add null style", new EventHandler(AddNullStyle)));
                    return verbs;
                }
            }

            #endregion

            #region Properties

            public NullableNumericTextBoxStyliserComponent Styliser
            {
                get { return this.styliser; }
            }
            #endregion

            #region Private Methods

            private void AddPositiveStyle(object sender, EventArgs e)
            {
                if ((this.styliser != null) && (this.styliser.PositiveStyle == null))
                    this.styliser.PositiveStyle = new TextBoxStyle();
            }

            private void AddNegativeStyle(object sender, EventArgs e)
            {
                if ((this.styliser != null) && (this.styliser.NegativeStyle == null))
                    this.styliser.NegativeStyle = new TextBoxStyle();
            }

            private void AddZeroStyle(object sender, EventArgs e)
            {
                if ((this.styliser != null) && (this.styliser.ZeroStyle == null))
                    this.styliser.ZeroStyle = new TextBoxStyle();
            }

            private void AddNullStyle(object sender, EventArgs e)
            {
                if ((this.Styliser != null) && (this.Styliser.ZeroStyle == null))
                    this.Styliser.NullStyle = new TextBoxStyle();
            }
            #endregion
        }
    }
}