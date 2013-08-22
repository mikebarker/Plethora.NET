using System;
using System.Linq;
using System.Windows;

namespace Plethora.Context.Wpf.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ContextManager.DefaultInstance.ContextChanged += DefaultInstance_ContextChanged;
        }

        void DefaultInstance_ContextChanged(object sender, EventArgs e)
        {
            var contextText = ContextManager.DefaultInstance
                .GetContexts()
                .Select(context => string.Format("{0} [{1}]", context.Name, context.Data));

            this.ContextTextBox.Text = string.Join("\r\n", contextText);
        }
    }
}
