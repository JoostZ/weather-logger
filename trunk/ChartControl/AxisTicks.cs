using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ChartControl
{
    class AxisTicks : Shape
    {
        protected double XSize { get; set; }
        protected double YSize { get; set; }
        private Axis _axis;

        public Axis Axis
        {
            get { return _axis; }
            set { _axis = value; }
        }

        private void UpdateUI()
        {
            rectGeometry = new RectangleGeometry(new Rect(new Size(XSize, YSize / 2)));
        }

        private RectangleGeometry rectGeometry = new RectangleGeometry();
        protected override Geometry DefiningGeometry
        {
            get { return rectGeometry; }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            XSize = availableSize.Width;
            YSize = availableSize.Height;
            UpdateUI();
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            XSize = finalSize.Width;
            YSize = finalSize.Height;
            UpdateUI();
            return base.ArrangeOverride(finalSize);
        }
    }
}
