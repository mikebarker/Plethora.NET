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
        public void PropertyChangedPropegation()
        {
            // setup
            DependentNotifyPropertyChangedImpl npc = new DependentNotifyPropertyChangedImpl();
            
            List<PropertyChangingEventArgs> changingProperties = new List<PropertyChangingEventArgs>();
            npc.PropertyChanging += (sender, e) => { changingProperties.Add(e); };

            List<PropertyChangedEventArgs> changedProperties = new List<PropertyChangedEventArgs>();
            npc.PropertyChanged += (sender, e) => { changedProperties.Add(e); };

            // test
            npc.Name = "blah";

            // exec
            Assert.AreEqual(2, changingProperties.Count);
            Assert.AreEqual("Name", changingProperties[0].PropertyName);
            Assert.AreEqual("ReversedName", changingProperties[1].PropertyName);
            Assert.AreEqual(true, changingProperties[1] is DependentPropertyChangingEventArgs);
            Assert.AreEqual("Name", ((DependentPropertyChangingEventArgs)changingProperties[1]).OriginPropertyName);

            Assert.AreEqual(2, changedProperties.Count);
            Assert.AreEqual("Name", changedProperties[0].PropertyName);
            Assert.AreEqual("ReversedName", changedProperties[1].PropertyName);
            Assert.AreEqual(true, changedProperties[1] is DependentPropertyChangedEventArgs);
            Assert.AreEqual("Name", ((DependentPropertyChangedEventArgs)changedProperties[1]).OriginPropertyName);
        }


    }

    #region

    public class DependentNotifyPropertyChangedImpl : DependentNotifyPropertyChanged, INotifyPropertyChanging
    {
        private string name;

        public event PropertyChangingEventHandler PropertyChanging
        {
            add { base.InternalPropertyChanging += value; }
            remove { base.InternalPropertyChanging -= value; }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.OnPropertyChanging(nameof(this.Name));
                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        [DependsOn("Name")]
        public string ReversedName
        {
            get
            {
                char[] nameArray = this.name.ToCharArray();
                Array.Reverse(nameArray);
                string reversedName = new string(nameArray);
                return reversedName;
            }
        }
    }

    #endregion
}
