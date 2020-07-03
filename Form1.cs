using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
namespace SetColor_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region  参数配置
        float[] maxx;//存储最大值最小值
        float[] maxy;
        float[] maxz;
        private double _ZBxT = 0;
        private double _ZByT = 0;
        private double _ZBzT = 0;
        private double _ZBv = 10;
        public int xds = 0;
        public int sum = 0;
        private double _ZBx = 0;
        private double _ZBy = 0;
        private double _ZBz = -30;
        /// <summary>
        /// 默认绘画模式为线条
        /// </summary>      
        /// <summary>
        /// X轴坐标
        /// </summary>
        private float _x = 0;
        /// <summary>
        /// Y轴坐标
        /// </summary>
        private float _y = 0;
        /// <summary>
        /// Z轴坐标
        /// </summary>
        private float _z = 0;
        private Boolean clicKtf = false;  //旋转
        private Boolean Drawxs = false;   //平移        
        private Boolean Zcdk = false;   //平移
        public float[] pointX;
        public float[] pointY;
        public float[] pointZ;
        public double[] KpointX;
        public double[] KpointY;
        public string[] KpointZ;
        float ddx, ddy;
        private int clickX;
        private int clickY;
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            double tempx = 0;
            double tempy = 0;
            double tempz = 0;
            //listBox1.Items.Clear();
            Drawxs = false;
            Zcdk = true;//再次读取
                        // CCValue.thStart = true;
            this.openFileDialog1.ShowDialog();
            string MyFileName = openFileDialog1.FileName;
            if (MyFileName.Trim() == "")
                return;
            StreamReader MyReader = null;
            try
            {
                MyReader = new StreamReader(MyFileName, System.Text.Encoding.Default);

                string content = MyReader.ReadToEnd();
                string[] str = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                char[] splitchar = new Char[] { ',' };//拆分数组
                string[] data1;
                maxx = new float[str.Length];
                maxy = new float[str.Length];
                maxz = new float[str.Length];

                for (int i = 0; i < str.Length - 1; i++)
                {
                    data1 = str[i].Split(splitchar);
                    maxx[i] = float.Parse(data1[0]);
                    maxy[i] = float.Parse(data1[1]);
                    maxz[i] = float.Parse(data1[2]);
                    // listBox1.Items.Add(str[i].ToString());
                    //this.richTextBox1.Text = this.richTextBox1.Text + pointX[i] + "," + pointY[i] + "," + pointZ[i] + "\r\n";// content;
                    tempx += maxx[i];
                    tempy += maxy[i];
                    tempz += maxz[i];
                }
                xds = str.Length;
                _ZBxT = tempx / xds;
                _ZByT = tempy / xds;
                _ZBzT = tempz / xds;
            }
            catch (Exception Err)
            {
                MessageBox.Show("读文本文件发生错误！请检查源文件是否是文本文件？" + Err.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            GC.Collect();
        }
        SharpGL.OpenGL gl;
        private void openGLControl1_MouseDown(object sender, MouseEventArgs e)
        {
            clicKtf = true;

            clickX = e.X;
            clickY = e.Y;
            GC.Collect();
        }

        private void openGLControl1_MouseUp(object sender, MouseEventArgs e)
        {
            clicKtf = false;
            ddx = _y;
            ddy = _x;
            GC.Collect();
        }

        private void openGLControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (clicKtf)
            {
                _y = ddx + e.X - clickX;
                _x = ddy + e.Y - clickY;
            }
            GC.Collect();
        }
        private void openGLControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                //_ZBx += 1;
                _ZBv -= 1;
            }
            else
            {
                //_ZBx -= 1;
                _ZBv += 1;
            }
            GC.Collect();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.openGLControl1.MouseWheel += new MouseEventHandler(openGLControl1_MouseWheel);
        }

        float Z_color;

        private void button2_Click(object sender, EventArgs e)
        {
            Z_color = (float)(300.5 / 1000);
            textBox1.Text = Z_color.ToString();
        }

        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            gl = this.openGLControl1.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);    // Clear The Screen And The Depth Buffer

            gl.LoadIdentity();					// Reset The View
            //gl.Translate(-1.5f, 0.0f, -6.0f);				// Move Left And Into The Screen
            gl.Translate(_ZBx, _ZBy, _ZBz); // 设置坐标，距离屏幕距离为6
                                            // gl.Rotate(rtri, 0.0f, 1.0f, 0.0f);              // Rotate The Pyramid On It's Y Axis
            gl.Rotate(_x, 1.0f, 0.0f, 0.0f);	// 绕X轴旋转
            gl.Rotate(_y, 0.0f, 1.0f, 0.0f);	// 绕Y轴旋转
            gl.Rotate(_z, 0.0f, 0.0f, 1.0f);
            #region 原始代码
            //gl.Begin(OpenGL.TRIANGLES);					// Start Drawing The Pyramid

            //gl.Color(1.0f, 0.0f, 0.0f);			// Red
            //gl.Vertex(0.0f, 1.0f, 0.0f);			// Top Of Triangle (Front)
            //gl.Color(0.0f, 1.0f, 0.0f);			// Green
            //gl.Vertex(-1.0f, -1.0f, 1.0f);			// Left Of Triangle (Front)
            //gl.Color(0.0f, 0.0f, 1.0f);			// Blue
            //gl.Vertex(1.0f, -1.0f, 1.0f);			// Right Of Triangle (Front)

            //gl.Color(1.0f, 0.0f, 0.0f);			// Red
            //gl.Vertex(0.0f, 1.0f, 0.0f);			// Top Of Triangle (Right)
            //gl.Color(0.0f, 0.0f, 1.0f);			// Blue
            //gl.Vertex(1.0f, -1.0f, 1.0f);			// Left Of Triangle (Right)
            //gl.Color(0.0f, 1.0f, 0.0f);			// Green
            //gl.Vertex(1.0f, -1.0f, -1.0f);			// Right Of Triangle (Right)

            //gl.Color(1.0f, 0.0f, 0.0f);			// Red
            //gl.Vertex(0.0f, 1.0f, 0.0f);			// Top Of Triangle (Back)
            //gl.Color(0.0f, 1.0f, 0.0f);			// Green
            //gl.Vertex(1.0f, -1.0f, -1.0f);			// Left Of Triangle (Back)
            //gl.Color(0.0f, 0.0f, 1.0f);			// Blue
            //gl.Vertex(-1.0f, -1.0f, -1.0f);			// Right Of Triangle (Back)

            //gl.Color(1.0f, 0.0f, 0.0f);			// Red
            //gl.Vertex(0.0f, 1.0f, 0.0f);			// Top Of Triangle (Left)
            //gl.Color(0.0f, 0.0f, 1.0f);			// Blue
            //gl.Vertex(-1.0f, -1.0f, -1.0f);			// Left Of Triangle (Left)
            //gl.Color(0.0f, 1.0f, 0.0f);			// Green
            //gl.Vertex(-1.0f, -1.0f, 1.0f);			// Right Of Triangle (Left)
            //gl.End();						// Done Drawing The Pyramid

            //gl.LoadIdentity();
            //gl.Translate(1.5f, 0.0f, -7.0f);				// Move Right And Into The Screen

            //gl.Rotate(rquad, 1.0f, 1.0f, 1.0f);			// Rotate The Cube On X, Y & Z

            //gl.Begin(OpenGL.QUADS);					// Start Drawing The Cube

            //gl.Color(0.0f, 1.0f, 0.0f);			// Set The Color To Green
            //gl.Vertex(1.0f, 1.0f, -1.0f);			// Top Right Of The Quad (Top)
            //gl.Vertex(-1.0f, 1.0f, -1.0f);			// Top Left Of The Quad (Top)
            //gl.Vertex(-1.0f, 1.0f, 1.0f);			// Bottom Left Of The Quad (Top)
            //gl.Vertex(1.0f, 1.0f, 1.0f);			// Bottom Right Of The Quad (Top)


            //gl.Color(1.0f, 0.5f, 0.0f);			// Set The Color To Orange
            //gl.Vertex(1.0f, -1.0f, 1.0f);			// Top Right Of The Quad (Bottom)
            //gl.Vertex(-1.0f, -1.0f, 1.0f);			// Top Left Of The Quad (Bottom)
            //gl.Vertex(-1.0f, -1.0f, -1.0f);			// Bottom Left Of The Quad (Bottom)
            //gl.Vertex(1.0f, -1.0f, -1.0f);			// Bottom Right Of The Quad (Bottom)

            //gl.Color(1.0f, 0.0f, 0.0f);			// Set The Color To Red
            //gl.Vertex(1.0f, 1.0f, 1.0f);			// Top Right Of The Quad (Front)
            //gl.Vertex(-1.0f, 1.0f, 1.0f);			// Top Left Of The Quad (Front)
            //gl.Vertex(-1.0f, -1.0f, 1.0f);			// Bottom Left Of The Quad (Front)
            //gl.Vertex(1.0f, -1.0f, 1.0f);			// Bottom Right Of The Quad (Front)

            //gl.Color(1.0f, 1.0f, 0.0f);			// Set The Color To Yellow
            //gl.Vertex(1.0f, -1.0f, -1.0f);			// Bottom Left Of The Quad (Back)
            //gl.Vertex(-1.0f, -1.0f, -1.0f);			// Bottom Right Of The Quad (Back)
            //gl.Vertex(-1.0f, 1.0f, -1.0f);			// Top Right Of The Quad (Back)
            //gl.Vertex(1.0f, 1.0f, -1.0f);			// Top Left Of The Quad (Back)

            //gl.Color(0.0f, 0.0f, 1.0f);			// Set The Color To Blue
            //gl.Vertex(-1.0f, 1.0f, 1.0f);			// Top Right Of The Quad (Left)
            //gl.Vertex(-1.0f, 1.0f, -1.0f);			// Top Left Of The Quad (Left)
            //gl.Vertex(-1.0f, -1.0f, -1.0f);			// Bottom Left Of The Quad (Left)
            //gl.Vertex(-1.0f, -1.0f, 1.0f);			// Bottom Right Of The Quad (Left)

            //gl.Color(1.0f, 0.0f, 1.0f);			// Set The Color To Violet
            //gl.Vertex(1.0f, 1.0f, -1.0f);			// Top Right Of The Quad (Right)
            //gl.Vertex(1.0f, 1.0f, 1.0f);			// Top Left Of The Quad (Right)
            //gl.Vertex(1.0f, -1.0f, 1.0f);			// Bottom Left Of The Quad (Right)
            //gl.Vertex(1.0f, -1.0f, -1.0f);			// Bottom Right Of The Quad (Right)
            //gl.End();                       // Done Drawing The Q




            //rtri += 1.0f;// 0.2f;						// Increase The Rotation Variable For The Triangle 
            //rquad -= 0.75f;//
            #endregion
            gl.Begin(OpenGL.GL_POINTS);
           // gl.Color(5.0f, 39.0f, 175.0f);
            if (!Drawxs)// 设置颜色
            {
                for (int i = 0; i < sum; i++)
                {

                    gl.Vertex((pointX[i] - _ZBxT) / _ZBv, (pointY[i] - _ZByT) / _ZBv, (pointZ[i] - _ZBzT) / _ZBv);
                    //gl.Vertex((pointZ[i] - _ZBzT) / _ZBv, (pointX[i] - _ZBxT) / _ZBv, (pointY[i] - _ZByT) / _ZBv);

                }
                if (Zcdk)
                {
                   // gl.Color(5.0f, 39.0f, 175.0f);
                    for (int i = 0; i < xds; i++)
                    {
                        Z_color = Math.Abs( maxz[i] / 1000);
                        if (Z_color > 3)
                        {
                            gl.Color(1.0f , 1.0f, 0.0f);
                            gl.Vertex((maxx[i] - _ZBxT) / _ZBv, (maxy[i] - _ZByT) / _ZBv, (maxz[i] - _ZBzT) / _ZBv);
                        }
                        else
                        {
                            gl.Color(0.2f , 1.0f , 0.0f );
                            gl.Vertex((maxx[i] - _ZBxT) / _ZBv, (maxy[i] - _ZByT) / _ZBv, (maxz[i] - _ZBzT) / _ZBv);
                        }
                       
                        //gl.Vertex((maxz[i] - _ZBzT + 0.01) / _ZBv, (maxx[i] - _ZBxT) / _ZBv, (maxy[i] - _ZByT) / _ZBv);

                    }
                }
            }
            //else
            //{
            //    for (int i = 0; i < cut; i++)
            //    {
            //        gl.Vertex((temx[bcsz[i]] - _ZBxT) / _ZBv, (miny[i] - _ZByT) / _ZBv, (minz[i] - _ZBzT) / _ZBv);
            //        gl.Vertex((temx[bcsz[i]] - _ZBxT) / _ZBv, (miny[i] - _ZByT) / _ZBv, (maxz[i] - _ZBzT) / _ZBv);
            //        gl.Vertex((temx[bcsz[i]] - _ZBxT) / _ZBv, (maxy[i] - _ZByT) / _ZBv, (minz[i] - _ZBzT) / _ZBv);
            //        gl.Vertex((temx[bcsz[i]] - _ZBxT) / _ZBv, (maxy[i] - _ZByT) / _ZBv, (maxz[i] - _ZBzT) / _ZBv);
            //    }
            //}
            gl.End();
            gl.Flush();
            GC.Collect();
        }
    }
}
