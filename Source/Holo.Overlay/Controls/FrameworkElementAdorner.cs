using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Holo.Overlay.Controls
{
    public class FrameworkElementAdorner : Adorner
    {
        private FrameworkElement child;

        public FrameworkElementAdorner(FrameworkElement adornedElement)
            : base(adornedElement)
        {
            adornedElement.SizeChanged += (s, e) => InvalidateMeasure();
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
                throw new ArgumentOutOfRangeException();
            return child;
        }

        public FrameworkElement Child
        {
            get { return child; }
            set
            {
                if (child != null)
                {
                    RemoveLogicalChild(child);
                    RemoveVisualChild(child);
                }
                child = value;
                if (child != null)
                {
                    AddLogicalChild(child);
                    AddVisualChild(child);
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            child.Arrange(new Rect(new Point(0, 0), finalSize));
            return new Size(child.ActualWidth, child.ActualHeight);
        }
    }
}
