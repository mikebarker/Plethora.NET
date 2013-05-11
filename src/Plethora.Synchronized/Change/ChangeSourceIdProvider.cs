using System;

namespace Plethora.Synchronized.Change
{
    public sealed class ChangeSourceIdProvider
    {
        #region Fields

        private readonly Guid sourceId = Guid.NewGuid();

        #endregion

        #region Properties

        public Guid ChangeSourceId
        {
            get { return this.sourceId; }
        }

        #endregion
    }
}
