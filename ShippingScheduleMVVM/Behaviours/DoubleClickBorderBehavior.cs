using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShippingScheduleMVVM.Behaviours
{
    public class DoubleClickBorderBehavior : Behavior<Border>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DoubleClickBorderBehavior));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(DoubleClickBorderBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                bool isBorderBackgroundBorder = ((e.OriginalSource as Border ?? new Border()).BorderThickness == new Thickness(0)) && ((e.OriginalSource as Border ?? new Border()).CornerRadius == new CornerRadius(3));
                // Check if the original source of the event is the Border element
                if (e.OriginalSource != AssociatedObject && !isBorderBackgroundBorder)
                {
                    // Mark the event as handled to prevent it from being processed by the Border element
                    e.Handled = true;
                    return;
                }

                ICommand command = Command;
                if (command != null && command.CanExecute(CommandParameter))
                {
                    command.Execute(CommandParameter);
                }
            }
        }
    }

}
