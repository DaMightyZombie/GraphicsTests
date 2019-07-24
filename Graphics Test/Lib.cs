using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
namespace Graphics_Test
{
    class Camera
    {
        //Postition of the camera
        public Vector3 position;
        //Rotation of the camera
        public Vector3 theta;
        //Position of the image plane relative to the camera
        Vector2 ImagePlanePos;

        const double zNear = 10d;
        const double zFar = 2000d;

        private Vector3 costheta;
        private Vector3 sintheta;

        public double ImagePlaneDistance;

        private readonly Brush brush;
        private readonly Pen pen;

        private int SelectedIconIndex;

        private static readonly Bitmap[] PointIcons = {
            new Bitmap(Application.StartupPath + @"\img\point_dot.ico"),
            new Bitmap(Application.StartupPath + @"\img\point_circle.ico"),
            new Bitmap(Application.StartupPath + @"\img\point_cross.ico"),
            new Bitmap(Application.StartupPath + @"\img\point_cross2.ico")

        };

        private Bitmap SelectedPointIcon;

        public Camera(double Width, double Height, Vector3 _position, Vector3 _rotation, double _ImagePlaneDistance)
        {
            ImagePlanePos = new Vector2(Width / 2, Height / 2);
            brush = Brushes.Green;
            pen = new Pen(Color.Green);
            ImagePlaneDistance = _ImagePlaneDistance;
            SetAng(_rotation);
            position = _position;
            SelectedPointIcon = PointIcons[0];
            SelectedIconIndex = 0;
        }

        public Vector2 Eye2Screen(Vector3 relativePosition)
        {
            double outX, outY;

            outX = ImagePlaneDistance * relativePosition.X / relativePosition.Z + ImagePlanePos.X;

            outY = ImagePlaneDistance * relativePosition.Y / relativePosition.Z + ImagePlanePos.Y;

            return new Vector2(outX, outY);
        }

        public void DrawSceneObject(SceneObject sceneObj, Graphics graphicsObj)
        {
            if (sceneObj is Point)
            {
                Point point = (Point)sceneObj;
                Point eyePoint;

                //Translate the Worldspace coordinates to eyespace
                World2Eye(point, out eyePoint);

                //Check if the point is between the two clipping planes
                if (Eye2Clipped(eyePoint))
                {
                    Vector2 pos = Eye2Screen(eyePoint.position);
                    //graphicsObj.FillRectangle(brush, (int)pos.X, (int)pos.Y, 5, 5);
                    graphicsObj.DrawImageUnscaled(SelectedPointIcon, new Rectangle((int)pos.X, (int)pos.Y, 5, 5));
                }
            }
            else if (sceneObj is Line)
            {
                Line line = (Line)sceneObj;
                Line eyeLine;

                //Translate the Worldspace coordinates to eyespace
                World2Eye(line, out eyeLine);

                //Clip the line if nessecary
                if (Eye2Clipped(eyeLine))
                {
                    Vector2 pos1 = Eye2Screen(eyeLine.point1);
                    Vector2 pos2 = Eye2Screen(eyeLine.point2);
                    graphicsObj.DrawLine(pen, (float)pos1.X, (float)pos1.Y, (float)pos2.X, (float)pos2.Y);
                }

            }
            else if (sceneObj is Tri)
            {
                Tri tri = (Tri)sceneObj;
                Tri eyeTri;

                World2Eye(tri, out eyeTri);

                if (Eye2Clipped(eyeTri, out Shape3D shape3D))
                {
                    List<PointF> points2D = new List<PointF>();
                    foreach (Vertex item in shape3D.Vertices)
                    {
                        Vector2 v = Eye2Screen(item.position);
                        points2D.Add(new PointF((float)v.X, (float)v.Y));
                    }
                    PointF[] points2DArr = points2D.ToArray();
                    graphicsObj.FillPolygon(Brushes.Azure, points2DArr);
                }
            }
        }

        private void World2Eye(Point point, out Point eyePoint)
        {
            eyePoint = new Point();
            eyePoint.position = VecWorld2Eye(point.position);
        }

        private void World2Eye(Line line, out Line eyeLine)
        {
            eyeLine = new Line();
            eyeLine.point1 = VecWorld2Eye(line.point1);
            eyeLine.point2 = VecWorld2Eye(line.point2);
        }

        private void World2Eye(Vertex vertex, out Vertex eyeVert)
        {
            eyeVert = new Vertex();
            eyeVert.position = VecWorld2Eye(vertex.position);
        }

        private void World2Eye(Tri tri, out Tri eyeTri)
        {
            eyeTri = new Tri();
            for (int i = 0; i < 2; i++)
            {
                eyeTri.Vertices[i].position = VecWorld2Eye(tri.Vertices[i].position);
            }
        }

        private Vector3 VecWorld2Eye(Vector3 world)
        {
            Vector3 delta = world - position;

            double x, y, z;

            x = costheta.Y * (sintheta.Z * delta.Y + costheta.Z * delta.X) - sintheta.Y * delta.Z;
            y = sintheta.X * (costheta.Y * delta.Z + sintheta.Y * (sintheta.Z * delta.Y + costheta.Z * delta.X)) + costheta.X * (costheta.Z * delta.Y - sintheta.Z * delta.X);
            z = costheta.X * (costheta.Y * delta.Z + sintheta.Y * (sintheta.Z * delta.Y + costheta.Z * delta.X)) - sintheta.X * (costheta.Z * delta.Y - sintheta.Z * delta.X);

            return new Vector3(x, y, z);
        }

        private bool Eye2Clipped(Point point)
        {
            if (zNear < point.position.Z && point.position.Z < zFar)
            {
                return true;
            }
            return false;
        }

        private bool Eye2Clipped(Vertex vertex)
        {
            if (zNear < vertex.position.Z && vertex.position.Z < zFar)
            {
                return true;
            }
            return false;
        }

        private bool Eye2Clipped(Tri tri, out Shape3D ClippedShape)
        {
            ClippedShape = new Tri();

            byte ClippedVertsFront = 0;
            byte ClippedVertsBack = 0;
            if (tri.Vertices[0].position.Z < zNear)
            {
                ClippedVertsFront |= 1;
            }
            if (tri.Vertices[1].position.Z < zNear)
            {
                ClippedVertsFront |= 2;
            }
            if (tri.Vertices[2].position.Z < zNear)
            {
                ClippedVertsFront |= 4;
            }


            if (tri.Vertices[0].position.Z > zFar)
            {
                ClippedVertsBack |= 1;
            }
            if (tri.Vertices[1].position.Z > zFar)
            {
                ClippedVertsBack |= 2;
            }
            if (tri.Vertices[2].position.Z > zFar)
            {
                ClippedVertsBack |= 4;
            }
            #region old stuff
            /*
            //are all vertices in front of the front clipping plane?
            if (ClippedVertsFront == 7)
            {
                return false;
            }

            //are all vertices behind the back clipping plane?
            if (ClippedVertsBack == 7)
            {
                return false;
            }

            //is exactly one vertex in front of the front clipping plane?
            if (ClippedVertsFront == 1 || ClippedVertsFront == 2 || ClippedVertsFront == 4)
            {
                ClippedShape = new Quad();

                if (ClippedVertsFront == 1)
                {
                    double factor1 = (ClippingPlaneFront - tri.Vertices[0].position.Z) / (tri.Vertices[2].position.Z - tri.Vertices[0].position.Z);
                    double factor2 = (ClippingPlaneFront - tri.Vertices[0].position.Z) / (tri.Vertices[1].position.Z - tri.Vertices[0].position.Z);
                    ClippedShape.Vertices = new Vertex[]
                    {
                        tri.Vertices[1],
                        tri.Vertices[2],
                        new Vertex(new Vector3(
                            tri.Vertices[2].position.X + factor1 * (tri.Vertices[0].position.X - tri.Vertices[2].position.X),
                            tri.Vertices[2].position.Y + factor1 * (tri.Vertices[0].position.Y - tri.Vertices[2].position.Y),
                            ClippingPlaneFront)),
                        new Vertex(new Vector3(
                            tri.Vertices[1].position.X + factor2 * (tri.Vertices[0].position.X - tri.Vertices[1].position.X),
                            tri.Vertices[1].position.Y + factor2 * (tri.Vertices[0].position.Y - tri.Vertices[1].position.Y),
                            ClippingPlaneFront))
                    };
                }
                else if (ClippedVertsFront == 2)
                {
                    double factor1 = (ClippingPlaneFront - tri.Vertices[1].position.Z) / (tri.Vertices[2].position.Z - tri.Vertices[1].position.Z);
                    double factor2 = (ClippingPlaneFront - tri.Vertices[1].position.Z) / (tri.Vertices[0].position.Z - tri.Vertices[1].position.Z);
                    ClippedShape.Vertices = new Vertex[]
                    {
                        tri.Vertices[0],
                        tri.Vertices[2],
                        new Vertex(new Vector3(
                            tri.Vertices[2].position.X + factor1 * (tri.Vertices[1].position.X - tri.Vertices[2].position.X),
                            tri.Vertices[2].position.Y + factor1 * (tri.Vertices[1].position.Y - tri.Vertices[2].position.Y),
                            ClippingPlaneFront)),
                        new Vertex(new Vector3(
                            tri.Vertices[0].position.X + factor2 * (tri.Vertices[1].position.X - tri.Vertices[0].position.X),
                            tri.Vertices[0].position.Y + factor2 * (tri.Vertices[1].position.Y - tri.Vertices[0].position.Y),
                            ClippingPlaneFront))
                    };
                }
                else if (ClippedVertsFront == 4)
                {
                    double factor1 = (ClippingPlaneFront - tri.Vertices[2].position.Z) / (tri.Vertices[0].position.Z - tri.Vertices[2].position.Z);
                    double factor2 = (ClippingPlaneFront - tri.Vertices[2].position.Z) / (tri.Vertices[1].position.Z - tri.Vertices[2].position.Z);
                    ClippedShape.Vertices = new Vertex[]
                    {
                        tri.Vertices[0],
                        tri.Vertices[1],
                        new Vertex(new Vector3(
                            tri.Vertices[1].position.X + factor2 * (tri.Vertices[2].position.X - tri.Vertices[1].position.X),
                            tri.Vertices[1].position.Y + factor2 * (tri.Vertices[2].position.Y - tri.Vertices[1].position.Y),
                            ClippingPlaneFront)),
                        new Vertex(new Vector3(
                            tri.Vertices[0].position.X + factor1 * (tri.Vertices[2].position.X - tri.Vertices[0].position.X),
                            tri.Vertices[0].position.Y + factor1 * (tri.Vertices[2].position.Y - tri.Vertices[0].position.Y),
                            ClippingPlaneFront))
                    };
                }
            }
            */
            #endregion

            double factor1, factor2;
            switch (ClippedVertsFront)
            {
                case 1://only vert #0 is clipped
                    ClippedShape = new Quad();
                    factor1 = (zNear - tri.Vertices[0].position.Z) / (tri.Vertices[2].position.Z - tri.Vertices[0].position.Z);
                    factor2 = (zNear - tri.Vertices[0].position.Z) / (tri.Vertices[1].position.Z - tri.Vertices[0].position.Z);
                    ClippedShape.Vertices = new Vertex[]
                    {
                        tri.Vertices[1],
                        tri.Vertices[2],
                        new Vertex(new Vector3(
                            tri.Vertices[2].position.X + factor1 * (tri.Vertices[0].position.X - tri.Vertices[2].position.X),
                            tri.Vertices[2].position.Y + factor1 * (tri.Vertices[0].position.Y - tri.Vertices[2].position.Y),
                            zNear)),
                        new Vertex(new Vector3(
                            tri.Vertices[1].position.X + factor2 * (tri.Vertices[0].position.X - tri.Vertices[1].position.X),
                            tri.Vertices[1].position.Y + factor2 * (tri.Vertices[0].position.Y - tri.Vertices[1].position.Y),
                            zNear))
                    };
                    break;
                case 2://only vert #1 is clipped
                    ClippedShape = new Quad();
                    factor1 = (zNear - tri.Vertices[1].position.Z) / (tri.Vertices[2].position.Z - tri.Vertices[1].position.Z);
                    factor2 = (zNear - tri.Vertices[1].position.Z) / (tri.Vertices[0].position.Z - tri.Vertices[1].position.Z);
                    ClippedShape.Vertices = new Vertex[]
                    {
                        tri.Vertices[0],
                        new Vertex(new Vector3(
                            tri.Vertices[2].position.X + factor1 * (tri.Vertices[1].position.X - tri.Vertices[2].position.X),
                            tri.Vertices[2].position.Y + factor1 * (tri.Vertices[1].position.Y - tri.Vertices[2].position.Y),
                            zNear)),
                        new Vertex(new Vector3(
                            tri.Vertices[0].position.X + factor2 * (tri.Vertices[1].position.X - tri.Vertices[0].position.X),
                            tri.Vertices[0].position.Y + factor2 * (tri.Vertices[1].position.Y - tri.Vertices[0].position.Y),
                            zNear)),
                        tri.Vertices[2]
                    };
                    break;
                case 4://only vert #2 is clipped
                    ClippedShape = new Quad();
                    factor1 = (zNear - tri.Vertices[2].position.Z) / (tri.Vertices[0].position.Z - tri.Vertices[2].position.Z);
                    factor2 = (zNear - tri.Vertices[2].position.Z) / (tri.Vertices[1].position.Z - tri.Vertices[2].position.Z);
                    ClippedShape.Vertices = new Vertex[]
                    {
                        tri.Vertices[0],
                        tri.Vertices[1],
                        new Vertex(new Vector3(
                            tri.Vertices[1].position.X + factor2 * (tri.Vertices[2].position.X - tri.Vertices[1].position.X),
                            tri.Vertices[1].position.Y + factor2 * (tri.Vertices[2].position.Y - tri.Vertices[1].position.Y),
                            zNear)),
                        new Vertex(new Vector3(
                            tri.Vertices[0].position.X + factor1 * (tri.Vertices[2].position.X - tri.Vertices[0].position.X),
                            tri.Vertices[0].position.Y + factor1 * (tri.Vertices[2].position.Y - tri.Vertices[0].position.Y),
                            zNear))
                    };
                    break;
                case 3://verts #0 and #1 are clipped
                    factor1 = (zNear - tri.Vertices[0].position.Z) / (tri.Vertices[2].position.Z - tri.Vertices[0].position.Z);
                    factor2 = (zNear - tri.Vertices[1].position.Z) / (tri.Vertices[2].position.Z - tri.Vertices[1].position.Z);
                    ClippedShape.Vertices = new Vertex[]
                    {
                        new Vertex(new Vector3()),
                        new Vertex(new Vector3()),
                        tri.Vertices[2]
                    };
                    break;
                case 5://verts #0 and #2 are clipped
                    break;
                case 6://verts #1 and #2 are clipped
                    break;
                case 7://all verts are clipped
                    return false;
                default: throw new Exception();
            }

            return true;
        }

        private bool Eye2Clipped(Line line)
        {
            double factor;

            //are both ends of the line in front of the front clipping plane?
            if (line.point1.Z <= zNear && line.point2.Z <= zNear)
            {
                return false;
            }

            //are both ends of the line behind the back clipping plane?
            if (line.point1.Z >= zFar && line.point2.Z >= zFar)
            {
                return false;
            }

            //is one end of the line in front of the front clipping plane?
            if (line.point1.Z < zNear || line.point2.Z < zNear)
            {
                factor = (zNear - line.point1.Z) / (line.point2.Z - line.point1.Z);

                if (line.point1.Z < zNear)
                {
                    line.point1.X = line.point1.X + factor * (line.point2.X - line.point1.X);

                    line.point1.Y = line.point1.Y + factor * (line.point2.Y - line.point1.Y);

                    line.point1.Z = zNear;
                }
                else
                {
                    line.point2.X = line.point1.X + factor * (line.point2.X - line.point1.X);

                    line.point2.Y = line.point1.Y + factor * (line.point2.Y - line.point1.Y);

                    line.point2.Z = zNear;
                }
            }

            //is one end of the line behind the back clipping plane?
            if ((line.point1.Z > zFar) || (line.point2.Z > zFar))
            {
                factor = (zFar - line.point1.Z) / (line.point2.Z - line.point1.Z);

                if (line.point1.Z > zFar)
                {
                    line.point1.X = line.point1.X + factor * (line.point2.X - line.point1.X);

                    line.point1.Y = line.point1.Y + factor * (line.point2.Y - line.point1.Y);

                    line.point1.Z = zFar;
                }
                else
                {
                    line.point2.X = line.point1.X + factor * (line.point2.X - line.point1.X);

                    line.point2.Y = line.point1.Y + factor * (line.point2.Y - line.point1.Y);

                    line.point2.Z = zFar;
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

        public void MoveLocal(Vector3 direction)
        {
            Vector3 worldDirection = new Vector3(
                costheta.Y * direction.X + sintheta.Y * direction.Z,
                direction.Y,
                costheta.Y * direction.Z - sintheta.Y * direction.X
            );
            position += worldDirection;
        }

        public void Rotate(Vector3 rotation)
        {
            SetAng(theta + rotation);
        }

        public void ChangeImagePlaneDistance(double amount)
        {
            ImagePlaneDistance += amount;
        }

        public void NextDotIcon()
        {
            SelectedIconIndex++;
            SelectedIconIndex %= 4;
            SelectedPointIcon = PointIcons[SelectedIconIndex];
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

        public static Vector2 operator +(Vector2 a, Vector2 b)
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
            MainCamera = new Camera(Width, Height, new Vector3(), new Vector3(), 600);
        }

        public void AddSceneObject(SceneObject sceneObject)
        {
            sceneObjects.Add(sceneObject);
        }

        public void DrawToGraphicsObj(Graphics graphicsObj)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            foreach (SceneObject target in sceneObjects)
            {
                MainCamera.DrawSceneObject(target, graphicsObj);
            }
            timer.Stop();
            Font font = new Font(FontFamily.GenericMonospace, 10f);
            graphicsObj.DrawString(MainCamera.theta.Repr(), font, Brushes.Green, 0, 0);
            graphicsObj.DrawString(MainCamera.position.Repr(), font, Brushes.Green, 0, 20);
            graphicsObj.DrawString(timer.ElapsedMilliseconds.ToString(), font, Brushes.Green, 0, 40);
            graphicsObj.DrawString(Keyboard.IsKeyDown(Key.T)? "yes":"no", font, Brushes.Green, 0, 60);
        }

        public void MoveScene(Vector3 direction)
        {
            foreach (SceneObject item in sceneObjects)
            {
                item.Move(direction);
            }
        }

        public void ClearScene()
        {
            sceneObjects.Clear();
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

    class Vertex : SceneObject
    {
        public Vector3 position;
        public Vertex(Vector3 _position)
        {
            position = _position;
        }
        public Vertex(double x, double y, double z)
        {
            position = new Vector3(x, y, z);
        }

        public Vertex()
        {
            position = new Vector3();
        }

        public override void Move(Vector3 direction)
        {
            position += direction;
        }
    }

    class Tri : Shape3D
    {
        public Tri()
        {
            Vertices = new Vertex[3] {new Vertex(), new Vertex(), new Vertex()};
        }
        public Tri(Vertex[] verts)
        {
            Vertices = verts;
        }
        public override void Move(Vector3 direction)
        {
            for (int i = 0; i < 2; i++)
            {
                Vertices[i].position += direction;
            }
        }
    }

    class Quad : Shape3D
    {
        public Quad()
        {
            Vertices = new Vertex[3];
        }
        public override void Move(Vector3 direction)
        {
            for (int i = 0; i < 3; i++)
            {
                Vertices[i].position += direction;
            }
        }
    }

    class Quint : Shape3D
    {
        public Quint()
        {
            Vertices = new Vertex[4];
        }
        public override void Move(Vector3 direction)
        {
            for (int i = 0; i < 4; i++)
            {
                Vertices[i].position += direction;
            }
        }
    }

    abstract class Shape3D : SceneObject
    {
        public Vertex[] Vertices;
    }
}
