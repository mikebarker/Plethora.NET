using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace Plethora.Synchronized.Sample
{
    public partial class Form1 : Form
    {
        private readonly SyncCollection<int, Person> serverCollection;
        private readonly DelayedChangeChannel<int, Person> delayedChangeChannel;
        private readonly ChangableCollection<int, Person> clientCollection;

        public Form1()
        {
            this.InitializeComponent();


            // Define the collection which would exist on the server
            this.serverCollection = new SyncCollection<int, Person>(p => p.Id);

            // Define a mock channel which transports changes between the server and client machines
            this.delayedChangeChannel = new DelayedChangeChannel<int, Person>(this.serverCollection);

            // Define a local client-side collection which is kept in-sync with the server-side collecction
            this.clientCollection = new ChangableCollection<int, Person>(this.delayedChangeChannel, this.delayedChangeChannel, p => p.Id, p => p.Clone());

            // To ensure that all Change events are raised on the main-window's thread, the ISynchronizeInvoke parameter can be specified.
            // This would remove the need for Invoking the call within the collection changed event below.
            //this.clientCollection = new ChangableCollection<int, Person>(delayedChangeChannel, delayedChangeChannel, p => p.Id, p => p.Clone(), System.Collections.Generic.Comparer<int>.Default, this);


            this.clientCollection.CollectionChanged += this.clientCollection_CollectionChanged;
            this.serverCollection.CollectionChanged += this.serverCollection_CollectionChanged;
        }

        void clientCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //One can also provide "this" as an ISynchronizeInvoke parameter to the collection, if desired. (as above)
            if (this.lstClientList.InvokeRequired)
            {
                Action<object, NotifyCollectionChangedEventArgs> action = this.clientCollection_CollectionChanged;
                this.lstClientList.Invoke(action, new[] { sender, e });
                return;
            }


            this.lstClientList.Items.Clear();
            foreach (Person person in this.clientCollection)
            {
                this.lstClientList.Items.Add(person);
            }
        }

        void serverCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.lstServerList.InvokeRequired)
            {
                Action<object, NotifyCollectionChangedEventArgs> action = this.serverCollection_CollectionChanged;
                this.lstServerList.Invoke(action, new[] {sender, e});
                return;
            }


            this.lstServerList.Items.Clear();
            using (this.serverCollection.EnterLock())
            {
                foreach (Person person in this.serverCollection)
                {
                    this.lstServerList.Items.Add(person);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Person person = new Person(this.txtFirstName.Text, this.txtSurname.Text, this.dtpDateOfBirth.Value, string.Empty);
            this.clientCollection.Add(person);
        }

        private void lstClientList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Person person = (Person)this.lstClientList.SelectedItem;
            if (person == null)
                return;

            this.txtId.Text = person.Id.ToString();
            this.txtFirstName.Text = person.FirstName;
            this.txtSurname.Text = person.Surname;
            this.dtpDateOfBirth.Value = person.DateOfBirth;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Person person = (Person)this.lstClientList.SelectedItem;
            if (person == null)
                return;

            this.clientCollection.Remove(person);
        }

        private void lstClientList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            Person person = (Person)this.lstClientList.Items[e.Index];
            if (person == null)
                return;

            bool isSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            Brush textBrush;
            if (this.clientCollection.IsLocalChange(person.Id))
            {
                textBrush = isSelected
                    ? Brushes.Gold
                    : Brushes.DarkOrange;
            }
            else
            {
                textBrush = isSelected
                    ? SystemBrushes.HighlightText
                    : SystemBrushes.ControlText;
            }


            e.DrawBackground();

            e.Graphics.DrawString(
                this.lstClientList.GetItemText(this.lstClientList.Items[e.Index]),
                e.Font,
                textBrush,
                e.Bounds,
                System.Drawing.StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }
    }
}
