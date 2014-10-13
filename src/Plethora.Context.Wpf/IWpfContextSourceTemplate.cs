namespace Plethora.Context.Wpf
{
    /// <summary>
    /// Interface which defines the the methods required by a template to create a <see cref="IWpfContextSource"/>
    /// </summary>
    public interface IWpfContextSourceTemplate
    {
        WpfContextSourceBase CreateContent();
    }
}
