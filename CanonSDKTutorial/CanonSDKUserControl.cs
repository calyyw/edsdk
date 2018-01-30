using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDSDKLib;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Security;

namespace CanonSDKTutorial
{
    [Guid("8d7d8518-ca58-4863-b94d-3c616fda7b35")]

    //[Guid("0D35CE73-9BAA-4E54-8480-403407A26135")]
    [SecuritySafeCritical] 
    public  partial  class CanonSDKUserControl : UserControl,IObjectSafety
    {

        public SDKHandler CameraHandler;
        List<int> AvList;
        List<int> TvList;
        List<int> ISOList;
        List<Camera> CamList;

        private String savePathText;
        delegate void D(object obj);

        /// <summary>
        /// 初始化
        /// </summary>
        public  CanonSDKUserControl()
        {
            InitializeComponent();
            //设置校正框的父窗体
            pictureBox.Parent = LiveViewPicBox;
            //初始化照相SDK驱动信息
            CameraHandler = new SDKHandler();

            CameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(SDK_CameraAdded);
            CameraHandler.FrameRateUpdated += new SDKHandler.FloatUpdate(SDK_FrameRateUpdated);
            CameraHandler.LiveViewUpdated += new SDKHandler.ImageUpdate(SDK_LiveViewUpdated);
            CameraHandler.ProgressChanged += new SDKHandler.ProgressHandler(SDK_ProgressChanged);


            //设置照片保存的位置，相机或者电脑
            //CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);

            //设置图片保存路径，默认的是系统照片文件夹下的RemotePhoto目录下
            savePathText = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RemotePhoto");
            RefreshCamera();

        }

        #region IObjectSafety 成员

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;

        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;
            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }
            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;
            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) && (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) && (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }
            return Rslt;
        }

        #endregion

        public void SDK_CameraAdded()
        {
            //检测到相机后刷新
            RefreshCamera();
        }

        public void SDK_ProgressChanged(int Progress)
        {
        }

        /// <summary>
        /// 实时更新图像预览框
        /// </summary>
        /// <param name="img">图片</param>
        public void SDK_LiveViewUpdated(Image img)
        {
            if (CameraHandler.IsLiveViewOn) LiveViewPicBox.Image = img;
            else LiveViewPicBox.Image = null;
        }

        public void SDK_FrameRateUpdated(float Value)
        {
            //this.Invoke((MethodInvoker)delegate { FrameRateLabel.Text = "FPS: " + Value.ToString("F2"); });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public string dispose()
        {
            //CameraHandler.LV_Stop();
            //CameraHandler.CS();

            CameraHandler.LV_Stop();
            uint err = CameraHandler.GetSetting(EDSDK.PropID_Evf_OutputDevice);

            Thread A = new Thread(delegate()
            {
                while (err != 0)
                {
                    Thread.Sleep(100);
                    err = CameraHandler.GetSetting(EDSDK.PropID_Evf_OutputDevice);
                }
                //CameraHandler.ImageSaveDirectory = savePathText;
                //CameraHandler.TakePhoto();
                CameraHandler.Dispose();
                
            }
                );
            A.Start();
            A.Join();

            return "ok";
        }

        /// <summary>
        /// 获得图片为base64位
        /// </summary>
        /// <returns></returns>
        public string getImageBase()
        {
            String base64 = CameraHandler.BaseImage;
            return base64;
        }

        public void RefreshCamera()
        {
            //关闭会话
            CloseSession();
            //清空界面上的相机列表
            //CameraListBox.Items.Clear();
            //利用相机驱动，获取相机列表
            CamList = CameraHandler.GetCameraList();
            //利用增强for循环为listBox设置值
            foreach (Camera cam in CamList) CameraListBox.Items.Add(cam.Info.szDeviceDescription);
            //如果相机个数大于0个，则默认选择第一个
            if (CamList.Count > 0) CameraListBox.SelectedIndex = 0;
        }



        /// <summary>
        /// 实时取景
        /// </summary>
        public string LiveViewButton()
        {
            Thread.Sleep(1000); 
            string liveViewText = "";
            if (!CameraHandler.IsEvfFilming)
            {
                if (!CameraHandler.IsLiveViewOn) { CameraHandler.StartLiveView(); liveViewText = "关闭实时取景"; }
                else { CameraHandler.StopLiveView(); liveViewText = "打开实时取景"; }
                //AvCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                //TvCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                //ISOCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                //WBCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                //WBUpDo.Enabled = !CameraHandler.IsLiveViewOn;
                //SaveToGroupBox.Enabled = 
                  //CameraHandler.IsLiveViewOn;
            }
            //textBox.Text = CameraHandler.CameraSessionOpen.ToString() + "$$" + CameraHandler.IsLiveViewOn.ToString() + "$$";
            return liveViewText;
        }

        //暂停实时预览
        public Boolean LiveViewPause()
        {
            CameraHandler.PauseLiveView = !CameraHandler.PauseLiveView;        

            return CameraHandler.PauseLiveView;
        }

 
        /// <summary>
        /// 打开会话
        /// </summary>
        public String OpenSession()
        {
            string text = "";
            //如果当前相机里表中有选中项，则打开
            if (CameraListBox.SelectedIndex >= 0)
            {
                //利用相机的驱动打开选中的相机
                CameraHandler.OpenSession(CamList[CameraListBox.SelectedIndex]);
                //设置会话按钮
                text = "关闭会话";

                //设置照片保存的位置，相机或者电脑
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);

                //设置相机的存储
                CameraHandler.SetCapacity(4096, 1024 * 1024);

               // SessionLabel.Text = CameraHandler.MainCamera.Info.szDeviceDescription;
                //AvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Av);
                //TvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Tv);
                //ISOList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_ISOSpeed);
                //foreach (int Av in AvList) AvCoBox.Items.Add(CameraValues.AV((uint)Av));
                //foreach (int Tv in TvList) TvCoBox.Items.Add(CameraValues.TV((uint)Tv));
                //foreach (int ISO in ISOList) ISOCoBox.Items.Add(CameraValues.ISO((uint)ISO));
                //AvCoBox.SelectedIndex = AvCoBox.Items.IndexOf(CameraValues.AV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Av)));
                //TvCoBox.SelectedIndex = TvCoBox.Items.IndexOf(CameraValues.TV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Tv)));
                //ISOCoBox.SelectedIndex = ISOCoBox.Items.IndexOf(CameraValues.ISO((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_ISOSpeed)));
                //int wbidx = (int)CameraHandler.GetSetting((uint)EDSDK.PropID_WhiteBalance);
                //WBCoBox.SelectedIndex = (wbidx > 8) ? wbidx - 1 : wbidx;
                try
                {
                    CameraHandler.GetSetting((uint)EDSDK.PropID_ColorTemperature);
                }
                catch (Exception)
                {

                    //throw;
                }

                //SettingsGroupBox.Enabled = true;
                //LiveViewGroupBox.Enabled = true;
            }
            //textBox.Text = CameraHandler.CameraSessionOpen.ToString() + "$$" + CameraHandler.IsLiveViewOn.ToString() + "$$";            
            //Thread.Sleep(1000);
            return text;
        }

        /// <summary>
        /// 关闭会话
        /// </summary>
        public void CloseSession()
        {
            //调用照相驱动，关闭会话
            CameraHandler.CloseSession();
            //清空界面的设置
            //AvCoBox.Items.Clear();
            //TvCoBox.Items.Clear();
            //ISOCoBox.Items.Clear();
            //SettingsGroupBox.Enabled = false;
            //LiveViewGroupBox.Enabled = false;
            //SessionButton.Text = "打开会话";
            //SessionLabel.Text = "没有回话打开";
        }

        /// <summary>
        /// 开始照相
        /// </summary>
        /// <param name="path">照片的电脑存储路径</param>
        public string TakePhotoButton(string path,string name)
        {
            //设置照片保存的位置，相机或者电脑
            //CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host); 
            //CameraHandler = new SDKHandler();

            //如果路径不存在则重新生成目录
            if (!String.IsNullOrEmpty(path)) {
                string dir = path + "\\" + name;
                savePathText = dir;
            }
            else
            {
                savePathText += "\\" + name;
            }

            //如果没有目录，则创建目录
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //设置默认的拍照参数
            //CameraHandler.SetSetting(EDSDK.PropID_ImageQuality,EDSDK.ImageSize_Small);

            //关闭实时预览
            //LiveViewButton();
            CameraHandler.LV_Stop();
            uint err = CameraHandler.GetSetting(EDSDK.PropID_Evf_OutputDevice);

            Thread A = new Thread(delegate()
                {
                    while (err != 0)
                    {
                        Thread.Sleep(100);
                        err = CameraHandler.GetSetting(EDSDK.PropID_Evf_OutputDevice);
                    }
                    CameraHandler.ImageSaveDirectory = savePathText;
                    CameraHandler.TakePhoto();
                }
                );
            //A.IsBackground = true;
            A.Start();
            A.Join();

            //拍照
            //CameraHandler.ImageSaveDirectory = savePathText;
            //CameraHandler.TakePhoto();



            //打开实时预览
            //CameraHandler.LV_Start();
            //Thread B = new Thread(delegate()
            //{
            //    while (CameraHandler.IsLiveViewOn == false)
            //    {
            //        Thread.Sleep(100);
            //        //CameraHandler.LV_Start();
            //    }
            //    //return savePathText;
            //}
            //);
            ////A.IsBackground = true;
            //B.Start();
            //B.Join();
            ////CameraHandler.LV_Start();
            //// LiveViewButton();



           // textBox.Text = CameraHandler.CameraSessionOpen.ToString() + "$$" + CameraHandler.IsLiveViewOn.ToString() + "$$";
            return savePathText;
        }





        //private void button_Click(object sender, EventArgs e)
        //{
        //    //Start("Hello World");

        //}
        public string GetString()
        {
            return DateTime.Now.ToString();
        }

        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public string[] GetStringArray()
        {
            return "H,e,l,l,o".Split(',');
        }

        public int GetInt()
        {
            return 1;
        }

        public double GetDouble()
        {
            return 1.0;
        }

        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        public object GetList()
        {
            return CameraListBox;
        }

        public string GetListItems()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < CameraListBox.Items.Count; i++)
            {
                sb.Append("{item:'" + CameraListBox.Items[i].ToString() + "'},");
            }
            return "[" + sb.ToString().Trim(',') + "]";
        }

        public void Start(object obj)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(ShowTime));
                t.Start(obj.ToString() + "，线程：" + i.ToString());
            }
        }



        void ShowTime(object obj)
        {
            if (this.CameraListBox.InvokeRequired)
            {
                D d = new D(DelegateShowTime);
                CameraListBox.Invoke(d, obj);
            }
            else
            {
                this.CameraListBox.Items.Add(obj);
            }
        }


        void DelegateShowTime(object obj)
        {
            this.CameraListBox.Items.Add(obj);
        }

        /// <summary>
        /// 记录鼠标的位置
        /// </summary>
        private Point mouse_offset;


        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //获取鼠标光标的位置（以屏幕坐标表示）
                Point mousePos = Control.MousePosition;
                //将获得的坐标平移指定的量

                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                //获取该控件的坐标位置
                //((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);

                //子控件
                Control control = ((Control)sender);

                //父控件
                Control parent = control.Parent;
                Point point = control.Parent.PointToClient(mousePos);
                Point pp = parent.Location;
                Console.WriteLine("1：" + control.Top);
                Console.WriteLine("2：" + control.Left);

                Console.WriteLine("3：" + pp.X);
                Console.WriteLine("4：" + pp.Y);

                //父控件尺寸
                int parentWidth = parent.Width;
                int parentHeight = parent.Height;

                //子控件尺寸
                int controlWidth = control.Width;
                int controlHeight = control.Height;


                //计算当前控件是否超出父控件的边界//如果超出则设置为边界的坐标
                if (point.X < 0)
                {
                    point.X = 0;
                }
                if (point.Y < 0)
                {
                    point.Y = 0;
                }

                if (point.X + controlWidth >= parentWidth)
                {
                    point.X = parentWidth - controlWidth;
                }

                if (point.Y + controlHeight >= parentHeight)
                {
                    point.Y = parentHeight - controlHeight;
                }


                //截取的起始点
                CameraHandler.x = (int)(1.0 * control.Left * CameraHandler.W / parentWidth);
                CameraHandler.y = (int)(1.0 * control.Top * CameraHandler.H / parentHeight);


                //截取的尺寸
                CameraHandler.width = (int)(1.0 * controlWidth * CameraHandler.W / parentWidth);
                CameraHandler.heigth = (int)(1.0 * controlHeight * CameraHandler.H / parentHeight);

                control.Location = point;

            }
        }



        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            //获得鼠标初始位置
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void CameraListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            label.Text = trackBar.Value.ToString();
            int x=trackBar.Value;

            string middlePhoto = "C:\\TEMP\\abc_middle.JPG";
            string CurrentPhoto="C:\\TEMP\\abc.JPG";

            CameraHandler.reColor(middlePhoto, CurrentPhoto,x);

        }

        public void remyColor(string middlePhoto, string CurrentPhoto, int x)
        {
            CameraHandler.reColor(middlePhoto, CurrentPhoto, x);
        }

        private void LiveViewPicBox_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {

        }

        private void label_Click(object sender, EventArgs e)
        {

        }
    }
}
