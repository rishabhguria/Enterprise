using Infragistics.Windows.DataPresenter;
using Prana.LogManager;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Prana.Rebalancer.RebalancerNew.Themes
{
    public partial class RebalancerTheme
    {
        public Border PreviousBorder { get; set; }
        private void GroupBySummariesPresenter_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Grid gd = null, gd2 = null;
                Border bd = null;
                StackPanel st = null;
                GroupBySummariesPresenter groupBySummaries = null;
                var bc = new BrushConverter();
                if (PreviousBorder != null)
                {
                    PreviousBorder.Background = (Brush)bc.ConvertFrom("#E7E8EA");
                }
                groupBySummaries = (sender as GroupBySummariesPresenter);
                if (groupBySummaries != null) st = (VisualTreeHelper.GetParent(groupBySummaries) as StackPanel);
                if (st != null) gd = (VisualTreeHelper.GetParent(st) as Grid);
                if (gd != null) gd2 = (VisualTreeHelper.GetParent(gd) as Grid);
                if (gd2 != null) bd = gd2.FindName("highlight") as Border;
                if (bd != null)
                {
                    PreviousBorder = bd;
                    PreviousBorder.Background = (Brush)bc.ConvertFrom("#44102975");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ListBox lb = (((sender as TextBox).Parent as StackPanel).Children[1] as ListBox);
                if (lb != null)
                {
                    foreach (var item in lb.ItemsSource as ReadOnlyObservableCollection<FieldChooserEntry>)
                    {
                        if ((sender as TextBox).Text.ToString() != "")
                        {
                            if (item.Field.Label.ToString().ToLower().Contains((sender as TextBox).Text.ToLower()))
                            {
                                (lb.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem).Visibility = Visibility.Visible;
                            }
                            else
                                (lb.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem).Visibility = Visibility.Collapsed;
                        }
                        else
                            (lb.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem).Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
