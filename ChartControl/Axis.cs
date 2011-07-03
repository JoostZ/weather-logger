using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ChartControl
{
    /// <summary>
    /// General representation of an axis
    /// </summary>
    class Axis : FrameworkElement
    {
        public double Max { get; set; }

        public double Min { get; set; }

        protected double XSize { get; set; }
        protected double YSize { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //Debug.WriteLine("OnRender");

            double width = Width;
            double height = Height;
            Pen pen = new Pen(Brushes.Red, 10.0);
            drawingContext.DrawLine(pen, new Point(0,0), new Point(XSize, 0));
            base.OnRender(drawingContext);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            XSize = availableSize.Width;
            YSize = availableSize.Height;
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            XSize = finalSize.Width;
            YSize = finalSize.Height;
            return base.ArrangeOverride(finalSize);
        }
    }
}
