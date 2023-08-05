
namespace Game_Set
{
    partial class Form_Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkbox_CheckAll = new MetroFramework.Controls.MetroCheckBox();
            this.progressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.label_MonitorBright = new MetroFramework.Controls.MetroLabel();
            this.button_Apply = new MetroFramework.Controls.MetroButton();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.trackbar_Brightness = new MetroFramework.Controls.MetroTrackBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.checkbox_CheckAll, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_MonitorBright, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.button_Apply, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkedListBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.trackbar_Brightness, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.35857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.64143F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(252, 411);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // checkbox_CheckAll
            // 
            this.checkbox_CheckAll.AutoSize = true;
            this.checkbox_CheckAll.CustomBackground = false;
            this.checkbox_CheckAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkbox_CheckAll.FontSize = MetroFramework.MetroLinkSize.Small;
            this.checkbox_CheckAll.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.checkbox_CheckAll.Location = new System.Drawing.Point(3, 3);
            this.checkbox_CheckAll.Name = "checkbox_CheckAll";
            this.checkbox_CheckAll.Size = new System.Drawing.Size(246, 24);
            this.checkbox_CheckAll.Style = MetroFramework.MetroColorStyle.Blue;
            this.checkbox_CheckAll.StyleManager = null;
            this.checkbox_CheckAll.TabIndex = 10;
            this.checkbox_CheckAll.Text = "모두선택";
            this.checkbox_CheckAll.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.checkbox_CheckAll.UseStyleColors = false;
            this.checkbox_CheckAll.UseVisualStyleBackColor = true;
            this.checkbox_CheckAll.CheckedChanged += new System.EventHandler(this.checkBox_checkAll_CheckedChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.FontSize = MetroFramework.MetroProgressBarSize.Medium;
            this.progressBar1.FontWeight = MetroFramework.MetroProgressBarWeight.Light;
            this.progressBar1.HideProgressText = true;
            this.progressBar1.Location = new System.Drawing.Point(3, 301);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.ProgressBarStyle = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.Size = new System.Drawing.Size(246, 21);
            this.progressBar1.Style = MetroFramework.MetroColorStyle.Blue;
            this.progressBar1.StyleManager = null;
            this.progressBar1.TabIndex = 8;
            this.progressBar1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.progressBar1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // label_MonitorBright
            // 
            this.label_MonitorBright.AutoSize = true;
            this.label_MonitorBright.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(1)))), ((int)(((byte)(178)))), ((int)(((byte)(7)))));
            this.label_MonitorBright.CustomBackground = false;
            this.label_MonitorBright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_MonitorBright.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.label_MonitorBright.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.label_MonitorBright.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.label_MonitorBright.Location = new System.Drawing.Point(3, 370);
            this.label_MonitorBright.Name = "label_MonitorBright";
            this.label_MonitorBright.Size = new System.Drawing.Size(246, 20);
            this.label_MonitorBright.Style = MetroFramework.MetroColorStyle.Blue;
            this.label_MonitorBright.StyleManager = null;
            this.label_MonitorBright.TabIndex = 9;
            this.label_MonitorBright.Text = "모니터 밝기";
            this.label_MonitorBright.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.label_MonitorBright.UseStyleColors = false;
            // 
            // button_Apply
            // 
            this.button_Apply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Apply.Highlight = false;
            this.button_Apply.Location = new System.Drawing.Point(3, 328);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(246, 39);
            this.button_Apply.Style = MetroFramework.MetroColorStyle.Blue;
            this.button_Apply.StyleManager = null;
            this.button_Apply.TabIndex = 7;
            this.button_Apply.Text = "적용";
            this.button_Apply.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(3, 34);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(246, 260);
            this.checkedListBox1.TabIndex = 0;
            // 
            // trackbar_Brightness
            // 
            this.trackbar_Brightness.BackColor = System.Drawing.Color.Transparent;
            this.trackbar_Brightness.CustomBackground = false;
            this.trackbar_Brightness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackbar_Brightness.LargeChange = ((uint)(5u));
            this.trackbar_Brightness.Location = new System.Drawing.Point(3, 393);
            this.trackbar_Brightness.Maximum = 100;
            this.trackbar_Brightness.Minimum = 0;
            this.trackbar_Brightness.MouseWheelBarPartitions = 10;
            this.trackbar_Brightness.Name = "trackbar_Brightness";
            this.trackbar_Brightness.Size = new System.Drawing.Size(246, 15);
            this.trackbar_Brightness.SmallChange = ((uint)(1u));
            this.trackbar_Brightness.Style = MetroFramework.MetroColorStyle.Blue;
            this.trackbar_Brightness.StyleManager = null;
            this.trackbar_Brightness.TabIndex = 6;
            this.trackbar_Brightness.Text = "밝기";
            this.trackbar_Brightness.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.trackbar_Brightness.Value = 50;
            this.trackbar_Brightness.ValueChanged += new System.EventHandler(this.trackbar_Brightness_ValueChanged);
            this.trackbar_Brightness.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackbar_Brightness_KeyUp);
            this.trackbar_Brightness.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackbar_Brightness_MouseUp);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(252, 411);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Main";
            this.Text = "PCBang-Set";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MetroFramework.Controls.MetroCheckBox checkbox_CheckAll;
        private MetroFramework.Controls.MetroProgressBar progressBar1;
        private MetroFramework.Controls.MetroLabel label_MonitorBright;
        private MetroFramework.Controls.MetroButton button_Apply;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private MetroFramework.Controls.MetroTrackBar trackbar_Brightness;
    }
}

