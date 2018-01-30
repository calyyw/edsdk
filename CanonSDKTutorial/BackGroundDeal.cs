using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace CanonSDKTutorial
{
    public class BackGroundDeal
    {

        //照片尺寸
        public static int PW = 358;//压缩宽
        public static int PH = 441;//压缩高


        //A或B区的边长
        public static int LEN = 10;

        public static void BaseColorExtraction()
        { 
            int [] r = new int [255];
            int[] g = new int[255];
            int[] b = new int[255];
        
        }


        /// <summary>
        /// 将证件照背景替换成纯白色,方案一
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap BackGroudTransfer(Bitmap bmp)
        {

            Bitmap a = new Bitmap(PW, PH); 
            Bitmap b = new Bitmap(PW, PH);

           

            int MaxR = 0;
            int MinR = 255;

            int MaxB = 0;
            int MinB = 255;

            int MaxG = 0;
            int MinG = 255;

            int Threshold_r;
            int Threshold_g;
            int Threshold_b ;

            int Threshold = 75;
            int Num = 5;

            

            Color BackgroundColor;

            //A区
            for (int j = 0; j < LEN; j++)
            {
                for (int i = 0; i < LEN - j; i++)
                {
                    BackgroundColor = bmp.GetPixel(i, j);

                    if (BackgroundColor.R > MaxR || BackgroundColor.R < MinR)
                    {
                        if (BackgroundColor.R > MaxR) MaxR = BackgroundColor.R;
                        if (BackgroundColor.R < MinR) MinR = BackgroundColor.R;
                    }

                    if (BackgroundColor.G > MaxG || BackgroundColor.G < MinG)
                    {
                        if (BackgroundColor.G > MaxG) MaxG = BackgroundColor.G;
                        if (BackgroundColor.G < MinG) MinG = BackgroundColor.G;
                    }

                    if (BackgroundColor.B > MaxB || BackgroundColor.B < MinB)
                    {
                        if (BackgroundColor.B > MaxB) MaxB = BackgroundColor.B;
                        if (BackgroundColor.B < MinB) MinB = BackgroundColor.B;
                    }                    
                }
            }

            //B区
            for (int j = 0; j < LEN; j++)
            {
                for (int i = PW - 1; i > PW-LEN + j+1; i--)
                {
                    BackgroundColor = bmp.GetPixel(i, j);

                    if (BackgroundColor.R > MaxR || BackgroundColor.R < MinR)
                    {
                        if (BackgroundColor.R > MaxR) MaxR = BackgroundColor.R;
                        if (BackgroundColor.R < MinR) MinR = BackgroundColor.R;
                    }

                    if (BackgroundColor.G > MaxG || BackgroundColor.G < MinG)
                    {
                        if (BackgroundColor.G > MaxG) MaxG = BackgroundColor.G;
                        if (BackgroundColor.G < MinG) MinG = BackgroundColor.G;
                    }

                    if (BackgroundColor.B > MaxB || BackgroundColor.B < MinB)
                    {
                        if (BackgroundColor.B > MaxB) MaxB = BackgroundColor.B;
                        if (BackgroundColor.B < MinB) MinB = BackgroundColor.B;
                    }
                }
            }


            Threshold_r=MaxR-MinR;
            Threshold_g=MaxG-MinG;
            Threshold_b=MaxB-MinB;



            for (int j = 0; j < PH; j++)
            {
                for (int i = 0; i < PW; i++)
                {
                    b.SetPixel(i, j, bmp.GetPixel(i, j)); 
                }            
            }
            //return b;

            for (int j = 0; j < PH; j++)
            {

                //int m = 0;
                for (int i = 0; i <= PW / 2; i++)
                {
                    BackgroundColor = bmp.GetPixel(i, j);

                    if (((BackgroundColor.R >= MinR - Threshold) && (BackgroundColor.R <= MaxR + Threshold)) && ((BackgroundColor.G >= MinG - Threshold) && (BackgroundColor.G <= MaxG + Threshold)) && ((BackgroundColor.B >= MinB - Threshold) && (BackgroundColor.B <= MaxB + Threshold)))
                    {
                        //if (BackgroundColor.R < MinR) MinR = BackgroundColor.R;

                        //if (BackgroundColor.R > MaxR) MaxR = BackgroundColor.R;

                        //Threshold_r = MaxR - MinR;


                        //if (BackgroundColor.G < MinG) MinG = BackgroundColor.G;

                        //if (BackgroundColor.G > MaxG) MaxG = BackgroundColor.G;

                        //Threshold_g = MaxG - MinG;


                        //if (BackgroundColor.B < MinB) MinB = BackgroundColor.B;

                        //if (BackgroundColor.B > MaxB) MaxB = BackgroundColor.B;

                        //Threshold_b = MaxB - MinB;


                        b.SetPixel(i, j, Color.White);

                        //m = 0;
                    }

                    else
                    {
                        bool Con = true;
                        for (int m = 1; m <= Num; m++)
                        {
                            Color BC = bmp.GetPixel(i + m, j);

                            //if (((BC.R >= MinR - Threshold_r / 2) && (BC.R <= MaxR + Threshold_r / 2)) && ((BC.G >= MinG - Threshold_g / 2) && (BC.G <= MaxG + Threshold_g / 2)) && ((BC.B >= MinB - Threshold_b / 2) && (BC.B <= MaxB + Threshold_b / 2)))

                            if (((BC.R >= MinR - Threshold) && (BC.R <= MaxR + Threshold)) && ((BC.G >= MinG - Threshold) && (BC.G <= MaxG + Threshold)) && ((BC.B >= MinB - Threshold) && (BC.B <= MaxB + Threshold)))
                            {
                                Con = false;
                                break;
                            }
                        }


                        if (Con)
                        {

                            //b.SetPixel(i, j, BackgroundColor);
                            i = PW / 2 + 1;
                        }
                        else
                        {
                            //不处理干扰点
                             b.SetPixel(i, j, Color.White);                        
                        }




                        //b.SetPixel(i, j, BackgroundColor);
                        //{i = PW / 2 + 1;}     

                        //m++;
                        //if(m>Num)
                        //{i = PW / 2 + 1;}                        
                    }
                }

                //int n = 0;
                for (int i = PW - 1; i >= PW / 2; i--)
                {
                    BackgroundColor = bmp.GetPixel(i, j);

                    //if (((BackgroundColor.R >= MinR - Threshold_r / 2) && (BackgroundColor.R <= MaxR + Threshold_r / 2)) && ((BackgroundColor.G >= MinG - Threshold_g / 2) && (BackgroundColor.G <= MaxG + Threshold_g / 2)) && ((BackgroundColor.B >= MinB - Threshold_b / 2) && (BackgroundColor.B <= MaxB + Threshold_b / 2)))

                    if (((BackgroundColor.R >= MinR - Threshold) && (BackgroundColor.R <= MaxR + Threshold)) && ((BackgroundColor.G >= MinG - Threshold) && (BackgroundColor.G <= MaxG + Threshold)) && ((BackgroundColor.B >= MinB - Threshold) && (BackgroundColor.B <= MaxB + Threshold)))
                    {
                        //if (BackgroundColor.R < MinR) MinR = BackgroundColor.R;

                        //if (BackgroundColor.R > MaxR) MaxR = BackgroundColor.R;

                        //Threshold_r = MaxR - MinR;


                        //if (BackgroundColor.G < MinG) MinG = BackgroundColor.G;

                        //if (BackgroundColor.G > MaxG) MaxG = BackgroundColor.G;

                        //Threshold_g = MaxG - MinG;


                        //if (BackgroundColor.B < MinB) MinB = BackgroundColor.B;

                        //if (BackgroundColor.B > MaxB) MaxB = BackgroundColor.B;

                        //Threshold_b = MaxB - MinB;


                        b.SetPixel(i, j, Color.White);
                        //m = 0;
                    }


                    else
                    {
                        bool Con = true;
                        for (int m = 1; m <= Num; m++)
                        {
                            Color BC = bmp.GetPixel(i - m, j);
                            if (((BC.R >= MinR - Threshold) && (BC.R <= MaxR + Threshold)) && ((BC.G >= MinG - Threshold) && (BC.G <= MaxG + Threshold)) && ((BC.B >= MinB - Threshold) && (BC.B <= MaxB + Threshold)))
                            {
                                Con = false;
                                break;
                            }

                        }

                        if (Con)
                        {
                            //b.SetPixel(i, j, BackgroundColor);
                            i = PW / 2 - 1;
                        }
                        else
                        {
                            //不处理
                             b.SetPixel(i, j, Color.White);
                        }
                        //b.SetPixel(i, j, BackgroundColor);
                        //i = PW / 2 -1;
                    }

                }
            }            
            return b;

        }
    }
}
