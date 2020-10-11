using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace ComputerGraphics
{
    abstract class CGObject : INotifyPropertyChanged
    {
        public SolidColorBrush inactiveObjectColor =  new SolidColorBrush(Colors.Black);
        public SolidColorBrush activeObjectColor = new SolidColorBrush(Colors.Blue);
        public Shape ObjectShape { get; set; }
        public bool IsSelected { get; set; } = false;

        public Grid grid = new Grid();
        public ColumnDefinition gridCol1 = new ColumnDefinition();
        public ColumnDefinition gridCol2 = new ColumnDefinition();
        public ColumnDefinition gridCol3 = new ColumnDefinition();
        public ColumnDefinition gridCol4 = new ColumnDefinition();

        public RowDefinition gridRow1 = new RowDefinition();
        public RowDefinition gridRow2 = new RowDefinition();
        public RowDefinition gridRow3 = new RowDefinition();

        public Label startingPointX = new Label();
        public DoubleUpDown startingPointXValue = new DoubleUpDown();
        public Label startingPointY = new Label();
        public DoubleUpDown startingPointYValue = new DoubleUpDown();

        public Label endingPointX = new Label();
        public DoubleUpDown endingPointXValue = new DoubleUpDown();
        public Label endingPointY = new Label();
        public DoubleUpDown endingPointYValue = new DoubleUpDown();
 
        public Label widthLabel = new Label();
        public DoubleUpDown widthValue = new DoubleUpDown();
        public Label heightLabel = new Label();
        public DoubleUpDown heightValue = new DoubleUpDown(); 

        public Binding startingPointXBinding = new Binding("StartingPointX");
        public Binding startingPointYBinding = new Binding("StartingPointY");
        public Binding endingPointXBinding = new Binding("EndingPointX");
        public Binding endingPointYBinding = new Binding("EndingPointY");

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(PropertyName));
        }

        public CGObject(Shape shape)
        {
            ObjectShape = shape;
            ObjectShape.AllowDrop = true;
            ObjectShape.Cursor = Cursors.Hand;
            ObjectShape.Stroke = inactiveObjectColor;
            ObjectShape.StrokeThickness = 2;
            ObjectShape.MouseEnter += new MouseEventHandler(onHover);
            ObjectShape.MouseLeave += new MouseEventHandler(onHoverEnd);

            grid.Width = 300;
            grid.MinWidth = 300;
            grid.ColumnDefinitions.Add(gridCol1);
            grid.ColumnDefinitions.Add(gridCol2);
            grid.ColumnDefinitions.Add(gridCol3);
            grid.ColumnDefinitions.Add(gridCol4);

        }

        public virtual void generateGrid()
        {
            grid.RowDefinitions.Add(gridRow1);
            grid.RowDefinitions.Add(gridRow2);
            grid.RowDefinitions.Add(gridRow3);

            gridRow1.Height = new GridLength(30);
            gridRow2.Height = new GridLength(30);
            gridRow3.Height = new GridLength(30);

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
        }

        public void onHover(object sender, MouseEventArgs e)
        {
            switch(Operation.option)
            {
                case (Operation.Option.Select):
                    ObjectShape.Stroke = activeObjectColor;
                    IsSelected = true;
                    break;
            }
        }
        public void onHoverEnd(object sender, MouseEventArgs e)
        {
            ObjectShape.Stroke = inactiveObjectColor;
            IsSelected = false;
        }

       

        public CGObject selectMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsSelected)
                return this;
            else
                return null;
        }
        abstract public void selectMouseUp(object sender, MouseButtonEventArgs e);
        abstract public void selectMouseMove(object sender, MouseEventArgs e);

        abstract public void moveMouseDown(object sender, MouseButtonEventArgs e);
        abstract public void moveMouseUp(object sender, MouseButtonEventArgs e);
        abstract public void moveMouseMove(object sender, MouseEventArgs e);

        abstract public void resizeMouseDown(object sender, MouseButtonEventArgs e);
        abstract public void resizeMouseUp(object sender, MouseButtonEventArgs e);
        abstract public void resizeMouseMove(object sender, MouseEventArgs e);

        abstract public void rotateMouseDown(object sender, MouseButtonEventArgs e);
        abstract public void rotateMouseUp(object sender, MouseButtonEventArgs e);
        abstract public void rotateMouseMove(object sender, MouseEventArgs e);

        abstract public void createMouseDown(object sender, MouseButtonEventArgs e);
        abstract public void createMouseUp(object sender, MouseButtonEventArgs e);
        abstract public void createMouseMove(object sender, MouseEventArgs e);

    }
}
