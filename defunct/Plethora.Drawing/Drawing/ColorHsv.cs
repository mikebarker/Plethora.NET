using System;
using System.Drawing;
using System.Text;

namespace Plethora.Drawing
{
    /// <summary>
    /// Represents an HSV color. 
    /// </summary>
    /// <remarks>
    /// An HSV color is one which is described by hue, saturation and value.
    /// </remarks>
    public struct ColorHsv : IEquatable<ColorHsv>
    {
        #region Constants

        private const float MAX_HUE = 360f;
        private const float MAX_SATURATION = 1f;
        private const float MAX_VALUE = 1f;
        #endregion

        #region Fields

        private readonly int alfa;
        private readonly float hue;
        private readonly float saturation;
        private readonly float value;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ColorHsv"/> class,
        /// specifying the hue, saturation and value levels.
        /// </summary>
        /// <param name="alfa">
        /// The alpha component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="hue">
        /// The hue of the color. Valid values are 0.0 to 360.0.
        /// </param>
        /// <param name="saturation">
        /// The stauration of the color. Valid values are 0.0 to 1.0.
        /// </param>
        /// <param name="value">
        /// The value of the color. Valid values are 0.0 to 1.0.
        /// </param>
        private ColorHsv(int alfa, float hue, float saturation, float value)
        {
            this.alfa = alfa;
            this.hue = hue;
            this.saturation = saturation;
            this.value = value;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the alfa component value of this <see cref="ColorHsv"/> structure.
        /// </summary>
        public int A
        {
            get { return this.alfa; }
        }

        /// <summary>
        /// Gets the hue component value of this <see cref="ColorHsv"/> structure.
        /// </summary>
        public float H
        {
            get { return this.hue; }
        }

        /// <summary>
        /// Gets the saturation component value of this <see cref="ColorHsv"/>
        /// structure.
        /// </summary>
        public float S
        {
            get { return this.saturation; }
        }

        /// <summary>
        /// Gets the value component value of this <see cref="ColorHsv"/>
        /// structure.
        /// </summary>
        public float V
        {
            get { return this.value; }
        }
        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates a <see cref="ColorHsv"/> structure from the specified hue,
        /// saturation and value values.
        /// </summary>
        /// <param name="hue">
        /// The hue of the color. Valid values are 0.0 through 360.0.
        /// </param>
        /// <param name="saturation">
        /// The stauration of the color. Valid values are 0.0 through 1.0.
        /// </param>
        /// <param name="value">
        /// The value of the color. Valid values are 0.0 through 1.0.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> structure which this method creates.
        /// </returns>
        public static ColorHsv FromAhsv(float hue, float saturation, float value)
        {
            return FromAhsv(ColorBase.FullAlfa, hue, saturation, value);
        }

        /// <summary>
        /// Creates a <see cref="ColorHsv"/> structure from the specified alfa,
        /// hue, saturation and value values.
        /// </summary>
        /// <param name="alfa">
        /// The alpha component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="hue">
        /// The hue of the color. Valid values are 0.0 through 360.0.
        /// </param>
        /// <param name="saturation">
        /// The stauration of the color. Valid values are 0.0 through 1.0.
        /// </param>
        /// <param name="value">
        /// The value of the color. Valid values are 0.0 through 1.0.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> structure which this method creates.
        /// </returns>
        public static ColorHsv FromAhsv(int alfa, float hue, float saturation, float value)
        {
            //Validation
            if ((alfa < 0) || (alfa > ColorBase.FullAlfa))
                throw new ArgumentOutOfRangeException(nameof(alfa), alfa,
                  ResourceProvider.ArgMustBeBetween(nameof(alfa), 0, ColorBase.FullAlfa));

            if ((hue < 0.0f) || (hue >= MAX_HUE))
                throw new ArgumentOutOfRangeException(nameof(hue), hue,
                  ResourceProvider.ArgMustBeBetween(nameof(hue), 0.0f, MAX_HUE));

            if ((saturation < 0.0f) || (saturation > MAX_SATURATION))
                throw new ArgumentOutOfRangeException(nameof(saturation), saturation,
                  ResourceProvider.ArgMustBeBetween(nameof(saturation), 0.0f, MAX_SATURATION));

            if ((value < 0.0f) || (value > MAX_VALUE))
                throw new ArgumentOutOfRangeException(nameof(value), value,
                  ResourceProvider.ArgMustBeBetween(nameof(value), 0.0f, MAX_VALUE));


            return new ColorHsv(alfa, hue, saturation, value);
        }

        /// <summary>
        /// Creates a <see cref="ColorHsv"/> structure from the specified red,
        /// green and blue values.
        /// </summary>
        /// <param name="red">
        /// The red component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="green">
        /// The green component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="blue">
        /// The blue component. Valid values are 0 through 255. 
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> structure which this method creates.
        /// </returns>
        public static ColorHsv FromArgb(int red, int green, int blue)
        {
            return FromArgb(ColorBase.FullAlfa, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="ColorHsv"/> structure from the specified alfa,
        /// red, green and blue values.
        /// </summary>
        /// <param name="alfa">
        /// The alfa component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="red">
        /// The red component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="green">
        /// The green component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="blue">
        /// The blue component. Valid values are 0 through 255. 
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> structure which this method creates.
        /// </returns>
        public static ColorHsv FromArgb(int alfa, int red, int green, int blue)
        {
            return FromColor(Color.FromArgb(alfa, red, green, blue));
        }

        /// <summary>
        /// Creates a <see cref="ColorHsv"/> structure from the specified Color.
        /// </summary>
        /// <param name="color">
        /// The Color struct from which the <see cref="ColorHsv"/> is created.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsv"/> structure which this method creates.
        /// </returns>
        public static ColorHsv FromColor(Color color)
        {
            float min = Math.Min(Math.Min(color.R, color.G), color.B) / 255.0f;
            float max = Math.Max(Math.Max(color.R, color.G), color.B) / 255.0f;

            float hue = color.GetHue();

            float saturation;
            if (max == 0f)
                saturation = 0f;
            else
                saturation = 1.0f - (min / max);

            float value = max;

            return FromAhsv(color.A, hue, saturation, value);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the Color representation of this <see cref="ColorHsv"/> structure.
        /// </summary>
        /// <returns>
        /// The Color represented by this <see cref="ColorHsv"/>.
        /// </returns>
        public Color ToColor()
        {
            float H_ = (this.H / 60f);
            int Hi = ((int)H_) % 6;

            float f = H_ - Hi;

            float p = this.value * (1 - this.saturation);
            float q = this.value * (1 - f * this.saturation);
            float t = this.value * (1 - (1 - f) * this.saturation);

            float _red = 0f;
            float _green = 0f;
            float _blue = 0f;

            switch (Hi)
            {
                case 0:
                    _red = this.value; _green = t; _blue = p;
                    break;
                case 1:
                    _red = q; _green = this.value; _blue = p;
                    break;
                case 2:
                    _red = p; _green = this.value; _blue = t;
                    break;
                case 3:
                    _red = p; _green = q; _blue = this.value;
                    break;
                case 4:
                    _red = t; _green = p; _blue = this.value;
                    break;
                case 5:
                    _red = this.value; _green = p; _blue = q;
                    break;
            }

            int red = Translate(_red);
            int green = Translate(_green);
            int blue = Translate(_blue);

            return Color.FromArgb(this.alfa, red, green, blue);
        }
        #endregion

        #region Private Methods

        private static int Translate(float colorComponent)
        {
            //Allow for rounding errors in which case colorPercent may be greater than 1, or less than 0.
            colorComponent = NumericHelper.Constrain(colorComponent, 0.0f, 1.0f);

            return Convert.ToInt32(NumericHelper.Translate(colorComponent, 255.0));
        }
        #endregion

        #region Implementation of IEquatable<ColorHsv>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ColorHsv other)
        {
            return
                (this.alfa == other.alfa) &&
                (this.hue == other.hue) &&
                (this.saturation == other.saturation) &&
                (this.value == other.value);
        }

        #endregion

        #region Object Overrrides

        /// <summary>
        /// Tests whether the specified object is a <see cref="ColorHsv"/>
        /// structure and is equivalent to this <see cref="ColorHsv"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>
        /// 'true' if obj is a <see cref="ColorHsv"/> structure equivalent to this
        /// <see cref="ColorHsv"/> structure; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorHsv))
                return false;

            ColorHsv other = (ColorHsv)obj;
            return this.Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="ColorHsv"/> structure.
        /// </summary>
        /// <returns>
        /// An integer value that specifies the hash code for this
        /// <see cref="ColorHsv"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(this.alfa, this.hue, this.saturation, this.value);
        }

        /// <summary>
        /// Converts this <see cref="ColorHsv"/> structure to a human-readable
        /// string.
        /// </summary>
        /// <returns>
        /// A String containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(32);
            sb.Append(this.GetType().Name);
            sb.Append(" [");
            sb.Append("A=");
            sb.Append(this.A);
            sb.Append(", H=");
            sb.Append(this.H);
            sb.Append(", S=");
            sb.Append(this.S);
            sb.Append(", V=");
            sb.Append(this.V);
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// Determines if two <see cref="ColorHsv"/> instances are equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorHsv"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorHsv"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are equal; else 'false'.
        /// </returns>
        public static bool operator ==(ColorHsv left, ColorHsv right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines if two <see cref="ColorHsv"/> instances are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorHsv"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorHsv"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are not equal; else 'false'.
        /// </returns>
        public static bool operator !=(ColorHsv left, ColorHsv right)
        {
            return (!(left == right));
        }
        #endregion
    }
}
