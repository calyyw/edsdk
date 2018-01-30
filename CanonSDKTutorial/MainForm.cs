using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EDSDKLib;
using System.Runtime.InteropServices;
using System.Threading;
namespace CanonSDKTutorial
{
   
    public partial class MainForm : Form
    {
       public SDKHandler CameraHandler;

        List<int> AvList;
        List<int> TvList;
        List<int> ISOList;
        List<Camera> CamList;

       

        public MainForm()
        {
            //初始化界面信息
            InitializeComponent();

            //设置拖动截取框的透明显示对象，显示实时取景的窗口
            pictureBox.Parent = LiveViewPicBox;

            //初始化照相SDK驱动信息
            CameraHandler = new SDKHandler();

            CameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(SDK_CameraAdded);
            CameraHandler.FrameRateUpdated += new SDKHandler.FloatUpdate(SDK_FrameRateUpdated);
            CameraHandler.LiveViewUpdated += new SDKHandler.ImageUpdate(SDK_LiveViewUpdated);
            CameraHandler.ProgressChanged += new SDKHandler.ProgressHandler(SDK_ProgressChanged);


            //设置图片保存路径，默认的是系统照片文件夹下的RemotePhoto目录下
            SavePathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RemotePhoto");

            SaveNameTextBox.Text = "CS.JPG";
            //SavePathTextBox.Text = "C:\\Users\\ITF\\Pictures\\RemotePhotoasasdf.JPG";

            RefreshCamera();
        }


        public void SDK_ProgressChanged(int Progress)
        {
        }

        /// <summary>
        /// 实时图像更新
        /// </summary>
        /// <param name="img"></param>
        public void SDK_LiveViewUpdated(Image img)
        {
            if (CameraHandler.IsLiveViewOn) LiveViewPicBox.Image = img;
            else LiveViewPicBox.Image = null;
        }
        /// <summary>
        /// 更新实时取景图像的更新码率,MethodInvoker,SDKHandler.ImageUpdate
        /// </summary>
        /// <param name="Value"></param>
        public void SDK_FrameRateUpdated(float Value)
        {
            this.Invoke((MethodInvoker)delegate { FrameRateLabel.Text = "FPS: " + Value.ToString("F2"); });

        }
        
        /// <summary>
        /// 电脑上增加相机触发事件
        /// </summary>
        public void SDK_CameraAdded()
        {
            RefreshCamera();
        }

        /// <summary>
        /// 会话按钮，打开或关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SessionButton_Click(object sender, EventArgs e)
        {
            if (CameraHandler.CameraSessionOpen) CloseSession();
            else OpenSession();
            //CameraHandler.test();

        }

        /// <summary>
        /// 刷新相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshCamera();
        }

        /// <summary>
        /// 实时取景打开关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LiveViewButton_Click(object sender, EventArgs e)
        {
            if (!CameraHandler.IsEvfFilming)
            {
                if (!CameraHandler.IsLiveViewOn) { CameraHandler.StartLiveView(); LiveViewButton.Text = "关闭实时取景"; }
                else { CameraHandler.StopLiveView(); LiveViewButton.Text = "打开实时取景"; }

                AvCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                TvCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                ISOCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                WBCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                WBUpDo.Enabled = !CameraHandler.IsLiveViewOn;
                SaveToGroupBox.Enabled = !CameraHandler.IsLiveViewOn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LiveView()
        {
            
            if (!CameraHandler.IsEvfFilming)
            {
                if (!CameraHandler.IsLiveViewOn) { CameraHandler.StartLiveView(); LiveViewButton.Text = "关闭实时取景"; }
                else { CameraHandler.StopLiveView(); LiveViewButton.Text = "打开实时取景"; }

                AvCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                TvCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                ISOCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                WBCoBox.Enabled = !CameraHandler.IsLiveViewOn;
                WBUpDo.Enabled = !CameraHandler.IsLiveViewOn;
                SaveToGroupBox.Enabled = !CameraHandler.IsLiveViewOn;
            }
        }


        //Thread LV = new Thread(new delegate(LiveView));

        //protected delegate void LV();

        //LV mylv = new LV(LiveView);

        /// <summary>
        /// 录制视频按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RecordButton_Click(object sender, EventArgs e)
        {
            if (!CameraHandler.IsEvfFilming && !CameraHandler.IsLiveViewOn) { SettingsGroupBox.Enabled = false; CameraHandler.StartEvfFilming(); RecordButton.Text = "Stop Recording"; }
            else if (CameraHandler.IsEvfFilming) { SettingsGroupBox.Enabled = true; CameraHandler.StopEvfFilming(); RecordButton.Text = "Start Recording"; }
        }

        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TakePhotoButton_Click(object sender, EventArgs e)
        {
            
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

                //CameraHandler.TakePhoto();

                if ((STComputerButton.Checked || STBothButton.Checked) && !Directory.Exists(SavePathTextBox.Text)) Directory.CreateDirectory(SavePathTextBox.Text);
                CameraHandler.ImageSaveDirectory = SavePathTextBox.Text + "\\" + SaveNameTextBox.Text;

                CameraHandler.TakePhoto();
            }
                );
            //A.IsBackground = true;
            A.Start();
            //A.Join();


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
            //A.IsBackground = true;
            //B.Start();

            //原始方法
            //if ((STComputerButton.Checked || STBothButton.Checked) && !Directory.Exists(SavePathTextBox.Text)) Directory.CreateDirectory(SavePathTextBox.Text);
            //CameraHandler.ImageSaveDirectory = SavePathTextBox.Text + "\\" + SaveNameTextBox.Text;

            //if ((string)TvCoBox.SelectedItem == "Bulb") CameraHandler.TakePhoto((uint)BulbUpDo.Value);
            //else CameraHandler.TakePhoto();

            //return SavePathTextBox.Text;

        }
        /// <summary>
        /// 选择路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BrowseButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(SavePathTextBox.Text)) 
                SaveFolderBrowser.SelectedPath = SavePathTextBox.Text;

            if (SaveFolderBrowser.ShowDialog() == DialogResult.OK) 
                SavePathTextBox.Text = SaveFolderBrowser.SelectedPath;
        }

        /// <summary>
        /// Av下拉列表组合框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_Av, CameraValues.AV((string)AvCoBox.SelectedItem));
        }
        /// <summary>
        /// Tv下拉列表组合框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TvCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_Tv, CameraValues.TV((string)TvCoBox.SelectedItem));

            if ((string)TvCoBox.SelectedItem == "Bulb") BulbUpDo.Enabled = true;
            else BulbUpDo.Enabled = false;
        }
        /// <summary>
        /// ISO下拉列表组合框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ISOCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_ISOSpeed, CameraValues.ISO((string)ISOCoBox.SelectedItem));
        }
        /// <summary>
        /// Wb下拉列表组合框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WBCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (WBCoBox.SelectedIndex)
            {
                case 0: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Auto); break;
                case 1: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Daylight); break;
                case 2: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Cloudy); break;
                case 3: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Tangsten); break;
                case 4: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Fluorescent); break;
                case 5: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Strobe); break;
                case 6: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_WhitePaper); break;
                case 7: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_Shade); break;
                case 8: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_ColorTemp); break;
                case 9: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_PCSet1); break;
                case 10: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_PCSet2); break;
                case 11: CameraHandler.SetSetting(EDSDK.PropID_WhiteBalance, EDSDK.WhiteBalance_PCSet3); break;
            }
            if (WBCoBox.SelectedIndex == 8) WBUpDo.Enabled = true;
            else WBUpDo.Enabled = false;
        }
        /// <summary>
        /// WB值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WBUpDo_ValueChanged(object sender, EventArgs e)
        {
            CameraHandler.SetSetting(EDSDK.PropID_ColorTemperature, (uint)WBUpDo.Value);
        }
        /// <summary>
        /// 设置保存路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">事件名称</param>
        public void SaveToButton_CheckedChanged(object sender, EventArgs e)
        {
            if (STCameraButton.Checked)
            {
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Camera);
                BrowseButton.Enabled = false;
                SavePathTextBox.Enabled = false;
            }
            else if (STComputerButton.Checked)
            {
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Host);

                //设置相机的存储
                CameraHandler.SetCapacity(4096, 1024 * 1024);

                BrowseButton.Enabled = true;
                SavePathTextBox.Enabled = true;
            }
            else if (STBothButton.Checked)
            {
                CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                BrowseButton.Enabled = true;
                SavePathTextBox.Enabled = true;
            }
        }

        /// <summary>
        /// 窗口关闭，释放资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CameraHandler.Dispose();
        }


        /**
        *关闭会话
        */
        public void CloseSession()
        {
            //调用照相驱动，关闭会话
            CameraHandler.CloseSession();
            //清空界面的设置
            AvCoBox.Items.Clear();
            TvCoBox.Items.Clear();
            ISOCoBox.Items.Clear();

            SettingsGroupBox.Enabled = false;
            LiveViewGroupBox.Enabled = false;

            SessionButton.Text = "打开会话";
            SessionLabel.Text = "没有回话打开";
        }

        /**
        *刷新相机
        */
        public void RefreshCamera()
        {
            //关闭会话
            CloseSession();
            //清空界面上的相机列表
            CameraListBox.Items.Clear();
            //利用相机驱动，获取相机列表
            CamList = CameraHandler.GetCameraList();
            //利用增强for循环为listBox设置值
            foreach (Camera cam in CamList) CameraListBox.Items.Add(cam.Info.szDeviceDescription);
            //如果相机个数大于0个，则默认选择第一个
            if (CamList.Count > 0) CameraListBox.SelectedIndex = 0;
        }

        /**
        *打开会话
        */
        public void OpenSession()
        {
            LiveViewGroupBox.Enabled = true;
            //如果当前相机里表中有选中项，则打开
            if (CameraListBox.SelectedIndex >= 0)
            {
                //利用相机的驱动打开选中的相机
                CameraHandler.OpenSession(CamList[CameraListBox.SelectedIndex]);
                //CameraHandler.OpenSession(null);
                //设置会话按钮
                SessionButton.Text = "关闭会话";
                
                SessionLabel.Text = CameraHandler.MainCamera.Info.szDeviceDescription;

                AvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Av);
                TvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Tv);
                ISOList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_ISOSpeed);

                foreach (int Av in AvList) AvCoBox.Items.Add(CameraValues.AV((uint)Av));
                foreach (int Tv in TvList) TvCoBox.Items.Add(CameraValues.TV((uint)Tv));
                foreach (int ISO in ISOList) ISOCoBox.Items.Add(CameraValues.ISO((uint)ISO));

                AvCoBox.SelectedIndex = AvCoBox.Items.IndexOf(CameraValues.AV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Av)));
                TvCoBox.SelectedIndex = TvCoBox.Items.IndexOf(CameraValues.TV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Tv)));
                ISOCoBox.SelectedIndex = ISOCoBox.Items.IndexOf(CameraValues.ISO((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_ISOSpeed)));

                int wbidx = (int)CameraHandler.GetSetting((uint)EDSDK.PropID_WhiteBalance);
                WBCoBox.SelectedIndex = (wbidx > 8) ? wbidx - 1 : wbidx;

                try
                {
                    WBUpDo.Value = CameraHandler.GetSetting((uint)EDSDK.PropID_ColorTemperature);
                }
                catch (Exception e)
                {
                    
                }
               
                SettingsGroupBox.Enabled = true;
                LiveViewGroupBox.Enabled = true;
            }
        }

        public void SessionLabel_Click(object sender, EventArgs e)
        {

        }

        public void MainForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 记录鼠标的位置
        /// </summary>
        private Point mouse_offset;

        /// <summary>
        /// 使校正控件可以拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
               
            }

        }


        /// <summary>
        /// 使校正控件可以拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //①按下鼠标的位置，相对pictureBox1的坐标
            mouse_offset = new Point(-e.X, -e.Y);
        }
      

        /// <summary>
        /// 使校正控件可以拖动，处理图片裁剪和裁剪框的移动位置
        /// </summary> 
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //②获取鼠标光标的位置（以屏幕坐标表示）
                Point mousePos = Control.MousePosition;

                //③将鼠标光标坐标平移指定的量,获取子控件左上角的相对屏幕的坐标，重要！                
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);



                //获取该控件的坐标位置
                //((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);

                //控件自身，也就是pictureBox1
                Control control = ((Control)sender);

                //Point p = control.Location;
                //父控件
                Control parent = control.Parent;

                //④鼠标在父控件（实时预览）工作区坐标的位置
                Point point = control.Parent.PointToClient(mousePos);

                //父控件相对屏幕位置，无用
                Point pp = parent.Location;     


                int parentWidth = parent.Width;
                int parentHeight = parent.Height;

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

                if (point.X+ controlWidth >= parentWidth)
                {
                    point.X = parentWidth- controlWidth;
                }
              
                if (point.Y+ controlHeight >= parentHeight)
                {
                    point.Y = parentHeight- controlHeight;
                }

                CameraHandler.x = (int)(1.0 * control.Left * CameraHandler.W / parentWidth);
                CameraHandler.y = (int)(1.0 * control.Top * CameraHandler.H / parentHeight);


                CameraHandler.width = (int)(1.0 * controlWidth * CameraHandler.W / parentWidth);
                CameraHandler.heigth = (int)(1.0 * controlHeight * CameraHandler.H / parentHeight);

                control.Location = point;
            }
        }


    }
}
