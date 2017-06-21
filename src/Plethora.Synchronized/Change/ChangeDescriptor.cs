using System;
using System.Runtime.Serialization;

namespace Plethora.Synchronized.Change
{
    [Serializable]
    public sealed class ChangeDescriptor : ISerializable, IEquatable<ChangeDescriptor>
    {
        private readonly Guid changeId;
        private readonly Guid sourceId;
        private readonly int version;
        private readonly string memberName;
        private readonly object[] arguments;
        private readonly object value;

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
            this.changeId = Guid.NewGuid();
            this.sourceId = sourceId;
            this.version = version;

            this.value = value;
            this.arguments = arguments;
            this.memberName = memberName;
        }

        private ChangeDescriptor(SerializationInfo info, StreamingContext context)
        {
            this.changeId = (Guid)info.GetValue("changeId", typeof(Guid));
            this.sourceId = (Guid)info.GetValue("sourceId", typeof(Guid));
            this.version = (int)info.GetValue("version", typeof(int));
            this.memberName = (string)info.GetValue("memberName", typeof(string));
            this.arguments = (object[])info.GetValue("arguments", typeof(object[]));
            this.value = (object)info.GetValue("value", typeof(object));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("changedId", this.ChangeId, typeof(Guid));
            info.AddValue("sourceId", this.SourceId, typeof(Guid));
            info.AddValue("version", this.Version, typeof(int));
            info.AddValue("memberName", this.MemberName, typeof(string));
            info.AddValue("arguments", this.Arguments, typeof(object[]));
            info.AddValue("value", this.Value, typeof(object));
        }

        #endregion

        #region Properties

        public Guid ChangeId
        {
            get { return this.changeId; }
        }

        public Guid SourceId
        {
            get { return this.sourceId; }
        }

        public int Version
        {
            get { return this.version; }
        }

        public string MemberName
        {
            get { return this.memberName; }
        }

        public object[] Arguments
        {
            get { return this.arguments; }
        }

        public object Value
        {
            get { return this.value; }
        }

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
