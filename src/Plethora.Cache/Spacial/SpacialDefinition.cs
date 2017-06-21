using System;

using JetBrains.Annotations;

using Plethora.Collections.Sets;

namespace Plethora.Cache.Spacial
{
    public struct SpaceRegion<T1>
    {
        private readonly ISetCore<T1> dimension1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceRegion{T1}"/> class.
        /// </summary>
        public SpaceRegion([NotNull] ISetCore<T1> dimension1)
        {
            if (dimension1 == null)
                throw new ArgumentNullException(nameof(dimension1));

            this.dimension1 = dimension1;
        }

        [NotNull]
        public ISetCore<T1> Dimension1
        {
            get { return this.dimension1; }
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
        public SpaceRegion([NotNull] ISetCore<T1> dimension1, [NotNull] ISetCore<T2> dimension2)
        {
            if (dimension1 == null)
                throw new ArgumentNullException(nameof(dimension1));

            if (dimension2 == null)
                throw new ArgumentNullException(nameof(dimension2));

            this.dimension1 = dimension1;
            this.dimension2 = dimension2;
        }

        [NotNull]
        public ISetCore<T1> Dimension1
        {
            get { return this.dimension1; }
        }

        [NotNull]
        public ISetCore<T2> Dimension2
        {
            get { return this.dimension2; }
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
        public SpaceRegion([NotNull] ISetCore<T1> dimension1, [NotNull] ISetCore<T2> dimension2, [NotNull] ISetCore<T3> dimension3)
        {
            if (dimension1 == null)
                throw new ArgumentNullException(nameof(dimension1));

            if (dimension2 == null)
                throw new ArgumentNullException(nameof(dimension2));

            if (dimension3 == null)
                throw new ArgumentNullException(nameof(dimension3));

            this.dimension1 = dimension1;
            this.dimension2 = dimension2;
            this.dimension3 = dimension3;
        }

        [NotNull]
        public ISetCore<T1> Dimension1
        {
            get { return this.dimension1; }
        }

        [NotNull]
        public ISetCore<T2> Dimension2
        {
            get { return this.dimension2; }
        }

        [NotNull]
        public ISetCore<T3> Dimension3
        {
            get { return this.dimension3; }
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
        public SpaceRegion([NotNull] ISetCore<T1> dimension1, [NotNull] ISetCore<T2> dimension2, [NotNull] ISetCore<T3> dimension3, [NotNull] ISetCore<T4> dimension4)
        {
            if (dimension1 == null)
                throw new ArgumentNullException(nameof(dimension1));

            if (dimension2 == null)
                throw new ArgumentNullException(nameof(dimension2));

            if (dimension3 == null)
                throw new ArgumentNullException(nameof(dimension3));

            if (dimension4 == null)
                throw new ArgumentNullException(nameof(dimension4));

            this.dimension1 = dimension1;
            this.dimension2 = dimension2;
            this.dimension3 = dimension3;
            this.dimension4 = dimension4;
        }

        [NotNull]
        public ISetCore<T1> Dimension1
        {
            get { return this.dimension1; }
        }

        [NotNull]
        public ISetCore<T2> Dimension2
        {
            get { return this.dimension2; }
        }

        [NotNull]
        public ISetCore<T3> Dimension3
        {
            get { return this.dimension3; }
        }

        [NotNull]
        public ISetCore<T4> Dimension4
        {
            get { return this.dimension4; }
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
