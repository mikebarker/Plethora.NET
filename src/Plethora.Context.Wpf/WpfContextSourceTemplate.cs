using System.Windows;
using System.Windows.Data;

namespace Plethora.Context.Wpf
{
    /// <summary>
    /// A template class which may be defined as a resource and used in several instance throughout XAML mark-up.
    /// </summary>
    /// <remarks>
    /// This class inherits from <see cref="FreezableCollection{T}"/> to allow the WPF DataContext
    /// to flow through the context source tree.
    /// </remarks>
    /// <example>
    /// This is used within XAML as follows:
    ///  <code>
    ///   <![CDATA[
    /// 
    ///     <UserControl.Resources>
    /// 
    ///         <context:WpfContextSourceTemplate x:Key="contextGrid"
    ///             Name="XamDataGrid" Data="{Binding RelativeSource={RelativeSource TemplatedParent}}" />
    /// 
    ///         <context:WpfContextSourceTemplate x:Key="contextStock"
    ///             Name="Stock" Data="{Binding Path=ActiveDataItem.StockMnemonic, RelativeSource={RelativeSource TemplatedParent}}"/>
    /// 
    ///         <context:WpfContextSourceTemplate x:Key="contextContract"
    ///             Name="Contract" Data="{Binding Path=ActiveDataItem.ContractId, RelativeSource={RelativeSource TemplatedParent}}" />
    /// 
    ///         <context:WpfContextSourceTemplate x:Key="contextClient"
    ///             Name="Client" Data="{Binding Path=ActiveDataItem.ClientCode, RelativeSource={RelativeSource TemplatedParent}}" />
    /// 
    ///         <context:WpfContextSourceTemplateCollection x:Key="contextCollectionTrade">
    ///             <StaticResource ResourceKey="contextStock" />
    ///             <StaticResource ResourceKey="contextContract" />
    ///             <StaticResource ResourceKey="contextClient" />
    ///         </context:WpfContextSourceTemplateCollection>
    /// 
    ///         <context:WpfContextSourceTemplateCollection x:Key="contextCollectionTradeGridRow">
    ///             <StaticResource ResourceKey="contextCollectionTrade" />
    ///             <StaticResource ResourceKey="contextGrid" />
    ///         </context:WpfContextSourceTemplateCollection>
    /// 
    ///     </UserControl.Resources>
    /// 
    ///   ]]>
    ///  </code>
    /// 
    /// These resources may then be referenced in the XAML mark-up of the <see cref="UIElement"/> as:
    ///  <code>
    ///   <![CDATA[
    ///            <DataPresenter:XamDataGrid Name="TradeGrid"
    ///                                       DataSource="{Binding ItemsCollectionView}"
    ///                                       ActiveDataItem="{Binding Path=SelectedItem, Mode=OneWayToSource}">
    ///
    ///                <!-- Context source using a multiple templates -->
    ///                <context:WpfContext.ContextSourceTemplate>
    ///                    <context:WpfContextSourceTemplateCollection>
    ///                        <StaticResource ResourceKey="contextGrid" />
    ///
    ///                        <StaticResource ResourceKey="contextStock" />
    ///                        <StaticResource ResourceKey="contextContract" />
    ///                        <StaticResource ResourceKey="contextClient" />
    ///                    </context:WpfContextSourceTemplateCollection>
    ///                </context:WpfContext.ContextSourceTemplate>
    ///   ...
    ///   ...
    ///            </DataPresenter>
    ///   
    ///            <DataPresenter:XamDataGrid Name="TradeGrid2"
    ///                                       DataSource="{Binding ItemsCollectionView}"
    ///                                       ActiveDataItem="{Binding Path=SelectedItem, Mode=OneWayToSource}">
    ///
    ///                <!-- Simple context source using a single template -->
    ///                <context:WpfContext.ContextSourceTemplate>
    ///                    <StaticResource ResourceKey="contextCollectionTradeGridRow" />
    ///                </context:WpfContext.ContextSourceTemplate>
    ///   ...
    ///   ...
    ///            </DataPresenter>
    ///   ]]>
    ///  </code>
    /// </example>
    /// <seealso cref="WpfContextSourceTemplateCollection"/>
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

        WpfContextSourceBase IWpfContextSourceTemplate.CreateContent()
        {
            WpfContextSource instance = new WpfContextSource();

            DepencyObjectHelper.CopyPropertyWithBinding(this, NameProperty, instance, WpfContextSource.ContextNameProperty, ModifyTemplateParent);
            DepencyObjectHelper.CopyPropertyWithBinding(this, RankProperty, instance, WpfContextSource.RankProperty, ModifyTemplateParent);
            DepencyObjectHelper.CopyPropertyWithBinding(this, DataProperty, instance, WpfContextSource.DataProperty, ModifyTemplateParent);

            return instance;
        }

        /// <summary>
        /// This method modifies bindings to TemplatedParent, and allows the context source to be 
        /// defined as a resource as a template, and used in several instance within the XAML mark-up
        /// to provide <see cref="WpfContextSource"/> items.
        /// </summary>
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
