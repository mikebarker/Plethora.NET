using System;
using System.Drawing;
using System.Text;

namespace Plethora.Drawing
{
    /// <summary>
    /// Represents a gray scale color.
    /// </summary>
    /// <remarks>
    /// The effective luminance is calculated by the formulae:
    ///   <c>L = 0.3R + 0.59G + 0.11B</c>  
    /// </remarks>
    public struct ColorGrayscale : IEquatable<ColorGrayscale>
    {
        #region Fields

        private int alfa;
        private int effectiveLuminance;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ColorGrayscale"/> class,
        /// specifying the alfa and effective luminance.
        /// </summary>
        /// <param name="alfa">
        /// The alpha component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="effectiveLuminance">
        /// The effective luminance of the color. Valid values are 0 through 255. 
        /// </param>
        private ColorGrayscale(int alfa, int effectiveLuminance)
        {
            this.alfa = alfa;
            this.effectiveLuminance = effectiveLuminance;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the alfa component value of this <see cref="ColorGrayscale"/> structure.
        /// </summary>
        public int A
        {
            get { return alfa; }
        }

        /// <summary>
        /// Gets the cyan component value of this <see cref="ColorGrayscale"/> structure.
        /// </summary>
        public int EffectiveLuminance
        {
            get { return effectiveLuminance; }
        }
        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates a <see cref="ColorGrayscale"/> structure from the specified
        /// effective luminance.
        /// </summary>
        /// <param name="effectiveLuminance">
        /// The effective luminance of the gray sacle color.
        /// Valid values are 0 through 255. 
        /// </param>
        /// <returns>
        /// The <see cref="ColorGrayscale"/> structure which this method creates.
        /// </returns>
        public static ColorGrayscale FromAl(int effectiveLuminance)
        {
            return FromAl(ColorBase.FullAlfa, effectiveLuminance);
        }

        /// <summary>
        /// Creates a <see cref="ColorGrayscale"/> structure from the specified alfa,
        /// and effective luminance values.
        /// </summary>
        /// <param name="alfa">
        /// The alfa component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="effectiveLuminance">
        /// The effective luminance of the gray sacle color.
        /// Valid values are 0 through 255. 
        /// </param>
        /// <returns>
        /// The <see cref="ColorGrayscale"/> structure which this method creates.
        /// </returns>
        public static ColorGrayscale FromAl(int alfa, int effectiveLuminance)
        {
            //Validation
            if ((alfa < 0) || (alfa > ColorBase.FullAlfa))
                throw new ArgumentOutOfRangeException("alfa", alfa,
                  ResourceProvider.ArgMustBeBetween("alfa", 0, ColorBase.FullAlfa));

            if ((effectiveLuminance < 0) || (effectiveLuminance > ColorBase.ComponentMaximum))
                throw new ArgumentOutOfRangeException("effectiveLuminance", effectiveLuminance,
                  ResourceProvider.ArgMustBeBetween("effectiveLuminance", 0, ColorBase.ComponentMaximum));


            return new ColorGrayscale(alfa, effectiveLuminance);
        }

        /// <summary>
        /// Creates a <see cref="ColorGrayscale"/> structure from the specified red,
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
        /// The <see cref="ColorGrayscale"/> structure which this method creates.
        /// </returns>
        public static ColorGrayscale FromArgb(int red, int green, int blue)
        {
            return FromArgb(ColorBase.FullAlfa, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="ColorGrayscale"/> structure from the specified alfa,
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
        /// The <see cref="ColorGrayscale"/> structure which this method creates.
        /// </returns>
        public static ColorGrayscale FromArgb(int alfa, int red, int green, int blue)
        {
            int effectiveLuminance = (int)((ColorBase.EffectiveLuminanceContribR * red) +
                                           (ColorBase.EffectiveLuminanceContribG * green) +
                                           (ColorBase.EffectiveLuminanceContribB * blue));

            return FromAl(alfa, effectiveLuminance);
        }

        /// <summary>
        /// Creates a <see cref="ColorGrayscale"/> structure from the specified Color.
        /// </summary>
        /// <param name="color">
        /// The Color struct from which the <see cref="ColorGrayscale"/> is created.
        /// </param>
        /// <returns>
        /// The <see cref="ColorGrayscale"/> structure which this method creates.
        /// </returns>
        public static ColorGrayscale FromColor(Color color)
        {
            return FromArgb(color.A, color.R, color.G, color.B);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the Color representation of this <see cref="ColorGrayscale"/> structure.
        /// </summary>
        /// <returns>
        /// The Color represented by this <see cref="ColorGrayscale"/>.
        /// </returns>
        public Color ToColor()
        {
            return Color.FromArgb(alfa, effectiveLuminance, effectiveLuminance, effectiveLuminance);
        }
        #endregion

        #region Implementation of IEquatable<ColorGrayscale>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(ColorGrayscale other)
        {
            return
                (this.alfa == other.alfa) &&
                (this.effectiveLuminance == other.effectiveLuminance);
        }

        #endregion

        #region Object Overrrides

        /// <summary>
        /// Tests whether the specified object is a <see cref="ColorGrayscale"/>
        /// structure and is equivalent to this <see cref="ColorGrayscale"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>
        /// 'true' if obj is a <see cref="ColorGrayscale"/> structure equivalent to this
        /// <see cref="ColorGrayscale"/> structure; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorGrayscale))
                return false;

            ColorGrayscale other = (ColorGrayscale)obj;
            return Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="ColorGrayscale"/> structure.
        /// </summary>
        /// <returns>
        /// An integer value that specifies the hash code for this
        /// <see cref="ColorGrayscale"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(alfa, effectiveLuminance);
        }

        /// <summary>
        /// Converts this <see cref="ColorGrayscale"/> structure to a human-readable
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
            sb.Append(", L=");
            sb.Append(this.EffectiveLuminance);
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// Determines if two <see cref="ColorGrayscale"/> instances are equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorGrayscale"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorGrayscale"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are equal; else 'false'.
        /// </returns>
        public static bool operator ==(ColorGrayscale left, ColorGrayscale right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines if two <see cref="ColorGrayscale"/> instances are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorGrayscale"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorGrayscale"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are not equal; else 'false'.
        /// </returns>
        public static bool operator !=(ColorGrayscale left, ColorGrayscale right)
        {
            return (!(left == right));
        }
        #endregion
    }
}
