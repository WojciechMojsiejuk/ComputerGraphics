using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace ComputerGraphics
{
    class CGEllipse : CGObject
    {
        private Ellipse ellipse;
        private Point startingPoint;
        private Point endingPoint;

        public bool StartingPointReferenceLock { get; set; } = false;
        public bool EndingPointReferenceLock { get; set; } = false;
        public bool EllipseReferenceLock { get; set; } = false;

        public double EllipseWidth
        {

            get { return ellipse.ActualWidth; }
            set
            {
                if ((ellipse.ActualWidth != value) && (value >= 0))
                {
                    if ((Operation.option != Operation.Option.Select) && (Operation.option != Operation.Option.Move))
                    {
                        ellipse.Width = value;
                        OnPropertyChanged("EllipseWidth");
                    }
                    if ((Operation.option == Operation.Option.Resize) && (EndingPointReferenceLock == false))
                    {
                        EllipseReferenceLock = true;
                        EndingPoint = new Point(StartingPoint.X + ellipse.ActualWidth, StartingPoint.Y + ellipse.ActualHeight);
                        EllipseReferenceLock = false;
                    }
                }
            }
        }
        public double EllipseHeight
        {

            get { return ellipse.ActualHeight; }
            set
            {
                if ((ellipse.ActualHeight != value) && (value >= 0))
                {
                    if ((Operation.option != Operation.Option.Select) && (Operation.option != Operation.Option.Move))
                    {
                        ellipse.Height = value;
                        OnPropertyChanged("EllipseHeight");
                    }
                    if ((Operation.option == Operation.Option.Resize) && (EndingPointReferenceLock == false))
                    {
                        EllipseReferenceLock = true;
                        EndingPoint = new Point(StartingPoint.X + ellipse.ActualWidth, StartingPoint.Y + ellipse.ActualHeight);
                        EllipseReferenceLock = false;
                    }
                }

            }
        }
        Binding ellipseHeightBinding = new Binding("EllipseHeight");
        Binding ellipseWidthBinding = new Binding("EllipseWidth");

        public Point StartingPoint
        {

            get { return startingPoint; }
            set
            {
                if (startingPoint != value)
                {
                    if (Operation.option != Operation.Option.Select)
                    {
                        startingPoint = value;
                        OnPropertyChanged("StartingPointX");
                        OnPropertyChanged("StartingPointY");
                    }
                    if ((Operation.option == Operation.Option.Move) && (EndingPointReferenceLock == false))
                    {
                        StartingPointReferenceLock = true;
                        EndingPoint = new Point(StartingPoint.X + ellipse.Width, StartingPoint.Y + ellipse.Height);
                        StartingPointReferenceLock = false;
                    }
                    else if ((Operation.option == Operation.Option.Resize) && (EllipseReferenceLock == false))
                    {
                        EndingPointReferenceLock = true;
                        EllipseWidth = EndingPoint.X - StartingPoint.X;
                        EllipseHeight = EndingPoint.Y - StartingPoint.Y;
                        EndingPointReferenceLock = false;
                    }
                    else if (Operation.option == Operation.Option.Create)
                    {
                        EllipseWidth = EndingPointX - StartingPointX;
                        EllipseHeight = EndingPointY - StartingPointY;
                    }

                    Canvas.SetLeft(ellipse, StartingPointX);
                    Canvas.SetTop(ellipse, StartingPointY);
                }
            }
        }
        public Point EndingPoint
        {

            get { return endingPoint; }
            set
            {
                if ((endingPoint != value))
                {
                    if (Operation.option != Operation.Option.Select)
                    {
                        endingPoint = value;
                        OnPropertyChanged("EndingPointX");
                        OnPropertyChanged("EndingPointY");
                    }
                    if ((Operation.option == Operation.Option.Move) && (StartingPointReferenceLock == false))
                    {
                        EndingPointReferenceLock = true;
                        StartingPoint = new Point(EndingPoint.X - ellipse.Width, EndingPoint.Y - ellipse.Height);
                        EndingPointReferenceLock = false;
                    }
                    else if ((Operation.option == Operation.Option.Resize) && (EllipseReferenceLock == false))
                    {
                        EndingPointReferenceLock = true;
                        EllipseWidth = EndingPoint.X - StartingPoint.X;
                        EllipseHeight = EndingPoint.Y - StartingPoint.Y;
                        EndingPointReferenceLock = false;
                    }
                    else if (Operation.option == Operation.Option.Create)
                    {
                        EllipseWidth = EndingPointX - StartingPointX;
                        EllipseHeight = EndingPointY - StartingPointY;
                    }
                }
            }
        }
        public double StartingPointX
        {

            get { return StartingPoint.X; }
            set
            {
                if (StartingPoint.X != value)
                {
                    StartingPoint = new Point(value, StartingPoint.Y);
                }
            }
        }
        public double StartingPointY
        {

            get { return StartingPoint.Y; }
            set
            {
                if (StartingPoint.Y != value)
                {
                    StartingPoint = new Point(StartingPoint.X, value);
                }
            }
        }
        public double EndingPointX
        {

            get { return EndingPoint.X; }
            set
            {
                if (EndingPoint.X != value)
                {
                    EndingPoint = new Point(value, EndingPoint.Y);
                }
            }
        }
        public double EndingPointY
        {

            get { return EndingPoint.Y; }
            set
            {
                if (EndingPoint.Y != value)
                {
                    EndingPoint = new Point(EndingPoint.X, value);
                }
            }
        }

        public bool IsProcessed { get; set; } = false;

        public CGEllipse(Shape shape) : base(shape)
        {
            ellipse = (Ellipse)ObjectShape;
            EllipseHeight = ellipse.ActualHeight;
            EllipseWidth = ellipse.ActualWidth;
            StartingPointX = StartingPoint.X;
            StartingPointY = StartingPoint.Y;
            StartingPoint = startingPoint;
            EndingPointX = EndingPoint.X;
            EndingPointY = EndingPoint.Y;
            EndingPoint = endingPoint;
            generateGrid();
        }

        public override void createMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = true;
            Canvas canvas = (Canvas)sender;
            StartingPoint = e.GetPosition(canvas);
            return;
        }

        public override void createMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsProcessed)
            {
                return;
            }
            Canvas canvas = (Canvas)sender;
            EndingPoint = e.GetPosition(canvas);
            return;
        }

        public override void createMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = false;
            return;
        }

        public override void moveMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = true;
            Canvas canvas = (Canvas)sender;
            StartingPoint = e.GetPosition(canvas);
            return;
        }

        public override void moveMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsProcessed)
            {
                return;
            }
            Canvas canvas = (Canvas)sender;
            StartingPoint = e.GetPosition(canvas);
        }

        public override void moveMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = false;
            return;
        }

        public override void resizeMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = true;
            Canvas canvas = (Canvas)sender;
            EndingPoint = e.GetPosition(canvas);
            return;
        }

        public override void resizeMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsProcessed)
            {
                return;
            }
            Canvas canvas = (Canvas)sender;
            EndingPoint = e.GetPosition(canvas);
        }

        public override void resizeMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = false;
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

        public override void generateGrid()
        {
            base.generateGrid();

            ellipseHeightBinding.Source = this;
            ellipseWidthBinding.Source = this;
            ellipseHeightBinding.Mode = BindingMode.TwoWay;
            ellipseWidthBinding.Mode = BindingMode.TwoWay;
            ellipseHeightBinding.NotifyOnSourceUpdated = true;
            ellipseWidthBinding.NotifyOnSourceUpdated = true;
            ellipseHeightBinding.NotifyOnTargetUpdated = true;
            ellipseWidthBinding.NotifyOnTargetUpdated = true;

            startingPointXBinding.Source = this;
            startingPointYBinding.Source = this;
            startingPointXBinding.Mode = BindingMode.TwoWay;
            startingPointYBinding.Mode = BindingMode.TwoWay;
            startingPointXBinding.NotifyOnSourceUpdated = true;
            startingPointYBinding.NotifyOnSourceUpdated = true;
            startingPointXBinding.NotifyOnTargetUpdated = true;
            startingPointYBinding.NotifyOnTargetUpdated = true;

            endingPointXBinding.Source = this;
            endingPointYBinding.Source = this;
            endingPointXBinding.Mode = BindingMode.TwoWay;
            endingPointYBinding.Mode = BindingMode.TwoWay;
            endingPointXBinding.NotifyOnSourceUpdated = true;
            endingPointYBinding.NotifyOnSourceUpdated = true;
            endingPointXBinding.NotifyOnTargetUpdated = true;
            endingPointYBinding.NotifyOnTargetUpdated = true;

            heightValue.SetBinding(DoubleUpDown.ValueProperty, ellipseHeightBinding);
            widthValue.SetBinding(DoubleUpDown.ValueProperty, ellipseWidthBinding);

            endingPointXValue.SetBinding(DoubleUpDown.ValueProperty, endingPointXBinding);
            endingPointYValue.SetBinding(DoubleUpDown.ValueProperty, endingPointYBinding);
            startingPointXValue.SetBinding(DoubleUpDown.ValueProperty, startingPointXBinding);
            startingPointYValue.SetBinding(DoubleUpDown.ValueProperty, startingPointYBinding);

            Grid.SetRow(startingPointX, 0);
            Grid.SetRow(startingPointY, 0);
            Grid.SetRow(startingPointXValue, 0);
            Grid.SetRow(startingPointYValue, 0);
            Grid.SetRow(endingPointX, 1);
            Grid.SetRow(endingPointY, 1);
            Grid.SetRow(endingPointXValue, 1);
            Grid.SetRow(endingPointYValue, 1);
            Grid.SetRow(widthLabel, 2);
            Grid.SetRow(widthValue, 2);
            Grid.SetRow(heightLabel, 2);
            Grid.SetRow(heightValue, 2);


            Grid.SetColumn(startingPointX, 0);
            Grid.SetColumn(startingPointY, 2);
            Grid.SetColumn(startingPointXValue, 1);
            Grid.SetColumn(startingPointYValue, 3);
            Grid.SetColumn(endingPointX, 0);
            Grid.SetColumn(endingPointY, 2);
            Grid.SetColumn(endingPointXValue, 1);
            Grid.SetColumn(endingPointYValue, 3);
            Grid.SetColumn(widthLabel, 0);
            Grid.SetColumn(widthValue, 1);
            Grid.SetColumn(heightLabel, 2);
            Grid.SetColumn(heightValue, 3);

            grid.Children.Add(startingPointX);
            grid.Children.Add(startingPointY);
            grid.Children.Add(startingPointXValue);
            grid.Children.Add(startingPointYValue);
            grid.Children.Add(endingPointX);
            grid.Children.Add(endingPointY);
            grid.Children.Add(endingPointXValue);
            grid.Children.Add(endingPointYValue);
            grid.Children.Add(widthLabel);
            grid.Children.Add(widthValue);
            grid.Children.Add(heightLabel);
            grid.Children.Add(heightValue);
        }
    }
}
