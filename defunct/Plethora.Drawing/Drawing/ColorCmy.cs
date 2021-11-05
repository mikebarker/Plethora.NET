using System;
using System.Drawing;
using System.Text;

namespace Plethora.Drawing
{
    /// <summary>
    /// Represents a CMY color. 
    /// </summary>
    /// <remarks>
    /// A CMY color is one which is described by cyan, magenta and yellow.
    /// </remarks>
    public struct ColorCmy : IEquatable<ColorCmy>
    {
        #region Fields

        private readonly int alfa;
        private readonly int cyan;
        private readonly int magenta;
        private readonly int yellow;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ColorCmy"/> class,
        /// specifying the hue, saturation and value levels.
        /// </summary>
        /// <param name="alfa">
        /// The alpha component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="cyan">
        /// The cyan component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="magenta">
        /// The magenta component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="yellow">
        /// The yellow component. Valid values are 0 through 255. 
        /// </param>
        private ColorCmy(int alfa, int cyan, int magenta, int yellow)
        {
            this.alfa = alfa;
            this.cyan = cyan;
            this.magenta = magenta;
            this.yellow = yellow;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the alfa component value of this <see cref="ColorCmy"/> structure.
        /// </summary>
        public int A
        {
            get { return this.alfa; }
        }

        /// <summary>
        /// Gets the cyan component value of this <see cref="ColorCmy"/> structure.
        /// </summary>
        public int C
        {
            get { return this.cyan; }
        }

        /// <summary>
        /// Gets the magenta component value of this <see cref="ColorCmy"/>
        /// structure.
        /// </summary>
        public int M
        {
            get { return this.magenta; }
        }

        /// <summary>
        /// Gets the yellow component value of this <see cref="ColorCmy"/>
        /// structure.
        /// </summary>
        public int Y
        {
            get { return this.yellow; }
        }
        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates a <see cref="ColorCmy"/> structure from the specified cyan,
        /// magenta, yellow and key (black) values.
        /// </summary>
        /// <param name="cyan">
        /// The cyan component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="magenta">
        /// The magenta component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="yellow">
        /// The yellow component. Valid values are 0 through 255. 
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmy"/> structure which this method creates.
        /// </returns>
        public static ColorCmy FromCmy(int cyan, int magenta, int yellow)
        {
            return FromCmy(ColorBase.FullAlfa, cyan, magenta, yellow);
        }

        /// <summary>
        /// Creates a <see cref="ColorCmy"/> structure from the specified alfa,
        /// hue, saturation and value values.
        /// </summary>
        /// <param name="alfa">
        /// The alpha component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="cyan">
        /// The cyan component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="magenta">
        /// The magenta component. Valid values are 0 through 255. 
        /// </param>
        /// <param name="yellow">
        /// The yellow component. Valid values are 0 through 255. 
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmy"/> structure which this method creates.
        /// </returns>
        public static ColorCmy FromCmy(int alfa, int cyan, int magenta, int yellow)
        {
            //Validation
            if ((alfa < 0) || (alfa > ColorBase.FullAlfa))
                throw new ArgumentOutOfRangeException(nameof(alfa), alfa,
                  ResourceProvider.ArgMustBeBetween(nameof(alfa), 0, ColorBase.FullAlfa));

            if ((cyan < 0) || (cyan > ColorBase.ComponentMaximum))
                throw new ArgumentOutOfRangeException(nameof(cyan), cyan,
                  ResourceProvider.ArgMustBeBetween(nameof(cyan), 0, ColorBase.ComponentMaximum));

            if ((magenta < 0) || (magenta > ColorBase.ComponentMaximum))
                throw new ArgumentOutOfRangeException(nameof(magenta), magenta,
                  ResourceProvider.ArgMustBeBetween(nameof(magenta), 0, ColorBase.ComponentMaximum));

            if ((yellow < 0) || (yellow > ColorBase.ComponentMaximum))
                throw new ArgumentOutOfRangeException(nameof(yellow), yellow,
                  ResourceProvider.ArgMustBeBetween(nameof(yellow), 0, ColorBase.ComponentMaximum));


            return new ColorCmy(alfa, cyan, magenta, yellow);
        }

        /// <summary>
        /// Creates a <see cref="ColorCmy"/> structure from the specified red,
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
        /// The <see cref="ColorCmy"/> structure which this method creates.
        /// </returns>
        public static ColorCmy FromArgb(int red, int green, int blue)
        {
            return FromArgb(ColorBase.FullAlfa, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="ColorCmy"/> structure from the specified alfa,
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
        /// The <see cref="ColorCmy"/> structure which this method creates.
        /// </returns>
        public static ColorCmy FromArgb(int alfa, int red, int green, int blue)
        {
            return FromColor(Color.FromArgb(alfa, red, green, blue));
        }

        /// <summary>
        /// Creates a <see cref="ColorCmy"/> structure from the specified Color.
        /// </summary>
        /// <param name="color">
        /// The Color struct from which the <see cref="ColorCmy"/> is created.
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmy"/> structure which this method creates.
        /// </returns>
        public static ColorCmy FromColor(Color color)
        {
            int cyan = ColorBase.ComponentMaximum - color.R;
            int magenta = ColorBase.ComponentMaximum - color.G;
            int yellow = ColorBase.ComponentMaximum - color.B;

            return FromCmy(color.A, cyan, magenta, yellow);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the Color representation of this <see cref="ColorCmy"/> structure.
        /// </summary>
        /// <returns>
        /// The Color represented by this <see cref="ColorCmy"/>.
        /// </returns>
        public Color ToColor()
        {
            int red = ColorBase.ComponentMaximum - this.cyan;
            int green = ColorBase.ComponentMaximum - this.magenta;
            int blue = ColorBase.ComponentMaximum - this.yellow;

            return Color.FromArgb(this.alfa, red, green, blue);
        }
        #endregion

        #region Implementation of IEquatable<ColorCmy>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ColorCmy other)
        {
            return
                (this.alfa == other.alfa) &&
                (this.cyan == other.cyan) &&
                (this.magenta == other.magenta) &&
                (this.yellow == other.yellow);
        }

        #endregion

        #region Object Overrrides

        /// <summary>
        /// Tests whether the specified object is a <see cref="ColorCmy"/>
        /// structure and is equivalent to this <see cref="ColorCmy"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>
        /// 'true' if obj is a <see cref="ColorCmy"/> structure equivalent to this
        /// <see cref="ColorCmy"/> structure; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorCmy))
                return false;

            ColorCmy other = (ColorCmy)obj;
            return this.Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="ColorCmy"/> structure.
        /// </summary>
        /// <returns>
        /// An integer value that specifies the hash code for this
        /// <see cref="ColorCmy"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(this.alfa, this.cyan, this.magenta, this.yellow);
        }

        /// <summary>
        /// Converts this <see cref="ColorCmy"/> structure to a human-readable
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
            sb.Append(", C=");
            sb.Append(this.C);
            sb.Append(", M=");
            sb.Append(this.M);
            sb.Append(", Y=");
            sb.Append(this.Y);
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// Determines if two <see cref="ColorCmy"/> instances are equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorCmy"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorCmy"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are equal; else 'false'.
        /// </returns>
        public static bool operator ==(ColorCmy left, ColorCmy right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines if two <see cref="ColorCmy"/> instances are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorCmy"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorCmy"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are not equal; else 'false'.
        /// </returns>
        public static bool operator !=(ColorCmy left, ColorCmy right)
        {
            return (!(left == right));
        }
        #endregion
    }
}
