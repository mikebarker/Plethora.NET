using System;
using System.Collections.Generic;

namespace Plethora.Workflow;

public sealed class StateDataPair
{
    private readonly string state;
    private readonly Dictionary<string, object> data;

    public StateDataPair(
        string state,
        IDictionary<string, object> data)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(data);


        this.state = state;
        this.data = new Dictionary<string, object>(data);
    }

    public string State
    {
        get { return this.state; }
    }

    public IDictionary<string, object> Data
    {
        get { return this.data; }
    }
}
