using System;
using Plethora.Synchronized.Change;

namespace Plethora.Synchronized
{
    public delegate void ConflictingChangeEventHandler(object sender, ConflictingChangeEventArgs e);

    public class ConflictingChangeEventArgs : EventArgs
    {
        private readonly ChangeDescriptor conflictingChange;
        private readonly Exception conflictException;

        public ConflictingChangeEventArgs(ChangeDescriptor conflictingChange, Exception conflictException)
        {
            //Validation
            if (conflictingChange == null)
                throw new ArgumentNullException("conflictingChange");

            if (conflictException == null)
                throw new ArgumentNullException("conflictException");


            this.conflictingChange = conflictingChange;
            this.conflictException = conflictException;
        }

        public ChangeDescriptor ConflictingChange
        {
            get { return conflictingChange; }
        }

        public Exception ConflictException
        {
            get { return conflictException; }
        }
    }
}
