using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphicsObj = e.Graphics;

            graphicsObj.DrawImage(myBitmap, 0, 0, myBitmap.Width, myBitmap.Height);

            graphicsObj.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Graphics graphicsObj;
            myBitmap = new Bitmap(ClientRectangle.Width,
               ClientRectangle.Height,
               System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            graphicsObj = Graphics.FromImage(myBitmap);
            /*
            Pen myPen = new Pen(Color.Plum, 3);
            Rectangle rectangleObj = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            graphicsObj.DrawEllipse(myPen, rectangleObj);
            */
            Scene = new Scene(ClientRectangle.Width, ClientRectangle.Height);
            /*
            Scene.AddSceneObject(new Point(new Vector3(0, 0, 0)));
            Scene.AddSceneObject(new Point(new Vector3(0, 0, 100)));
            Scene.AddSceneObject(new Point(new Vector3(0, 100, 0)));
            Scene.AddSceneObject(new Point(new Vector3(0, 100, 100)));
            Scene.AddSceneObject(new Point(new Vector3(100, 0, 0)));
            Scene.AddSceneObject(new Point(new Vector3(100, 0, 100)));
            Scene.AddSceneObject(new Point(new Vector3(100, 100, 0)));
            Scene.AddSceneObject(new Point(new Vector3(100, 100, 100)));
            */
            
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
            
            
            Scene.MoveScene(new Vector3(0, 0, 50));

            Scene.DrawToGraphicsObj(graphicsObj);
            graphicsObj.Dispose();

            Console.WriteLine($"Screen Dimensions: {ClientRectangle.Width}, {ClientRectangle.Height}");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys pressedKey = e.KeyCode;

            if (pressedKey == Keys.Right)
            {
                Scene.MoveScene(new Vector3(-1, 0, 0));
            }
            else if (pressedKey == Keys.Left)
            {
                Scene.MoveScene(new Vector3(1, 0, 0));
            }
            else if (pressedKey == Keys.Up)
            {
                Scene.MoveScene(new Vector3(0, 1, 0));
            }
            else if (pressedKey == Keys.Down)
            {
                Scene.MoveScene(new Vector3(0, -1, 0));
            }
            else if (pressedKey == Keys.Add)
            {
                Scene.MoveScene(new Vector3(0, 0, 1));
            }
            else if (pressedKey == Keys.Subtract)
            {
                Scene.MoveScene(new Vector3(0, 0, -1));
            }


            UpdateImage();
        }

        public void UpdateImage()
        {
            Graphics graphicsObj = Graphics.FromImage(myBitmap);

            graphicsObj.Clear(Color.Black);

            Scene.DrawToGraphicsObj(graphicsObj);

            Refresh();
        }
    }
}
