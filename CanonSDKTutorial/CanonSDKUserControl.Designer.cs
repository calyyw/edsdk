namespace CanonSDKTutorial
{
    partial class CanonSDKUserControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button = new System.Windows.Forms.Button();
            this.CameraListBox = new System.Windows.Forms.ListBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.LiveViewPicBox = new System.Windows.Forms.PictureBox();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(1068, 66);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(115, 40);
            this.button.TabIndex = 0;
            this.button.Text = "测试";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // CameraListBox
            // 
            this.CameraListBox.FormattingEnabled = true;
            this.CameraListBox.ItemHeight = 12;
            this.CameraListBox.Location = new System.Drawing.Point(1068, 10);
            this.CameraListBox.Name = "CameraListBox";
            this.CameraListBox.Size = new System.Drawing.Size(115, 40);
            this.CameraListBox.TabIndex = 1;
            this.CameraListBox.SelectedIndexChanged += new System.EventHandler(this.CameraListBox_SelectedIndexChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.pictureBox.Image = global::CanonSDKTutorial.Properties.Resources.tx;
            this.pictureBox.Location = new System.Drawing.Point(271, 80);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(358, 441);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 7;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            // 
            // LiveViewPicBox
            // 
            this.LiveViewPicBox.Location = new System.Drawing.Point(0, 0);
            this.LiveViewPicBox.Name = "LiveViewPicBox";
            this.LiveViewPicBox.Size = new System.Drawing.Size(900, 600);
            this.LiveViewPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LiveViewPicBox.TabIndex = 2;
            this.LiveViewPicBox.TabStop = false;
            this.LiveViewPicBox.Click += new System.EventHandler(this.LiveViewPicBox_Click);
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(960, 10);
            this.trackBar.Maximum = 255;
            this.trackBar.Name = "trackBar";
            this.trackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar.Size = new System.Drawing.Size(45, 600);
            this.trackBar.TabIndex = 8;
            this.trackBar.Value = 235;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(1114, 138);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(23, 12);
            this.label.TabIndex = 9;
            this.label.Text = "235";
            this.label.Click += new System.EventHandler(this.label_Click);
            // 
            // CanonSDKUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.LiveViewPicBox);
            this.Controls.Add(this.CameraListBox);
            this.Controls.Add(this.button);
            this.Controls.Add(this.trackBar);
            this.Name = "CanonSDKUserControl";
            this.Size = new System.Drawing.Size(1228, 600);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button;
        private System.Windows.Forms.ListBox CameraListBox;
        private System.Windows.Forms.PictureBox LiveViewPicBox;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label label;
    }
}
