using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Plethora.Workflow
{
    public sealed class StateDataPair
    {
        private readonly string state;
        private readonly Dictionary<string, object> data;

        public StateDataPair(
            [NotNull] string state,
            [NotNull] IDictionary<string, object> data)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (data == null)
                throw new ArgumentNullException(nameof(data));


            this.state = state;
            this.data = new Dictionary<string, object>(data);
        }

        [NotNull]
        public string State
        {
            get { return this.state; }
        }

        [NotNull]
        public IDictionary<string, object> Data
        {
            get { return this.data; }
        }
    }
}
