using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerGraphics
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CGObject selectedObject = null;
        private List<CGObject> objectsList = new List<CGObject>();

        
        public MainWindow()
        {
            
            InitializeComponent();
        }

        public Canvas GetCanvas()
        {
            return imageCanvas;
        }

        private void SelectOperation(object sender, RoutedEventArgs e)
        {
            try
            {
                ICommandSource option = (ICommandSource)e.OriginalSource;
                System.Diagnostics.Debug.WriteLine(option.CommandParameter);
                switch (option.CommandParameter)
                {
                    case "Select":
                        Operation.option = Operation.Option.Select;
                        break;
                    case "Move":
                        Operation.option = Operation.Option.Move;
                        break;
                    case "Resize":
                        Operation.option = Operation.Option.Resize;
                        break;
                    case "Rotate":
                        Operation.option = Operation.Option.Rotate;
                        break;
                    default:
                        Operation.option = Operation.Option.Select;
                        break;
                }
            }
            catch(InvalidCastException ice)
            {
                System.Diagnostics.Debug.WriteLine(ice);
            }
        }

        private void CreateObject(object sender, RoutedEventArgs e)
        {
            ICommandSource option = (ICommandSource)e.OriginalSource;
            System.Diagnostics.Debug.WriteLine(option.CommandParameter);
            switch (option.CommandParameter)
            {
                case "Line":
                    Line l = new Line();
                    CGLine cgLine = new CGLine(l);
                    objectsList.Add(cgLine);
                    selectedObject = cgLine;
                    try
                    {
                        rightPanel.Children.RemoveAt(0);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ;
                    }
                    selectedObject.grid.UpdateLayout();
                    rightPanel.Children.Add(selectedObject.grid);
                    rightPanel.UpdateLayout();
                    imageCanvas.Children.Add(selectedObject.ObjectShape);
                    break;
                case "Rectangle":
                    Rectangle r = new Rectangle();
                    CGRectangle obj = new CGRectangle(r);
                    objectsList.Add(obj);
                    selectedObject = obj;
                    try
                    {
                        rightPanel.Children.RemoveAt(0);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ;
                    }
                    selectedObject.grid.UpdateLayout();
                    rightPanel.Children.Add(selectedObject.grid);
                    rightPanel.UpdateLayout();
                    imageCanvas.Children.Add(selectedObject.ObjectShape);
                    break;
                case "Circle":
                    Ellipse ellipse = new Ellipse();
                    CGEllipse cGEllipse = new CGEllipse(ellipse);
                    objectsList.Add(cGEllipse);
                    selectedObject = cGEllipse;
                    try
                    {
                        rightPanel.Children.RemoveAt(0);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ;
                    }
                    selectedObject.grid.UpdateLayout();
                    rightPanel.Children.Add(selectedObject.grid);
                    rightPanel.UpdateLayout();
                    imageCanvas.Children.Add(selectedObject.ObjectShape);
                    break;
                case "BezierCurve":
                    break;
                case "Polygon":
                    break;
                default:
                    Operation.option = Operation.Option.Select;
                    break;
            }
            Operation.option = Operation.Option.Create;
        }

        public void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (Operation.option)
            {
                case (Operation.Option.Select):
                    foreach(var elem in objectsList)
                    {
                        try
                        {
                            rightPanel.Children.RemoveAt(0);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            ;
                        }
                        selectedObject = null;
                        if (elem.selectMouseDown(sender, e) != null)
                        {
                            selectedObject = elem;
                            selectedObject.grid.UpdateLayout();
                            rightPanel.Children.Add(selectedObject.grid);
                            rightPanel.UpdateLayout();
                            break;
                        }
                    }
                    break;
                case (Operation.Option.Move):
                    if (selectedObject != null)
                    {
                        selectedObject.moveMouseDown(sender, e);
                    }
                    break;
                case (Operation.Option.Resize):
                    if (selectedObject != null)
                    {
                        selectedObject.resizeMouseDown(sender, e);
                    }
                    break;
                case (Operation.Option.Rotate):
                    if (selectedObject != null)
                    {
                        selectedObject.rotateMouseDown(sender, e);
                    }
                    break;
                case (Operation.Option.Create):
                    if (selectedObject != null)
                    {
                        selectedObject.createMouseDown(sender, e);
                    }
                    break;
            }
        }

        public void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (Operation.option)
            {
                case (Operation.Option.Select):
                    if (selectedObject != null)
                    {
                        selectedObject.selectMouseUp(sender, e);
                    }
                    break;
                case (Operation.Option.Move):
                    if (selectedObject != null)
                    {
                        selectedObject.moveMouseUp(sender, e);
                    }
                    break;
                case (Operation.Option.Resize):
                    if (selectedObject != null)
                    {
                        selectedObject.resizeMouseUp(sender, e);
                    }
                    break;
                case (Operation.Option.Rotate):
                    if (selectedObject != null)
                    {
                        selectedObject.rotateMouseUp(sender, e);
                    }
                    break;
                case (Operation.Option.Create):
                    if (selectedObject != null)
                    {
                        selectedObject.createMouseUp(sender, e);
                        Operation.option = Operation.Option.Select;
                        selectedObject = null;
                    }
                    break;
            }
        }

        public void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (Operation.option)
            {
                case (Operation.Option.Select):
                    break;
                case (Operation.Option.Move):
                    if (selectedObject != null)
                    {
                        selectedObject.moveMouseMove(sender, e);
                    }
                    break;
                case (Operation.Option.Resize):
                    if (selectedObject != null)
                    {
                        selectedObject.resizeMouseMove(sender, e);
                    }
                    break;
                case (Operation.Option.Rotate):
                    if (selectedObject != null)
                    {
                        selectedObject.rotateMouseMove(sender, e);
                    }
                    break;
                case (Operation.Option.Create):
                    if (selectedObject != null)
                    {
                        selectedObject.createMouseMove(sender, e);
                    }
                    break;
            }
        }
    }
}
