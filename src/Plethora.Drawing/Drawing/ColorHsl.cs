using System;
using System.Drawing;
using System.Text;

namespace Plethora.Drawing
{
    /// <summary>
    /// Represents an HSL color. 
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   An HSL color is one which is described by hue, saturation and luminance.
    ///  </para>
    ///  <para>
    ///   The term luminance is used in preference to the .NET term brightness, to
    ///   avoid confusion as the the meaning of 'B' (brightness or blue).
    ///  </para>
    /// </remarks>
    public struct ColorHsl : IEquatable<ColorHsl>
    {
        #region Constants

        private const float MAX_HUE = 360f;
        private const float MAX_SATURATION = 1f;
        private const float MAX_LUMINANCE = 1f;

        private const float HALF = 1.0f / 2.0f;
        private const float THIRD = 1.0f / 3.0f;
        private const float TWO_THIRDS = 2.0f / 3.0f;
        private const float SIXTH = 1.0f / 6.0f;
        #endregion

        #region Fields

        private readonly int alfa;
        private readonly float hue;
        private readonly float saturation;
        private readonly float luminance;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ColorHsl"/> class,
        /// specifying the hue, saturation and luminance values.
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
        /// <param name="luminance">
        /// The luminance of the color. Valid values are 0.0 to 1.0.
        /// </param>
        private ColorHsl(int alfa, float hue, float saturation, float luminance)
        {
            this.alfa = alfa;
            this.hue = hue;
            this.saturation = saturation;
            this.luminance = luminance;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the alfa component value of this <see cref="ColorHsl"/> structure.
        /// </summary>
        public int A
        {
            get { return this.alfa; }
        }

        /// <summary>
        /// Gets the hue component value of this <see cref="ColorHsl"/> structure.
        /// </summary>
        public float H
        {
            get { return this.hue; }
        }

        /// <summary>
        /// Gets the saturation component value of this <see cref="ColorHsl"/>
        /// structure.
        /// </summary>
        public float S
        {
            get { return this.saturation; }
        }

        /// <summary>
        /// Gets the luminance component value of this <see cref="ColorHsl"/>
        /// structure.
        /// </summary>
        public float L
        {
            get { return this.luminance; }
        }
        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates a <see cref="ColorHsl"/> structure from the specified hue,
        /// saturation and luminance values.
        /// </summary>
        /// <param name="hue">
        /// The hue of the color. Valid values are 0.0 through 360.0.
        /// </param>
        /// <param name="saturation">
        /// The stauration of the color. Valid values are 0.0 through 1.0.
        /// </param>
        /// <param name="luminance">
        /// The luminance of the color. Valid values are 0.0 through 1.0.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsl"/> structure which this method creates.
        /// </returns>
        public static ColorHsl FromAhsl(float hue, float saturation, float luminance)
        {
            return FromAhsl(ColorBase.FullAlfa, hue, saturation, luminance);
        }

        /// <summary>
        /// Creates a <see cref="ColorHsl"/> structure from the specified alfa,
        /// hue, saturation and luminance values.
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
        /// <param name="luminance">
        /// The luminance of the color. Valid values are 0.0 through 1.0.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsl"/> structure which this method creates.
        /// </returns>
        public static ColorHsl FromAhsl(int alfa, float hue, float saturation, float luminance)
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

            if ((luminance < 0.0f) || (luminance > MAX_LUMINANCE))
                throw new ArgumentOutOfRangeException(nameof(luminance), luminance,
                  ResourceProvider.ArgMustBeBetween(nameof(luminance), 0.0f, MAX_LUMINANCE));


            return new ColorHsl(alfa, hue, saturation, luminance);
        }

        /// <summary>
        /// Creates a <see cref="ColorHsl"/> structure from the specified red,
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
        /// The <see cref="ColorHsl"/> structure which this method creates.
        /// </returns>
        public static ColorHsl FromArgb(int red, int green, int blue)
        {
            return FromArgb(ColorBase.FullAlfa, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="ColorHsl"/> structure from the specified alfa,
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
        /// The <see cref="ColorHsl"/> structure which this method creates.
        /// </returns>
        public static ColorHsl FromArgb(int alfa, int red, int green, int blue)
        {
            return FromColor(Color.FromArgb(alfa, red, green, blue));
        }

        /// <summary>
        /// Creates a <see cref="ColorHsl"/> structure from the specified Color.
        /// </summary>
        /// <param name="color">
        /// The Color struct from which the <see cref="ColorHsl"/> is created.
        /// </param>
        /// <returns>
        /// The <see cref="ColorHsl"/> structure which this method creates.
        /// </returns>
        public static ColorHsl FromColor(Color color)
        {
            return FromAhsl(color.A, color.GetHue(), color.GetSaturation(), color.GetBrightness());
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the Color representation of this <see cref="ColorHsl"/> structure.
        /// </summary>
        /// <returns>
        /// The Color represented by this <see cref="ColorHsl"/>.
        /// </returns>
        public Color ToColor()
        {
            float Q;
            if (this.luminance < 0.5f)
                Q = this.luminance * (1f + this.saturation);
            else
                Q = this.luminance + this.saturation - (this.luminance * this.saturation);

            float P = (2f * this.luminance) - Q;

            float Hk = this.hue / MAX_HUE;

            float Tr = Hk + THIRD;
            float Tg = Hk;
            float Tb = Hk - THIRD;

            int red = GetColorComponent(Tr, P, Q);
            int green = GetColorComponent(Tg, P, Q);
            int blue = GetColorComponent(Tb, P, Q);

            return Color.FromArgb(this.alfa, red, green, blue);
        }
        #endregion

        #region Private Methods

        private static int GetColorComponent(float Tc, float P, float Q)
        {
            float colorPercent = GetColorComponentPercent(Tc, P, Q);

            //Allow for rounding errors in which case colorPercent may be greater than 1, or less than 0.
            colorPercent = NumericHelper.Constrain(colorPercent, 0.0f, 1.0f);

            return Convert.ToInt32(NumericHelper.Translate(colorPercent, 255.0));
        }

        private static float GetColorComponentPercent(float Tc, float P, float Q)
        {
            Tc = NumericHelper.Wrap(Tc, 0.0f, 1.0f);

            if (Tc < SIXTH)
                return P + ((Q - P) * Tc * 6.0f);
            else if ((SIXTH <= Tc) && (Tc < HALF))
                return Q;
            else if ((HALF <= Tc) && (Tc < TWO_THIRDS))
                return P + ((Q - P) * (TWO_THIRDS - Tc) * 6.0f);
            else
                return P;
        }
        #endregion

        #region Implementation of IEquatable<ColorHsl>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ColorHsl other)
        {
            return
                (this.alfa == other.alfa) &&
                (this.hue == other.hue) &&
                (this.saturation == other.saturation) &&
                (this.luminance == other.luminance);
        }
        #endregion

        #region Object Overrrides

        /// <summary>
        /// Tests whether the specified object is a <see cref="ColorHsl"/>
        /// structure and is equivalent to this <see cref="ColorHsl"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>
        /// 'true' if obj is a <see cref="ColorHsl"/> structure equivalent to this
        /// <see cref="ColorHsl"/> structure; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorHsl))
                return false;

            
            ColorHsl other = (ColorHsl)obj;
            return this.Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="ColorHsl"/> structure.
        /// </summary>
        /// <returns>
        /// An integer value that specifies the hash code for this
        /// <see cref="ColorHsl"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(this.alfa, this.hue, this.saturation, this.luminance);
        }

        /// <summary>
        /// Converts this <see cref="ColorHsl"/> structure to a human-readable
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
            sb.Append(", L=");
            sb.Append(this.L);
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// Determines if two <see cref="ColorHsl"/> instances are equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorHsl"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorHsl"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are equal; else 'false'.
        /// </returns>
        public static bool operator ==(ColorHsl left, ColorHsl right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines if two <see cref="ColorHsl"/> instances are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorHsl"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorHsl"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are not equal; else 'false'.
        /// </returns>
        public static bool operator !=(ColorHsl left, ColorHsl right)
        {
            return (!(left == right));
        }
        #endregion
    }
}
