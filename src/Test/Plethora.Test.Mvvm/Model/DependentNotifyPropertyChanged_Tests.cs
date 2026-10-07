using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Mvvm;
using Plethora.Mvvm.Model;

namespace Plethora.Test.Mvvm.Model;

[TestClass]
public class DependentNotifyPropertyChanged_Tests
{
    [TestMethod]
    public void PropertyChangedPropegation_PropertyChanged()
    {
        // setup
        DependentNotifyPropertyChangedImpl npc = new();
        
        List<string> changingProperties = new();
        npc.PropertyChanging += (sender, e) => { changingProperties.Add(e.PropertyName); };

        List<string> changedProperties = new();
        npc.PropertyChanged += (sender, e) => { changedProperties.Add(e.PropertyName); };

        Inner inner = new()
        {
            Value = "blah"
        };

        // test
        npc.Inner = inner;

        // exec
        Assert.HasCount(3, changingProperties);
        Assert.Contains("Inner", changingProperties);
        Assert.Contains("Name", changingProperties);
        Assert.Contains("ReversedName", changingProperties);

        Assert.HasCount(3, changedProperties);
        Assert.Contains("Inner", changedProperties);
        Assert.Contains("Name", changedProperties);
        Assert.Contains("ReversedName", changedProperties);
    }

    [TestMethod]
    public void PropertyChangedPropegation_InnerPropertyChanged()
    {
        // setup
        DependentNotifyPropertyChangedImpl npc = new DependentNotifyPropertyChangedImpl();
        
        List<string> changingProperties = new();
        npc.PropertyChanging += (sender, e) => { changingProperties.Add(e.PropertyName); };

        List<string> changedProperties = new();
        npc.PropertyChanged += (sender, e) => { changedProperties.Add(e.PropertyName); };

        // test
        npc.Inner.Value = "blah";

        // exec
        Assert.HasCount(2, changingProperties);
        Assert.Contains("Name", changingProperties);
        Assert.Contains("ReversedName", changingProperties);

        Assert.HasCount(2, changedProperties);
        Assert.Contains("Name", changedProperties);
        Assert.Contains("ReversedName", changedProperties);
    }
}

#region Test classes

public class Inner : NotifyPropertyChanged, INotifyPropertyChanging
{
    private string value;

    public event PropertyChangingEventHandler PropertyChanging
    {
        add { base.InternalPropertyChanging += value; }
        remove { base.InternalPropertyChanging -= value; }
    }

    public string Value
    {
        get { return this.value; }
        set
        {
            this.OnPropertyChanging();
            this.value = value;
            this.OnPropertyChanged();
        }
    }
}

public class DependentNotifyPropertyChangedImpl : DependentNotifyPropertyChanged, INotifyPropertyChanging
{
    private Inner inner = new();

    public event PropertyChangingEventHandler PropertyChanging
    {
        add { base.InternalPropertyChanging += value; }
        remove { base.InternalPropertyChanging -= value; }
    }

    public Inner Inner
    {
        get { return this.inner; }
        set
        {
            this.OnPropertyChanging();
            this.inner = value;
            this.OnPropertyChanged();
        }
    }

    [Plethora.Mvvm.Model.DependsOn("Inner.Value")]
    public string Name
    {
        get { return this.Inner.Value; }
    }

    [Plethora.Mvvm.Model.DependsOn("Name")]
    public string ReversedName
    {
        get
        {
            char[] nameArray = this.Name.ToCharArray();
            Array.Reverse(nameArray);
            string reversedName = new(nameArray);
            return reversedName;
        }
    }
}

#endregion
