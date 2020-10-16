using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ComputerGraphics.CGobjects
{
    public class CGPoint : CGObject
    {
        public bool IsProcessed { get; set; } = false;
        private Point point;

        public Point Point
        {
            get
            {
                return point;
            }
            set
            {
                point = value;
                OnPropertyChanged("Point");
                Canvas.SetLeft(ellipse, point.X-3);
                Canvas.SetTop(ellipse, point.Y-3);
            }
        }

        public Ellipse ellipse;

        public CGPoint(Shape shape) : base(shape)
        {
            this.ellipse = (Ellipse)shape;
            ellipse.StrokeThickness = 1;
            ellipse.Width = 6;
            ellipse.Height = 6;
            ellipse.Fill = new SolidColorBrush(Colors.Red);
            base.inactiveObjectColor = new SolidColorBrush(Colors.Red);

        }

        public override void createMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = true;
            Canvas canvas = (Canvas)sender;
            Point = e.GetPosition(canvas);
            canvas.Children.Add(ellipse);
            return;
        }

        public override void createMouseMove(object sender, MouseEventArgs e)
        {

            return;
        }

        public override void createMouseUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void moveMouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void moveMouseMove(object sender, MouseEventArgs e)
        {
            return;
        }

        public override void moveMouseUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void resizeMouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void resizeMouseMove(object sender, MouseEventArgs e)
        {
            return;
        }

        public override void resizeMouseUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void rotateMouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void rotateMouseMove(object sender, MouseEventArgs e)
        {
            return;
        }

        public override void rotateMouseUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void selectMouseMove(object sender, MouseEventArgs e)
        {
            return;
        }

        public override void selectMouseUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }
    }
}
