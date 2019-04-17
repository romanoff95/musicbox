namespace musicbox.Menu
{
    partial class UserPermissonForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <paramFormat name="disposing">true if managed resources should be disposed; otherwise, false.</paramFormat>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxPermMoneylessModeUse = new System.Windows.Forms.CheckBox();
            this.checkBoxPermVolumeChange = new System.Windows.Forms.CheckBox();
            this.checkBoxPermTimeChange = new System.Windows.Forms.CheckBox();
            this.checkBoxPermPriceChange = new System.Windows.Forms.CheckBox();
            this.checkBoxPermViewCounters = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxPermMoneylessModeUse);
            this.groupBox1.Controls.Add(this.checkBoxPermVolumeChange);
            this.groupBox1.Controls.Add(this.checkBoxPermTimeChange);
            this.groupBox1.Controls.Add(this.checkBoxPermPriceChange);
            this.groupBox1.Controls.Add(this.checkBoxPermViewCounters);
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 296);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // checkBoxPermMoneylessModeUse
            // 
            this.checkBoxPermMoneylessModeUse.AutoSize = true;
            this.checkBoxPermMoneylessModeUse.Location = new System.Drawing.Point(8, 214);
            this.checkBoxPermMoneylessModeUse.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.checkBoxPermMoneylessModeUse.Name = "checkBoxPermMoneylessModeUse";
            this.checkBoxPermMoneylessModeUse.Size = new System.Drawing.Size(532, 28);
            this.checkBoxPermMoneylessModeUse.TabIndex = 9;
            this.checkBoxPermMoneylessModeUse.Text = "Разрешить пользователю включать бесплатный режим";
            this.checkBoxPermMoneylessModeUse.UseVisualStyleBackColor = true;
            // 
            // checkBoxPermVolumeChange
            // 
            this.checkBoxPermVolumeChange.AutoSize = true;
            this.checkBoxPermVolumeChange.Location = new System.Drawing.Point(8, 171);
            this.checkBoxPermVolumeChange.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.checkBoxPermVolumeChange.Name = "checkBoxPermVolumeChange";
            this.checkBoxPermVolumeChange.Size = new System.Drawing.Size(435, 28);
            this.checkBoxPermVolumeChange.TabIndex = 8;
            this.checkBoxPermVolumeChange.Text = "Разрешить пользователю менять громкость";
            this.checkBoxPermVolumeChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxPermTimeChange
            // 
            this.checkBoxPermTimeChange.AutoSize = true;
            this.checkBoxPermTimeChange.Location = new System.Drawing.Point(8, 129);
            this.checkBoxPermTimeChange.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.checkBoxPermTimeChange.Name = "checkBoxPermTimeChange";
            this.checkBoxPermTimeChange.Size = new System.Drawing.Size(396, 28);
            this.checkBoxPermTimeChange.TabIndex = 7;
            this.checkBoxPermTimeChange.Text = "Разрешить пользователю менять время";
            this.checkBoxPermTimeChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxPermPriceChange
            // 
            this.checkBoxPermPriceChange.AutoSize = true;
            this.checkBoxPermPriceChange.Location = new System.Drawing.Point(8, 87);
            this.checkBoxPermPriceChange.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.checkBoxPermPriceChange.Name = "checkBoxPermPriceChange";
            this.checkBoxPermPriceChange.Size = new System.Drawing.Size(437, 28);
            this.checkBoxPermPriceChange.TabIndex = 6;
            this.checkBoxPermPriceChange.Text = "Разрешить пользователю менять стоимость";
            this.checkBoxPermPriceChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxPermViewCounters
            // 
            this.checkBoxPermViewCounters.AutoSize = true;
            this.checkBoxPermViewCounters.Location = new System.Drawing.Point(8, 43);
            this.checkBoxPermViewCounters.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.checkBoxPermViewCounters.Name = "checkBoxPermViewCounters";
            this.checkBoxPermViewCounters.Size = new System.Drawing.Size(422, 28);
            this.checkBoxPermViewCounters.TabIndex = 5;
            this.checkBoxPermViewCounters.Text = "Разрешить пользователю видеть счётчики";
            this.checkBoxPermViewCounters.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(456, 309);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Закрыть";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // UserPermissonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(573, 349);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserPermissonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserPermisson";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.CheckBox checkBoxPermMoneylessModeUse;
        public System.Windows.Forms.CheckBox checkBoxPermVolumeChange;
        public System.Windows.Forms.CheckBox checkBoxPermTimeChange;
        public System.Windows.Forms.CheckBox checkBoxPermPriceChange;
        public System.Windows.Forms.CheckBox checkBoxPermViewCounters;
        private System.Windows.Forms.Button btnCancel;
    }
}