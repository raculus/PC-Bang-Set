
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button_Apply = new System.Windows.Forms.Button();
            this.checkBox_checkAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 42);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(213, 204);
            this.checkedListBox1.TabIndex = 0;
            // 
            // button_Apply
            // 
            this.button_Apply.Location = new System.Drawing.Point(12, 253);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(213, 37);
            this.button_Apply.TabIndex = 1;
            this.button_Apply.Text = "적용";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // checkBox_checkAll
            // 
            this.checkBox_checkAll.AutoSize = true;
            this.checkBox_checkAll.Location = new System.Drawing.Point(12, 12);
            this.checkBox_checkAll.Name = "checkBox_checkAll";
            this.checkBox_checkAll.Size = new System.Drawing.Size(79, 21);
            this.checkBox_checkAll.TabIndex = 3;
            this.checkBox_checkAll.Text = "모두선택";
            this.checkBox_checkAll.UseVisualStyleBackColor = true;
            this.checkBox_checkAll.CheckedChanged += new System.EventHandler(this.checkBox_checkAll_CheckedChanged);
            // 
            // Form_Main
            // 
            this.AcceptButton = this.button_Apply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 303);
            this.Controls.Add(this.checkBox_checkAll);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.checkedListBox1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Main";
            this.Text = "PCBang-Set";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.CheckBox checkBox_checkAll;
    }
}

