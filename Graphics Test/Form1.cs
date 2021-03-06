﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace Graphics_Test
{
    public partial class Form1 : Form
    {
        private Bitmap myBitmap;
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
            Scene = new Scene(ClientRectangle.Width, ClientRectangle.Height);

            const int SceneIndex = 4;

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

                Scene.MainCamera.SetPos(new Vector3(0, 0, -200));
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

                Scene.MainCamera.SetPos(new Vector3(0, 0, -200));
            }
            #endregion
            #region plotter test 3d sine
            else if (SceneIndex == 2)
            {
                const int dim = 50;
                const int halfdim = dim / 2;

                for (int i = -halfdim; i < halfdim; i++)
                {
                    for (int j = -halfdim; j < halfdim; j++)
                    {
                        //3d sine
                        Scene.AddSceneObject(new Point(new Vector3(i * 15, (Math.Sin(i) + Math.Sin(j)) * 7, j * 15)));
                    }
                }

                Scene.MainCamera.SetPos(new Vector3(0, -450, -520));
                Scene.MainCamera.SetAng(new Vector3(-0.75, 0, 0));
            }
            #endregion
            #region plotter test ripple
            else if (SceneIndex == 3)
            {
                const int dim = 50;
                const int halfdim = dim / 2;

                for (int i = -halfdim; i < halfdim; i++)
                {
                    for (int j = -halfdim; j < halfdim; j++)
                    {
                        //ripple
                        double distance = Math.Sqrt(i*i + j*j);
                        Scene.AddSceneObject(new Point(new Vector3(i * 15, -Math.Cos(distance) * 2500 / (distance* distance + 100), j * 15)));
                    }
                }

                Scene.MainCamera.SetPos(new Vector3(0, -450, -520));
                Scene.MainCamera.SetAng(new Vector3(-0.75, 0, 0));
            }
            #endregion
            #region plotter test curve
            else if (SceneIndex == 4)
            {
                const int dim = 50;
                const int halfdim = dim / 2;

                for (int i = -halfdim; i < halfdim; i++)
                {
                    for (int j = -halfdim; j < halfdim; j++)
                    {
                        Scene.AddSceneObject(new Point(new Vector3(i * 15, (j*j-i*i) >> 2 , j * 15)));
                    }
                }

                Scene.MainCamera.SetPos(new Vector3(0, -450, -520));
                Scene.MainCamera.SetAng(new Vector3(-0.75, 0, 0));
            }
            #endregion
            #region Tri test
            if (SceneIndex == 5)
            {
                Scene.AddSceneObject(new Tri(new Vertex[] {new Vertex(0,0,0), new Vertex(0,0,100), new Vertex(100,0,100)}));

                Scene.MainCamera.SetPos(new Vector3(0, 0, -200));
            }
            #endregion

            Scene.DrawToGraphicsObj(graphicsObj);
            graphicsObj.Dispose();

            Text = $"Resolution: {ClientRectangle.Width}, {ClientRectangle.Height}";
        }
        
        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keys pressedKey = e.KeyCode;
            
            if (pressedKey == Keys.F3)
            {
                Scene.MainCamera.NextDotIcon();
            }
        }
        
        public void UpdateImage()
        {
            Graphics graphicsObj = Graphics.FromImage(myBitmap);

            graphicsObj.Clear(Color.Black);

            ApplyUserInput();

            Scene.DrawToGraphicsObj(graphicsObj);

            Invalidate();

            graphicsObj.Dispose();
        }

        public void ApplyUserInput()
        {
            if (Keyboard.IsKeyDown(Key.W))
            {
                Scene.MainCamera.MoveLocal(new Vector3(0, 0, 5));
            }
            if (Keyboard.IsKeyDown(Key.S))
            {
                Scene.MainCamera.MoveLocal(new Vector3(0, 0, -5));
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                Scene.MainCamera.MoveLocal(new Vector3(-5, 0, 0));
            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                Scene.MainCamera.MoveLocal(new Vector3(5, 0, 0));
            }
            if (Keyboard.IsKeyDown(Key.Space))
            {
                Scene.MainCamera.MoveLocal(new Vector3(0, -5, 0));
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Scene.MainCamera.MoveLocal(new Vector3(0, 5, 0));
            }
            if (Keyboard.IsKeyDown(Key.F1))
            {
                Scene.MainCamera.ChangeImagePlaneDistance(5);
            }
            if (Keyboard.IsKeyDown(Key.F2))
            {
                Scene.MainCamera.ChangeImagePlaneDistance(-5);
            }
            if (Keyboard.IsKeyDown(Key.Up))
            {
                Scene.MainCamera.Rotate(new Vector3(0.03, 0, 0));
            }
            if (Keyboard.IsKeyDown(Key.Down))
            {
                Scene.MainCamera.Rotate(new Vector3(-0.03, 0, 0));
            }
            if (Keyboard.IsKeyDown(Key.Left))
            {
                Scene.MainCamera.Rotate(new Vector3(0, -0.03, 0));
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                Scene.MainCamera.Rotate(new Vector3(0, 0.03, 0));
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateImage();
        }
    }
}
