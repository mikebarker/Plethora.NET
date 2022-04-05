using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Mvvm.Model;

namespace Plethora.Test.Mvvm.Model
{
    [TestClass]
    public class DependentNotifyPropertyChanged_Tests
    {
        [TestMethod]
        public void PropertyChangedPropegation_PropertyChanged()
        {
            // setup
            DependentNotifyPropertyChangedImpl npc = new DependentNotifyPropertyChangedImpl();
            
            List<string> changingProperties = new();
            npc.PropertyChanging += (sender, e) => { changingProperties.Add(e.PropertyName); };

            List<string> changedProperties = new();
            npc.PropertyChanged += (sender, e) => { changedProperties.Add(e.PropertyName); };

            var inner = new Inner();
            inner.Value = "blah";

            // test
            npc.Inner = inner;

            // exec
            Assert.AreEqual(3, changingProperties.Count);
            Assert.IsTrue(changingProperties.Contains("Inner"));
            Assert.IsTrue(changingProperties.Contains("Name"));
            Assert.IsTrue(changingProperties.Contains("ReversedName"));

            Assert.AreEqual(3, changedProperties.Count);
            Assert.IsTrue(changedProperties.Contains("Inner"));
            Assert.IsTrue(changedProperties.Contains("Name"));
            Assert.IsTrue(changedProperties.Contains("ReversedName"));
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
            Assert.AreEqual(2, changingProperties.Count);
            Assert.IsTrue(changingProperties.Contains("Name"));
            Assert.IsTrue(changingProperties.Contains("ReversedName"));

            Assert.AreEqual(2, changedProperties.Count);
            Assert.IsTrue(changedProperties.Contains("Name"));
            Assert.IsTrue(changedProperties.Contains("ReversedName"));
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
        private Inner inner = new Inner();

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

        [DependsOn("Inner.Value")]
        public string Name
        {
            get { return this.Inner.Value; }
        }

        [DependsOn("Name")]
        public string ReversedName
        {
            get
            {
                char[] nameArray = this.Name.ToCharArray();
                Array.Reverse(nameArray);
                string reversedName = new string(nameArray);
                return reversedName;
            }
        }
    }

    #endregion
}
