using Microsoft.Xaml.Behaviors;
using ShippingScheduleMVVM.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ShippingScheduleMVVM.Behaviours
{
    public class SliderThumbDragCompletedBehavior : Behavior<Slider>
    {
        private Thumb? _thumb;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= OnLoaded;

            if (_thumb != null)
                _thumb.DragCompleted -= OnDragCompleted;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _thumb = FindVisualChild<Thumb>(AssociatedObject);

            if (_thumb != null)
                _thumb.DragCompleted += OnDragCompleted;
        }

        private void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            var command = GetCommand();
            if (command != null && command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        public RecalculateWindowCommand Command
        {
            get { return (RecalculateWindowCommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(SliderThumbDragCompletedBehavior), new PropertyMetadata(null));

        private ICommand GetCommand()
        {
            return (ICommand)GetValue(CommandProperty);
        }

        private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            var child = default(T);
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var visual = VisualTreeHelper.GetChild(parent, i);
                child = visual as T ?? FindVisualChild<T>(visual);
                if (child != null)
                {
                    break;
                }
            }
            return child ?? null;
        }
    }

}
