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

namespace ComputerGraphics
{
    class CGLine : CGObject
    {
        private Line line;
        public bool StartingPointReferenceLock { get; set; } = false;
        public bool EndingPointReferenceLock { get; set; } = false;

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
                        line.X1 = value;
                        OnPropertyChanged("StartingPointX");
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
                    line.Y1 = value;
                    OnPropertyChanged("StartingPointY");
                    
                    
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
                    line.X2 = value;
                    OnPropertyChanged("EndingPointX");

                   
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
                    line.Y2 = value;
                    OnPropertyChanged("EndingPointY");
                }
            }
        }

        Binding startingPointXBinding = new Binding("StartingPointX");
        Binding startingPointYBinding = new Binding("StartingPointY");
        Binding endingPointXBinding = new Binding("EndingPointX");
        Binding endingPointYBinding = new Binding("EndingPointY");

        public bool IsProcessed { get; set; } = false;

        public CGLine(Shape shape) : base(shape)
        {
            line = (Line)ObjectShape;
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
