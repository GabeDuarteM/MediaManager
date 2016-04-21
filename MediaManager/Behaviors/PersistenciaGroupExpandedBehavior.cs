// Developed by: Gabriel Duarte
// 
// Created at: 12/11/2015 20:11

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace MediaManager.Behaviors
{
    internal class PersistenciaGroupExpandedBehavior : Behavior<Expander>
    {
        #region Public Properties

        public object GroupName
        {
            get { return (object) GetValue(GroupNameProperty); }

            set { SetValue(GroupNameProperty, value); }
        }

        #endregion Public Properties

        #region Static Fields

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
                                                                                                  "GroupName",
                                                                                                  typeof(object),
                                                                                                  typeof(
                                                                                                      PersistenciaGroupExpandedBehavior
                                                                                                      ),
                                                                                                  new PropertyMetadata(
                                                                                                      default(object)));

        private static readonly DependencyProperty ExpandedStateStoreProperty =
            DependencyProperty.RegisterAttached(
                                                "ExpandedStateStore",
                                                typeof(IDictionary<object, bool>),
                                                typeof(PersistenciaGroupExpandedBehavior),
                                                new PropertyMetadata(default(IDictionary<object, bool>)));

        #endregion Static Fields

        #region Methods

        protected override void OnAttached()
        {
            base.OnAttached();

            bool? expanded = GetExpandedState();

            if (expanded != null)
            {
                AssociatedObject.IsExpanded = expanded.Value;
            }

            AssociatedObject.Expanded += OnExpanded;
            AssociatedObject.Collapsed += OnCollapsed;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Expanded -= OnExpanded;
            AssociatedObject.Collapsed -= OnCollapsed;

            base.OnDetaching();
        }

        private ItemsControl FindItemsControl()
        {
            DependencyObject current = AssociatedObject;

            while (current != null && !(current is ItemsControl))
            {
                current = VisualTreeHelper.GetParent(current);
            }

            if (current == null)
            {
                return null;
            }

            return current as ItemsControl;
        }

        private bool? GetExpandedState()
        {
            IDictionary<object, bool> dict = GetExpandedStateStore();

            if (!dict.ContainsKey(GroupName))
            {
                return null;
            }

            return dict[GroupName];
        }

        private IDictionary<object, bool> GetExpandedStateStore()
        {
            ItemsControl itemsControl = FindItemsControl();

            if (itemsControl == null)
            {
                throw new Exception(
                    "Behavior needs to be attached to an Expander that is contained inside an ItemsControl");
            }

            var dict = (IDictionary<object, bool>) itemsControl.GetValue(ExpandedStateStoreProperty);

            if (dict == null)
            {
                dict = new Dictionary<object, bool>();
                itemsControl.SetValue(ExpandedStateStoreProperty, dict);
            }

            return dict;
        }

        private void OnCollapsed(object sender, RoutedEventArgs e)
        {
            SetExpanded(false);
        }

        private void OnExpanded(object sender, RoutedEventArgs e)
        {
            SetExpanded(true);
        }

        private void SetExpanded(bool expanded)
        {
            IDictionary<object, bool> dict = GetExpandedStateStore();

            dict[GroupName] = expanded;
        }

        #endregion Methods
    }
}
