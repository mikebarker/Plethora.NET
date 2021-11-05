using System;
using System.Drawing;
using System.Reflection;
using System.Resources;

namespace Plethora.Drawing
{
    /// <summary>
    /// Extension of the <see cref="ToolboxBitmapAttribute"/>, which allows an image or
    /// icon to be retrieved from resources objects.
    /// </summary>
    /// <example>
    ///  <para>
    ///   This is attribute is used as follows:
    ///    <code>
    ///       [ToolboxBitmapEx(typeof(MyApplication.Properties.Resources), "MyControl")]
    ///       public class MyControl : UserControl
    ///       {
    ///       }
    ///    </code>
    ///   where MyApplication.Properties.Resources is the assembly's automatically created
    ///   resource, with an <see cref="Image"/> or <see cref="Icon"/> named "MyControl".
    ///  </para>
    ///  <para>
    ///   The "MyControl" file does NOT need to have the "Embedded Resource" build action set.
    ///  </para>
    /// </example>
    public class ToolboxBitmapExAttribute : ToolboxBitmapAttribute
    {
        #region Fields

        private static readonly FieldInfo largeImageField = typeof(ToolboxBitmapAttribute)
            .GetField("largeImage", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo smallImageField = typeof(ToolboxBitmapAttribute)
            .GetField("smallImage", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo largeDimField = typeof(ToolboxBitmapAttribute)
            .GetField("largeDim", BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly FieldInfo smallDimField = typeof(ToolboxBitmapAttribute)
            .GetField("smallDim", BindingFlags.Static | BindingFlags.NonPublic);
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="ToolboxBitmapExAttribute"/> class.
        /// </summary>
        protected ToolboxBitmapExAttribute()
            : base("/")
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="ToolboxBitmapExAttribute"/> class.
        /// </summary>
        public ToolboxBitmapExAttribute(Type resourcesType, string imageName)
            : this()
        {
            try
            {
                this.SetImagesFromResource(resourcesType, imageName);
            }
            catch (Exception)
            {
                this.SmallImage = null;
                this.LargeImage = null;
            }
        }
        #endregion

        #region Properties

        public Image SmallImage
        {
            get { return GetImage(smallImageField); }
            protected set { SetImage(smallImageField, value); }
        }

        public Image LargeImage
        {
            get { return GetImage(largeImageField); }
            protected set { SetImage(largeImageField, value); }
        }

        protected static Point SmallDim
        {
            get
            {
                if (smallDimField == null)
                    return Point.Empty;

                return (Point)smallDimField.GetValue(null);
            }
        }

        protected static Point LargeDim
        {
            get
            {
                if (largeDimField == null)
                    return Point.Empty;

                return (Point)largeDimField.GetValue(null);
            }
        }
        #endregion

        #region Non-public Methods

        protected void SetImagesFromResource(Type t, string imageName)
        {
            if (t == null)
                return;

            if (t.FullName == null)
                return;

            if (imageName == null)
                return;

            var resourceManager = new ResourceManager(t.FullName, t.Assembly);
            object obj = resourceManager.GetObject(imageName);
            if (obj == null)
                return;

            if (obj is Image)
            {
                Image image = (Image)obj;
                Point largeDim = LargeDim;
                this.SmallImage = (Image)obj;
                this.LargeImage = new Bitmap(image, largeDim.X, largeDim.Y);
            }

            if (obj is Icon)
            {
                Icon icon = (Icon)obj;
                Point smallDim = SmallDim;
                Point largeDim = LargeDim;
                this.SmallImage = new Icon(icon, smallDim.X, smallDim.Y).ToBitmap();
                this.LargeImage = new Icon(icon, largeDim.X, largeDim.Y).ToBitmap();
            }

            return;
        }

        private void SetImage(FieldInfo imageField, Image image)
        {
            if (imageField == null)
                return;

            imageField.SetValue(this, image);
        }

        private Image GetImage(FieldInfo imageField)
        {
            if (imageField == null)
                return null;

            var image = imageField.GetValue(this) as Image;
            return image;
        }
        #endregion
    }
}
