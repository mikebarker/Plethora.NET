using Plethora.Collections.Sets;
using System;

namespace Plethora.Spacial
{
    public class SpaceRegion
    {
        private readonly ISetCore[] dimensions;

        public SpaceRegion(params ISetCore[] dimensions)
        {
            ArgumentNullException.ThrowIfNull(dimensions);


            this.dimensions = (ISetCore[])dimensions.Clone();
        }

        public ISetCore[] Dimensions => this.dimensions;

        public bool IsEmpty
        {
            get
            {
                foreach (var dimension in this.dimensions)
                {
                    if (dimension.IsEmpty == true)
                        return true;
                }

                return false;
            }
        }
    }

    public class SpaceRegion<T1> : SpaceRegion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceRegion{T1}"/> class.
        /// </summary>
        public SpaceRegion(ISetCore<T1> dimension1)
            : base (dimension1)
        {
        }

        public ISetCore<T1> Dimension1 => (ISetCore<T1>)this.Dimensions[0];
    }

    public class SpaceRegion<T1, T2> : SpaceRegion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceRegion{T1,T2}"/> class.
        /// </summary>
        public SpaceRegion(
            ISetCore<T1> dimension1,
            ISetCore<T2> dimension2)
            : base(dimension1, dimension2)
        {
        }

        public ISetCore<T1> Dimension1 => (ISetCore<T1>)this.Dimensions[0];

        public ISetCore<T2> Dimension2 => (ISetCore<T2>)this.Dimensions[1];
    }

    public class SpaceRegion<T1, T2, T3> : SpaceRegion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceRegion{T1,T2,T3}"/> class.
        /// </summary>
        public SpaceRegion(
            ISetCore<T1> dimension1,
            ISetCore<T2> dimension2,
            ISetCore<T3> dimension3)
            : base(dimension1, dimension2, dimension3)
        {
        }

        public ISetCore<T1> Dimension1 => (ISetCore<T1>)this.Dimensions[0];

        public ISetCore<T2> Dimension2 => (ISetCore<T2>)this.Dimensions[1];

        public ISetCore<T3> Dimension3 => (ISetCore<T3>)this.Dimensions[2];
    }

    public class SpaceRegion<T1, T2, T3, T4> : SpaceRegion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceRegion{T1,T2,T3,T4}"/> class.
        /// </summary>
        public SpaceRegion(
            ISetCore<T1> dimension1,
            ISetCore<T2> dimension2,
            ISetCore<T3> dimension3,
            ISetCore<T4> dimension4)
            : base(dimension1, dimension2, dimension3, dimension4)
        {
        }

        public ISetCore<T1> Dimension1 => (ISetCore<T1>)this.Dimensions[0];

        public ISetCore<T2> Dimension2 => (ISetCore<T2>)this.Dimensions[1];

        public ISetCore<T3> Dimension3 => (ISetCore<T3>)this.Dimensions[2];

        public ISetCore<T4> Dimension4 => (ISetCore<T4>)this.Dimensions[3];
    }
}
