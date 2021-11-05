using System;

using JetBrains.Annotations;

using Plethora.Synchronized.Change;

namespace Plethora.Synchronized
{
    public delegate void ConflictingChangeEventHandler(object sender, ConflictingChangeEventArgs e);

    public class ConflictingChangeEventArgs : EventArgs
    {
        private readonly ChangeDescriptor conflictingChange;
        private readonly Exception conflictException;

        public ConflictingChangeEventArgs([NotNull] ChangeDescriptor conflictingChange, [NotNull] Exception conflictException)
        {
            //Validation
            if (conflictingChange == null)
                throw new ArgumentNullException(nameof(conflictingChange));

            if (conflictException == null)
                throw new ArgumentNullException(nameof(conflictException));


            this.conflictingChange = conflictingChange;
            this.conflictException = conflictException;
        }

        [NotNull]
        public ChangeDescriptor ConflictingChange
        {
            get { return this.conflictingChange; }
        }

        [NotNull]
        public Exception ConflictException
        {
            get { return this.conflictException; }
        }
    }
}
