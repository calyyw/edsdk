namespace CanonSDKTutorial
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.LiveViewButton = new System.Windows.Forms.Button();
            this.RecordButton = new System.Windows.Forms.Button();
            this.FrameRateLabel = new System.Windows.Forms.Label();
            this.CameraListBox = new System.Windows.Forms.ListBox();
            this.SessionButton = new System.Windows.Forms.Button();
            this.SessionLabel = new System.Windows.Forms.Label();
            this.InitGroupBox = new System.Windows.Forms.GroupBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.LiveViewGroupBox = new System.Windows.Forms.GroupBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.LiveViewPicBox = new System.Windows.Forms.PictureBox();
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.WBUpDo = new System.Windows.Forms.NumericUpDown();
            this.WBCoBox = new System.Windows.Forms.ComboBox();
            this.SaveNameTextBox = new System.Windows.Forms.TextBox();
            this.SavePathTextBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.SaveToGroupBox = new System.Windows.Forms.GroupBox();
            this.STBothButton = new System.Windows.Forms.RadioButton();
            this.STComputerButton = new System.Windows.Forms.RadioButton();
            this.STCameraButton = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TakePhotoButton = new System.Windows.Forms.Button();
            this.BulbUpDo = new System.Windows.Forms.NumericUpDown();
            this.ISOCoBox = new System.Windows.Forms.ComboBox();
            this.TvCoBox = new System.Windows.Forms.ComboBox();
            this.AvCoBox = new System.Windows.Forms.ComboBox();
            this.SaveFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.InitGroupBox.SuspendLayout();
            this.LiveViewGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewPicBox)).BeginInit();
            this.SettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WBUpDo)).BeginInit();
            this.SaveToGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BulbUpDo)).BeginInit();
            this.SuspendLayout();
            // 
            // LiveViewButton
            // 
            this.LiveViewButton.Location = new System.Drawing.Point(24, 630);
            this.LiveViewButton.Name = "LiveViewButton";
            this.LiveViewButton.Size = new System.Drawing.Size(100, 21);
            this.LiveViewButton.TabIndex = 2;
            this.LiveViewButton.Text = "打开实时界面";
            this.LiveViewButton.UseVisualStyleBackColor = true;
            this.LiveViewButton.Click += new System.EventHandler(this.LiveViewButton_Click);
            // 
            // RecordButton
            // 
            this.RecordButton.Location = new System.Drawing.Point(157, 630);
            this.RecordButton.Name = "RecordButton";
            this.RecordButton.Size = new System.Drawing.Size(100, 21);
            this.RecordButton.TabIndex = 3;
            this.RecordButton.Text = "录像";
            this.RecordButton.UseVisualStyleBackColor = true;
            this.RecordButton.Click += new System.EventHandler(this.RecordButton_Click);
            // 
            // FrameRateLabel
            // 
            this.FrameRateLabel.AutoSize = true;
            this.FrameRateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FrameRateLabel.Location = new System.Drawing.Point(272, 632);
            this.FrameRateLabel.Name = "FrameRateLabel";
            this.FrameRateLabel.Size = new System.Drawing.Size(50, 15);
            this.FrameRateLabel.TabIndex = 5;
            this.FrameRateLabel.Text = "FPS: 24";
            // 
            // CameraListBox
            // 
            this.CameraListBox.FormattingEnabled = true;
            this.CameraListBox.ItemHeight = 12;
            this.CameraListBox.Location = new System.Drawing.Point(6, 42);
            this.CameraListBox.Name = "CameraListBox";
            this.CameraListBox.Size = new System.Drawing.Size(121, 136);
            this.CameraListBox.TabIndex = 6;
            // 
            // SessionButton
            // 
            this.SessionButton.Location = new System.Drawing.Point(9, 189);
            this.SessionButton.Name = "SessionButton";
            this.SessionButton.Size = new System.Drawing.Size(84, 21);
            this.SessionButton.TabIndex = 7;
            this.SessionButton.Text = "打开会话";
            this.SessionButton.UseVisualStyleBackColor = true;
            this.SessionButton.Click += new System.EventHandler(this.SessionButton_Click);
            // 
            // SessionLabel
            // 
            this.SessionLabel.AutoSize = true;
            this.SessionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionLabel.Location = new System.Drawing.Point(6, 15);
            this.SessionLabel.Name = "SessionLabel";
            this.SessionLabel.Size = new System.Drawing.Size(113, 16);
            this.SessionLabel.TabIndex = 8;
            this.SessionLabel.Text = "没有打开的会话";
            this.SessionLabel.Click += new System.EventHandler(this.SessionLabel_Click);
            // 
            // InitGroupBox
            // 
            this.InitGroupBox.Controls.Add(this.RefreshButton);
            this.InitGroupBox.Controls.Add(this.CameraListBox);
            this.InitGroupBox.Controls.Add(this.SessionLabel);
            this.InitGroupBox.Controls.Add(this.SessionButton);
            this.InitGroupBox.Location = new System.Drawing.Point(12, 11);
            this.InitGroupBox.Name = "InitGroupBox";
            this.InitGroupBox.Size = new System.Drawing.Size(135, 218);
            this.InitGroupBox.TabIndex = 9;
            this.InitGroupBox.TabStop = false;
            this.InitGroupBox.Text = "初始化";
            // 
            // RefreshButton
            // 
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshButton.Location = new System.Drawing.Point(99, 189);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(31, 21);
            this.RefreshButton.TabIndex = 9;
            this.RefreshButton.Text = "↻";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // LiveViewGroupBox
            // 
            this.LiveViewGroupBox.Controls.Add(this.pictureBox);
            this.LiveViewGroupBox.Controls.Add(this.LiveViewButton);
            this.LiveViewGroupBox.Controls.Add(this.FrameRateLabel);
            this.LiveViewGroupBox.Controls.Add(this.RecordButton);
            this.LiveViewGroupBox.Controls.Add(this.LiveViewPicBox);
            this.LiveViewGroupBox.Location = new System.Drawing.Point(153, 11);
            this.LiveViewGroupBox.Name = "LiveViewGroupBox";
            this.LiveViewGroupBox.Size = new System.Drawing.Size(920, 665);
            this.LiveViewGroupBox.TabIndex = 10;
            this.LiveViewGroupBox.TabStop = false;
            this.LiveViewGroupBox.Text = "实时界面";
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.pictureBox.Image = global::CanonSDKTutorial.Properties.Resources.tx;
            this.pictureBox.Location = new System.Drawing.Point(184, 42);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(358, 441);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 6;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // LiveViewPicBox
            // 
            this.LiveViewPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LiveViewPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LiveViewPicBox.Location = new System.Drawing.Point(11, 19);
            this.LiveViewPicBox.Name = "LiveViewPicBox";
            this.LiveViewPicBox.Size = new System.Drawing.Size(900, 600);
            this.LiveViewPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LiveViewPicBox.TabIndex = 1;
            this.LiveViewPicBox.TabStop = false;
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Controls.Add(this.label7);
            this.SettingsGroupBox.Controls.Add(this.WBUpDo);
            this.SettingsGroupBox.Controls.Add(this.WBCoBox);
            this.SettingsGroupBox.Controls.Add(this.SaveNameTextBox);
            this.SettingsGroupBox.Controls.Add(this.SavePathTextBox);
            this.SettingsGroupBox.Controls.Add(this.BrowseButton);
            this.SettingsGroupBox.Controls.Add(this.SaveToGroupBox);
            this.SettingsGroupBox.Controls.Add(this.label4);
            this.SettingsGroupBox.Controls.Add(this.label3);
            this.SettingsGroupBox.Controls.Add(this.label2);
            this.SettingsGroupBox.Controls.Add(this.label6);
            this.SettingsGroupBox.Controls.Add(this.label5);
            this.SettingsGroupBox.Controls.Add(this.label1);
            this.SettingsGroupBox.Controls.Add(this.TakePhotoButton);
            this.SettingsGroupBox.Controls.Add(this.BulbUpDo);
            this.SettingsGroupBox.Controls.Add(this.ISOCoBox);
            this.SettingsGroupBox.Controls.Add(this.TvCoBox);
            this.SettingsGroupBox.Controls.Add(this.AvCoBox);
            this.SettingsGroupBox.Location = new System.Drawing.Point(17, 682);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(435, 167);
            this.SettingsGroupBox.TabIndex = 11;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "设置";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(334, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 16);
            this.label7.TabIndex = 9;
            this.label7.Text = "照片名称";
            // 
            // WBUpDo
            // 
            this.WBUpDo.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.WBUpDo.Location = new System.Drawing.Point(176, 42);
            this.WBUpDo.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.WBUpDo.Minimum = new decimal(new int[] {
            2800,
            0,
            0,
            0});
            this.WBUpDo.Name = "WBUpDo";
            this.WBUpDo.ReadOnly = true;
            this.WBUpDo.Size = new System.Drawing.Size(110, 21);
            this.WBUpDo.TabIndex = 8;
            this.WBUpDo.Value = new decimal(new int[] {
            5600,
            0,
            0,
            0});
            this.WBUpDo.ValueChanged += new System.EventHandler(this.WBUpDo_ValueChanged);
            // 
            // WBCoBox
            // 
            this.WBCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WBCoBox.FormattingEnabled = true;
            this.WBCoBox.Items.AddRange(new object[] {
            "Auto",
            "Daylight",
            "Cloudy",
            "Tangsten",
            "Fluorescent",
            "Strobe",
            "White Paper",
            "Shade",
            "Color Temperature",
            "PCSet1",
            "PCSet2",
            "PCSet3"});
            this.WBCoBox.Location = new System.Drawing.Point(176, 18);
            this.WBCoBox.Name = "WBCoBox";
            this.WBCoBox.Size = new System.Drawing.Size(110, 20);
            this.WBCoBox.TabIndex = 7;
            this.WBCoBox.SelectedIndexChanged += new System.EventHandler(this.WBCoBox_SelectedIndexChanged);
            // 
            // SaveNameTextBox
            // 
            this.SaveNameTextBox.Location = new System.Drawing.Point(6, 140);
            this.SaveNameTextBox.Name = "SaveNameTextBox";
            this.SaveNameTextBox.Size = new System.Drawing.Size(316, 21);
            this.SaveNameTextBox.TabIndex = 6;
            // 
            // SavePathTextBox
            // 
            this.SavePathTextBox.Location = new System.Drawing.Point(6, 116);
            this.SavePathTextBox.Name = "SavePathTextBox";
            this.SavePathTextBox.Size = new System.Drawing.Size(316, 21);
            this.SavePathTextBox.TabIndex = 6;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(328, 114);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(99, 21);
            this.BrowseButton.TabIndex = 5;
            this.BrowseButton.Text = "选择文件存储位置";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // SaveToGroupBox
            // 
            this.SaveToGroupBox.Controls.Add(this.STBothButton);
            this.SaveToGroupBox.Controls.Add(this.STComputerButton);
            this.SaveToGroupBox.Controls.Add(this.STCameraButton);
            this.SaveToGroupBox.Location = new System.Drawing.Point(331, 18);
            this.SaveToGroupBox.Name = "SaveToGroupBox";
            this.SaveToGroupBox.Size = new System.Drawing.Size(96, 92);
            this.SaveToGroupBox.TabIndex = 4;
            this.SaveToGroupBox.TabStop = false;
            this.SaveToGroupBox.Text = "保存在";
            // 
            // STBothButton
            // 
            this.STBothButton.AutoSize = true;
            this.STBothButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STBothButton.Location = new System.Drawing.Point(6, 66);
            this.STBothButton.Name = "STBothButton";
            this.STBothButton.Size = new System.Drawing.Size(86, 20);
            this.STBothButton.TabIndex = 0;
            this.STBothButton.Text = "两个都存";
            this.STBothButton.UseVisualStyleBackColor = true;
            this.STBothButton.CheckedChanged += new System.EventHandler(this.SaveToButton_CheckedChanged);
            // 
            // STComputerButton
            // 
            this.STComputerButton.AutoSize = true;
            this.STComputerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STComputerButton.Location = new System.Drawing.Point(6, 42);
            this.STComputerButton.Name = "STComputerButton";
            this.STComputerButton.Size = new System.Drawing.Size(56, 20);
            this.STComputerButton.TabIndex = 0;
            this.STComputerButton.Text = "电脑";
            this.STComputerButton.UseVisualStyleBackColor = true;
            this.STComputerButton.CheckedChanged += new System.EventHandler(this.SaveToButton_CheckedChanged);
            // 
            // STCameraButton
            // 
            this.STCameraButton.AutoSize = true;
            this.STCameraButton.Checked = true;
            this.STCameraButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STCameraButton.Location = new System.Drawing.Point(6, 18);
            this.STCameraButton.Name = "STCameraButton";
            this.STCameraButton.Size = new System.Drawing.Size(56, 20);
            this.STCameraButton.TabIndex = 0;
            this.STCameraButton.TabStop = true;
            this.STCameraButton.Text = "相机";
            this.STCameraButton.UseVisualStyleBackColor = true;
            this.STCameraButton.CheckedChanged += new System.EventHandler(this.SaveToButton_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(106, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Bulb (s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(106, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "ISO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(106, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tv";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(292, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 16);
            this.label6.TabIndex = 3;
            this.label6.Text = "K";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(292, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "WB";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(106, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Av";
            // 
            // TakePhotoButton
            // 
            this.TakePhotoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TakePhotoButton.Location = new System.Drawing.Point(240, 66);
            this.TakePhotoButton.Name = "TakePhotoButton";
            this.TakePhotoButton.Size = new System.Drawing.Size(82, 44);
            this.TakePhotoButton.TabIndex = 2;
            this.TakePhotoButton.Text = "照相";
            this.TakePhotoButton.UseVisualStyleBackColor = true;
            this.TakePhotoButton.Click += new System.EventHandler(this.TakePhotoButton_Click);
            // 
            // BulbUpDo
            // 
            this.BulbUpDo.Location = new System.Drawing.Point(6, 92);
            this.BulbUpDo.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.BulbUpDo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BulbUpDo.Name = "BulbUpDo";
            this.BulbUpDo.Size = new System.Drawing.Size(94, 21);
            this.BulbUpDo.TabIndex = 1;
            this.BulbUpDo.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // ISOCoBox
            // 
            this.ISOCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ISOCoBox.FormattingEnabled = true;
            this.ISOCoBox.Location = new System.Drawing.Point(6, 67);
            this.ISOCoBox.Name = "ISOCoBox";
            this.ISOCoBox.Size = new System.Drawing.Size(94, 20);
            this.ISOCoBox.TabIndex = 0;
            this.ISOCoBox.SelectedIndexChanged += new System.EventHandler(this.ISOCoBox_SelectedIndexChanged);
            // 
            // TvCoBox
            // 
            this.TvCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TvCoBox.FormattingEnabled = true;
            this.TvCoBox.Location = new System.Drawing.Point(6, 42);
            this.TvCoBox.Name = "TvCoBox";
            this.TvCoBox.Size = new System.Drawing.Size(94, 20);
            this.TvCoBox.TabIndex = 0;
            this.TvCoBox.SelectedIndexChanged += new System.EventHandler(this.TvCoBox_SelectedIndexChanged);
            // 
            // AvCoBox
            // 
            this.AvCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AvCoBox.FormattingEnabled = true;
            this.AvCoBox.Location = new System.Drawing.Point(6, 18);
            this.AvCoBox.Name = "AvCoBox";
            this.AvCoBox.Size = new System.Drawing.Size(94, 20);
            this.AvCoBox.TabIndex = 0;
            this.AvCoBox.SelectedIndexChanged += new System.EventHandler(this.AvCoBox_SelectedIndexChanged);
            // 
            // SaveFolderBrowser
            // 
            this.SaveFolderBrowser.Description = "Save Images To...";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 861);
            this.Controls.Add(this.SettingsGroupBox);
            this.Controls.Add(this.LiveViewGroupBox);
            this.Controls.Add(this.InitGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Canon SDK 教程";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.InitGroupBox.ResumeLayout(false);
            this.InitGroupBox.PerformLayout();
            this.LiveViewGroupBox.ResumeLayout(false);
            this.LiveViewGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewPicBox)).EndInit();
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WBUpDo)).EndInit();
            this.SaveToGroupBox.ResumeLayout(false);
            this.SaveToGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BulbUpDo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox LiveViewPicBox;
        private System.Windows.Forms.Button LiveViewButton;
        private System.Windows.Forms.Button RecordButton;
        private System.Windows.Forms.Label FrameRateLabel;
        private System.Windows.Forms.ListBox CameraListBox;
        private System.Windows.Forms.Button SessionButton;
        private System.Windows.Forms.Label SessionLabel;
        private System.Windows.Forms.GroupBox InitGroupBox;
        private System.Windows.Forms.GroupBox LiveViewGroupBox;
        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.Button TakePhotoButton;
        private System.Windows.Forms.NumericUpDown BulbUpDo;
        private System.Windows.Forms.ComboBox ISOCoBox;
        private System.Windows.Forms.ComboBox TvCoBox;
        private System.Windows.Forms.ComboBox AvCoBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox SaveToGroupBox;
        private System.Windows.Forms.RadioButton STBothButton;
        private System.Windows.Forms.RadioButton STComputerButton;
        private System.Windows.Forms.RadioButton STCameraButton;
        private System.Windows.Forms.TextBox SavePathTextBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.FolderBrowserDialog SaveFolderBrowser;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.ComboBox WBCoBox;
        private System.Windows.Forms.NumericUpDown WBUpDo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox SaveNameTextBox;
    }
}

