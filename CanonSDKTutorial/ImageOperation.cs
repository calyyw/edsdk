using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CanonSDKTutorial
{
    /// <summary>
    /// 图片操作类
    /// </summary>
    public class ImageOperation
    {
        private static Color m_BackgroundColor;
        public static int SolubilityThreshold = 100000;//容差值

        /// <summary>
        /// 剪裁 -- 用GDI+ 
        /// </summary>
        /// <param name="b">原始Bitmap</param>
        /// <param name="StartX">开始坐标X</param>
        /// <param name="StartY">开始坐标Y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>剪裁后的Bitmap</returns>
        public static Bitmap Cut(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///  Resize图片 
        /// </summary>
        /// <param name="bmp">原始Bitmap </param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <param name="Mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH, int Mode)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 处理背景的方法
        /// </summary>
        /// <param name="bmp">图片的bitmap对象</param>
        /// <returns></returns>
        public static Bitmap TransparentBackground(Bitmap bmp)
        {
            if (bmp == null)
                return null;
            //根据传入的图片，获得图片的宽和高
            Int32 width = bmp.Width;
            Int32 height = bmp.Height;
            //重新生成新的一张图片
            Bitmap result = new Bitmap(width, height);
            //设置左上角的颜色为背景色
            m_BackgroundColor = bmp.GetPixel(0, 0);
            //遍历每一个像素，来设置背景色，为透明的
            for (Int32 y = 0; y < height; y++)
                for (Int32 x = 0; x < width; x++)
                {
                    //如果是背景色，则设置为透明的//或者白色，不是背景色，则从原图中拿出之前的颜色
                    if (TestIfColorBelongsToBackground(bmp, x, y))
                        result.SetPixel(x, y, Color.White);
                    else
                        result.SetPixel(x, y, bmp.GetPixel(x, y));
                }
            return result;
        }        

        /// <summary>
        /// 这是需要优化的方法 目前边境的人将包含 背景色
        /// This is the method need to be optimized
        /// Currently the border of people will contains
        /// the background color
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static Boolean TestIfColorBelongsToBackground(Bitmap bmp, Int32 x, Int32 y)
        {
            if (bmp == null)
                return false;

            Int32 width = bmp.Width;
            Int32 height = bmp.Height;

            Color cp = bmp.GetPixel(x, y);
            Color cp0;
            Color cp1;
            Color cp2;
            Color cp3;
            Color cp4;
            Color cp5;
            Color cp6;
            Color cp7;

            int cpValue = cp.ToArgb();


            int back = m_BackgroundColor.ToArgb();

            int maxBg = back + SolubilityThreshold;
            int minBg = back - SolubilityThreshold;
            int minCp0;
            int maxCp0;
            int minCp1;
            int maxCp1;
            int minCp2;
            int maxCp2;
            int minCp3;
            int maxCp3;
            int minCp4;
            int maxCp4;
            int minCp5;
            int maxCp5;
            int minCp6;
            int maxCp6;
            int minCp7;
            int maxCp7;




            // p0  p1  p2
            // p3  p   p4
            // p5  p6  p7
            // cp0 = bmp.GetPixel(x - 1, y - 1);
            // cp1 = bmp.GetPixel(x,     y - 1);
            // cp2 = bmp.GetPixel(x + 1, y - 1);
            // cp3 = bmp.GetPixel(x - 1, y);
            // cp4 = bmp.GetPixel(x + 1, y);
            // cp5 = bmp.GetPixel(x - 1, y + 1);
            // cp6 = bmp.GetPixel(x,     y + 1);
            // cp7 = bmp.GetPixel(x + 1, y + 1);
            //当前点的位置，如果是
            if (x == 0 && y == 0)
            {
                cp4 = bmp.GetPixel(x + 1, y);
                cp6 = bmp.GetPixel(x, y + 1);
                cp7 = bmp.GetPixel(x + 1, y + 1);

                int cp4Value = cp4.ToArgb();
                int cp6Value = cp6.ToArgb();
                int cp7Value = cp7.ToArgb();
                minCp4 = cp4Value - SolubilityThreshold;
                maxCp4 = cp4Value + SolubilityThreshold;
                minCp6 = cp6Value - SolubilityThreshold;
                maxCp6 = cp6Value + SolubilityThreshold;
                minCp7 = cp7Value - SolubilityThreshold;
                maxCp7 = cp7Value + SolubilityThreshold;
                return ((cpValue <= maxCp4 && cpValue >= minCp4) && (cpValue <= maxCp6 && cpValue >= minCp6) && (cpValue <= maxCp7 && cpValue >= minCp7) && (cpValue <= maxBg && cpValue >= minBg));
            }
            else if (x == 0 && y == height - 1)
            {
                cp1 = bmp.GetPixel(x, y - 1);
                cp2 = bmp.GetPixel(x + 1, y - 1);
                cp4 = bmp.GetPixel(x + 1, y);

                int cp1Value = cp1.ToArgb();
                int cp2Value = cp2.ToArgb();
                int cp4Value = cp4.ToArgb();

                minCp1 = cp1Value - SolubilityThreshold;
                maxCp1 = cp1Value + SolubilityThreshold;
                minCp2 = cp2Value - SolubilityThreshold;
                maxCp2 = cp2Value + SolubilityThreshold;
                minCp4 = cp4Value - SolubilityThreshold;
                maxCp4 = cp4Value + SolubilityThreshold;

                return ((cpValue <= maxCp1 && cpValue >= minCp1) && (cpValue <= maxCp2 && cpValue >= minCp2) && (cpValue <= maxCp4 && cpValue >= minCp4) && (cpValue <= maxBg && cpValue >= minBg));
            }
            else if (x == width - 1 && y == 0)
            {
                cp3 = bmp.GetPixel(x - 1, y);
                cp5 = bmp.GetPixel(x - 1, y + 1);
                cp6 = bmp.GetPixel(x, y + 1);
                int cp3Value = cp3.ToArgb();
                int cp5Value = cp5.ToArgb();
                int cp6Value = cp6.ToArgb();

                minCp3 = cp3Value - SolubilityThreshold;
                maxCp3 = cp3Value + SolubilityThreshold;
                minCp5 = cp5Value - SolubilityThreshold;
                maxCp5 = cp5Value + SolubilityThreshold;
                minCp6 = cp6Value - SolubilityThreshold;
                maxCp6 = cp6Value + SolubilityThreshold;
                return ((cpValue <= maxCp3 && cpValue >= minCp3) && (cpValue <= maxCp5 && cpValue >= minCp5) && (cpValue <= maxCp6 && cpValue >= minCp6) && (cpValue <= maxBg && cpValue >= minBg));
            }
            else if (x == width - 1 && y == height - 1)
            {
                cp0 = bmp.GetPixel(x - 1, y - 1);
                cp1 = bmp.GetPixel(x, y - 1);
                cp3 = bmp.GetPixel(x - 1, y);
                int cp0Value = cp0.ToArgb();
                int cp1Value = cp1.ToArgb();
                int cp3Value = cp3.ToArgb();

                minCp0 = cp0Value - SolubilityThreshold;
                maxCp0 = cp0Value + SolubilityThreshold;
                minCp1 = cp1Value - SolubilityThreshold;
                maxCp1 = cp1Value + SolubilityThreshold;
                minCp3 = cp3Value - SolubilityThreshold;
                maxCp3 = cp3Value + SolubilityThreshold;
                return ((cpValue <= maxCp0 && cpValue >= minCp0) && (cpValue <= maxCp1 && cpValue >= minCp1) && (cpValue <= maxCp3 && cpValue >= minCp3) && (cpValue <= maxBg && cpValue >= minBg));
            }
            else if (x != 0 && x != width - 1 && y == 0)
            {
                cp3 = bmp.GetPixel(x - 1, y);
                cp4 = bmp.GetPixel(x + 1, y);
                cp5 = bmp.GetPixel(x - 1, y + 1);
                cp6 = bmp.GetPixel(x, y + 1);
                cp7 = bmp.GetPixel(x + 1, y + 1);

                int cp3Value = cp3.ToArgb();
                int cp4Value = cp4.ToArgb();
                int cp5Value = cp5.ToArgb();
                int cp6Value = cp6.ToArgb();
                int cp7Value = cp7.ToArgb();

                minCp3 = cp3Value - SolubilityThreshold;
                maxCp3 = cp3Value + SolubilityThreshold;
                minCp4 = cp4Value - SolubilityThreshold;
                maxCp4 = cp4Value + SolubilityThreshold;
                minCp5 = cp5Value - SolubilityThreshold;
                maxCp5 = cp5Value + SolubilityThreshold;
                minCp6 = cp6Value - SolubilityThreshold;
                maxCp6 = cp6Value + SolubilityThreshold;
                minCp7 = cp7Value - SolubilityThreshold;
                maxCp7 = cp7Value + SolubilityThreshold;
                return ((cpValue <= maxCp3 && cpValue >= minCp3) && (cpValue <= maxCp4 && cpValue >= minCp4) && (cpValue <= maxCp5 && cpValue >= minCp5) && (cpValue <= maxCp6 && cpValue >= minCp6) && (cpValue <= maxCp7 && cpValue >= minCp7) && (cpValue <= maxBg && cpValue >= minBg));
            }
            else if (x != 0 && x != width - 1 && y == height - 1)
            {
                cp0 = bmp.GetPixel(x - 1, y - 1);
                cp1 = bmp.GetPixel(x, y - 1);
                cp2 = bmp.GetPixel(x + 1, y - 1);
                cp3 = bmp.GetPixel(x - 1, y);
                cp4 = bmp.GetPixel(x + 1, y);

                int cp0Value = cp0.ToArgb();
                int cp1Value = cp1.ToArgb();
                int cp2Value = cp2.ToArgb();
                int cp3Value = cp3.ToArgb();
                int cp4Value = cp4.ToArgb();

                minCp0 = cp0Value - SolubilityThreshold;
                maxCp0 = cp0Value + SolubilityThreshold;
                minCp1 = cp1Value - SolubilityThreshold;
                maxCp1 = cp1Value + SolubilityThreshold;
                minCp2 = cp2Value - SolubilityThreshold;
                maxCp2 = cp2Value + SolubilityThreshold;
                minCp3 = cp3Value - SolubilityThreshold;
                maxCp3 = cp3Value + SolubilityThreshold;
                minCp4 = cp4Value - SolubilityThreshold;
                maxCp4 = cp4Value + SolubilityThreshold;

                return ((cpValue <= maxCp0 && cpValue >= minCp0) && (cpValue <= maxCp1 && cpValue >= minCp1) && (cpValue <= maxCp2 && cpValue >= minCp2) && (cpValue <= maxCp3 && cpValue >= minCp3) && (cpValue <= maxCp4 && cpValue >= minCp4) && (cpValue <= maxBg && cpValue >= minBg));
            }
            else if (x == 0 && y != 0 && y != height - 1)
            {
                cp1 = bmp.GetPixel(x, y - 1);
                cp2 = bmp.GetPixel(x + 1, y - 1);
                cp4 = bmp.GetPixel(x + 1, y);
                cp6 = bmp.GetPixel(x, y + 1);
                cp7 = bmp.GetPixel(x + 1, y + 1);


                int cp1Value = cp1.ToArgb();
                int cp2Value = cp2.ToArgb();
                int cp4Value = cp4.ToArgb();
                int cp6Value = cp6.ToArgb();
                int cp7Value = cp7.ToArgb();

                minCp1 = cp1Value - SolubilityThreshold;
                maxCp1 = cp1Value + SolubilityThreshold;
                minCp2 = cp2Value - SolubilityThreshold;
                maxCp2 = cp2Value + SolubilityThreshold;
                minCp4 = cp4Value - SolubilityThreshold;
                maxCp4 = cp4Value + SolubilityThreshold;
                minCp6 = cp6Value - SolubilityThreshold;
                maxCp6 = cp6Value + SolubilityThreshold;
                minCp7 = cp7Value - SolubilityThreshold;
                maxCp7 = cp7Value + SolubilityThreshold;

                return ((cpValue <= maxCp1 && cpValue >= minCp1) && (cpValue <= maxCp2 && cpValue >= minCp2) && (cpValue <= maxCp4 && cpValue >= minCp4) && (cpValue <= maxCp6 && cpValue >= minCp6) && (cpValue <= maxCp7 && cpValue >= minCp7) && (cpValue <= maxBg && cpValue >= minBg));
            }
            else if (x == width - 1 && y != 0 && y != height - 1)
            {
                cp0 = bmp.GetPixel(x - 1, y - 1);
                cp1 = bmp.GetPixel(x, y - 1);
                cp3 = bmp.GetPixel(x - 1, y);
                cp5 = bmp.GetPixel(x - 1, y + 1);
                cp6 = bmp.GetPixel(x, y + 1);

                int cp0Value = cp0.ToArgb();
                int cp1Value = cp1.ToArgb();
                int cp3Value = cp3.ToArgb();
                int cp5Value = cp5.ToArgb();
                int cp6Value = cp6.ToArgb();

                minCp0 = cp0Value - SolubilityThreshold;
                maxCp0 = cp0Value + SolubilityThreshold;
                minCp1 = cp1Value - SolubilityThreshold;
                maxCp1 = cp1Value + SolubilityThreshold;
                minCp3 = cp3Value - SolubilityThreshold;
                maxCp3 = cp3Value + SolubilityThreshold;
                minCp5 = cp5Value - SolubilityThreshold;
                maxCp5 = cp5Value + SolubilityThreshold;
                minCp6 = cp6Value - SolubilityThreshold;
                maxCp6 = cp6Value + SolubilityThreshold;
                return ((cpValue <= maxCp0 && cpValue >= minCp0) && (cpValue <= maxCp1 && cpValue >= minCp1) && (cpValue <= maxCp3 && cpValue >= minCp3) && (cpValue <= maxCp5 && cpValue >= minCp5) && (cpValue <= maxCp6 && cpValue >= minCp6) && (cpValue <= maxBg && cpValue >= minBg));
            }
            else
            {
                cp0 = bmp.GetPixel(x - 1, y - 1);
                cp1 = bmp.GetPixel(x, y - 1);
                cp2 = bmp.GetPixel(x + 1, y - 1);
                cp3 = bmp.GetPixel(x - 1, y);
                cp4 = bmp.GetPixel(x + 1, y);
                cp5 = bmp.GetPixel(x - 1, y + 1);
                cp6 = bmp.GetPixel(x, y + 1);
                cp7 = bmp.GetPixel(x + 1, y + 1);


                int cp0Value = cp0.ToArgb();
                int cp1Value = cp1.ToArgb();
                int cp2Value = cp2.ToArgb();
                int cp3Value = cp3.ToArgb();
                int cp4Value = cp4.ToArgb();
                int cp5Value = cp5.ToArgb();
                int cp6Value = cp6.ToArgb();
                int cp7Value = cp7.ToArgb();

                minCp0 = cp0Value - SolubilityThreshold;
                maxCp0 = cp0Value + SolubilityThreshold;
                minCp1 = cp1Value - SolubilityThreshold;
                maxCp1 = cp1Value + SolubilityThreshold;
                minCp2 = cp2Value - SolubilityThreshold;
                maxCp2 = cp2Value + SolubilityThreshold;
                minCp3 = cp3Value - SolubilityThreshold;
                maxCp3 = cp3Value + SolubilityThreshold;
                minCp4 = cp4Value - SolubilityThreshold;
                maxCp4 = cp4Value + SolubilityThreshold;
                minCp5 = cp5Value - SolubilityThreshold;
                maxCp5 = cp5Value + SolubilityThreshold;
                minCp6 = cp6Value - SolubilityThreshold;
                maxCp6 = cp6Value + SolubilityThreshold;
                minCp7 = cp7Value - SolubilityThreshold;
                maxCp7 = cp7Value + SolubilityThreshold;

                return ((cpValue <= maxCp0 && cpValue >= minCp0) && (cpValue <= maxCp1 && cpValue >= minCp1) && (cpValue <= maxCp2 && cpValue >= minCp2) && (cpValue <= maxCp3 && cpValue >= minCp3) && (cpValue <= maxCp4 && cpValue >= minCp4) && (cpValue <= maxCp5 && cpValue >= minCp5) && (cpValue <= maxCp6 && cpValue >= minCp6) && (cpValue <= maxCp7 && cpValue >= minCp7) && (cpValue <= maxBg && cpValue >= minBg));
            }
        }


        #region 变量定义

        //灰度等级
        static byte greyLevel;

        //不透明
        static bool opaque = true;
        //static bool opaque = false;

        //不透明度阈值
        static int OpacityThreshold = 235;

        //反向蒙版
        //static bool invertedMask = false;
        static bool invertedMask = true;

        //加载的图片
        //Bitmap loadedImage;

        //原图
        //Bitmap originalMaskImage;

        //蒙版图片
        static Bitmap maskImage;

        //利用蒙版处理过的程序
       public  static Bitmap maskedImage;

# endregion

        /// <summary>
        /// 创建32位图，并清空Alpha信息
        /// </summary>
        /// <param name="tmpImage"></param>
        /// <returns></returns>
        private static Bitmap Create32bppImageAndClearAlpha(Bitmap tmpImage)
        {
            // 声明将返回的新图像的功能
            Bitmap returnedImage = new Bitmap(tmpImage.Width, tmpImage.Height, PixelFormat.Format32bppArgb);

            // 创建一个图形实例来绘制新的图像
            Rectangle rect = new Rectangle(0, 0, tmpImage.Width, tmpImage.Height);
            Graphics g = Graphics.FromImage(returnedImage);


            // 创建一个图像attribe
            ImageAttributes imageAttributes = new ImageAttributes();

            float[][] colorMatrixElements = {
                        new float[] {1,0,0,0,0},
                        new float[] {0,1,0,0,0},
                        new float[] {0,0,1,0,0},
                        new float[] {0,0,0,0,0},
                        new float[] {0,0,0,0,1}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);


            // 绘制原始图像
            g.DrawImage(tmpImage, rect, 0, 0, tmpImage.Width, tmpImage.Height, GraphicsUnit.Pixel, imageAttributes);
            g.Dispose();
            return returnedImage;
        }

        /// <summary>
        /// 处理图片蒙版
        /// </summary>
        public static void PrepareMaskImage(Bitmap originalMaskImage, Bitmap loadedImage)
        {
            //int OpacityThreshold=x;

            //如果图像蒙版不为空
            if (originalMaskImage != null)
            {
                //设置光标为等待样式
                //this.Cursor = Cursors.WaitCursor;

                //创建蒙版图片
                maskImage = Create32bppImageAndClearAlpha(originalMaskImage);
                //锁住图片
                BitmapData bmpData = maskImage.LockBits(new Rectangle(0, 0, maskImage.Width, maskImage.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, maskImage.PixelFormat);
                
                byte[] maskImageRGBData = new byte[bmpData.Stride * bmpData.Height];

                //将bmpData复制到maskImageRGBData中
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, maskImageRGBData, 0, maskImageRGBData.Length);

                //opaque = this.checkBoxAllowPartialOpacity.Checked == false;
                //OpacityThreshold = this.trackBar1.Value;
                //invertedMask = this.checkBoxInvertMask.Checked;

                //i+2与i += 4
                for (int i = 0; i < maskImageRGBData.Length; i += 4)
                {
                    //convert to gray scale R:0.30 G:0.59 B:0.11
                    greyLevel = (byte)(0.3 * maskImageRGBData[i + 2] + 0.59 * maskImageRGBData[i + 1] + 0.11 * maskImageRGBData[i]);

                    //通明处理,0或255
                    if (opaque)
                    {
                        greyLevel = (greyLevel < OpacityThreshold) ? byte.MinValue : byte.MaxValue;
                    }

                    //反向母板
                    if (invertedMask)
                    {
                        greyLevel = (byte)(255 - (int)greyLevel);
                    }


                    //灌回数据
                    maskImageRGBData[i] = greyLevel;
                    maskImageRGBData[i + 1] = greyLevel;
                    maskImageRGBData[i + 2] = greyLevel;

                }
                System.Runtime.InteropServices.Marshal.Copy(maskImageRGBData, 0, bmpData.Scan0, maskImageRGBData.Length);
                maskImage.UnlockBits(bmpData);
                //this.pictureBox2.Image = maskImage;
                //this.Cursor = Cursors.Default;
                // 如果加载的图像是可用的，我们有一切来计算的蒙面图像
                //if (this.loadedImage != null)

                //处理加载图片
                if (loadedImage != null)
                {
                    PrepareMaskedImage( loadedImage);
                }
            }
        }

        /// <summary>
        /// 利用蒙版处理图片
        /// </summary>
        public static void PrepareMaskedImage(Bitmap loadedImage)
        {
            //设置光标为等待状态
            //this.Cursor = Cursors.WaitCursor;
            //判断加载的图片和蒙版图片都不为空才继续操作
            if (loadedImage != null && maskImage != null)
            {
                //如果蒙版的宽和需要处理图片的款都不一致，则报错
                if (loadedImage.Width !=maskImage.Width || loadedImage.Height != maskImage.Height)
                {
                    //MessageBox.Show("错误:蒙版和图像必须具有相同的尺寸", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //this.pictureBox3.Image = null;
                }
                else
                {

                    //创建透明头像，根据加载的图像
                    maskedImage = Create32bppImageAndClearAlpha(loadedImage);


                    //透明头像需要处理的数据
                    BitmapData bmpData1 = maskedImage.LockBits(new Rectangle(0, 0, maskedImage.Width, maskedImage.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, maskedImage.PixelFormat);                  
                    byte[] maskedImageRGBAData = new byte[bmpData1.Stride * bmpData1.Height];
                    //加锁图片
                    System.Runtime.InteropServices.Marshal.Copy(bmpData1.Scan0, maskedImageRGBAData, 0, maskedImageRGBAData.Length);


                    //处理蒙版数据
                    BitmapData bmpData2 = maskImage.LockBits(new Rectangle(0, 0, maskImage.Width, maskImage.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, maskImage.PixelFormat);
                    byte[] maskImageRGBAData = new byte[bmpData2.Stride * bmpData2.Height];
                    //赋值数据
                    System.Runtime.InteropServices.Marshal.Copy(bmpData2.Scan0, maskImageRGBAData, 0, maskImageRGBAData.Length);

                    //Alpha层
                    for (int i = 0; i < maskedImageRGBAData.Length; i += 4)
                    {
                        maskedImageRGBAData[i + 3] = maskImageRGBAData[i];
                    }
                    System.Runtime.InteropServices.Marshal.Copy(maskedImageRGBAData, 0, bmpData1.Scan0, maskedImageRGBAData.Length);
                    maskedImage.UnlockBits(bmpData1);
                    maskImage.UnlockBits(bmpData2);

                    //this.pictureBox3.Image = maskedImage;
                }
                //this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// RGBA转换为RGB
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <returns>位图</returns>
        public static Bitmap RGBA2RGB(Bitmap bitmap)
        {
            // 声明将返回的新图像的功能
            Bitmap returnedImage = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);

            int width = bitmap.Width;
            int height = bitmap.Height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color Source = bitmap.GetPixel(i, j);
                    Color Target = Source;
                    //通过蒙版来处理，自动抠图，如果阿尔法层为黑色的话，则认为是背景，设置为白色背景
                    if (Source.A == 0)
                    {
                        Target = Color.White;
                    }
                    returnedImage.SetPixel(i, j, Target);
                }
            }
            return returnedImage;
        }
        



        /// <summary>
        /// 创建32位图，并清空Alpha信息
        /// </summary>
        /// <param name="tmpImage"></param>
        /// <returns></returns>
        public static Bitmap Create32bppImageAndCreateAlpha(Bitmap tmpImage)
        {

            // 声明将返回的新图像的功能
            Bitmap returnedImage = new Bitmap(tmpImage.Width, tmpImage.Height, PixelFormat.Format32bppArgb);

            // 创建一个图形实例来绘制新的图像
            Rectangle rect = new Rectangle(0, 0, tmpImage.Width, tmpImage.Height);
            Graphics g = Graphics.FromImage(returnedImage);


            // 创建一个图像attribe
            ImageAttributes imageAttributes = new ImageAttributes();

            float[][] colorMatrixElements = {
                        new float[] {1,0,0,0.30F,0},
                        new float[] {0,1,0,0.59F,0},
                        new float[] {0,0,1,0.11F,0},
                        new float[] {0,0,0,0,0},
                        new float[] {0,0,0,0,1}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);


            // 绘制原始图像
            g.DrawImage(tmpImage, rect, 0, 0, tmpImage.Width, tmpImage.Height, GraphicsUnit.Pixel, imageAttributes);
            g.Dispose();
            return returnedImage;
        }

        /// <summary>
        /// RGBA转换为RGB
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <returns>位图</returns>
        public static Bitmap RGBATORGB(Bitmap bitmap_in,int x)
        {
            Bitmap bitmap = Create32bppImageAndCreateAlpha(bitmap_in);
            int myOpacityThreshold = x;
            // 声明将返回的新图像的功能
            Bitmap returnedImage = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);

            int width = bitmap.Width;
            int height = bitmap.Height;
            int Y;
            int Cr;
            int Cb;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color Source = bitmap.GetPixel(i, j);
                    //Color Target = Source;

                    //通明处理,0或255
                    if (opaque)
                    {
                        //肤色检测,Cr(V)反映了RGB输入信号红色部分与RGB信号亮度值之间的差异。而Cb(U)反映的是RGB输入信号蓝色部分与RGB信号亮度值之间的差异
                        //Y = (byte)((Source.B * 1868 + Source.G * 9617 + Source.R * 4899 + 8192) / 1.0 * 16384);

                        Y = (byte)( Source.R * 0.299+ Source.G * 0.587 +Source.B * 0.114 );
                        Cb = (byte)(-Source.R * 0.169 - Source.G * 0.331 + Source.B * 0.500) + 128;
                        Cr = (byte)(Source.R*0.500-Source.G * 0.419 -Source.B * 0.081)+128 ;

                        //%公式 Y = 0.2990*R + 0.5780*G + 0.1140*B + 0 
                        //%公式 Cr = 0.5000*R - 0.4187*G - 0.0813*B + 128 
                        //%公式 Cb = -0.1687*R - 0.3313*G + 0.5000*B + 128

                        if (Source.A > myOpacityThreshold)
                        {
                            //肤色监测
                            //if( !(Cr >= 130 && Cr <= 183 ))
                            //if (!(Cr >= 133 && Cr <= 173 && Cb >= 77 && Cb <= 127))
                            {
                                Source = Color.White;
                            }
                        }                           


                        //Target.A = (Target.A < OpacityThreshold) ? byte.MinValue : byte.MaxValue;                        
                    
                    }
                    returnedImage.SetPixel(i, j, Source);


                    //反向母板
                    //if (invertedMask)
                    //{
                    //    greyLevel = (byte)(255 - (int)greyLevel);
                    //}

                    //Color Source = bitmap.GetPixel(i, j);
                    //Color Target = Source;
                    //通过蒙版来处理，自动抠图，如果阿尔法层为黑色的话，则认为是背景，设置为白色背景
                    //if (Source.A == 0)
                    //{
                    //    Target = Color.White;
                    //}
                    //returnedImage.SetPixel(i, j, Target);

                }
            }
            return returnedImage;
        }

        #region getThumImage
        /**/
        /// <summary>  
        /// 生成缩略图  
        /// </summary>  
        /// <param name="sourceFile">原始图片文件</param>  
        /// <param name="quality">质量压缩比</param>  
        /// <param name="multiple">收缩倍数</param>  
        /// <param name="outputFile">输出文件名</param>  
        /// <returns>成功返回true,失败则返回false</returns>  
        public static bool getThumImage(Bitmap sourceImage, long quality, String outputFile)
        {
            try
            {
                //质量比例
                long imageQuality = quality;

                //前文件
                //Bitmap sourceImage = new Bitmap(sourceFile);

                //文件格式
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");


                //
                
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, imageQuality);

                myEncoderParameters.Param[0] = myEncoderParameter;


                float xWidth = sourceImage.Width;
                float yWidth = sourceImage.Height;
                Bitmap newImage = new Bitmap((int)(xWidth / 1), (int)(yWidth / 1));


                Graphics g = Graphics.FromImage(newImage);

                g.DrawImage(sourceImage, 0, 0, xWidth / 1, yWidth / 1);
                g.Dispose();

                //输出路径，文件格式，文件编码
                newImage.Save(outputFile, myImageCodecInfo, myEncoderParameters);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion getThumImage  


        /**/
        /// <summary>  
        /// 获取图片编码信息  
        /// </summary>  
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }  
    }
}
