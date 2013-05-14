using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace Plethora.Synchronized.Example
{
    public partial class Form1 : Form
    {
        private readonly SyncCollection<int, Person> serverCollection;
        private readonly DelayedChangeChannel<int, Person> delayedChangeChannel;
        private readonly ChangableCollection<int, Person> clientCollection;

        public Form1()
        {
            InitializeComponent();


            // Define the collection which would exist on the server
            this.serverCollection = new SyncCollection<int, Person>(p => p.Id);

            // Define a mock channel which transports changes between the server and client machines
            this.delayedChangeChannel = new DelayedChangeChannel<int, Person>(serverCollection);

            // Define a local client-side collection which is kept in-sync with the server-side collecction
            this.clientCollection = new ChangableCollection<int, Person>(delayedChangeChannel, delayedChangeChannel, p => p.Id, p => p.Clone());

            // To ensure that all Change events are raised on the main-window's thread, the ISynchronizeInvoke parameter can be specified.
            // This would remove the need for Invoking the call within the collection changed event below.
            //this.clientCollection = new ChangableCollection<int, Person>(delayedChangeChannel, delayedChangeChannel, p => p.Id, p => p.Clone(), System.Collections.Generic.Comparer<int>.Default, this);


            this.clientCollection.CollectionChanged += clientCollection_CollectionChanged;
            this.serverCollection.CollectionChanged += serverCollection_CollectionChanged;
        }

        void clientCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //One can also provide "this" as an ISynchronizeInvoke parameter to the collection, if desired. (as above)
            if (lstClientList.InvokeRequired)
            {
                Action<object, NotifyCollectionChangedEventArgs> action = this.clientCollection_CollectionChanged;
                lstClientList.Invoke(action, new[] { sender, e });
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
            if (lstServerList.InvokeRequired)
            {
                Action<object, NotifyCollectionChangedEventArgs> action = this.serverCollection_CollectionChanged;
                lstServerList.Invoke(action, new[] {sender, e});
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
            Person person = new Person(txtFirstName.Text, txtSurname.Text, dtpDateOfBirth.Value, string.Empty);
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

            Color textColor = clientCollection.IsLocalChange(person.Id)
                                    ? Color.Red
                                    : Color.Black;

            using(Brush textBrush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString( // Draw the appropriate text in the ListBox
                    person.ToString(),
                    lstClientList.Font,
                    textBrush,
                    0,
                    e.Index*lstClientList.ItemHeight
                    );
            }
        }
    }
}
