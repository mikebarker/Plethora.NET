using Plethora.Collections.Sets;

namespace Plethora.Cache.Spacial
{
    public struct SpaceRegion<T1>
    {
        private readonly ISetCore<T1> dimension1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceRegion{T1}"/> class.
        /// </summary>
        public SpaceRegion(ISetCore<T1> dimension1)
        {
            this.dimension1 = dimension1;
        }

        public ISetCore<T1> Dimension1
        {
            get { return dimension1; }
        }

        public bool IsEmpty
        {
            get
            {
                return
                    (this.dimension1.IsEmpty == true);
            }
        }
    }

    public struct SpaceRegion<T1, T2>
    {
        private readonly ISetCore<T1> dimension1;
        private readonly ISetCore<T2> dimension2;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceRegion{T1, T2}"/> class.
        /// </summary>
        public SpaceRegion(ISetCore<T1> dimension1, ISetCore<T2> dimension2)
        {
            this.dimension1 = dimension1;
            this.dimension2 = dimension2;
        }

        public ISetCore<T1> Dimension1
        {
            get { return dimension1; }
        }

        public ISetCore<T2> Dimension2
        {
            get { return dimension2; }
        }

        public bool IsEmpty
        {
            get
            {
                return
                    (this.dimension1.IsEmpty == true) ||
                    (this.dimension2.IsEmpty == true);
            }
        }
    }

    public struct SpaceRegion<T1, T2, T3>
    {
        private readonly ISetCore<T1> dimension1;
        private readonly ISetCore<T2> dimension2;
        private readonly ISetCore<T3> dimension3;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceRegion{T1,T2,T3}"/> class.
        /// </summary>
        public SpaceRegion(ISetCore<T1> dimension1, ISetCore<T2> dimension2, ISetCore<T3> dimension3)
        {
            this.dimension1 = dimension1;
            this.dimension2 = dimension2;
            this.dimension3 = dimension3;
        }

        public ISetCore<T1> Dimension1
        {
            get { return dimension1; }
        }

        public ISetCore<T2> Dimension2
        {
            get { return dimension2; }
        }

        public ISetCore<T3> Dimension3
        {
            get { return dimension3; }
        }

        public bool IsEmpty
        {
            get
            {
                return
                    (this.dimension1.IsEmpty == true) ||
                    (this.dimension2.IsEmpty == true) ||
                    (this.dimension3.IsEmpty == true);
            }
        }
    }

    public struct SpaceRegion<T1, T2, T3, T4>
    {
        private readonly ISetCore<T1> dimension1;
        private readonly ISetCore<T2> dimension2;
        private readonly ISetCore<T3> dimension3;
        private readonly ISetCore<T4> dimension4;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceRegion{T1,T2,T3,T4}"/> class.
        /// </summary>
        public SpaceRegion(ISetCore<T1> dimension1, ISetCore<T2> dimension2, ISetCore<T3> dimension3, ISetCore<T4> dimension4)
        {
            this.dimension1 = dimension1;
            this.dimension2 = dimension2;
            this.dimension3 = dimension3;
            this.dimension4 = dimension4;
        }

        public ISetCore<T1> Dimension1
        {
            get { return dimension1; }
        }

        public ISetCore<T2> Dimension2
        {
            get { return dimension2; }
        }

        public ISetCore<T3> Dimension3
        {
            get { return dimension3; }
        }

        public ISetCore<T4> Dimension4
        {
            get { return dimension4; }
        }

        public bool IsEmpty
        {
            get
            {
                return
                    (this.dimension1.IsEmpty == true) ||
                    (this.dimension2.IsEmpty == true) ||
                    (this.dimension3.IsEmpty == true) ||
                    (this.dimension4.IsEmpty == true);
            }
        }
    }
}
