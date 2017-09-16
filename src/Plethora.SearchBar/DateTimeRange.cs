using System;

namespace Plethora.SearchBar
{
    public struct DateTimeRange
    {
        private readonly DateTime min;
        private readonly DateTime max;

        public DateTimeRange(DateTime min, DateTime max)
        {
            this.min = min;
            this.max = max;
        }

        public DateTime Min
        {
            get { return this.min; }
        }

        public DateTime Max
        {
            get { return this.max; }
        }

        public override string ToString()
        {
            return string.Format("[{0} : {1}]",
                this.min.ToShortDateString(),
                this.max.ToShortDateString());
        }
    }
}
