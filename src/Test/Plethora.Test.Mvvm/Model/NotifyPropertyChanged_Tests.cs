using System.Collections.Generic;
using System.ComponentModel;

using NUnit.Framework;

using Plethora.Mvvm.Model;

namespace Plethora.Test.Mvvm.Model
{
    [TestFixture]
    public class NotifyPropertyChanged_Tests
    {
        [Test]
        public void IsNotINotifyPropertyChanging()
        {
            // setup
            NotifyPropertyChangedImpl npc = new NotifyPropertyChangedImpl();

            // test
            // ReSharper disable once SuspiciousTypeConversion.Global
            bool isINotifyPropertyChanging = npc is INotifyPropertyChanging;

            // exec
            Assert.IsFalse(isINotifyPropertyChanging);
        }

        [Test]
        public void PropertyNotifyByExpression()
        {
            // setup
            NotifyPropertyChangedImpl npc = new NotifyPropertyChangedImpl();
            List<string> changedPropertyNames = new List<string>();
            npc.PropertyChanged += (sender, e) => { changedPropertyNames.Add(e.PropertyName); };

            // test
            npc.PropertyNotifyByExpression = "blah";

            // exec
            Assert.AreEqual(1, changedPropertyNames.Count);
            Assert.AreEqual("PropertyNotifyByExpression", changedPropertyNames[0]);
        }

        [Test]
        public void PropertyNotifyByName()
        {
            // setup
            NotifyPropertyChangedImpl npc = new NotifyPropertyChangedImpl();
            List<string> changedPropertyNames = new List<string>();
            npc.PropertyChanged += (sender, e) => { changedPropertyNames.Add(e.PropertyName); };

            // test
            npc.PropertyNotifyByName = "blah";

            // exec
            Assert.AreEqual(1, changedPropertyNames.Count);
            Assert.AreEqual("PropertyNotifyByName", changedPropertyNames[0]);
        }

        [Test]
        public void NotifyingDisabled()
        {
            // setup
            NotifyPropertyChangedImpl npc = new NotifyPropertyChangedImpl();
            List<string> changedPropertyNames = new List<string>();
            npc.PropertyChanged += (sender, e) => { changedPropertyNames.Add(e.PropertyName); };

            // test
            npc.IsNotifying = false;
            npc.PropertyNotifyByExpression = "blah";

            // exec
            Assert.AreEqual(0, changedPropertyNames.Count);
        }

        [Test]
        public void PropertyChanging()
        {
            // setup
            NotifyPropertyChangedImplWithChanging npc = new NotifyPropertyChangedImplWithChanging();
            List<string> changingPropertyNames = new List<string>();
            npc.PropertyChanging += (sender, e) => { changingPropertyNames.Add(e.PropertyName); };

            List<string> changedPropertyNames = new List<string>();
            npc.PropertyChanged += (sender, e) => { changedPropertyNames.Add(e.PropertyName); };

            // test
            npc.PropertyNotifyByExpression = "blah";

            // exec
            Assert.AreEqual(1, changingPropertyNames.Count);
            Assert.AreEqual("PropertyNotifyByExpression", changingPropertyNames[0]);
            Assert.AreEqual(1, changedPropertyNames.Count);
            Assert.AreEqual("PropertyNotifyByExpression", changedPropertyNames[0]);
        }


        #region

        private class NotifyPropertyChangedImpl : NotifyPropertyChanged
        {
            private string propertyNotifyByExpression;
            private string propertyNotifyByName;

            public string PropertyNotifyByExpression
            {
                get { return this.propertyNotifyByExpression; }
                set
                {
                    this.OnPropertyChanging(() => this.PropertyNotifyByExpression);
                    this.propertyNotifyByExpression = value;
                    this.OnPropertyChanged(() => this.PropertyNotifyByExpression);
                }
            }

            public string PropertyNotifyByName
            {
                get { return this.propertyNotifyByName; }
                set
                {
                    this.OnPropertyChanging("PropertyNotifyByName");
                    this.propertyNotifyByName = value;
                    this.OnPropertyChanged("PropertyNotifyByName");
                }
            }
        }

        private class NotifyPropertyChangedImplWithChanging : NotifyPropertyChangedImpl, INotifyPropertyChanging
        {
            public event PropertyChangingEventHandler PropertyChanging
            {
                add { base.InternalPropertyChanging += value; }
                remove { base.InternalPropertyChanging -= value; }
            }
        }
        #endregion
    }
}
