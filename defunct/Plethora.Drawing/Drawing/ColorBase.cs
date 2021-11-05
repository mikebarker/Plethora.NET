namespace Plethora.Drawing
{
    /// <summary>
    /// Contains the constant values for colors.
    /// </summary>
    public static class ColorBase
    {
        #region Constants

        private const int FULL_ALFA = 255;
        private const int COLOR_COMPONENT_MAX = 255;

        private const float EFFECTIVE_LUMINANCE_CONTIB_R = 0.3f;
        private const float EFFECTIVE_LUMINANCE_CONTIB_G = 0.59f;
        private const float EFFECTIVE_LUMINANCE_CONTIB_B = 0.11f;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the full value of 'alfa' for a color.
        /// </summary>
        public static int FullAlfa
        {
            get { return FULL_ALFA; }
        }

        /// <summary>
        /// Gets the maximum value that a color component may take
        /// </summary>
        public static int ComponentMaximum
        {
            get { return COLOR_COMPONENT_MAX; }
        }

        /// <summary>
        /// Gets the contribution which the red component of a color makes to the
        /// over all effective luminance in gray-scale.
        /// </summary>
        public static float EffectiveLuminanceContribR
        {
            get { return EFFECTIVE_LUMINANCE_CONTIB_R; }
        }

        /// <summary>
        /// Gets the contribution which the green component of a color makes to the
        /// over all effective luminance in gray-scale.
        /// </summary>
        public static float EffectiveLuminanceContribG
        {
            get { return EFFECTIVE_LUMINANCE_CONTIB_G; }
        }

        /// <summary>
        /// Gets the contribution which the blue component of a color makes to the
        /// over all effective luminance in gray-scale.
        /// </summary>
        public static float EffectiveLuminanceContribB
        {
            get { return EFFECTIVE_LUMINANCE_CONTIB_B; }
        }
        #endregion
    }
}
