namespace Thinning.UI.Views
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    public static class ViewBoxTracking
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(UIElement), typeof(ViewBoxTracking), new PropertyMetadata(null, SourceChanged));

        public static void SetSource(TileBrush brush, UIElement value) => brush.SetValue(SourceProperty, value);

        public static UIElement GetSource(TileBrush brush) => (UIElement)brush.GetValue(SourceProperty);

        public static int ZoomFactor = 3;

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs ea)
        {
            if (ea.NewValue is UIElement source && d is ImageBrush target)
            {
                source.MouseLeave += (s, e) =>
                {
                    target.Viewbox = new Rect(0, 0, 1, 1);
                };

                source.MouseMove += (s, e) => SourceMouseMove(target, e.GetPosition(source), source.RenderSize);
            }
        }

        private static void SourceMouseMove(ImageBrush target, Point currentPoint, Size sourceSize)
        {
            double viewBoxWidth = sourceSize.Width / ZoomFactor;
            double viewBoxHeight = sourceSize.Height / ZoomFactor;

            double x = Math.Max(0, currentPoint.X - viewBoxWidth / 2);
            double y = Math.Max(0, currentPoint.Y - viewBoxHeight / 2);

            if (x + viewBoxWidth > sourceSize.Width)
            {
                x = sourceSize.Width - viewBoxWidth;
            }

            if (y + viewBoxHeight > sourceSize.Height)
            {
                y = sourceSize.Height - viewBoxHeight;
            }

            x /= sourceSize.Width;
            y /= sourceSize.Height;

            target.Viewbox = new Rect(x, y, 1d / ZoomFactor, 1d / ZoomFactor);
        }
    }

}
