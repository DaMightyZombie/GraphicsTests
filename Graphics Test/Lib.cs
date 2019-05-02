using System;
using System.Collections.Generic;
using System.Drawing;

namespace Graphics_Test
{
    class Lib
    {

    }

    class Camera
    {
        //Postition of the camera
        //Vector3 position;
        //Rotation of the camera
        //Vector3 rotation;
        //Position of the image plane relative to the camera
        Vector3 ImagePlanePos;

        const double ClippingPlaneFront = 10d;
        const double ClippingPlaneBack = 500d;

        //int fov;

        //int width, height;

        public double ImagePlaneDistance = 100d;

        private readonly Brush brush;
        private readonly Pen pen;

        public Camera(double Width, double Height)
        {
            //position = new Vector3();
            //rotation = new Vector3();
            ImagePlanePos = new Vector3(Width/2,Height/2, ImagePlaneDistance);
            brush = Brushes.Green;
            pen = new Pen(Color.Green);
        }

        public Vector2 LocalSpace2ScreenSpace(Vector3 relativePosition)
        {
            double outX, outY;

            outX = (ImagePlanePos.Z * relativePosition.X / relativePosition.Z) * 7 + ImagePlanePos.X;

            outY = (ImagePlanePos.Z * relativePosition.Y / relativePosition.Z) * 7 + ImagePlanePos.Y;

            return new Vector2(outX, outY);
        }

        public void DrawSceneObject(SceneObject sceneObj, Graphics graphicsObj)
        {
            if (sceneObj is Point)
            {
                Point point = (Point)sceneObj;

                //Check if the point is between the two clipping planes
                if (ClippingPlaneFront < point.position.Z && point.position.Z < ClippingPlaneBack)
                {
                    Vector2 pos = LocalSpace2ScreenSpace(((Point)sceneObj).position);
                    graphicsObj.FillRectangle(brush, (float)pos.X, (float)pos.Y, 3, 3);
                }
            }
            else if (sceneObj is Line)
            {
                Line line = (Line)sceneObj;

                Line clippedLine = line.Clone();

                Clip(clippedLine);

                Vector2 pos1 = LocalSpace2ScreenSpace(clippedLine.point1);
                Vector2 pos2 = LocalSpace2ScreenSpace(clippedLine.point2);
                graphicsObj.DrawLine(pen, (float)pos1.X, (float)pos1.Y, (float)pos2.X, (float)pos2.Y);
            }
        }

        private void Clip(Line line)
        {/*
            double factor;

            Console.WriteLine($"Pt 1 was {line.point1.Repr()}, Pt 2 was {line.point2.Repr()}");

            //are both ends of the line in front of the front clipping plane?
            if (line.point1.Z <= ClippingPlaneFront && line.point2.Z <= ClippingPlaneFront)
            {
                return;
            }

            //are both ends of the line behind the back clipping plane?
            if (line.point1.Z >= ClippingPlaneBack && line.point2.Z >= ClippingPlaneBack)
            {
                return;
            }

            //is one end of the line in front of the front clipping plane?
            if (line.point1.Z < ClippingPlaneFront || line.point2.Z < ClippingPlaneFront)
            {
                factor = (ClippingPlaneFront - line.point1.Y) / (line.point2.Y - line.point1.Y);

                Console.WriteLine($"Front Factor is {factor}");
                Console.WriteLine(line.point1.Repr());


                if (line.point1.Z < ClippingPlaneFront)
                {
                    line.point1.X = line.point1.X + factor * (line.point2.X - line.point1.X);

                    line.point1.Y = line.point1.Y + factor * (line.point2.Y - line.point1.Y);

                    line.point1.Z = ClippingPlaneFront;
                }
                else
                {
                    line.point2.X = line.point1.X + factor * (line.point2.X - line.point1.X);

                    line.point2.Y = line.point1.Y + factor * (line.point2.Y - line.point1.Y);

                    line.point2.Z = ClippingPlaneFront;
                }
            }

            Console.WriteLine($"Z: {(int)line.point1.Z}");
            //is one end of the line behind the back clipping plane?
            if ((line.point1.Z > ClippingPlaneBack) || (line.point2.Z > ClippingPlaneBack))
            {
                Console.WriteLine($"Is {line.point1.Z} > {ClippingPlaneBack} ?");
                Console.WriteLine(line.point1.Z > ClippingPlaneBack);
                Console.WriteLine($"Points: {line.point1.Z}, {line.point2.Z}");

                factor = (ClippingPlaneBack - line.point1.Y) / (line.point2.Y - line.point1.Y);
                Console.WriteLine($"Back Factor is {factor}");

                if (line.point1.Z > ClippingPlaneBack)
                {
                    line.point1.X = line.point1.X + factor * (line.point2.X - line.point1.X);

                    line.point1.Y = line.point1.Y + factor * (line.point2.Y - line.point1.Y);

                    line.point1.Z = ClippingPlaneBack;
                }
                else
                {
                    line.point2.X = line.point1.X + factor * (line.point2.X - line.point1.X);

                    line.point2.Y = line.point1.Y + factor * (line.point2.Y - line.point1.Y);

                    line.point2.Z = ClippingPlaneBack;
                }
            }
            Console.WriteLine($"Pt 1 is {line.point1.Repr()}, Pt 2 is {line.point2.Repr()}");*/
        }
    }

    struct Vector2
    {
        public double X, Y;

        public Vector2(double _x, double _y)
        {
            X = _x;
            Y = _y;
        }

        public string Repr()
        {
            return $"({X}, {Y})";
        }

        public static Vector2 operator + (Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }
    }

    struct Vector3
    {
        public double X, Y, Z;

        public Vector3(double _x, double _y, double _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }

        public string Repr()
        {
            return $"({X}, {Y}, {Z})";
        }

        public static Vector3 operator + (Vector3 a, Vector3 b)
        {
            return new Vector3(a.X+b.X, a.Y+b.Y, a.Z+b.Z);
        }
    }

    class Scene
    {
        Camera MainCamera;
        List<SceneObject> sceneObjects;

        Vector2 Dimensions;

        public Scene(int Width, int Height)
        {
            sceneObjects = new List<SceneObject>();
            MainCamera = new Camera(Width, Height);
            Dimensions = new Vector2(Width, Height);
        }

        public void AddSceneObject(SceneObject sceneObject)
        {
            sceneObjects.Add(sceneObject);
        }

        public void DrawToGraphicsObj(Graphics graphicsObj)
        {
            foreach (SceneObject target in sceneObjects)
            {
                MainCamera.DrawSceneObject(target, graphicsObj);
            }
        }

        public void MoveScene(Vector3 direction)
        {
            foreach (SceneObject item in sceneObjects)
            {
                item.Move(direction);
            }
        }
    }

    abstract class SceneObject
    {
        public abstract void Move(Vector3 direction);
    }

    class Point : SceneObject
    {
        public Vector3 position;
        public Point(Vector3 _position)
        {
            position = _position;
        }

        public override void Move(Vector3 direction)
        {
            position += direction;
        }
    }

    class Line : SceneObject
    {
        public Vector3 point1, point2;
        public Line(Vector3 _point1, Vector3 _point2)
        {
            point1 = _point1;
            point2 = _point2;
        }

        public override void Move(Vector3 direction)
        {
            point1 += direction;
            point2 += direction;
        }

        public Line Clone()
        {
            return new Line(new Vector3(point1.X, point1.Y, point1.Z), new Vector3(point2.X, point2.Y, point2.Z));
        }
    }
}
