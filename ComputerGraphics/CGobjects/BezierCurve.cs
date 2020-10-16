﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ComputerGraphics.CGobjects
{
    class BezierCurve : CGObject
    {
        public ObservableCollection<CGPoint> landmarks = new ObservableCollection<CGPoint>();
        public bool FirstElemInitalization { get; set; } = true;

        PathFigure bezierFigure = new PathFigure();

        public Path ControlPointsPath = new Path();
        public Path BezierCurvePath = new Path();

        const int STEP = 100;


        public BezierCurve(Shape shape):base(shape)
        {
            CGPoint cgPoint = new CGPoint((Ellipse)shape);
            landmarks.Add(cgPoint);
            landmarks.CollectionChanged += this.OnCollectionChanged;
            
            
            //ControlPointsPen.Brush = new SolidColorBrush(Colors.BlanchedAlmond);
            //ControlPointsPen.

            //BezierCurvePen.Brush = new SolidColorBrush(Colors.Black);

        }

        public static long GetBinCoeff(long N, long K)
        {
            // This function gets the total number of unique combinations based upon N and K.
            // N is the total number of items.
            // K is the size of the group.
            // Total number of unique combinations = N! / ( K! (N - K)! ).
            // This function is less efficient, but is more likely to not overflow when N and K are large.
           
            long r = 1;
            long d;
            if (K > N) return 0;
            for (d = 1; d <= K; d++)
            {
                r *= N--;
                r /= d;
            }
            return r;
        }

        public static double BernsteinPolynomial(double t, int i, int n)
        {
            return GetBinCoeff(n, i) * Math.Pow(t, i) * Math.Pow((1 - t), (n - i));
        }



        public override void createMouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
 
            if(FirstElemInitalization)
            {
                // First point is initialized in constructor
                landmarks[0].Point = e.GetPosition(canvas);
                FirstElemInitalization = false;

            }
            else
            {
                var elem = new CGPoint(new Ellipse());
                
                canvas.Children.Add(elem.ellipse);
                elem.Point = e.GetPosition(canvas);
                landmarks.Add(elem);
            }
            
            return;
        }

       

        public override CGObject selectMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach(var elem in landmarks)
            {
                if (elem.selectMouseDown(sender, e) != null)
                    return elem;
            }
            return null;
        }

        void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Point of bezier curve changed");
            DrawBezierCurve();
        }

        void DrawBezierCurve()
        {
            bezierFigure.StartPoint = landmarks[0].Point;


            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();

            for(int index = 1; index < landmarks.Count; index++)
            {
                LineSegment myLineSegment = new LineSegment();
                myLineSegment.Point = landmarks[index].Point;
                myPathSegmentCollection.Add(myLineSegment);
            }

            bezierFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(bezierFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            ControlPointsPath.Stroke = Brushes.BlanchedAlmond;
            ControlPointsPath.StrokeThickness = 1;
            ControlPointsPath.StrokeDashArray = new DoubleCollection(new List<double>(){4,4});
            ControlPointsPath.Data = myPathGeometry;
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