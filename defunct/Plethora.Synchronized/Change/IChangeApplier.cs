namespace Plethora.Synchronized.Change
{
    public interface IChangeApplier
    {
        void Apply(ChangeDescriptor change);
    }
}
