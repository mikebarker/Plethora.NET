﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace Plethora.Context.Wpf
{
    internal class WpfContextSource : WpfContextSourceBase
    {
        #region Implementation of WpfContextSourceBase

        public override IEnumerable<ContextInfo> Contexts
        {
            get { return new[] { this.Context }; }
        }

        #endregion

        #region Properties

        #region ContextName DependencyProperty

        public string ContextName
        {
            get { return (string)GetValue(ContextNameProperty); }
            set { SetValue(ContextNameProperty, value); }
        }

        public static readonly DependencyProperty ContextNameProperty = DependencyProperty.Register(
            "ContextName",
            typeof(string),
            typeof(WpfContextSource),
            new PropertyMetadata(default(string), PropertyChangedCallback));

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
            typeof(WpfContextSource),
            new PropertyMetadata(default(int), PropertyChangedCallback));

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
            typeof(WpfContextSource),
            new PropertyMetadata(default(object), PropertyChangedCallback));

        #endregion

        public ContextInfo Context
        {
            get { return new ContextInfo(this.ContextName, this.Rank, this.Data); }
        }

        #endregion

        #region Private Methods

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var contextSource = (WpfContextSource)dependencyObject;
            contextSource.OnContextChanged(EventArgs.Empty);
        }

        #endregion
    }
}
