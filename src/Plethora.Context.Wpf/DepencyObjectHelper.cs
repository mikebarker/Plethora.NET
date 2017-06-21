using System;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Context.Wpf
{
    internal static class DepencyObjectHelper
    {
        public static void CopyPropertyWithBinding(
            DependencyObject source, DependencyProperty sourceProperty,
            DependencyObject target, DependencyProperty targetProperty)
        {
            CopyPropertyWithBinding(
                source, sourceProperty,
                target, targetProperty,
                null);
        }

        public static void CopyPropertyWithBinding(
            DependencyObject source, DependencyProperty sourceProperty,
            DependencyObject target, DependencyProperty targetProperty,
            Action<Binding> bindingModifier)
        {
            //Validation
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (sourceProperty == null)
                throw new ArgumentNullException(nameof(sourceProperty));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (targetProperty == null)
                throw new ArgumentNullException(nameof(targetProperty));

            if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                throw new ArgumentException("Target property is not assignable from source property.");


            Binding binding = BindingOperations.GetBinding(source, sourceProperty);
            if (binding == null)
            {
                object value = source.GetValue(sourceProperty);
                target.SetValue(targetProperty, value);
            }
            else
            {
                Binding clone = new Binding();
                clone.AsyncState = binding.AsyncState;
                clone.BindingGroupName = binding.BindingGroupName;
                clone.BindsDirectlyToSource = binding.BindsDirectlyToSource;
                clone.Converter = binding.Converter;
                clone.ConverterCulture = binding.ConverterCulture;
                clone.ConverterParameter = binding.ConverterParameter;
//TODO: .Net 4.5
//              clone.Delay = binding.Delay;
                clone.FallbackValue = binding.FallbackValue;
                clone.IsAsync = binding.IsAsync;
                clone.Mode = binding.Mode;
                clone.NotifyOnSourceUpdated = binding.NotifyOnSourceUpdated;
                clone.NotifyOnTargetUpdated = binding.NotifyOnTargetUpdated;
                clone.NotifyOnValidationError = binding.NotifyOnValidationError;
                clone.Path = binding.Path;
                clone.StringFormat = binding.StringFormat;
                clone.TargetNullValue = binding.TargetNullValue;
                clone.UpdateSourceExceptionFilter = binding.UpdateSourceExceptionFilter;
                clone.UpdateSourceTrigger = binding.UpdateSourceTrigger;
                clone.ValidatesOnDataErrors = binding.ValidatesOnDataErrors;
                clone.ValidatesOnExceptions = binding.ValidatesOnExceptions;
//TODO: .Net 4.5
//              clone.ValidatesOnNotifyDataErrors = binding.ValidatesOnNotifyDataErrors;
                clone.XPath = binding.XPath;

                if (binding.ElementName != null)
                    clone.ElementName = binding.ElementName;
                else if (binding.Source != null)
                    clone.Source = binding.Source;
                else if (binding.RelativeSource != null)
                    clone.RelativeSource = binding.RelativeSource;

                if (bindingModifier != null)
                    bindingModifier(clone);

                BindingOperations.SetBinding(target, targetProperty, clone);
            }
        }
    }
}
