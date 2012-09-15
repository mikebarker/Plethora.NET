using System;

namespace Plethora.Cache.Spacial
{
    public class SpacialDefinition<T1>
    {
        #region Fields

        private readonly DimensionDefinition<T1> dimension1;
        #endregion

        #region Constructors

        public SpacialDefinition(
            DimensionDefinition<T1> dimension1)
        {
            //Validation
            if (dimension1 == null)
                throw new ArgumentNullException("dimension1");


            this.dimension1 = dimension1;
        }
        #endregion
        
        #region Public Members

        public SpacialRegion<T1> CreateRegion(
            Range<T1> dim1)
        {
            return new SpacialRegion<T1>(this, dim1);
        }
        #endregion

        #region Internal Members

        internal DimensionDefinition<T1> Dimension1
        {
            get { return this.dimension1; }
        }
        #endregion
    }

    public class SpacialDefinition<T1, T2>
    {
        #region Fields

        private readonly DimensionDefinition<T1> dimension1;
        private readonly DimensionDefinition<T2> dimension2;
        #endregion

        #region Constructors

        public SpacialDefinition(
            DimensionDefinition<T1> dimension1,
            DimensionDefinition<T2> dimension2)
        {
            //Validation
            if (dimension1 == null)
                throw new ArgumentNullException("dimension1");

            if (dimension2 == null)
                throw new ArgumentNullException("dimension2");


            this.dimension1 = dimension1;
            this.dimension2 = dimension2;
        }
        #endregion
        
        #region Public Members

        public SpacialRegion<T1, T2> CreateRegion(
            Range<T1> dim1,
            Range<T2> dim2)
        {
            return new SpacialRegion<T1, T2>(this, dim1, dim2);
        }
        #endregion

        #region Internal Members

        internal DimensionDefinition<T1> Dimension1
        {
            get { return this.dimension1; }
        }

        internal DimensionDefinition<T2> Dimension2
        {
            get { return this.dimension2; }
        }
        #endregion
    }

    public class SpacialDefinition<T1, T2, T3>
    {
        #region Fields

        private readonly DimensionDefinition<T1> dimension1;
        private readonly DimensionDefinition<T2> dimension2;
        private readonly DimensionDefinition<T3> dimension3;
        #endregion

        #region Constructors

        public SpacialDefinition(
            DimensionDefinition<T1> dimension1,
            DimensionDefinition<T2> dimension2,
            DimensionDefinition<T3> dimension3)
        {
            //Validation
            if (dimension1 == null)
                throw new ArgumentNullException("dimension1");

            if (dimension2 == null)
                throw new ArgumentNullException("dimension2");

            if (dimension3 == null)
                throw new ArgumentNullException("dimension3");


            this.dimension1 = dimension1;
            this.dimension2 = dimension2;
            this.dimension3 = dimension3;
        }
        #endregion
        
        #region Public Members

        public SpacialRegion<T1, T2, T3> CreateRegion(
            Range<T1> dim1,
            Range<T2> dim2,
            Range<T3> dim3)
        {
            return new SpacialRegion<T1, T2, T3>(this, dim1, dim2, dim3);
        }
        #endregion

        #region Internal Members

        internal DimensionDefinition<T1> Dimension1
        {
            get { return this.dimension1; }
        }

        internal DimensionDefinition<T2> Dimension2
        {
            get { return this.dimension2; }
        }

        internal DimensionDefinition<T3> Dimension3
        {
            get { return this.dimension3; }
        }
        #endregion
    }

    public class SpacialDefinition<T1, T2, T3, T4>
    {
        #region Fields

        private readonly DimensionDefinition<T1> dimension1;
        private readonly DimensionDefinition<T2> dimension2;
        private readonly DimensionDefinition<T3> dimension3;
        private readonly DimensionDefinition<T4> dimension4;
        #endregion

        #region Constructors

        public SpacialDefinition(
            DimensionDefinition<T1> dimension1,
            DimensionDefinition<T2> dimension2,
            DimensionDefinition<T3> dimension3,
            DimensionDefinition<T4> dimension4)
        {
            //Validation
            if (dimension1 == null)
                throw new ArgumentNullException("dimension1");

            if (dimension2 == null)
                throw new ArgumentNullException("dimension2");

            if (dimension3 == null)
                throw new ArgumentNullException("dimension3");

            if (dimension4 == null)
                throw new ArgumentNullException("dimension4");


            this.dimension1 = dimension1;
            this.dimension2 = dimension2;
            this.dimension3 = dimension3;
            this.dimension4 = dimension4;
        }
        #endregion
        
        #region Public Members

        public SpacialRegion<T1, T2, T3, T4> CreateRegion(
            Range<T1> dim1,
            Range<T2> dim2,
            Range<T3> dim3,
            Range<T4> dim4)
        {
            return new SpacialRegion<T1, T2, T3, T4>(this, dim1, dim2, dim3, dim4);
        }
        #endregion

        #region Internal Members

        internal DimensionDefinition<T1> Dimension1
        {
            get { return this.dimension1; }
        }

        internal DimensionDefinition<T2> Dimension2
        {
            get { return this.dimension2; }
        }

        internal DimensionDefinition<T3> Dimension3
        {
            get { return this.dimension3; }
        }

        internal DimensionDefinition<T4> Dimension4
        {
            get { return this.dimension4; }
        }
        #endregion
    }
}
