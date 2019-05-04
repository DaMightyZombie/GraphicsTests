using System;
using System.Collections.Generic;
using System.Drawing;

namespace Graphics_Test
{
    class Camera
    {
        //Postition of the camera
        Vector3 position;
        //Rotation of the camera
        Vector3 theta;
        //Position of the image plane relative to the camera
        Vector2 ImagePlanePos;

        const double ClippingPlaneFront = 10d;
        const double ClippingPlaneBack = 1000d;

        private Vector3 costheta;
        private Vector3 sintheta;

        public double ImagePlaneDistance;

        private readonly Brush brush;
        private readonly Pen pen;

        public Camera(double Width, double Height, Vector3 _position, Vector3 _rotation, double _ImagePlaneDistance)
        {
            ImagePlanePos = new Vector2(Width/2,Height/2);
            brush = Brushes.Green;
            pen = new Pen(Color.Green);
            ImagePlaneDistance = _ImagePlaneDistance;
            SetAng(_rotation);
            position = _position;
        }

        public Vector2 LocalSpace2ScreenSpace(Vector3 relativePosition)
        {
            double outX, outY;

            outX = (ImagePlaneDistance * relativePosition.X / relativePosition.Z) * 7 + ImagePlanePos.X;

            outY = (ImagePlaneDistance * relativePosition.Y / relativePosition.Z) * 7 + ImagePlanePos.Y;

            return new Vector2(outX, outY);
        }

        public void DrawSceneObject(SceneObject sceneObj, Graphics graphicsObj)
        {
            if (sceneObj is Point)
            {
                Point point = (Point)sceneObj;
                Point eyePoint = new Point();

                //Translate the Worldspace coordinates to eyespace
                World2Eye(point, eyePoint);
                
                //Check if the point is between the two clipping planes
                if (Eye2Clipped(eyePoint))
                {
                    Vector2 pos = LocalSpace2ScreenSpace(eyePoint.position);
                    graphicsObj.FillRectangle(brush, (float)pos.X, (float)pos.Y, 3, 3);
                }
                
            }
            else if (sceneObj is Line)
            {
                Line line = (Line)sceneObj;
                Line eyeLine = new Line();

                //Translate the Worldspace coordinates to eyespace
                World2Eye(line, eyeLine);
                
                //Clip the line if nessecary
                if (Eye2Clipped(eyeLine))
                {
                    Vector2 pos1 = LocalSpace2ScreenSpace(eyeLine.point1);
                    Vector2 pos2 = LocalSpace2ScreenSpace(eyeLine.point2);
                    graphicsObj.DrawLine(pen, (float)pos1.X, (float)pos1.Y, (float)pos2.X, (float)pos2.Y);
                }
                
            }
        }

        private void World2Eye(Point point, Point eyePoint)
        {
            eyePoint.position = VecWorld2Eye(point.position);
        }

        private void World2Eye(Line line, Line eyeLine)
        {
            eyeLine.point1 = VecWorld2Eye(line.point1);
            eyeLine.point2 = VecWorld2Eye(line.point2);
        }

        private Vector3 VecWorld2Eye(Vector3 world)
        {
            Vector3 delta = world - position;

            double x, y, z;

            x = costheta.Y * (sintheta.Z * delta.Y + costheta.Z * delta.X) - sintheta.Y * delta.Z;
            y = sintheta.X * (costheta.Y * delta.Z + sintheta.Y * (sintheta.Z * delta.Y + costheta.Z * delta.X)) + costheta.X * (costheta.Z * delta.Y - sintheta.Z * delta.X);
            z = costheta.X * (costheta.Y * delta.Z + sintheta.Y * (sintheta.Z * delta.Y + costheta.Z * delta.X)) + sintheta.X * (costheta.Z * delta.Y - sintheta.Z * delta.X);

            return new Vector3(x, y, z);
        }

        private bool Eye2Clipped(Point point)
        {
            if (ClippingPlaneFront < point.position.Z && point.position.Z < ClippingPlaneBack)
            {
                return true;
            }
            return false;
        }

        private bool Eye2Clipped(Line line)
        {
            double factor;

            //are both ends of the line in front of the front clipping plane?
            if (line.point1.Z <= ClippingPlaneFront && line.point2.Z <= ClippingPlaneFront)
            {
                return false;
            }

            //are both ends of the line behind the back clipping plane?
            if (line.point1.Z >= ClippingPlaneBack && line.point2.Z >= ClippingPlaneBack)
            {
                return false;
            }

            //is one end of the line in front of the front clipping plane?
            if (line.point1.Z < ClippingPlaneFront || line.point2.Z < ClippingPlaneFront)
            {
                factor = (ClippingPlaneFront - line.point1.Z) / (line.point2.Z - line.point1.Z);

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

            //is one end of the line behind the back clipping plane?
            if ((line.point1.Z > ClippingPlaneBack) || (line.point2.Z > ClippingPlaneBack))
            {
                factor = (ClippingPlaneBack - line.point1.Z) / (line.point2.Z - line.point1.Z);

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

            return true;
        }

        public void SetAng(Vector3 ang)
        {
            theta = ang;
            costheta = theta.MemberwiseCosine();
            sintheta = theta.MemberwiseSine();
        }

        public void SetPos(Vector3 pos)
        {
            position = pos;
        }

        public void Move(Vector3 direction)
        {
            position += direction;
        }

        public void ChangeImagePlaneDistance(double d)
        {
            ImagePlaneDistance += d;
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

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public Vector3 MemberwiseSine()
        {
            Vector3 result = new Vector3();
            result.X = Math.Sin(this.X);
            result.Y = Math.Sin(this.Y);
            result.Z = Math.Sin(this.Z);
            return result;
        }

        public Vector3 MemberwiseCosine()
        {
            Vector3 result = new Vector3();
            result.X = Math.Cos(this.X);
            result.Y = Math.Cos(this.Y);
            result.Z = Math.Cos(this.Z);
            return result;
        }
    }

    class Scene
    {
        public Camera MainCamera;
        List<SceneObject> sceneObjects;

        public Scene(int Width, int Height)
        {
            sceneObjects = new List<SceneObject>();
            MainCamera = new Camera(Width, Height, new Vector3(), new Vector3(), 100);
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

        public Point()
        {
            position = new Vector3();
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

        public Line()
        {
            point1 = new Vector3();
            point2 = new Vector3();
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
