using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using EDSDKLib;
using System.Drawing.Imaging;

namespace CanonSDKTutorial
{


    /// <summary>
    /// SDK处理封装类
    /// </summary>
    public class SDKHandler : IDisposable
    {
        #region Variables

        /// <summary>
        /// 所使用的相机
        /// </summary>
        public Camera MainCamera { get; private set; }
        /// <summary>
        /// 是否是相机会话打开状态
        /// </summary>
        public bool CameraSessionOpen { get; private set; }
        /// <summary>
        /// 是否是实时取景状态
        /// </summary>
        public bool IsLiveViewOn { get; private set; }
        /// <summary>
        /// 是否是实时取景录像状态
        /// </summary>
        public bool IsEvfFilming { get; private set; }
        /// <summary>
        /// 目录下的照片将被保存
        /// </summary>
        public string ImageSaveDirectory { get; set; }
        /// <summary>
        /// 处理错误当SDK发生错误时
        /// </summary>
        /// 

        public string BaseImage;

        public uint Error 
        {
            get;
            set;
        }


        //public int x =10;     //裁剪框的横坐标
        //public int y =10;     //裁剪框的竖坐标

        ////相对容器的位置
        //public int x = 281;     //裁剪框的横坐标
        //public int y = 90;     //裁剪框的竖坐标




        //相对父控件的位置
        public int x = 271;     //裁剪框的横坐标
        public int y = 80;     //裁剪框的竖坐标


        //框的尺寸
        public int width =358;  //裁剪框的宽
        public int heigth =441; //裁剪框的高

        //父框的尺寸
        public int pwidth =900;  //裁剪框的宽
        public int pheigth =600; //裁剪框的高



        //1000D照片尺寸
        //public int W = 1936;     //原照片尺寸
        //public int H = 1288;     //原照片尺寸

        //700D相机照片尺寸
        public int W = 1920;     //原照片尺寸
        public int H = 1280;     //原照片尺寸

        //1000D最大尺寸
        //public int W = 3888;     //原照片尺寸
        //public int H = 2592;     //原照片尺寸
        


        public int PW = 358;//压缩宽
        public int PH = 441;//压缩高


        /// <summary>
        /// 实时录像缓冲队列
        /// </summary>
        private Queue<byte[]> FrameBuffer = new Queue<byte[]>(1000);
        /// <summary>
        ///在拍照时，实时取景必须停下来
        /// </summary>
        public bool PauseLiveView { get; set; }

        #endregion
        
        #region Events

        #region SDK Events

        public event EDSDK.EdsCameraAddedHandler SDKCameraAddedEvent;
        public event EDSDK.EdsObjectEventHandler SDKObjectEvent;
        public event EDSDK.EdsProgressCallback SDKProgressCallbackEvent;
        public event EDSDK.EdsPropertyEventHandler SDKPropertyEvent;
        public event EDSDK.EdsStateEventHandler SDKStateEvent;

        #endregion

        #region Custom Events

        public delegate void CameraAddedHandler();
        public delegate void ProgressHandler(int Progress);
        public delegate void ImageUpdate(Image img);
        public delegate void FloatUpdate(float Value);

        /// <summary>
        /// 如果增加了一个摄像头时触发
        /// </summary>
        public event CameraAddedHandler CameraAdded;
        /// <summary>
        /// 如果任何进程报告进展时触发
        /// </summary>
        public event ProgressHandler ProgressChanged;
        /// <summary>
        /// 如果实时取景图像更新时触发
        /// </summary>
        public event ImageUpdate LiveViewUpdated;
        /// <summary>
        /// 如果一个新的帧率计算时触发
        /// </summary>
        public event FloatUpdate FrameRateUpdated;

        #endregion

        #endregion


        #region Basic SDK and Session handling

        /// <summary>
        /// 初始化SDK和添加事件
        /// </summary>
        public SDKHandler()
        {
            //初始化佳能驱动
            Error = EDSDK.EdsInitializeSDK();

            SDKCameraAddedEvent += new EDSDK.EdsCameraAddedHandler(SDKHandler_CameraAddedEvent);
            //注册
            EDSDK.EdsSetCameraAddedHandler(SDKCameraAddedEvent, IntPtr.Zero);

            SDKStateEvent += new EDSDK.EdsStateEventHandler(Camera_SDKStateEvent);
            SDKPropertyEvent += new EDSDK.EdsPropertyEventHandler(Camera_SDKPropertyEvent);
            SDKProgressCallbackEvent += new EDSDK.EdsProgressCallback(Camera_SDKProgressCallbackEvent);            
            SDKObjectEvent += new EDSDK.EdsObjectEventHandler(Camera_SDKObjectEvent);



            /// <summary>
            /// 尺寸
            /// </summary>
            x = (int)(1.0 * x * W / pwidth);
            y = (int)(1.0 * y * H / pheigth);


            //确定截取点
            //CameraHandler.x = (int)(1.0 * control.Left * CameraHandler.W / parentWidth);
            //CameraHandler.y = (int)(1.0 * control.Top * CameraHandler.H / parentHeight);


            width = (int)(1.0 * width * W / pwidth);
            heigth = (int)(1.0 * heigth * H / pheigth);
            //截取的尺寸
            //CameraHandler.width = (int)(1.0 * controlWidth * CameraHandler.W / parentWidth);
            //CameraHandler.heigth = (int)(1.0 * controlHeight * CameraHandler.H / parentHeight);
        }


        /// <summary>
        /// 获取所有连接的相机
        /// </summary>
        /// <returns>返回相机集合</returns>
        public List<Camera> GetCameraList()
        {
            IntPtr camlist;
            //获取相机列表
            Error = EDSDK.EdsGetCameraList(out camlist);

            //获取每一个相机对象，从相机列表中获得
            int c;
            Error = EDSDK.EdsGetChildCount(camlist, out c);

            List<Camera> OutCamList = new List<Camera>();
            for (int i = 0; i < c; i++)
            {
                IntPtr cptr;
                Error = EDSDK.EdsGetChildAtIndex(camlist, i, out cptr);
                OutCamList.Add(new Camera(cptr));
            }
            return OutCamList;
        }

        /// <summary>
        /// 用给定的摄像机打开一个会话
        /// </summary>
        /// <param name="NewCamera">使用的相机</param>
        public void OpenSession(Camera NewCamera)
        {
            if (CameraSessionOpen) Error = EDSDK.EdsCloseSession(MainCamera.Ref);
            if (NewCamera != null)
            {
                MainCamera = NewCamera;
                Error = EDSDK.EdsOpenSession(MainCamera.Ref);


                //注册
                EDSDK.EdsSetCameraStateEventHandler(MainCamera.Ref, EDSDK.StateEvent_All, SDKStateEvent, IntPtr.Zero);

                //处理SDKObjectEvent，拍照时相应
                EDSDK.EdsSetObjectEventHandler(MainCamera.Ref, EDSDK.ObjectEvent_All, SDKObjectEvent, IntPtr.Zero);
                //注册
                EDSDK.EdsSetPropertyEventHandler(MainCamera.Ref, EDSDK.PropertyEvent_All, SDKPropertyEvent, IntPtr.Zero);
                CameraSessionOpen = true;
            }
        }

        /// <summary>
        /// 关闭当前摄像机的会话
        /// </summary>
        public void CloseSession()
        {
            if (CameraSessionOpen)
            {
                Error = EDSDK.EdsCloseSession(MainCamera.Ref);
                CameraSessionOpen = false;
            }
        }

        /// <summary>
        /// 关闭打开的会话终止的SDK
        /// </summary>
        public void Dispose()
        {
            if (CameraSessionOpen) Error = EDSDK.EdsCloseSession(MainCamera.Ref);           
            {
                //LV_Stop();
                
                //CameraSessionOpen = false;
                //Error = EDSDK.EdsTerminateSDK();
            }
        }

        #endregion

        #region Eventhandling

        /// <summary>
        /// 一个新的照相机被插入到计算机中
        /// </summary>
        /// <param name="inContext">添加相机的指针</param>
        /// <returns>一个edsdk错误代码</returns>
        private uint SDKHandler_CameraAddedEvent(IntPtr inContext)
        {
            //这里的新相机
            if (CameraAdded != null) CameraAdded();
            return EDSDK.EDS_ERR_OK;
        }

        /// <summary>
        /// 一个objectevent解雇
        /// </summary>
        /// <param name="inEvent">的objectevent ID</param>
        /// <param name="inRef">指向对象（拍照的图片）的指针</param>
        /// <param name="inContext"></param>
        /// <returns>一个edsdk错误代码</returns>
        private uint Camera_SDKObjectEvent(uint inEvent, IntPtr inRef, IntPtr inContext)
        {
            //处理对象事件
            switch (inEvent)
            {
                case EDSDK.ObjectEvent_All:
                    break;
                case EDSDK.ObjectEvent_DirItemCancelTransferDT:
                    break;
                case EDSDK.ObjectEvent_DirItemContentChanged:
                    break;
                case EDSDK.ObjectEvent_DirItemCreated:
                    //DownloadImage(inRef, ImageSaveDirectory);
                    break;
                case EDSDK.ObjectEvent_DirItemInfoChanged:
                    break;
                case EDSDK.ObjectEvent_DirItemRemoved:
                    break;
                case EDSDK.ObjectEvent_DirItemRequestTransfer: //触发条件，指针inRef指向照片
                    DownloadImage(inRef, ImageSaveDirectory);
                    break;
                case EDSDK.ObjectEvent_DirItemRequestTransferDT:
                    break;
                case EDSDK.ObjectEvent_FolderUpdateItems:
                    break;
                case EDSDK.ObjectEvent_VolumeAdded:
                    break;
                case EDSDK.ObjectEvent_VolumeInfoChanged:
                    break;
                case EDSDK.ObjectEvent_VolumeRemoved:
                    break;
                case EDSDK.ObjectEvent_VolumeUpdateItems:
                    break;
            }

            return EDSDK.EDS_ERR_OK;
        }

        /// <summary>
        /// 取得进展
        /// </summary>
        /// <param name="inPercent">进步百分比</param>
        /// <param name="inContext">...</param>
        /// <param name="outCancel">设置为“取消事件”</param>
        /// <returns>一个edsdk错误代码</returns>
        private uint Camera_SDKProgressCallbackEvent(uint inPercent, IntPtr inContext, ref bool outCancel)
        {
            //处理进展
            if (ProgressChanged != null) ProgressChanged((int)inPercent);
            return EDSDK.EDS_ERR_OK;
        }

        /// <summary>
        /// 属性改变
        /// </summary>
        /// <param name="inEvent">The PropetyEvent ID</param>
        /// <param name="inPropertyID">The Property ID</param>
        /// <param name="inParameter">Event Parameter</param>
        /// <param name="inContext">...</param>
        /// <returns>一个edsdk错误代码</returns>
        private uint Camera_SDKPropertyEvent(uint inEvent, uint inPropertyID, uint inParameter, IntPtr inContext)
        {
            //Handle property event here
            switch (inEvent)
            {
                case EDSDK.PropertyEvent_All:
                    break;
                case EDSDK.PropertyEvent_PropertyChanged:
                    break;
                case EDSDK.PropertyEvent_PropertyDescChanged:
                    break;
            }

            switch (inPropertyID)
            {
                case EDSDK.PropID_AEBracket:
                    break;
                case EDSDK.PropID_AEMode:
                    break;
                case EDSDK.PropID_AEModeSelect:
                    break;
                case EDSDK.PropID_AFMode:
                    break;
                case EDSDK.PropID_Artist:
                    break;
                case EDSDK.PropID_AtCapture_Flag:
                    break;
                case EDSDK.PropID_Av:
                    break;
                case EDSDK.PropID_AvailableShots:
                    break;
                case EDSDK.PropID_BatteryLevel:
                    break;
                case EDSDK.PropID_BatteryQuality:
                    break;
                case EDSDK.PropID_BodyIDEx:
                    break;
                case EDSDK.PropID_Bracket:
                    break;
                case EDSDK.PropID_CFn:
                    break;
                case EDSDK.PropID_ClickWBPoint:
                    break;
                case EDSDK.PropID_ColorMatrix:
                    break;
                case EDSDK.PropID_ColorSaturation:
                    break;
                case EDSDK.PropID_ColorSpace:
                    break;
                case EDSDK.PropID_ColorTemperature:
                    break;
                case EDSDK.PropID_ColorTone:
                    break;
                case EDSDK.PropID_Contrast:
                    break;
                case EDSDK.PropID_Copyright:
                    break;
                case EDSDK.PropID_DateTime:
                    break;
                case EDSDK.PropID_DepthOfField:
                    break;
                case EDSDK.PropID_DigitalExposure:
                    break;
                case EDSDK.PropID_DriveMode:
                    break;
                case EDSDK.PropID_EFCompensation:
                    break;
                case EDSDK.PropID_Evf_AFMode:
                    break;
                case EDSDK.PropID_Evf_ColorTemperature:
                    break;
                case EDSDK.PropID_Evf_DepthOfFieldPreview:
                    break;
                case EDSDK.PropID_Evf_FocusAid:
                    break;
                case EDSDK.PropID_Evf_Histogram:
                    break;
                case EDSDK.PropID_Evf_HistogramStatus:
                    break;
                case EDSDK.PropID_Evf_ImagePosition:
                    break;
                case EDSDK.PropID_Evf_Mode:
                    break;
                case EDSDK.PropID_Evf_OutputDevice:
                    if (IsEvfFilming == true) DownloadEvfFilm();//录像
                    else if (IsLiveViewOn == true) DownloadEvf();//实时预览
                    break;
                case EDSDK.PropID_Evf_WhiteBalance:
                    break;
                case EDSDK.PropID_Evf_Zoom:
                    break;
                case EDSDK.PropID_Evf_ZoomPosition:
                    break;
                case EDSDK.PropID_ExposureCompensation:
                    break;
                case EDSDK.PropID_FEBracket:
                    break;
                case EDSDK.PropID_FilterEffect:
                    break;
                case EDSDK.PropID_FirmwareVersion:
                    break;
                case EDSDK.PropID_FlashCompensation:
                    break;
                case EDSDK.PropID_FlashMode:
                    break;
                case EDSDK.PropID_FlashOn:
                    break;
                case EDSDK.PropID_FocalLength:
                    break;
                case EDSDK.PropID_FocusInfo:
                    break;
                case EDSDK.PropID_GPSAltitude:
                    break;
                case EDSDK.PropID_GPSAltitudeRef:
                    break;
                case EDSDK.PropID_GPSDateStamp:
                    break;
                case EDSDK.PropID_GPSLatitude:
                    break;
                case EDSDK.PropID_GPSLatitudeRef:
                    break;
                case EDSDK.PropID_GPSLongitude:
                    break;
                case EDSDK.PropID_GPSLongitudeRef:
                    break;
                case EDSDK.PropID_GPSMapDatum:
                    break;
                case EDSDK.PropID_GPSSatellites:
                    break;
                case EDSDK.PropID_GPSStatus:
                    break;
                case EDSDK.PropID_GPSTimeStamp:
                    break;
                case EDSDK.PropID_GPSVersionID:
                    break;
                case EDSDK.PropID_HDDirectoryStructure:
                    break;
                case EDSDK.PropID_ICCProfile:
                    break;
                case EDSDK.PropID_ImageQuality:
                    break;
                case EDSDK.PropID_ISOBracket:
                    break;
                case EDSDK.PropID_ISOSpeed:
                    break;
                case EDSDK.PropID_JpegQuality:
                    break;
                case EDSDK.PropID_LensName:
                    break;
                case EDSDK.PropID_LensStatus:
                    break;
                case EDSDK.PropID_Linear:
                    break;
                case EDSDK.PropID_MakerName:
                    break;
                case EDSDK.PropID_MeteringMode:
                    break;
                case EDSDK.PropID_NoiseReduction:
                    break;
                case EDSDK.PropID_Orientation:
                    break;
                case EDSDK.PropID_OwnerName:
                    break;
                case EDSDK.PropID_ParameterSet:
                    break;
                case EDSDK.PropID_PhotoEffect:
                    break;
                case EDSDK.PropID_PictureStyle:
                    break;
                case EDSDK.PropID_PictureStyleCaption:
                    break;
                case EDSDK.PropID_PictureStyleDesc:
                    break;
                case EDSDK.PropID_ProductName:
                    break;
                case EDSDK.PropID_Record:
                    break;
                case EDSDK.PropID_RedEye:
                    break;
                case EDSDK.PropID_SaveTo:
                    break;
                case EDSDK.PropID_Sharpness:
                    break;
                case EDSDK.PropID_ToneCurve:
                    break;
                case EDSDK.PropID_ToningEffect:
                    break;
                case EDSDK.PropID_Tv:
                    break;
                case EDSDK.PropID_Unknown:
                    break;
                case EDSDK.PropID_WBCoeffs:
                    break;
                case EDSDK.PropID_WhiteBalance:
                    break;
                case EDSDK.PropID_WhiteBalanceBracket:
                    break;
                case EDSDK.PropID_WhiteBalanceShift:
                    break;
            }
            return EDSDK.EDS_ERR_OK;
        }

        /// <summary>
        /// 相机状态改变
        /// </summary>
        /// <param name="inEvent">The StateEvent ID</param>
        /// <param name="inParameter">Parameter from this event</param>
        /// <param name="inContext">...</param>
        /// <returns>An EDSDK errorcode</returns>
        private uint Camera_SDKStateEvent(uint inEvent, uint inParameter, IntPtr inContext)
        {
            //Handle state event here
            switch (inEvent)
            {
                case EDSDK.StateEvent_All:
                    break;
                case EDSDK.StateEvent_AfResult:
                    break;
                case EDSDK.StateEvent_BulbExposureTime:
                    break;
                case EDSDK.StateEvent_CaptureError:
                    break;
                case EDSDK.StateEvent_InternalError:
                    break;
                case EDSDK.StateEvent_JobStatusChanged://触发条件，拍照命令EDSDK.CameraCommand_TakePicture触发，inParameter == 0表示待机状态，inParameter == 1表示拍照状态
                    if (inParameter == 0) PauseLiveView = false;
                    break;
                case EDSDK.StateEvent_Shutdown:
                    break;
                case EDSDK.StateEvent_ShutDownTimerUpdate:
                    break;
                case EDSDK.StateEvent_WillSoonShutDown:
                    break;
            }
            return EDSDK.EDS_ERR_OK;
        }

        #endregion

        #region Camera commands

        //public void test()
        //{       
        //    //5

        //    String test = "C:\\Users\\YYW\\Pictures\\RemotePhoto\\test.jpg";

        //    Bitmap bm = new Bitmap(Image.FromFile(test));

        //    Bitmap bm_c = ImageOperation.Create32bppImageAndCreateAlpha(bm);

        //    Bitmap bm_b = ImageOperation.RGBATORGB(bm_c);


        //    //设置图片dpi
        //    bm_b.SetResolution(350, 350);

        //    //修改完后保存图片
        //    bm_b.Save("C:\\Users\\YYW\\Pictures\\RemotePhoto\\CS.jpg");
        
        //}

        /// <summary>
        /// 下载一个图像到给定的目录
        /// </summary>
        /// <param name="ObjectPointer">指向对象的指针,从sdkobjectevent得到它，指向照片。</param>
        /// <param name="directory"></param>
        public void DownloadImage(IntPtr ObjectPointer, string directory)
        {
            EDSDK.EdsDirectoryItemInfo dirInfo;

            IntPtr streamRef;

            Error = EDSDK.EdsGetDirectoryItemInfo(ObjectPointer, out dirInfo);

            string CurrentPhoto = "";

            string middlePhoto = "";

            string tempcurrentphoto = "";

            //这里的directory是联合之后路径
            String dir = Path.GetDirectoryName(directory);//目录

            //文件信息
            String sourceFileName = dirInfo.szFileName;

            //获得原文件名中最后的.点
            int i = sourceFileName.LastIndexOf(".");

            //如果i大于0
            if (i > 0)
            {
                //文件扩展名
                String extension = sourceFileName.Substring(i);

                //如果发现设置的文件后缀，与相机上的文件后缀不一致，则，修改文件后缀
                if (!Path.GetFileName(directory).ToLower().EndsWith(extension.ToLower()))
                {   //无用
                    String full = Path.GetFullPath(directory);

                    //获取directory的路径名和文件名
                    string temp = Path.GetDirectoryName(directory) + "\\" + Path.GetFileNameWithoutExtension(directory);

                    //后面在文件名加上_temp,以及修改后缀
                    tempcurrentphoto = temp + "_temp" + extension;

                    middlePhoto = temp + "_middle" + extension;

                    //后面在文件名加上_temp,以及修改后缀
                    CurrentPhoto = temp + extension;
                }
                else
                {
                    int x = directory.LastIndexOf(".");

                    string fullnamewithoutextend = directory.Substring(0, x);
                    tempcurrentphoto = fullnamewithoutextend+"_temp"+extension;

                    middlePhoto = fullnamewithoutextend + "_middle" + extension;

                    CurrentPhoto = directory;
                }
            }
        


            //CR2为佳能的RAW的原片，不下载，太浪费资源了
            if (!CurrentPhoto.ToUpper().EndsWith("CR2")) 
            {
                //创建文件流streamRef，指向tempcurrentphoto，关键下载
                Error = EDSDK.EdsCreateFileStream(tempcurrentphoto, EDSDK.EdsFileCreateDisposition.CreateAlways, EDSDK.EdsAccess.ReadWrite, out streamRef);

                uint blockSize = 1024 * 1024;
                uint remainingBytes = dirInfo.Size;
                //明天修改文件存储的方式。甚是苦恼
                do
                {
                    if (remainingBytes < blockSize) { blockSize = (uint)(remainingBytes / 512) * 512; }
                    remainingBytes -= blockSize;

                    //将ObjectPointer指定的图片的大小下载到streamRef
                    Error = EDSDK.EdsDownload(ObjectPointer, blockSize, streamRef);
                } while (remainingBytes > 512);


                //将ObjectPointer指定的图片的大小下载到streamRef
                Error = EDSDK.EdsDownload(ObjectPointer, remainingBytes, streamRef);

                Error = EDSDK.EdsDownloadComplete(ObjectPointer);

                Error = EDSDK.EdsRelease(ObjectPointer);
                Error = EDSDK.EdsRelease(streamRef);

                //如果有照片的话，则先覆盖，后压缩保存
                if (File.Exists(tempcurrentphoto))
                {

                    try
                    {


                        //原图片tempcurrentphoto
                        Image Im = Image.FromFile(tempcurrentphoto);

                        Bitmap bm = new Bitmap(Im);

                        //裁剪后的图片
                        Bitmap bm_c = ImageOperation.Cut(bm, this.x, this.y, this.width, this.heigth);
                        //压缩成355*441的图片
                        Bitmap bm_r = ImageOperation.ResizeImage(bm_c, this.PW, this.PH, 0);

                        //释放资源
                        Im.Dispose();
                        bm.Dispose();

                        //删除原图
                        //File.Delete(tempcurrentphoto);


                        //保存裁减图片，以后图像处理都是在这张图上操作
                        bm_r.SetResolution(350, 350); 
                        bm_r.Save(middlePhoto);                        

                        //蒙版去背景
                        //Bitmap bm_t = ImageOperation.Create32bppImageAndCreateAlpha(bm_r);
                        Bitmap bm_b = ImageOperation.RGBATORGB(bm_r, 200);


                        //图片压缩,需要进一步处理
                        //ImageOperation.getThumImage(bm_b, 50L, CurrentPhoto);

                        //修改完后保存图片
                        bm_b.SetResolution(350, 350); 
                        bm_b.Save(CurrentPhoto);
                        
                        //生成base64位图片
                        MemoryStream m = new MemoryStream();
                        bm_b.Save(m, ImageFormat.Jpeg);
                        this.BaseImage = Convert.ToBase64String(m.GetBuffer());
                    }
                    catch(Exception e)
                    {
                        throw e;                    
                    }
                    //File.Delete(CurrentPhoto);

                }
            }
            PauseLiveView = false;
            //LV_Start();
            StartLiveView();
        }
        

        //通用背景处理方法
        public void reColor(string middlePhoto, string CurrentPhoto,int x)
        {
            Bitmap re_bm = new Bitmap(Image.FromFile(middlePhoto));

            //蒙版去背景
            Bitmap re_bm_b = ImageOperation.RGBATORGB(re_bm, x);

            //图片压缩,需要进一步处理
            //ImageOperation.getThumImage(bm_b, 50L, CurrentPhoto);

            //修改完后保存图片
            re_bm_b.SetResolution(350, 350); 
            re_bm_b.Save(CurrentPhoto);

            //生成base64位图片
            MemoryStream m = new MemoryStream();
            re_bm_b.Save(m, ImageFormat.Jpeg);
            this.BaseImage = Convert.ToBase64String(m.GetBuffer());
        }


        /// <summary>
        /// 获取当前摄像机设置的可能值的列表。
        /// Only the PropertyIDs "AEModeSelect", "ISO", "Av", "Tv", "MeteringMode" and "ExposureCompensation" are allowed.
        /// </summary>
        /// <param name="PropID">The property ID</param>
        /// <returns>A list of available values for the given property ID</returns>
        public List<int> GetSettingsList(uint PropID)
        {
            if (MainCamera.Ref != IntPtr.Zero)
            {
                if (PropID == EDSDK.PropID_AEModeSelect || PropID == EDSDK.PropID_ISOSpeed || PropID == EDSDK.PropID_Av
                    || PropID == EDSDK.PropID_Tv || PropID == EDSDK.PropID_MeteringMode || PropID == EDSDK.PropID_ExposureCompensation)
                {
                    EDSDK.EdsPropertyDesc des;
                    Error = EDSDK.EdsGetPropertyDesc(MainCamera.Ref, PropID, out des);
                    return des.PropDesc.Take(des.NumElements).ToList();
                }
                else throw new ArgumentException("Method cannot be used with this Property ID");
            }
            else { throw new ArgumentNullException("Camera or camera reference is null/zero"); }
        }

        /// <summary>
        /// 获取给定属性标识的当前设置
        /// </summary>
        /// <param name="PropID">The property ID</param>
        /// <returns>The current setting of the camera</returns>
        public uint GetSetting(uint PropID)
        {
            if (MainCamera.Ref != IntPtr.Zero)
            {
                unsafe
                {
                    uint property = 0;
                    EDSDK.EdsDataType dataType;
                    int dataSize;

                    IntPtr ptr = new IntPtr(&property);

                    Error = EDSDK.EdsGetPropertySize(MainCamera.Ref, PropID, 0, out dataType, out dataSize);
                    Error = EDSDK.EdsGetPropertyData(MainCamera.Ref, PropID, 0, dataSize, ptr);
                    return property;
                }
            }
            else { throw new ArgumentNullException("Camera or camera reference is null/zero"); }
        }

        /// <summary>
        /// 为给定的属性标识设置值
        /// </summary>
        /// <param name="PropID">The property ID</param>
        /// <param name="Value">The value which will be set</param>
        public void SetSetting(uint PropID, uint Value)
        {
            if (MainCamera.Ref != IntPtr.Zero)
            {
                int propsize;
                EDSDK.EdsDataType proptype;

                //if (PropID == EDSDK.PropID_SaveTo)
                //{
                //    do
                //    {
                //        Error = EDSDK.EdsGetPropertySize(MainCamera.Ref, PropID, 0, out proptype, out propsize);
                //        Error = EDSDK.EdsSetPropertyData(MainCamera.Ref, PropID, 0, propsize, Value);
                //    }
                //    while (Error != 0);
                //}
                //else
                //{
                    Error = EDSDK.EdsGetPropertySize(MainCamera.Ref, PropID, 0, out proptype, out propsize);
                    Error = EDSDK.EdsSetPropertyData(MainCamera.Ref, PropID, 0, propsize, Value);
                //}
            }
            else { throw new ArgumentNullException("Camera or camera reference is null/zero"); }
        }

        /// <summary>
        /// 开始实时取景
        /// </summary>
        public void StartLiveView()
        {
            if (!IsLiveViewOn)
            {
                IsLiveViewOn = true;
                PauseLiveView = false;
                //触发EDSDK.PropID_Evf_OutputDevice事件，执行DownloadEvf()方法
                SetSetting(EDSDK.PropID_Evf_OutputDevice, EDSDK.EvfOutputDevice_PC);
            }
        }

        /// <summary>
        /// 关闭实时取景
        /// </summary>
        public void StopLiveView()
        {
            IsLiveViewOn = false;
        }



        /// <summary>
        /// 开始实时取景并录像
        /// </summary>
        public void StartEvfFilming()
        {
            if (!IsLiveViewOn)
            {
                SetSetting(EDSDK.PropID_Evf_OutputDevice, EDSDK.EvfOutputDevice_PC);
                //SetSetting(EDSDK.PropID_Evf_OutputDevice, EDSDK.EvfOutputDevice_PC);
                IsLiveViewOn = true;
                IsEvfFilming = true;
            }
        }

        /// <summary>
        /// Stops LiveView and filming
        /// </summary>
        public void StopEvfFilming()
        {
            IsLiveViewOn = false;
            IsEvfFilming = false;
        }

        /// <summary>
        /// 锁定或解锁相机UI
        /// </summary>
        /// <param name="LockState">True 是锁定, false是解锁</param>
        public void UILock(bool LockState)
        {
            if (LockState == true) Error = EDSDK.EdsSendStatusCommand(MainCamera.Ref, EDSDK.CameraState_UILock, 0);
            else Error = EDSDK.EdsSendStatusCommand(MainCamera.Ref, EDSDK.CameraState_UIUnLock, 0);
        }


        public void SetCapacity(int bytesPerSector, int numberOfFreeClusters)
        {
            //CheckState();
            new Thread(delegate()
            {

                EDSDK.EdsCapacity capacity;
                capacity.BytesPerSector = bytesPerSector;
                capacity.NumberOfFreeClusters = numberOfFreeClusters;
                capacity.Reset = 1;

                //new EDSDK.EdsCapacity(numberOfFreeClusters, bytesPerSector, true);
                //Capacity capacity = new Capacity(numberOfFreeClusters, bytesPerSector, true);
                //ErrorHandler.CheckError(this, CanonSDK.EdsSetCapacity(CamRef, capacity));
                EDSDK.EdsSetCapacity(MainCamera.Ref, capacity);
                
            }).Start();
        }

        /// <summary>
        /// 用当前相机设置的照片
        /// </summary>
        public uint TakePhoto()
        {
            uint a = 1;
            PauseLiveView = true;
            new Thread(delegate()
            {
                int BusyCount = 0;
                uint err = EDSDK.EDS_ERR_OK;

                while (BusyCount < 200)
                {
                    //拍照，(因为设置了EDSDK.PropID_SaveTo)，会触发EDSDK.ObjectEvent_DirItemRequestTransfer事件，从而会下载
                    err = EDSDK.EdsSendCommand(MainCamera.Ref, EDSDK.CameraCommand_TakePicture, 0);
                    if (err == EDSDK.EDS_ERR_DEVICE_BUSY) { BusyCount++; Thread.Sleep(50); }
                    else { break; }
                }

                //解决同步闪光问题
                //while (BusyCount < 200)
                //{
                //    //CameraHandler.StopLiveView();
                //    //uint err = CameraHandler.GetSetting(EDSDK.PropID_Evf_OutputDevice);

                //    //拍照，因为设置了EDSDK.PropID_SaveTo，会触发EDSDK.ObjectEvent_DirItemRequestTransfer事件，从而会下载
                //    err = EDSDK.EdsSendCommand(MainCamera.Ref, EDSDK.CameraCommand_TakePicture, 0);

                //    if (err == EDSDK.EDS_ERR_DEVICE_BUSY) 
                //    { BusyCount++; Thread.Sleep(50); }
                //    else 
                //    {
                //        Thread.Sleep(500);
                //        //SetSetting(EDSDK.PropID_Evf_OutputDevice, EDSDK.EvfOutputDevice_PC);
                //        break;
                //    }
                //}

                //解决700D不拍照问题，已通过其他方式解决
                //do
                //{
                //    //拍照，因为设置了EDSDK.PropID_SaveTo，拍照会保存到电脑，从而会触发EDSDK.ObjectEvent_DirItemRequestTransfer事件，从而会下载
                //    err = EDSDK.EdsSendCommand(MainCamera.Ref, EDSDK.CameraCommand_TakePicture, 0); Thread.Sleep(50);
                //    //if (err == EDSDK.EDS_ERR_DEVICE_BUSY) { BusyCount++; Thread.Sleep(50); }
                //    //else { break; }
                //}
                //while(err!=0);

                Error = err;
                a = err;
            }).Start();
            return a;
        }

        /// <summary>
        /// 在灯泡模式与当前相机设置的照片
        /// </summary>
        /// <param name="BulbTime">在几毫秒的时间，快门将打开</param>
        public void TakePhoto(uint BulbTime)
        {
            PauseLiveView = true;
            new Thread(delegate()
            {
                if (BulbTime < 1000) { throw new ArgumentException("Bulbtime 必须大于1000ms"); }
                int BusyCount = 0;
                uint err = EDSDK.EDS_ERR_OK;
                while (BusyCount < 200)
                {
                    err = EDSDK.EdsSendCommand(MainCamera.Ref, EDSDK.CameraCommand_BulbStart, 0);
                    if (err == EDSDK.EDS_ERR_DEVICE_BUSY) { BusyCount++; Thread.Sleep(50); }
                    else { break; }
                }
                Error = err;

                Thread.Sleep((int)BulbTime);
                Error = EDSDK.EdsSendCommand(MainCamera.Ref, EDSDK.CameraCommand_BulbEnd, 0);
            }).Start();
        }


        //Delegate myLV= new D

        //protected delegate void LV();




        public void LV_Start()
        {
            Thread A=new Thread(delegate()
            {
                
            //       if (!IsLiveViewOn)
            //{
            //    IsLiveViewOn = true;
            //    PauseLiveView = false;
            //    //触发EDSDK.PropID_Evf_OutputDevice事件，执行DownloadEvf()方法
            //    SetSetting(EDSDK.PropID_Evf_OutputDevice, EDSDK.EvfOutputDevice_PC);
            //}

                /// <summary>
                /// 开始实时取景
                /// </summary>
                StartLiveView();

                ///// <summary>
                ///// 关闭实时取景
                ///// </summary>
                //StopLiveView();

            });
            A.Start();
            A.Join();
        }


        public void LV_Stop()
        {
            Thread B =new Thread(delegate()
            {

                //       if (!IsLiveViewOn)
                //{
                //    IsLiveViewOn = true;
                //    PauseLiveView = false;
                //    //触发EDSDK.PropID_Evf_OutputDevice事件，执行DownloadEvf()方法
                //    SetSetting(EDSDK.PropID_Evf_OutputDevice, EDSDK.EvfOutputDevice_PC);
                //}

                /// <summary>
                /// 开始实时取景
                /// </summary>
                //StartLiveView();

                ///// <summary>
                ///// 关闭实时取景

                ///// </summary>
                StopLiveView();

            });
            B.Start();
            B.Join();            
        }



        public void CS()
        {
            Thread C = new Thread(delegate()
            {

                //       if (!IsLiveViewOn)
                //{
                //    IsLiveViewOn = true;
                //    PauseLiveView = false;
                //    //触发EDSDK.PropID_Evf_OutputDevice事件，执行DownloadEvf()方法
                //    SetSetting(EDSDK.PropID_Evf_OutputDevice, EDSDK.EvfOutputDevice_PC);
                //}

                /// <summary>
                /// 开始实时取景
                /// </summary>
                //StartLiveView();

                ///// <summary>
                ///// 关闭实时取景
                ///// </summary>
                //StopLiveView();

                CloseSession();

            });
            C.Start();
            C.Join();
        }

        /// <summary>
        /// Downloads the LiveView image
        /// 下载实时预览图片
        /// </summary>
        private void DownloadEvf()
        {
            new Thread(delegate()
            {
                //To give the camera time to switch the mirror
                Thread.Sleep(1500);

                IntPtr jpgPointer;

                IntPtr stream = IntPtr.Zero;
                IntPtr EvfImageRef = IntPtr.Zero;

                UnmanagedMemoryStream ums;

                uint err;
                uint length;
                //create streams
                Error = EDSDK.EdsCreateMemoryStream(0, out stream);
                Error = EDSDK.EdsCreateEvfImageRef(stream, out EvfImageRef);

                Stopwatch watch = new Stopwatch();  //stopwatch for FPS calculation
                float lastfr = 24; //last actual FPS

                //运行实时取景，根据IsLiveViewOn的状态
                while (IsLiveViewOn)
                {
                    //暂停
                    if (!PauseLiveView)
                    {
                        watch.Restart();
                        //download current LiveView image
                        err = EDSDK.EdsDownloadEvfImage(MainCamera.Ref, EvfImageRef);

                        unsafe
                        {
                            //get pointer and create stream
                            Error = EDSDK.EdsGetPointer(stream, out jpgPointer);
                            Error = EDSDK.EdsGetLength(stream, out length);
                            ums = new UnmanagedMemoryStream((byte*)jpgPointer.ToPointer(), length, length, FileAccess.Read);

                            //fire the LiveViewUpdated event with the LiveView image created from the stream
                            try
                            {
                                //手动触发
                                if (LiveViewUpdated != null) LiveViewUpdated(Image.FromStream(ums));

                            }
                            catch (Exception e)
                            {
                                
                            }
                            ums.Close();
                        }
                        //calculate the framerate and fire the FrameRateUpdated event
                        lastfr = lastfr * 0.9f + (100f / watch.ElapsedMilliseconds);
                        if (FrameRateUpdated != null) FrameRateUpdated(lastfr);
                    }
                    else Thread.Sleep(200);
                }

                //Release and finish
                if (stream != IntPtr.Zero) { Error = EDSDK.EdsRelease(stream); }
                if (EvfImageRef != IntPtr.Zero) { Error = EDSDK.EdsRelease(EvfImageRef); }
                //stop the LiveView
                //SetSetting(EDSDK.PropID_Evf_OutputDevice, EDSDK.EvfOutputDevice_TFT);
                SetSetting(EDSDK.PropID_Evf_OutputDevice, 0);
            }).Start();
        }
        
        /// <summary>
        /// 记录实时预览图像
        /// </summary>
        private void DownloadEvfFilm()
        {
            new Thread(delegate()
            {
                //给相机的时间来切换镜子
                Thread.Sleep(1500);

                IntPtr jpgPointer;
                IntPtr stream = IntPtr.Zero;
                IntPtr EvfImageRef = IntPtr.Zero;
                UnmanagedMemoryStream ums;
                uint err;
                uint length;
                err = EDSDK.EdsCreateMemoryStream(0, out stream);
                err = EDSDK.EdsCreateEvfImageRef(stream, out EvfImageRef);

                //Download one frame to init the video size
                //下载一个框架初始化视频大小
                err = EDSDK.EdsDownloadEvfImage(MainCamera.Ref, EvfImageRef);
                unsafe
                {
                    Error = EDSDK.EdsGetPointer(stream, out jpgPointer);
                    Error = EDSDK.EdsGetLength(stream, out length);
                    ums = new UnmanagedMemoryStream((byte*)jpgPointer.ToPointer(), length, length, FileAccess.Read);

                    Bitmap bmp = new Bitmap(ums);
                    StartEvfVideoWriter(bmp.Width, bmp.Height);//似乎没什么用
                    bmp.Dispose();
                    ums.Close();
                }

                Stopwatch watch = new Stopwatch();
                byte[] barr;        //bitmap byte array
                const long ft = 41; //Frametime at 24FPS (actually 41.66)
                float lastfr = 24;  //last actual FPS

                int LVUpdateBreak1 = 0;

                //Run LiveView
                //运行实时预览
                while (IsEvfFilming)
                {
                    watch.Restart();
                    err = EDSDK.EdsDownloadEvfImage(MainCamera.Ref, EvfImageRef);
                    unsafe
                    {
                        Error = EDSDK.EdsGetPointer(stream, out jpgPointer);
                        Error = EDSDK.EdsGetLength(stream, out length);
                        ums = new UnmanagedMemoryStream((byte*)jpgPointer.ToPointer(), length, length, FileAccess.Read);
                        barr = new byte[length];
                        ums.Read(barr, 0, (int)length);

                        //For better performance the LiveView is only updated with every 4th frame
                        //为了更好的性能，朗视只是更新了每第四帧
                        if (LVUpdateBreak1 == 0 && LiveViewUpdated != null) { LiveViewUpdated(Image.FromStream(ums)); LVUpdateBreak1 = 4; }
                        LVUpdateBreak1--;
                        FrameBuffer.Enqueue(barr);

                        ums.Close();
                    }
                    //To get a steady framerate:
                    //有一个稳定的帧率：
                    while (true) if (watch.ElapsedMilliseconds >= ft) break;
                    lastfr = lastfr * 0.9f + (100f / watch.ElapsedMilliseconds);
                    if (FrameRateUpdated != null) FrameRateUpdated(lastfr);
                }

                //Release and finish
                if (stream != IntPtr.Zero) { Error = EDSDK.EdsRelease(stream); }
                if (EvfImageRef != IntPtr.Zero) { Error = EDSDK.EdsRelease(EvfImageRef); }
                SetSetting(EDSDK.PropID_Evf_OutputDevice, EDSDK.EvfOutputDevice_TFT);
            }).Start();
        }

        /// <summary>
        /// Writes video frames from the buffer to a file
        /// 将视频帧从缓冲区写入文件
        /// </summary>
        /// <param name="Width">Width of the video</param>
        /// <param name="Height">Height of the video</param>
        private void StartEvfVideoWriter(int Width, int Height)
        {
            new Thread(delegate()
            {
                byte[] byteArray;
                ImageConverter ic = new ImageConverter();
                Image img;

                while (IsEvfFilming)
                {
                    while (FrameBuffer.Count > 0)
                    {
                        byteArray = FrameBuffer.Dequeue();
                        img = (Image)ic.ConvertFrom(byteArray);
                        //Save video frame here. e.g. with the VideoFileWriter from the AForge library.
                        //保存视频帧在这里。例如，从图书馆videofilewriter AForge。
                    }
                    if (IsEvfFilming) Thread.Sleep(10);
                }
            }).Start();
        }

        #endregion
    }

    /**
     * 相机对象
     */
    public class Camera
    {
        internal IntPtr Ref;
        public EDSDK.EdsDeviceInfo Info { get; private set; }

        public uint Error
        {
            get { return EDSDK.EDS_ERR_OK; }
            set { if (value != EDSDK.EDS_ERR_OK) throw new Exception("SDK Error: " + value); }
        }

        public Camera(IntPtr Reference)
        {
            if (Reference == IntPtr.Zero) throw new ArgumentNullException("Camera pointer is zero");
            this.Ref = Reference;
            EDSDK.EdsDeviceInfo dinfo;
            Error = EDSDK.EdsGetDeviceInfo(Reference, out dinfo);
            this.Info = dinfo;
        }
    }

    public static class CameraValues
    {
        private static CultureInfo cInfo = new CultureInfo("en-US");

        public static string AV(uint v)
        {
            switch (v)
            {
                case 0x08:
                    return "1";
                case 0x40:
                    return "11";
                case 0x0B:
                    return "1.1";
                case 0x43:
                    return "13 (1/3)";
                case 0x0C:
                    return "1.2";
                case 0x44:
                    return "13";
                case 0x0D:
                    return "1.2 (1/3)";
                case 0x45:
                    return "14";
                case 0x10:
                    return "1.4";
                case 0x48:
                    return "16";
                case 0x13:
                    return "1.6";
                case 0x4B:
                    return "18";
                case 0x14:
                    return "1.8";
                case 0x4C:
                    return "19";
                case 0x15:
                    return "1.8 (1/3)";
                case 0x4D:
                    return "20";
                case 0x18:
                    return "2";
                case 0x50:
                    return "22";
                case 0x1B:
                    return "2.2";
                case 0x53:
                    return "25";
                case 0x1C:
                    return "2.5";
                case 0x54:
                    return "27";
                case 0x1D:
                    return "2.5 (1/3)";
                case 0x55:
                    return "29";
                case 0x20:
                    return "2.8";
                case 0x58:
                    return "32";
                case 0x23:
                    return "3.2";
                case 0x5B:
                    return "36";
                case 0x24:
                    return "3.5";
                case 0x5C:
                    return "38";
                case 0x25:
                    return "3.5 (1/3)";
                case 0x5D:
                    return "40";
                case 0x28:
                    return "4";
                case 0x60:
                    return "45";
                case 0x2B:
                    return "4.5";
                case 0x63:
                    return "51";
                case 0x2C:
                    return "4.5 (1/3)";
                case 0x64:
                    return "54";
                case 0x2D:
                    return "5.0";
                case 0x65:
                    return "57";
                case 0x30:
                    return "5.6";
                case 0x68:
                    return "64";
                case 0x33:
                    return "6.3";
                case 0x6B:
                    return "72";
                case 0x34:
                    return "6.7";
                case 0x6C:
                    return "76";
                case 0x35:
                    return "7.1";
                case 0x6D:
                    return "80";
                case 0x38:
                    return " 8";
                case 0x70:
                    return "91";
                case 0x3B:
                    return "9";
                case 0x3C:
                    return "9.5";
                case 0x3D:
                    return "10";

                case 0xffffffff:
                default:
                    return "N/A";
            }
        }

        public static string ISO(uint v)
        {
            switch (v)
            {
                case 0x00000000:
                    return "Auto ISO";
                case 0x00000028:
                    return "ISO 6";
                case 0x00000030:
                    return "ISO 12";
                case 0x00000038:
                    return "ISO 25";
                case 0x00000040:
                    return "ISO 50";
                case 0x00000048:
                    return "ISO 100";
                case 0x0000004b:
                    return "ISO 125";
                case 0x0000004d:
                    return "ISO 160";
                case 0x00000050:
                    return "ISO 200";
                case 0x00000053:
                    return "ISO 250";
                case 0x00000055:
                    return "ISO 320";
                case 0x00000058:
                    return "ISO 400";
                case 0x0000005b:
                    return "ISO 500";
                case 0x0000005d:
                    return "ISO 640";
                case 0x00000060:
                    return "ISO 800";
                case 0x00000063:
                    return "ISO 1000";
                case 0x00000065:
                    return "ISO 1250";
                case 0x00000068:
                    return "ISO 1600";
                case 0x00000070:
                    return "ISO 3200";
                case 0x00000078:
                    return "ISO 6400";
                case 0x00000080:
                    return "ISO 12800";
                case 0x00000088:
                    return "ISO 25600";
                case 0x00000090:
                    return "ISO 51200";
                case 0x00000098:
                    return "ISO 102400";
                case 0xffffffff:
                default:
                    return "N/A";
            }
        }

        public static string TV(uint v)
        {
            switch (v)
            {
                case 0x0C:
                    return "Bulb";
                case 0x5D:
                    return "1/25";
                case 0x10:
                    return "30\"";
                case 0x60:
                    return "1/30";
                case 0x13:
                    return "25\"";
                case 0x63:
                    return "1/40";
                case 0x14:
                    return "20\"";
                case 0x64:
                    return "1/45";
                case 0x15:
                    return "20\" (1/3)";
                case 0x65:
                    return "1/50";
                case 0x18:
                    return "15\"";
                case 0x68:
                    return "1/60";
                case 0x1B:
                    return "13\"";
                case 0x6B:
                    return "1/80";
                case 0x1C:
                    return "10\"";
                case 0x6C:
                    return "1/90";
                case 0x1D:
                    return "10\" (1/3)";
                case 0x6D:
                    return "1/100";
                case 0x20:
                    return "8\"";
                case 0x70:
                    return "1/125";
                case 0x23:
                    return "6\" (1/3)";
                case 0x73:
                    return "1/160";
                case 0x24:
                    return "6\"";
                case 0x74:
                    return "1/180";
                case 0x25:
                    return "5\"";
                case 0x75:
                    return "1/200";
                case 0x28:
                    return "4\"";
                case 0x78:
                    return "1/250";
                case 0x2B:
                    return "3\"2";
                case 0x7B:
                    return "1/320";
                case 0x2C:
                    return "3\"";
                case 0x7C:
                    return "1/350";
                case 0x2D:
                    return "2\"5";
                case 0x7D:
                    return "1/400";
                case 0x30:
                    return "2\"";
                case 0x80:
                    return "1/500";
                case 0x33:
                    return "1\"6";
                case 0x83:
                    return "1/640";
                case 0x34:
                    return "1\"5";
                case 0x84:
                    return "1/750";
                case 0x35:
                    return "1\"3";
                case 0x85:
                    return "1/800";
                case 0x38:
                    return "1\"";
                case 0x88:
                    return "1/1000";
                case 0x3B:
                    return "0\"8";
                case 0x8B:
                    return "1/1250";
                case 0x3C:
                    return "0\"7";
                case 0x8C:
                    return "1/1500";
                case 0x3D:
                    return "0\"6";
                case 0x8D:
                    return "1/1600";
                case 0x40:
                    return "0\"5";
                case 0x90:
                    return "1/2000";
                case 0x43:
                    return "0\"4";
                case 0x93:
                    return "1/2500";
                case 0x44:
                    return "0\"3";
                case 0x94:
                    return "1/3000";
                case 0x45:
                    return "0\"3 (1/3)";
                case 0x95:
                    return "1/3200";
                case 0x48:
                    return "1/4";
                case 0x98:
                    return "1/4000";
                case 0x4B:
                    return "1/5";
                case 0x9B:
                    return "1/5000";
                case 0x4C:
                    return "1/6";
                case 0x9C:
                    return "1/6000";
                case 0x4D:
                    return "1/6 (1/3)";
                case 0x9D:
                    return "1/6400";
                case 0x50:
                    return "1/8";
                case 0xA0:
                    return "1/8000";
                case 0x53:
                    return "1/10 (1/3)";
                case 0x54:
                    return "1/10";
                case 0x55:
                    return "1/13";
                case 0x58:
                    return "1/15";
                case 0x5B:
                    return "1/20 (1/3)";
                case 0x5C:
                    return "1/20";

                case 0xffffffff:
                default:
                    return "N/A";
            }
        }


        public static uint AV(string v)
        {
            switch (v)
            {
                case "1":
                    return 0x08;
                case "11":
                    return 0x40;
                case "1.1":
                    return 0x0B;
                case "13 (1/3)":
                    return 0x43;
                case "1.2":
                    return 0x0C;
                case "13":
                    return 0x44;
                case "1.2 (1/3)":
                    return 0x0D;
                case "14":
                    return 0x45;
                case "1.4":
                    return 0x10;
                case "16":
                    return 0x48;
                case "1.6":
                    return 0x13;
                case "18":
                    return 0x4B;
                case "1.8":
                    return 0x14;
                case "19":
                    return 0x4C;
                case "1.8 (1/3)":
                    return 0x15;
                case "20":
                    return 0x4D;
                case "2":
                    return 0x18;
                case "22":
                    return 0x50;
                case "2.2":
                    return 0x1B;
                case "25":
                    return 0x53;
                case "2.5":
                    return 0x1C;
                case "27":
                    return 0x54;
                case "2.5 (1/3)":
                    return 0x1D;
                case "29":
                    return 0x55;
                case "2.8":
                    return 0x20;
                case "32":
                    return 0x58;
                case "3.2":
                    return 0x23;
                case "36":
                    return 0x5B;
                case "3.5":
                    return 0x24;
                case "38":
                    return 0x5C;
                case "3.5 (1/3)":
                    return 0x25;
                case "40":
                    return 0x5D;
                case "4":
                    return 0x28;
                case "45":
                    return 0x60;
                case "4.5":
                    return 0x2B;
                case "51":
                    return 0x63;
                case "4.5 (1/3)":
                    return 0x2C;
                case "54":
                    return 0x64;
                case "5.0":
                    return 0x2D;
                case "57":
                    return 0x65;
                case "5.6":
                    return 0x30;
                case "64":
                    return 0x68;
                case "6.3":
                    return 0x33;
                case "72":
                    return 0x6B;
                case "6.7":
                    return 0x34;
                case "76":
                    return 0x6C;
                case "7.1":
                    return 0x35;
                case "80":
                    return 0x6D;
                case " 8":
                    return 0x38;
                case "91":
                    return 0x70;
                case "9":
                    return 0x3B;
                case "9.5":
                    return 0x3C;
                case "10":
                    return 0x3D;

                case "N/A":
                default:
                    return 0xffffffff;
            }
        }

        public static uint ISO(string v)
        {
            switch (v)
            {
                case "Auto ISO":
                    return 0x00000000;
                case "ISO 6":
                    return 0x00000028;
                case "ISO 12":
                    return 0x00000030;
                case "ISO 25":
                    return 0x00000038;
                case "ISO 50":
                    return 0x00000040;
                case "ISO 100":
                    return 0x00000048;
                case "ISO 125":
                    return 0x0000004b;
                case "ISO 160":
                    return 0x0000004d;
                case "ISO 200":
                    return 0x00000050;
                case "ISO 250":
                    return 0x00000053;
                case "ISO 320":
                    return 0x00000055;
                case "ISO 400":
                    return 0x00000058;
                case "ISO 500":
                    return 0x0000005b;
                case "ISO 640":
                    return 0x0000005d;
                case "ISO 800":
                    return 0x00000060;
                case "ISO 1000":
                    return 0x00000063;
                case "ISO 1250":
                    return 0x00000065;
                case "ISO 1600":
                    return 0x00000068;
                case "ISO 3200":
                    return 0x00000070;
                case "ISO 6400":
                    return 0x00000078;
                case "ISO 12800":
                    return 0x00000080;
                case "ISO 25600":
                    return 0x00000088;
                case "ISO 51200":
                    return 0x00000090;
                case "ISO 102400":
                    return 0x00000098;

                case "N/A":
                default:
                    return 0xffffffff;
            }
        }

        public static uint TV(string v)
        {
            switch (v)
            {
                case "Bulb":
                    return 0x0C;
                case "1/25":
                    return 0x5D;
                case "30\"":
                    return 0x10;
                case "1/30":
                    return 0x60;
                case "25\"":
                    return 0x13;
                case "1/40":
                    return 0x63;
                case "20\"":
                    return 0x14;
                case "1/45":
                    return 0x64;
                case "20\" (1/3)":
                    return 0x15;
                case "1/50":
                    return 0x65;
                case "15\"":
                    return 0x18;
                case "1/60":
                    return 0x68;
                case "13\"":
                    return 0x1B;
                case "1/80":
                    return 0x6B;
                case "10\"":
                    return 0x1C;
                case "1/90":
                    return 0x6C;
                case "10\" (1/3)":
                    return 0x1D;
                case "1/100":
                    return 0x6D;
                case "8\"":
                    return 0x20;
                case "1/125":
                    return 0x70;
                case "6\" (1/3)":
                    return 0x23;
                case "1/160":
                    return 0x73;
                case "6\"":
                    return 0x24;
                case "1/180":
                    return 0x74;
                case "5\"":
                    return 0x25;
                case "1/200":
                    return 0x75;
                case "4\"":
                    return 0x28;
                case "1/250":
                    return 0x78;
                case "3\"2":
                    return 0x2B;
                case "1/320":
                    return 0x7B;
                case "3\"":
                    return 0x2C;
                case "1/350":
                    return 0x7C;
                case "2\"5":
                    return 0x2D;
                case "1/400":
                    return 0x7D;
                case "2\"":
                    return 0x30;
                case "1/500":
                    return 0x80;
                case "1\"6":
                    return 0x33;
                case "1/640":
                    return 0x83;
                case "1\"5":
                    return 0x34;
                case "1/750":
                    return 0x84;
                case "1\"3":
                    return 0x35;
                case "1/800":
                    return 0x85;
                case "1\"":
                    return 0x38;
                case "1/1000":
                    return 0x88;
                case "0\"8":
                    return 0x3B;
                case "1/1250":
                    return 0x8B;
                case "0\"7":
                    return 0x3C;
                case "1/1500":
                    return 0x8C;
                case "0\"6":
                    return 0x3D;
                case "1/1600":
                    return 0x8D;
                case "0\"5":
                    return 0x40;
                case "1/2000":
                    return 0x90;
                case "0\"4":
                    return 0x43;
                case "1/2500":
                    return 0x93;
                case "0\"3":
                    return 0x44;
                case "1/3000":
                    return 0x94;
                case "0\"3 (1/3)":
                    return 0x45;
                case "1/3200":
                    return 0x95;
                case "1/4":
                    return 0x48;
                case "1/4000":
                    return 0x98;
                case "1/5":
                    return 0x4B;
                case "1/5000":
                    return 0x9B;
                case "1/6":
                    return 0x4C;
                case "1/6000":
                    return 0x9C;
                case "1/6 (1/3)":
                    return 0x4D;
                case "1/6400":
                    return 0x9D;
                case "1/8":
                    return 0x50;
                case "1/8000":
                    return 0xA0;
                case "1/10 (1/3)":
                    return 0x53;
                case "1/10":
                    return 0x54;
                case "1/13":
                    return 0x55;
                case "1/15":
                    return 0x58;
                case "1/20 (1/3)":
                    return 0x5B;
                case "1/20":
                    return 0x5C;

                case "N/A":
                default:
                    return 0xffffffff;
            }
        }
    }

}
