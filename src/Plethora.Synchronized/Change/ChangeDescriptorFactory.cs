namespace Plethora.Synchronized.Change
{
    public sealed class ChangeDescriptorFactory
    {
        #region Fields

        private readonly ChangeSourceIdProvider sourceIdProvider;

        #endregion

        #region Constructors

        public ChangeDescriptorFactory(ChangeSourceIdProvider sourceIdProvider)
        {
            this.sourceIdProvider = sourceIdProvider;
        }

        #endregion

        #region Properties

        public ChangeSourceIdProvider SourceIdProvider
        {
            get { return this.sourceIdProvider; }
        }

        #endregion

        #region Factory Methods

        public ChangeDescriptor CallMethod(int version, string methodName, object[] arguments)
        {
            return new ChangeDescriptor(this.sourceIdProvider.ChangeSourceId, version, methodName, arguments, null);
        }

        public ChangeDescriptor SetProperty(int version, string propertyName, object value)
        {
            return SetProperty(version, propertyName, null, value);
        }

        public ChangeDescriptor SetProperty(int version, string propertyName, object[] arguments, object value)
        {
            return new ChangeDescriptor(this.sourceIdProvider.ChangeSourceId, version, propertyName, arguments, value);
        }

        public ChangeDescriptor SetField(int version, string fieldName, object value)
        {
            return new ChangeDescriptor(this.sourceIdProvider.ChangeSourceId, version, fieldName, null, value);
        }

        #endregion
    }

}
