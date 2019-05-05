using System;
using System.Drawing;
using System.Windows.Forms;

namespace Graphics_Test
{
    public partial class Form1 : Form
    {
        private System.Drawing.Bitmap myBitmap;
        Scene Scene;
        public Form1()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(myBitmap, 0, 0, myBitmap.Width, myBitmap.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Graphics graphicsObj;
            myBitmap = new Bitmap(ClientRectangle.Width,
               ClientRectangle.Height,
               System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            graphicsObj = Graphics.FromImage(myBitmap);
            /*
            Pen myPen = new Pen(Color.Plum, 3);
            Rectangle rectangleObj = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            graphicsObj.DrawEllipse(myPen, rectangleObj);
            */
            Scene = new Scene(ClientRectangle.Width, ClientRectangle.Height);

            int SceneIndex = 3;

            #region point test
            if (SceneIndex == 0)
            {
                //Cube of points
                Scene.AddSceneObject(new Point(new Vector3(0, 0, 0)));
                Scene.AddSceneObject(new Point(new Vector3(0, 0, 100)));
                Scene.AddSceneObject(new Point(new Vector3(0, 100, 0)));
                Scene.AddSceneObject(new Point(new Vector3(0, 100, 100)));
                Scene.AddSceneObject(new Point(new Vector3(100, 0, 0)));
                Scene.AddSceneObject(new Point(new Vector3(100, 0, 100)));
                Scene.AddSceneObject(new Point(new Vector3(100, 100, 0)));
                Scene.AddSceneObject(new Point(new Vector3(100, 100, 100)));
            }
            #endregion
            #region line test
            else if (SceneIndex == 1)
            {
                //Cube of lines
                Scene.AddSceneObject(new Line(new Vector3(0, 0, 0), new Vector3(0, 0, 100)));
                Scene.AddSceneObject(new Line(new Vector3(0, 0, 0), new Vector3(0, 100, 0)));
                Scene.AddSceneObject(new Line(new Vector3(0, 0, 0), new Vector3(100, 0, 0)));
                Scene.AddSceneObject(new Line(new Vector3(100, 100, 0), new Vector3(100, 100, 100)));
                Scene.AddSceneObject(new Line(new Vector3(100, 100, 0), new Vector3(0, 100, 0)));
                Scene.AddSceneObject(new Line(new Vector3(100, 100, 0), new Vector3(100, 0, 0)));
                Scene.AddSceneObject(new Line(new Vector3(0, 100, 100), new Vector3(100, 100, 100)));
                Scene.AddSceneObject(new Line(new Vector3(0, 100, 100), new Vector3(0, 0, 100)));
                Scene.AddSceneObject(new Line(new Vector3(0, 100, 100), new Vector3(0, 100, 0)));
                Scene.AddSceneObject(new Line(new Vector3(100, 0, 100), new Vector3(0, 0, 100)));
                Scene.AddSceneObject(new Line(new Vector3(100, 0, 100), new Vector3(100, 100, 100)));
                Scene.AddSceneObject(new Line(new Vector3(100, 0, 100), new Vector3(100, 0, 0)));

                //House
                Scene.AddSceneObject(new Line(new Vector3(500, 0, 0), new Vector3(500, 0, 100)));
                Scene.AddSceneObject(new Line(new Vector3(500, 0, 0), new Vector3(500, 100, 0)));
                Scene.AddSceneObject(new Line(new Vector3(500, 0, 0), new Vector3(600, 0, 0)));
                Scene.AddSceneObject(new Line(new Vector3(600, 100, 0), new Vector3(600, 100, 100)));
                Scene.AddSceneObject(new Line(new Vector3(600, 100, 0), new Vector3(500, 100, 0)));
                Scene.AddSceneObject(new Line(new Vector3(600, 100, 0), new Vector3(600, 0, 0)));
                Scene.AddSceneObject(new Line(new Vector3(500, 100, 100), new Vector3(600, 100, 100)));
                Scene.AddSceneObject(new Line(new Vector3(500, 100, 100), new Vector3(500, 0, 100)));
                Scene.AddSceneObject(new Line(new Vector3(500, 100, 100), new Vector3(500, 100, 0)));
                Scene.AddSceneObject(new Line(new Vector3(600, 0, 100), new Vector3(500, 0, 100)));
                Scene.AddSceneObject(new Line(new Vector3(600, 0, 100), new Vector3(600, 100, 100)));
                Scene.AddSceneObject(new Line(new Vector3(600, 0, 100), new Vector3(600, 0, 0)));

                Scene.AddSceneObject(new Line(new Vector3(550, -50, 0), new Vector3(600, 0, 0)));
                Scene.AddSceneObject(new Line(new Vector3(550, -50, 0), new Vector3(500, 0, 0)));
                Scene.AddSceneObject(new Line(new Vector3(550, -50, 0), new Vector3(550, -50, 100)));
                Scene.AddSceneObject(new Line(new Vector3(550, -50, 100), new Vector3(500, 0, 100)));
                Scene.AddSceneObject(new Line(new Vector3(550, -50, 100), new Vector3(600, 0, 100)));

                //vertical lines
                Scene.AddSceneObject(new Line(new Vector3(0, 100, 0), new Vector3(0, 100, 300)));
                Scene.AddSceneObject(new Line(new Vector3(50, 100, 0), new Vector3(50, 100, 300)));
                Scene.AddSceneObject(new Line(new Vector3(100, 100, 0), new Vector3(100, 100, 300)));
                Scene.AddSceneObject(new Line(new Vector3(150, 100, 0), new Vector3(150, 100, 300)));
                Scene.AddSceneObject(new Line(new Vector3(200, 100, 0), new Vector3(200, 100, 300)));
                Scene.AddSceneObject(new Line(new Vector3(-50, 100, 0), new Vector3(-50, 100, 300)));
                Scene.AddSceneObject(new Line(new Vector3(-100, 100, 0), new Vector3(-100, 100, 300)));
                Scene.AddSceneObject(new Line(new Vector3(-150, 100, 0), new Vector3(-150, 100, 300)));
                Scene.AddSceneObject(new Line(new Vector3(-200, 100, 0), new Vector3(-200, 100, 300)));

                //horizontal lines
                Scene.AddSceneObject(new Line(new Vector3(-300, 100, 0), new Vector3(300, 100, 0)));
                Scene.AddSceneObject(new Line(new Vector3(-300, 100, 50), new Vector3(300, 100, 50)));
                Scene.AddSceneObject(new Line(new Vector3(-300, 100, 100), new Vector3(300, 100, 100)));
                Scene.AddSceneObject(new Line(new Vector3(-300, 100, 150), new Vector3(300, 100, 150)));
                Scene.AddSceneObject(new Line(new Vector3(-300, 100, 200), new Vector3(300, 100, 200)));

                //diagonals
                Scene.AddSceneObject(new Line(new Vector3(50, 100, 0), new Vector3(350, 100, 300)));
                Scene.AddSceneObject(new Line(new Vector3(50, 100, 0), new Vector3(-250, 100, 300)));
            }
            #endregion
            #region plotter test 3d sine
            else if (SceneIndex == 2)
            {
                const int dim = 25;

                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        //3d sine
                        Scene.AddSceneObject(new Point(new Vector3(i * 10, (Math.Sin(i) + Math.Sin(j)) * 3 + 100, j * 10)));
                    }
                }
            }
            #endregion
            #region plotter test ripple
            else if (SceneIndex == 3)
            {
                const int dim = 25;
                const int halfdim = dim / 2;

                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        //ripple
                        double distance = Math.Sqrt((i-halfdim)*(i-halfdim) + (j-halfdim)*(j-halfdim));
                        Scene.AddSceneObject(new Point(new Vector3(i * 15, -Math.Sin(distance)*60/distance, j * 15)));
                    }
                }
            }
            #endregion

            Scene.MainCamera.SetPos(new Vector3(50, 50, -200));

            Scene.DrawToGraphicsObj(graphicsObj);
            graphicsObj.Dispose();

            Text = $"Resolution: {ClientRectangle.Width}, {ClientRectangle.Height}";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys pressedKey = e.KeyCode;
            
            if (pressedKey == Keys.D)
            {
                Scene.MainCamera.MoveLocal(new Vector3(5, 0, 0));
            }
            else if (pressedKey == Keys.A)
            {
                Scene.MainCamera.MoveLocal(new Vector3(-5, 0, 0));
            }
            else if (pressedKey == Keys.Space)
            {
                Scene.MainCamera.MoveLocal(new Vector3(0, -5, 0));
            }
            else if (pressedKey == Keys.ControlKey)
            {
                Scene.MainCamera.MoveLocal(new Vector3(0, 5, 0));
            }
            else if (pressedKey == Keys.S)
            {
                Scene.MainCamera.MoveLocal(new Vector3(0, 0, -5));
            }
            else if (pressedKey == Keys.W)
            {
                Scene.MainCamera.MoveLocal(new Vector3(0, 0, 5));
            }
            else if (pressedKey == Keys.F1)
            {
                Scene.MainCamera.ChangeImagePlaneDistance(5);
            }
            else if (pressedKey == Keys.F2)
            {
                Scene.MainCamera.ChangeImagePlaneDistance(-5);
            }
            else if (pressedKey == Keys.Right)
            {
                Scene.MainCamera.Rotate(new Vector3(0, 0.03, 0));
            }
            else if (pressedKey == Keys.Left)
            {
                Scene.MainCamera.Rotate(new Vector3(0, -0.03, 0));
            }
            else if (pressedKey == Keys.Up)
            {
                Scene.MainCamera.Rotate(new Vector3(0.03, 0, 0));
            }
            else if (pressedKey == Keys.Down)
            {
                Scene.MainCamera.Rotate(new Vector3(-0.03, 0, 0));
            }
            else if (pressedKey == Keys.F3)
            {
                Scene.MainCamera.NextDotIcon();
            }
        }

        public void UpdateImage()
        {
            Graphics graphicsObj = Graphics.FromImage(myBitmap);

            graphicsObj.Clear(Color.Black);

            Scene.DrawToGraphicsObj(graphicsObj);

            Invalidate();

            graphicsObj.Dispose();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateImage();
        }
    }
}
