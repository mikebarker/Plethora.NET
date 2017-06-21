using System;
using System.Drawing;
using System.Text;

namespace Plethora.Drawing
{
    /// <summary>
    /// Represents a CMYK color.
    /// </summary>
    /// <remarks>
    /// A CMYK color is one which is described by cyan, magenta, yellow and
    /// key (black).
    /// </remarks>
    public struct ColorCmyk : IEquatable<ColorCmyk>
    {
        #region Fields

        private readonly int alfa;
        private readonly int cyan;
        private readonly int magenta;
        private readonly int yellow;
        private readonly int key;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ColorCmyk"/> class,
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
        /// <param name="key">
        /// The key (black) component. Valid values are 0 through 255. 
        /// </param>
        private ColorCmyk(int alfa, int cyan, int magenta, int yellow, int key)
        {
            this.alfa = alfa;
            this.cyan = cyan;
            this.magenta = magenta;
            this.yellow = yellow;
            this.key = key;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the alfa component value of this <see cref="ColorCmyk"/> structure.
        /// </summary>
        public int A
        {
            get { return this.alfa; }
        }

        /// <summary>
        /// Gets the cyan component value of this <see cref="ColorCmyk"/> structure.
        /// </summary>
        public int C
        {
            get { return this.cyan; }
        }

        /// <summary>
        /// Gets the magenta component value of this <see cref="ColorCmyk"/>
        /// structure.
        /// </summary>
        public int M
        {
            get { return this.magenta; }
        }

        /// <summary>
        /// Gets the yellow component value of this <see cref="ColorCmyk"/>
        /// structure.
        /// </summary>
        public int Y
        {
            get { return this.yellow; }
        }

        /// <summary>
        /// Gets the key (black) component value of this <see cref="ColorCmyk"/>
        /// structure.
        /// </summary>
        public int K
        {
            get { return this.key; }
        }
        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates a <see cref="ColorCmyk"/> structure from the specified cyan,
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
        /// <param name="key">
        /// The key (black) component. Valid values are 0 through 255. 
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> structure which this method creates.
        /// </returns>
        public static ColorCmyk FromCmyk(int cyan, int magenta, int yellow, int key)
        {
            return FromCmyk(ColorBase.FullAlfa, cyan, magenta, yellow, key);
        }

        /// <summary>
        /// Creates a <see cref="ColorCmyk"/> structure from the specified alfa,
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
        /// <param name="key">
        /// The key (black) component. Valid values are 0 through 255. 
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> structure which this method creates.
        /// </returns>
        public static ColorCmyk FromCmyk(int alfa, int cyan, int magenta, int yellow, int key)
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

            if ((key < 0) || (key > ColorBase.ComponentMaximum))
                throw new ArgumentOutOfRangeException(nameof(key), key,
                  ResourceProvider.ArgMustBeBetween(nameof(key), 0, ColorBase.ComponentMaximum));


            return new ColorCmyk(alfa, cyan, magenta, yellow, key);
        }

        /// <summary>
        /// Creates a <see cref="ColorCmyk"/> structure from the specified red,
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
        /// The <see cref="ColorCmyk"/> structure which this method creates.
        /// </returns>
        public static ColorCmyk FromArgb(int red, int green, int blue)
        {
            return FromArgb(ColorBase.FullAlfa, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="ColorCmyk"/> structure from the specified alfa,
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
        /// The <see cref="ColorCmyk"/> structure which this method creates.
        /// </returns>
        public static ColorCmyk FromArgb(int alfa, int red, int green, int blue)
        {
            return FromColor(Color.FromArgb(alfa, red, green, blue));
        }

        /// <summary>
        /// Creates a <see cref="ColorCmyk"/> structure from the specified ColorCmy.
        /// </summary>
        /// <param name="colorCmy">
        /// The ColorCmy struct from which the <see cref="ColorCmyk"/> is created.
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> structure which this method creates.
        /// </returns>
        public static ColorCmyk FromColorCmy(ColorCmy colorCmy)
        {
            int min = Math.Min(Math.Min(colorCmy.C, colorCmy.M), colorCmy.Y);

            int cyan = 0;
            int magenta = 0;
            int yellow = 0;
            int key = 0;

            if (min == ColorBase.ComponentMaximum)
            {
                key = ColorBase.ComponentMaximum;
            }
            else
            {
                key = min;

                cyan = GetCmykComponent(colorCmy.C, key);
                magenta = GetCmykComponent(colorCmy.M, key);
                yellow = GetCmykComponent(colorCmy.Y, key);
            }

            return FromCmyk(colorCmy.A, cyan, magenta, yellow, key);
        }

        /// <summary>
        /// Creates a <see cref="ColorCmyk"/> structure from the specified Color.
        /// </summary>
        /// <param name="color">
        /// The Color struct from which the <see cref="ColorCmyk"/> is created.
        /// </param>
        /// <returns>
        /// The <see cref="ColorCmyk"/> structure which this method creates.
        /// </returns>
        public static ColorCmyk FromColor(Color color)
        {
            ColorCmy colorCmy = ColorCmy.FromColor(color);
            return FromColorCmy(colorCmy);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the Color representation of this <see cref="ColorCmyk"/> structure.
        /// </summary>
        /// <returns>
        /// The Color represented by this <see cref="ColorCmyk"/>.
        /// </returns>
        public Color ToColor()
        {
            int _cyan = GetCmyComponent(this.C, this.K);
            int _magenta = GetCmyComponent(this.M, this.K);
            int _yellow = GetCmyComponent(this.Y, this.K);

            ColorCmy colorCmy = ColorCmy.FromCmy(this.alfa, _cyan, _magenta, _yellow);
            return colorCmy.ToColor();
        }
        #endregion

        #region Private Methods

        private static int GetCmyComponent(int CmykComponent, int key)
        {
            return (int)(CmykComponent * (1f - ((float)key / (float)ColorBase.ComponentMaximum))) + key;
        }

        private static int GetCmykComponent(int CmyComponent, int key)
        {
            return (int)(ColorBase.ComponentMaximum * (CmyComponent - key) / (ColorBase.ComponentMaximum - key));
        }
        #endregion

        #region Implementation of IEquatable<ColorCmyk>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ColorCmyk other)
        {
            return
                (this.alfa == other.alfa) &&
                (this.cyan == other.cyan) &&
                (this.magenta == other.magenta) &&
                (this.yellow == other.yellow) &&
                (this.key == other.key);
        }

        #endregion

        #region Object Overrrides

        /// <summary>
        /// Tests whether the specified object is a <see cref="ColorCmyk"/>
        /// structure and is equivalent to this <see cref="ColorCmyk"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>
        /// 'true' if obj is a <see cref="ColorCmyk"/> structure equivalent to this
        /// <see cref="ColorCmyk"/> structure; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorCmyk))
                return false;

            ColorCmyk other = (ColorCmyk)obj;
            return this.Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="ColorCmyk"/> structure.
        /// </summary>
        /// <returns>
        /// An integer value that specifies the hash code for this
        /// <see cref="ColorCmyk"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(this.alfa, this.cyan, this.magenta, this.yellow, this.key);
        }

        /// <summary>
        /// Converts this <see cref="ColorCmyk"/> structure to a human-readable
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
            sb.Append(", K=");
            sb.Append(this.K);
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// Determines if two <see cref="ColorCmyk"/> instances are equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorCmyk"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorCmyk"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are equal; else 'false'.
        /// </returns>
        public static bool operator ==(ColorCmyk left, ColorCmyk right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines if two <see cref="ColorCmyk"/> instances are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="ColorCmyk"/> to be compared.</param>
        /// <param name="right">The right <see cref="ColorCmyk"/> to be compared.</param>
        /// <returns>
        /// 'true' if the values of its operands are not equal; else 'false'.
        /// </returns>
        public static bool operator !=(ColorCmyk left, ColorCmyk right)
        {
            return (!(left == right));
        }
        #endregion
    }
}
