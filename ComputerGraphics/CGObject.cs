using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ComputerGraphics
{
    abstract class CGObject : INotifyPropertyChanged
    {
        public SolidColorBrush inactiveObjectColor =  new SolidColorBrush(Colors.Black);
        public SolidColorBrush activeObjectColor = new SolidColorBrush(Colors.Blue);
        public Shape ObjectShape { get; set; }
        public bool IsSelected { get; set; } = false;

        public Grid grid = new Grid();
        ColumnDefinition gridCol1 = new ColumnDefinition();
        ColumnDefinition gridCol2 = new ColumnDefinition();
        ColumnDefinition gridCol3 = new ColumnDefinition();
        ColumnDefinition gridCol4 = new ColumnDefinition();

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

        abstract public void generateGrid();

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
