using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Plethora.Drawing
{
    /// <summary>
    /// Helper class for working with images.
    /// </summary>
    public static class ImageHelper
    {
        #region Fields

        private static ColorMatrix grayScaleColorMatrix;
        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a ColorMatrix which may be used to transform an image
        /// into a gray scale image.
        /// </summary>
        public static ColorMatrix GrayscaleColorMatrix
        {
            get
            {
                if (grayScaleColorMatrix == null)
                {
                    float contribR = ColorBase.EffectiveLuminanceContribR;
                    float contribG = ColorBase.EffectiveLuminanceContribG;
                    float contribB = ColorBase.EffectiveLuminanceContribB;

                    float[][] matrixArray = 
                        {
                            new[] { contribR, contribR, contribR, 0f, 0f },
                            new[] { contribG, contribG, contribG, 0f, 0f },
                            new[] { contribB, contribB, contribB, 0f, 0f },
                            new[] {       0f,       0f,       0f, 1f, 0f },
                            new[] {       0f,       0f,       0f, 0f, 1f }
                        };

                    grayScaleColorMatrix = new ColorMatrix(matrixArray);
                }

                return grayScaleColorMatrix;
            }
        }
        #endregion

        #region Public Static Methods

        /// <summary>
        /// Converts an image to gray scale.
        /// </summary>
        /// <param name="image">
        /// The image to be converted.
        /// </param>
        /// <returns>
        /// A Bitmap containing the image in grayscale.
        /// </returns>
        public static Image ToGrayscale(this Image image)
        {
            //Validation
            if (image == null)
                throw new ArgumentNullException(nameof(image));


            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ImageAttributes ia = new ImageAttributes();
                ia.SetColorMatrix(GrayscaleColorMatrix);

                g.DrawImage(
                  image,
                  new Rectangle(0, 0, image.Width, image.Height),
                  0, 0, image.Width, image.Height,
                  GraphicsUnit.Pixel,
                  ia);
            }
            return bmp;
        }
        #endregion
    }
}
