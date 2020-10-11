using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    class CGRectangle : CGObject
    {
        private Rectangle Rectangle;
        private Point StartingPoint;
        private Point EndingPoint;

        public double RectangleWidth {
            
            get { return Rectangle.ActualWidth; }
            set{
                if (Rectangle.ActualWidth != value)
                {
                    Rectangle.Width = value;
                    OnPropertyChanged("RectangleWidth");
                }
            }}

        public double RectangleHeight
        {

            get { return Rectangle.ActualHeight; }
            set
            {
                if (Rectangle.ActualHeight != value)
                {
                    Rectangle.Height = value;
                    OnPropertyChanged("RectangleHeight");
                }
            }
        }
        Binding rectangleHeightBinding = new Binding("RectangleHeight");
        Binding rectangleWidthBinding = new Binding("RectangleWidth");

        public double StartingPointX
        {

            get { return StartingPoint.X; }
            set
            {
                if (StartingPoint.X != value)
                {
                    StartingPoint = new Point(value, StartingPoint.Y);
                    OnPropertyChanged("StartingPointX");
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
                    OnPropertyChanged("StartingPointY");
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
                    OnPropertyChanged("EndingPointX");
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
                    OnPropertyChanged("EndingPointY");
                }
            }
        }

        Binding startingPointXBinding = new Binding("StartingPointX");
        Binding startingPointYBinding = new Binding("StartingPointY");
        Binding endingPointXBinding = new Binding("EndingPointX");
        Binding endingPointYBinding = new Binding("EndingPointY");


        
        public bool IsProcessed { get; set; } = false;

        RowDefinition gridRow1 = new RowDefinition();
        RowDefinition gridRow2 = new RowDefinition();
        RowDefinition gridRow3 = new RowDefinition();
        RowDefinition gridRow4 = new RowDefinition();

        Label startingPointX = new Label();
        DoubleUpDown startingPointXValue = new DoubleUpDown();
        Label startingPointY = new Label();
        DoubleUpDown startingPointYValue = new DoubleUpDown();

        Label endingPointX = new Label();
        DoubleUpDown endingPointXValue = new DoubleUpDown();
        Label endingPointY = new Label();
        DoubleUpDown endingPointYValue = new DoubleUpDown();

        Label widthLabel = new Label();
        DoubleUpDown widthValue = new DoubleUpDown();
        Label heightLabel = new Label();
        DoubleUpDown heightValue = new DoubleUpDown();

        public CGRectangle(Rectangle shape) : base(shape)
        {
            Rectangle = (Rectangle)ObjectShape;
            RectangleHeight = Rectangle.ActualHeight;
            RectangleWidth = Rectangle.ActualWidth;
            StartingPointX = StartingPoint.X;
            StartingPointY = StartingPoint.Y;
            EndingPointX = EndingPoint.X;
            EndingPointY = EndingPoint.Y;
            generateGrid();
        }

        public override void selectMouseUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void selectMouseMove(object sender, MouseEventArgs e)
        {
            return;
        }

        public override void moveMouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void moveMouseUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void moveMouseMove(object sender, MouseEventArgs e)
        {
            return;
        }

        public override void resizeMouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void resizeMouseUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void resizeMouseMove(object sender, MouseEventArgs e)
        {
            return;
        }

        public override void rotateMouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void rotateMouseUp(object sender, MouseButtonEventArgs e)
        {
            return;
        }

        public override void rotateMouseMove(object sender, MouseEventArgs e)
        {
            return;
        }

        public override void createMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = true;
            Canvas canvas = (Canvas)sender;
            StartingPoint = e.GetPosition(canvas);
            Canvas.SetLeft(Rectangle, StartingPointX);
            Canvas.SetTop(Rectangle, StartingPointY);
            canvas.Children.Add(Rectangle);
            return;
        }

        public override void createMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = false;
            return;
        }

        public override void createMouseMove(object sender, MouseEventArgs e)
        {
            if(!IsProcessed)
            {
                return;
            }
            Canvas canvas = (Canvas)sender;
            EndingPoint = e.GetPosition(canvas);

            if(EndingPointX <= StartingPointX)
            {
                if (EndingPointY <= StartingPointY)
                {
                    var pos = StartingPoint;
                    StartingPoint = EndingPoint;
                    EndingPoint = pos;
                }
                else
                {
                    var temp1 = StartingPoint;
                    var temp2 = EndingPoint;
                    StartingPoint = new Point(temp2.X, temp1.Y);
                    EndingPoint = new Point(temp1.X, temp2.Y); ;
                }
            }
            else
            {
                if (EndingPointY <= StartingPointY)
                {
                    var temp1 = StartingPoint;
                    var temp2 = EndingPoint;
                    StartingPoint = new Point(temp1.X, temp2.Y);
                    EndingPoint = new Point(temp2.X, temp1.Y); ;
                }
            }

            var w = EndingPointX - StartingPointX;
            var h = EndingPointY - StartingPointY;

            RectangleWidth = w;
            RectangleHeight = h;

            Canvas.SetLeft(Rectangle, StartingPointX);
            Canvas.SetTop(Rectangle, StartingPointY);

            return;
        }

        public override void generateGrid()
        {
            grid.RowDefinitions.Add(gridRow1);
            grid.RowDefinitions.Add(gridRow2);
            grid.RowDefinitions.Add(gridRow3);

            gridRow1.Height = new GridLength(30);
            gridRow2.Height = new GridLength(30);
            gridRow3.Height = new GridLength(30);
            gridRow4.Height = new GridLength(30);

            startingPointXValue.Height = 30;
            startingPointYValue.Height = 30;
            endingPointXValue.Height = 30;
            endingPointYValue.Height = 30;
            widthValue.Height = 30;
            heightValue.Height = 30;

            startingPointX.Content = "Start X:";
            startingPointY.Content = "Start Y:";
            endingPointX.Content = "End X:";
            endingPointY.Content = "End Y:";
            widthLabel.Content = "Width:";
            heightLabel.Content = "Height:";

            rectangleHeightBinding.Source = this;
            rectangleWidthBinding.Source = this;
            rectangleHeightBinding.Mode = BindingMode.TwoWay;
            rectangleWidthBinding.Mode = BindingMode.TwoWay;
            rectangleHeightBinding.NotifyOnSourceUpdated = true;
            rectangleWidthBinding.NotifyOnSourceUpdated = true;
            rectangleHeightBinding.NotifyOnTargetUpdated = true;
            rectangleWidthBinding.NotifyOnTargetUpdated = true;

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

            heightValue.SetBinding(DoubleUpDown.ValueProperty, rectangleHeightBinding);
            widthValue.SetBinding(DoubleUpDown.ValueProperty, rectangleWidthBinding);

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
