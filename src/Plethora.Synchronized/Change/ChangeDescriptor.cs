using System;

namespace Plethora.Synchronized.Change
{
    [Serializable]
    public sealed class ChangeDescriptor : IEquatable<ChangeDescriptor>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ChangeDescriptor"/> class.
        /// </summary>
        /// <remarks>
        /// Provided for de-serializaion.
        /// </remarks>
        public ChangeDescriptor()
        {
        }

        internal ChangeDescriptor(Guid sourceId, int version, string memberName, object[] arguments, object value)
        {
            this.ChangeId = Guid.NewGuid();
            this.SourceId = sourceId;
            this.Version = version;

            this.Value = value;
            this.Arguments = arguments;
            this.MemberName = memberName;
        }

        #endregion

        #region Properties

        public Guid ChangeId { get; set; }
        public Guid SourceId { get; set; }
        public int Version { get; set; }

        public string MemberName { get; set; }
        public object[] Arguments { get; set; }
        public object Value { get; set; }

        #endregion

        #region Implementation of IEquatable<ChangeDataBase>

        public override bool Equals(object obj)
        {
            ChangeDescriptor other = obj as ChangeDescriptor;
            if (other == null)
                return false;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return
                this.ChangeId.GetHashCode();
        }

        public bool Equals(ChangeDescriptor other)
        {
            if (other == null)
                return false;

            return
                this.ChangeId == other.ChangeId;
        }

        #endregion
    }

}
