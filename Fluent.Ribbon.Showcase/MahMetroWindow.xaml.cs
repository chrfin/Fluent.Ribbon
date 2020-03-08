﻿namespace FluentTest
{
    using System;
    using System.Windows;
    using ControlzEx.Theming;
    using Fluent;
    using MahApps.Metro.Controls;

    public partial class MahMetroWindow : IRibbonWindow
    {
        public MahMetroWindow()
        {
            this.InitializeComponent();

            this.TestContent.Backstage.UseHighestAvailableAdornerLayer = false;

            this.Loaded += this.MahMetroWindow_Loaded;
            this.Closed += this.MahMetroWindow_Closed;
        }

        private void MahMetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.TitleBar = this.FindChild<RibbonTitleBar>("RibbonTitleBar");
            this.TitleBar.InvalidateArrange();
            this.TitleBar.UpdateLayout();

            // We need this inside this window because MahApps.Metro is not loaded globally inside the Fluent.Ribbon Showcase application.
            // This code is not required in an application that loads the MahApps.Metro styles globally.
            ThemeManager.ChangeTheme(this, ThemeManager.DetectTheme(Application.Current));
            ThemeManager.ThemeChanged += this.SyncThemes;
        }

        private void SyncThemes(object sender, ThemeChangedEventArgs e)
        {
            ThemeManager.ThemeChanged -= this.SyncThemes;

            ThemeManager.ChangeTheme(this, e.NewTheme);

            ThemeManager.ThemeChanged += this.SyncThemes;
        }

        private void MahMetroWindow_Closed(object sender, EventArgs e)
        {
            ThemeManager.ThemeChanged -= this.SyncThemes;
        }

        #region TitelBar

        /// <summary>
        /// Gets ribbon titlebar
        /// </summary>
        public RibbonTitleBar TitleBar
        {
            get { return (RibbonTitleBar)this.GetValue(TitleBarProperty); }
            private set { this.SetValue(TitleBarPropertyKey, value); }
        }

        // ReSharper disable once InconsistentNaming
        private static readonly DependencyPropertyKey TitleBarPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TitleBar), typeof(RibbonTitleBar), typeof(MahMetroWindow), new PropertyMetadata());

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="TitleBar"/>.
        /// </summary>
        public static readonly DependencyProperty TitleBarProperty = TitleBarPropertyKey.DependencyProperty;

        #endregion
    }
}