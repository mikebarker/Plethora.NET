using System.Windows;
using System.Windows.Data;

namespace Plethora.Context.Wpf
{
    public class WpfContextSourceTemplate : Freezable, IWpfContextSourceTemplate
    {
        #region Name DependencyProperty

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name",
            typeof(string),
            typeof(WpfContextSourceTemplate),
            new PropertyMetadata(default(string)));

        #endregion

        #region Rank DependencyProperty

        public int Rank
        {
            get { return (int)GetValue(RankProperty); }
            set { SetValue(RankProperty, value); }
        }

        public static readonly DependencyProperty RankProperty = DependencyProperty.Register(
            "Rank",
            typeof(int),
            typeof(WpfContextSourceTemplate),
            new PropertyMetadata(default(int)));

        #endregion

        #region Data DependencyProperty

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data",
            typeof(object),
            typeof(WpfContextSourceTemplate),
            new PropertyMetadata(default(object)));

        #endregion

        public WpfContextSourceBase CreateContent()
        {
            WpfContextSource instance = new WpfContextSource();

            DepencyObjectHelper.CopyPropertyWithBinding(this, NameProperty, instance, WpfContextSource.ContextNameProperty, ModifyTemplateParent);
            DepencyObjectHelper.CopyPropertyWithBinding(this, RankProperty, instance, WpfContextSource.RankProperty, ModifyTemplateParent);
            DepencyObjectHelper.CopyPropertyWithBinding(this, DataProperty, instance, WpfContextSource.DataProperty, ModifyTemplateParent);

            return instance;
        }

        private static void ModifyTemplateParent(Binding binding)
        {
            //Redirect TemplateParent binding to use the UIElement property of the source
            if ((binding.RelativeSource != null) && (binding.RelativeSource.Mode == RelativeSourceMode.TemplatedParent))
            {
                binding.RelativeSource = RelativeSource.Self;

                PropertyPath path;
                var bindingPath = binding.Path;
                if ((bindingPath == null) || (bindingPath.Path == null))
                {
                    path = new PropertyPath("UIElement");
                }
                else
                {
                    path = new PropertyPath("UIElement." + bindingPath.Path, bindingPath.PathParameters);
                }

                binding.Path = path;
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new WpfContextSourceTemplate();
        }
    }
}
