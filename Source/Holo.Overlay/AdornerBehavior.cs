using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Holo.Overlay.Controls;

namespace Holo.Overlay
{
    public static class AdornerBehavior
    {
        #region AdornerContentProperty
        public static readonly DependencyProperty AdornerContentProperty =
            DependencyProperty.RegisterAttached("AdornerContent", typeof(object), typeof(AdornerBehavior), new FrameworkPropertyMetadata(AdornerContent_PropertyChanged));

        public static void SetAdornerContent(FrameworkElement element, object value) =>
            element.SetValue(AdornerContentProperty, value);

        public static object GetAdornerContent(FrameworkElement element) =>
            element.GetValue(AdornerContentProperty);

        private static void AdornerContent_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ShowOrHideAdornerInternal((FrameworkElement)d);
        #endregion AdornerContentProperty

        #region AdornedElementProperty
        public static readonly DependencyProperty AdornedElementProperty =
            DependencyProperty.RegisterAttached("AdornedElement", typeof(FrameworkElement), typeof(AdornerBehavior), new FrameworkPropertyMetadata(AdornerContent_PropertyChanged));

        public static void SetAdornedElement(FrameworkElement element, FrameworkElement value) =>
            element.SetValue(AdornedElementProperty, value);

        public static FrameworkElement GetAdornedElement(FrameworkElement element) =>
            (FrameworkElement)element.GetValue(AdornedElementProperty);
        #endregion AdornedElementProperty

        private static void ShowOrHideAdornerInternal(FrameworkElement element)
        {
            if (GetAdornerContent(element) == null)
                RemoveAdornerInternal(element);
            else
                AddAdornerInternal(element);
        }

        private static void RemoveAdornerInternal(FrameworkElement element)
        {
            var data = AdornerData.GetAdornerData(element);
            if (data == null) //No adorner to remove
                return;
            data.Layer.Remove(data.Adorner);
            AdornerData.SetAdornerData(element, null);
        }

        private static void AddAdornerInternal(FrameworkElement element)
        {
            //Before to add, clean the layer of element's adorner data.
            RemoveAdornerInternal(element);

            var adornedElement = FindAdornedElement(element);
            var layer = GetAdornerLayer(adornedElement);
            if (layer != null)
            {
                var content = GetAdornerContent(element);
                var contentPrensenter = new ContentPresenter() { Content = content };
                var adorner = new FrameworkElementAdorner(adornedElement) { Child = contentPrensenter };
                layer.Add(adorner);
                AdornerData.SetAdornerData(element, new AdornerData() { Layer = layer, Adorner = adorner });
            }
        }

        private static FrameworkElement FindAdornedElement(FrameworkElement element)
        {
            var adornedElement = GetAdornedElement(element) ?? element;
            var window = adornedElement as Window;
            if (window == null) return adornedElement;
            var windowElement = window.Content as FrameworkElement;
            if (windowElement != null)
                adornedElement = windowElement;
            else
                adornedElement = VisualTreeHelper.GetChild(adornedElement, 0) as FrameworkElement;
            return adornedElement;
        }

        private static AdornerLayer GetAdornerLayer(Visual visual)
        {
            var decorator = visual as AdornerDecorator;
            if (decorator != null)
                return decorator.AdornerLayer;
            var presenter = visual as ScrollContentPresenter;
            if (presenter != null)
                return presenter.AdornerLayer;
            var window1 = visual as Window;
            var window = window1;
            var visualContent = window?.Content as Visual;
            return AdornerLayer.GetAdornerLayer(visualContent ?? visual);
        }

        private class AdornerData
        {
            public AdornerLayer Layer { get; set; }
            public Adorner Adorner { get; set; }

            public static readonly DependencyProperty AdornerDataProperty =
                DependencyProperty.RegisterAttached("AdornerData", typeof(object), typeof(AdornerData));

            public static void SetAdornerData(FrameworkElement element, AdornerData value) =>
                element.SetValue(AdornerDataProperty, value);

            public static AdornerData GetAdornerData(FrameworkElement element) =>
                (AdornerData)element.GetValue(AdornerDataProperty);
        }
    }
}
