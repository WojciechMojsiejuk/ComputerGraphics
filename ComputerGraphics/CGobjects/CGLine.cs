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
    class CGLine : CGObject
    {
        private Line line;
        private double lineWidth;
        private double lineHeight;
        public bool StartingPointReferenceLock { get; set; } = false;
        public bool EndingPointReferenceLock { get; set; } = false;

        public bool LineReferenceLock { get; set; } = false;

        public double StartingPointX
        {
            get
            {
                return line.X1;
            }
            set
            {
                if (line.X1 != value)
                {
                    if (Operation.option != Operation.Option.Select)
                    {
                        if ((Operation.option == Operation.Option.Move) && (EndingPointReferenceLock == false))
                        {
                            StartingPointReferenceLock = true;
                            EndingPointX += value - line.X1;
                            StartingPointReferenceLock = false;
                        }
                        if (Operation.option != Operation.Option.Select)
                        {
                            line.X1 = value;
                            OnPropertyChanged("StartingPointX");
                            LineWidth = Math.Abs(line.X2 - line.X1);
                        }
                    }
                    
                }
            }
        }
        public double StartingPointY
        {
            get
            {
                return line.Y1;
            }
            set
            {
                if (line.Y1 != value)
                {
                    if ((Operation.option == Operation.Option.Move) && (EndingPointReferenceLock == false))
                    {
                        StartingPointReferenceLock = true;
                        EndingPointY += value - line.Y1;
                        StartingPointReferenceLock = false;
                    }
                    if (Operation.option != Operation.Option.Select)
                    {
                        line.Y1 = value;
                        OnPropertyChanged("StartingPointY");
                        LineHeight = Math.Abs(line.Y2 - line.Y1);
                    }
                }
            }
        }
        public double EndingPointX
        {
            get
            {
                return line.X2;
            }
            set
            {
                if (line.X2 != value)
                {
                    if ((Operation.option == Operation.Option.Move) && (StartingPointReferenceLock == false))
                    {
                        EndingPointReferenceLock = true;
                        StartingPointX += value - line.X2;
                        EndingPointReferenceLock = false;
                    }
                    if (Operation.option != Operation.Option.Select)
                    {
                        line.X2 = value;
                        OnPropertyChanged("EndingPointX");
                        LineWidth = Math.Abs(line.X2 - line.X1);
                    }

                }
            }
        }
        public double EndingPointY
        {
            get
            {
                return line.Y2;
            }
            set
            {
                if (line.Y2 != value)
                {
                    if ((Operation.option == Operation.Option.Move) && (StartingPointReferenceLock == false))
                    {
                        EndingPointReferenceLock = true;
                        StartingPointY += value - line.Y2;
                        EndingPointReferenceLock = false;
                    }
                    if (Operation.option != Operation.Option.Select)
                    {
                        line.Y2 = value;
                        OnPropertyChanged("EndingPointY");
                        LineHeight = Math.Abs(line.Y2-line.Y1);
                    }    
                }
            }
        }

        public double LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                if (lineWidth != value)
                {
                    lineWidth = value;
                    OnPropertyChanged("LineWidth");
                }
            }
            
        }
        public double LineHeight
        {
            get
            {
                return lineHeight;
            }
            set
            {
                if (lineHeight != value)
                {
                    lineHeight = value;
                    OnPropertyChanged("LineHeight");
                }
            }
        }

        Binding lineHeightBinding = new Binding("LineHeight");
        Binding lineWidthBinding = new Binding("LineWidth");

        public bool IsProcessed { get; set; } = false;

        public CGLine(Shape shape) : base(shape)
        {
            line = (Line)ObjectShape;
            LineHeight = line.ActualHeight;
            LineWidth = line.ActualWidth;
            StartingPointX = line.X1;
            StartingPointY = line.Y1;
            EndingPointX = line.X2;
            EndingPointY = line.Y2;
            generateGrid();
        }

        public override void createMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = true;
            Canvas canvas = (Canvas)sender;
            var pos = e.GetPosition(canvas);
            StartingPointX = pos.X;
            StartingPointY = pos.Y;
            return;
        }

        public override void createMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsProcessed)
            {
                return;
            }
            Canvas canvas = (Canvas)sender;
            var pos = e.GetPosition(canvas);
            EndingPointX = pos.X;
            EndingPointY = pos.Y;
            return;
        }

        public override void createMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = false;
            return;
        }

        public override void generateGrid()
        {
            base.generateGrid();
            lineHeightBinding.Source = this;
            lineWidthBinding.Source = this;
            lineHeightBinding.Mode = BindingMode.TwoWay;
            lineWidthBinding.Mode = BindingMode.TwoWay;
            lineHeightBinding.NotifyOnSourceUpdated = true;
            lineWidthBinding.NotifyOnSourceUpdated = true;
            lineHeightBinding.NotifyOnTargetUpdated = true;
            lineWidthBinding.NotifyOnTargetUpdated = true;

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

            heightValue.SetBinding(DoubleUpDown.ValueProperty, lineHeightBinding);
            widthValue.SetBinding(DoubleUpDown.ValueProperty, lineWidthBinding);
            
            heightValue.ShowButtonSpinner = false;
            widthValue.ShowButtonSpinner = false;
            heightValue.AllowTextInput = false;
            widthValue.AllowTextInput = false;

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
            return;
        }

        public override void moveMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsProcessed = true;
            Canvas canvas = (Canvas)sender;
            var pos = e.GetPosition(canvas);
            StartingPointX = pos.X;
            StartingPointY = pos.Y;
            return;
        }

        public override void moveMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsProcessed)
            {
                return;
            }
            Canvas canvas = (Canvas)sender;
            var pos = e.GetPosition(canvas);
            StartingPointX = pos.X;
            StartingPointY = pos.Y;
            return;
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
            var pos = e.GetPosition(canvas);
            EndingPointX = pos.X;
            EndingPointY = pos.Y;
            return;
        }

        public override void resizeMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsProcessed)
            {
                return;
            }
            Canvas canvas = (Canvas)sender;
            var pos = e.GetPosition(canvas);
            EndingPointX = pos.X;
            EndingPointY = pos.Y;
            return;
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
    }
}
