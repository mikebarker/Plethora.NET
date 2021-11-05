using System;
using System.Windows;
using System.Windows.Markup;

namespace Plethora.Xaml.Wpf
{
    /// <summary>
    /// Merges two styles into a single resulting style.
    /// </summary>
    /// <example>
    /// The <see cref="MultiStyleExtension"/> is used as follows:
    /// <code><![CDATA[
    ///     <Style x:Key="readOnlyStyle" TargetType="TextBoxBase">
    ///         <Style.Triggers>
    ///             <Trigger Property="IsReadOnly" Value="True">
    ///                 <Setter Property="Background" Value="Transparent" />
    ///             </Trigger>
    ///         </Style.Triggers>
    ///     </Style>
    /// 
    ///     <Style x:Key="uppercaseStyle" TargetType="TextBox">
    ///         <Style.Setters>
    ///             <Setter Property="CharacterCasing" Value="Upper" />
    ///         </Style.Setters>
    ///     </Style>
    /// 
    ///     <Style x:Key="readOnlyUppercaseStyle" TargetType="{TextBox}" BasedOn="{xaml:MultiStyle {StaticResource readOnlyStyle}, {StaticResource uppercaseStyle}}" />
    /// ]]></code>
    /// </example>
    /// <remarks>
    /// <para>
    /// Setters and Triggers from <see cref="Style1"/> are used in preference from those on <see cref="Style2"/>.
    /// </para>
    /// <para>
    /// Based on code found at:
    /// <see cref="http://web.archive.org/web/20101125040337/http://bea.stollnitz.com/blog/?p=384"/>
    /// </para>
    /// </remarks>
    [MarkupExtensionReturnType(typeof(Style))]
    public class MultiStyleExtension : MarkupExtension
    {
        private object style1;
        private object style2;

        /// <summary>
        /// Initialises a new instance of the <see cref="MultiStyleExtension"/> class.
        /// </summary>
        /// <param name="style1">The first style to be merged, or a resource key referencing a style.</param>
        /// <param name="style2">The second style to be merged, or a resource key referencing a style.</param>
        public MultiStyleExtension(object style1, object style2)
        {
            if (style1 == null)
                throw new ArgumentNullException(nameof(style1));

            if (style2 == null)
                throw new ArgumentNullException(nameof(style2));


            this.style1 = (Style)style1;
            this.style2 = (Style)style2;
        }

        /// <summary>
        /// Gets and sets the first style to be merged, or a resource key referencing a style.
        /// </summary>
        public object Style1
        {
            get { return this.style1; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                this.style1 = value;
            }
        }

        /// <summary>
        /// Gets and sets the first style to be merged, or a resource key referencing a style.
        /// </summary>
        public object Style2
        {
            get { return this.style2; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                this.style2 = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="Style"/> which is the result of merging <see cref="Style1"/> and <see cref="Style2"/>.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>
        /// The resulting merged <see cref="Style"/>.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Style result = new Style();
            Merge(result, GetAsStyle(this.style2, serviceProvider));
            Merge(result, GetAsStyle(this.style1, serviceProvider));

            return result;
        }

        private static Style GetAsStyle(object obj, IServiceProvider serviceProvider)
        {
            if (obj is Style)
            {
                return (Style)obj;
            }
            else
            {
                object objStyle = new StaticResourceExtension(obj).ProvideValue(serviceProvider);

                if (objStyle == null)
                    throw new InvalidOperationException(ResourceProvider.ResourceNotLocated(obj));

                Style style = objStyle as Style;

                if (style == null)
                    throw new InvalidOperationException(ResourceProvider.ResourceIsNotOfType(obj, typeof(Style)));

                return style;
            }
        }

        private static void Merge(Style destinationStyle, Style sourceStyle)
        {
            if (destinationStyle.TargetType.IsAssignableFrom(sourceStyle.TargetType))
            {
                destinationStyle.TargetType = sourceStyle.TargetType;
            }

            if (sourceStyle.BasedOn != null)
            {
                Merge(destinationStyle, sourceStyle.BasedOn);
            }

            foreach (SetterBase setter in sourceStyle.Setters)
            {
                destinationStyle.Setters.Add(setter);
            }

            foreach (TriggerBase trigger in sourceStyle.Triggers)
            {
                destinationStyle.Triggers.Add(trigger);
            }

            foreach (object key in sourceStyle.Resources.Keys)
            {
                destinationStyle.Resources[key] = sourceStyle.Resources[key];
            }
        }
    }
}
