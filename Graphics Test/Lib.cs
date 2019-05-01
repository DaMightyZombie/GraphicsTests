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
        Vector3 position;
        //Rotation of the camera
        Vector3 rotation;
        //Position of the image plane relative to the camera
        Vector3 ImagePlanePos;

        int fov;

        int width, height;

        Brush brush;
        Pen pen;

        public Camera(double Width, double Height)
        {
            position = new Vector3();
            rotation = new Vector3();
            ImagePlanePos = new Vector3(Width/2,Height/2,140);
            brush = Brushes.Green;
            pen = new Pen(Color.Green);
        }

        public Vector2 LocalSpace2ScreenSpace(Vector3 relativePosition)
        {
            double outX, outY;

            outX = ImagePlanePos.Z * relativePosition.X / relativePosition.Z + ImagePlanePos.X;

            outY = ImagePlanePos.Z * relativePosition.Y / relativePosition.Z + ImagePlanePos.Y;

            return new Vector2(outX, outY);
        }

        public void DrawSceneObject(SceneObject sceneObj, Graphics graphicsObj)
        {
            if (sceneObj is Point)
            {
                Vector2 pos = LocalSpace2ScreenSpace(((Point)sceneObj).position);
                graphicsObj.FillRectangle(brush, (float)pos.X, (float)pos.Y, 3, 3);
            }
            else if (sceneObj is Line)
            {
                Vector2 pos1 = LocalSpace2ScreenSpace(((Line)sceneObj).point1);
                Vector2 pos2 = LocalSpace2ScreenSpace(((Line)sceneObj).point2);
                graphicsObj.DrawLine(pen, (float)pos1.X, (float)pos1.Y, (float)pos2.X, (float)pos2.Y);
            }
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
    }
}
