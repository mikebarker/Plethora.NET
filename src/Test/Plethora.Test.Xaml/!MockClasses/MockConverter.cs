using System;
using System.Globalization;
using System.Windows.Data;

namespace Plethora.Test.Xaml
{
    internal class MockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            this.Value = value;
            this.TargetType = targetType;
            this.Parameter = parameter;
            this.Culture = culture;

            return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Value { get; private set; }

        public Type TargetType { get; private set; }

        public object Parameter { get; private set; }

        public CultureInfo Culture { get; private set; }
    }
}
