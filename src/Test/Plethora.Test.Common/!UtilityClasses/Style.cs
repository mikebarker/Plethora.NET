using System;

namespace Plethora.Test.UtilityClasses
{
    interface IStyle
    {
        string FontName { get; }
        int? FontSize { get; }
        FontProperty FontProperty { get; }
    }

    class Style : IStyle
    {
        private readonly string fontName;
        private readonly int? fontSize;
        private readonly FontProperty fontProperty;

        public Style(string fontName, int? fontSize, FontProperty fontProperty)
        {
            this.fontName = fontName;
            this.fontSize = fontSize;
            this.fontProperty = fontProperty;
        }


        public string FontName
        {
            get { return fontName; }
        }

        public int? FontSize
        {
            get { return fontSize; }
        }

        public FontProperty FontProperty
        {
            get { return fontProperty; }
        }
    }

    [Flags]
    enum FontProperty
    {
        None = 0x00,
        Bold = 0x01,
        Italic = 0x02,
        Underline = 0x04,
        StrikeThrough = 0x08,
    }
}
