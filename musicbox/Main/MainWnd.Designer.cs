using System;
namespace musicbox.Main
{
    partial class MainWnd
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
            /*lock (locker)
            {
                //mciSendString("close all", null, 0, IntPtr.Zero);
                PlayingQueue.Clear();
                PlayingQueue.Enqueue(null);
            }*/
            //wh.Set();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
            this.pbTitles = new System.Windows.Forms.PictureBox();
            this.pbTopTen = new System.Windows.Forms.PictureBox();
            this.pbOrder = new System.Windows.Forms.PictureBox();
            this.progressBarPlayList = new System.Windows.Forms.ProgressBar();
            this.pbPlayListItems = new System.Windows.Forms.PictureBox();
            this.pbPlayListBottomScrollBtn = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pbPlayListScrollBar = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pbPlayListTopScrollBtn = new System.Windows.Forms.PictureBox();
            this.pbPriceList = new System.Windows.Forms.PictureBox();
            this.axShockwaveFlash1 = new AxShockwaveFlashObjects.AxShockwaveFlash();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTopTen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayListItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayListBottomScrollBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayListScrollBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayListTopScrollBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPriceList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash1)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTitles
            // 
            this.pbTitles.Image = ((System.Drawing.Image)(resources.GetObject("pbTitles.Image")));
            this.pbTitles.Location = new System.Drawing.Point(0, 259);
            this.pbTitles.Name = "pbTitles";
            this.pbTitles.Size = new System.Drawing.Size(1280, 94);
            this.pbTitles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbTitles.TabIndex = 6;
            this.pbTitles.TabStop = false;
            // 
            // pbTopTen
            // 
            this.pbTopTen.Image = ((System.Drawing.Image)(resources.GetObject("pbTopTen.Image")));
            this.pbTopTen.Location = new System.Drawing.Point(744, 354);
            this.pbTopTen.Name = "pbTopTen";
            this.pbTopTen.Size = new System.Drawing.Size(534, 423);
            this.pbTopTen.TabIndex = 24;
            this.pbTopTen.TabStop = false;
            // 
            // pbOrder
            // 
            this.pbOrder.Image = ((System.Drawing.Image)(resources.GetObject("pbOrder.Image")));
            this.pbOrder.Location = new System.Drawing.Point(744, 774);
            this.pbOrder.Name = "pbOrder";
            this.pbOrder.Size = new System.Drawing.Size(536, 252);
            this.pbOrder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbOrder.TabIndex = 25;
            this.pbOrder.TabStop = false;
            // 
            // progressBarPlayList
            // 
            this.progressBarPlayList.BackColor = System.Drawing.Color.Black;
            this.progressBarPlayList.ForeColor = System.Drawing.Color.Red;
            this.progressBarPlayList.Location = new System.Drawing.Point(129, 417);
            this.progressBarPlayList.Name = "progressBarPlayList";
            this.progressBarPlayList.Size = new System.Drawing.Size(597, 15);
            this.progressBarPlayList.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarPlayList.TabIndex = 32;
            // 
            // pbPlayListItems
            // 
            this.pbPlayListItems.Image = ((System.Drawing.Image)(resources.GetObject("pbPlayListItems.Image")));
            this.pbPlayListItems.Location = new System.Drawing.Point(114, 354);
            this.pbPlayListItems.Name = "pbPlayListItems";
            this.pbPlayListItems.Size = new System.Drawing.Size(631, 420);
            this.pbPlayListItems.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPlayListItems.TabIndex = 26;
            this.pbPlayListItems.TabStop = false;
            // 
            // pbPlayListBottomScrollBtn
            // 
            this.pbPlayListBottomScrollBtn.Image = ((System.Drawing.Image)(resources.GetObject("pbPlayListBottomScrollBtn.Image")));
            this.pbPlayListBottomScrollBtn.Location = new System.Drawing.Point(0, 696);
            this.pbPlayListBottomScrollBtn.Name = "pbPlayListBottomScrollBtn";
            this.pbPlayListBottomScrollBtn.Size = new System.Drawing.Size(114, 78);
            this.pbPlayListBottomScrollBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPlayListBottomScrollBtn.TabIndex = 31;
            this.pbPlayListBottomScrollBtn.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(93, 432);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(21, 264);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox5.TabIndex = 30;
            this.pictureBox5.TabStop = false;
            // 
            // pbPlayListScrollBar
            // 
            this.pbPlayListScrollBar.Image = ((System.Drawing.Image)(resources.GetObject("pbPlayListScrollBar.Image")));
            this.pbPlayListScrollBar.Location = new System.Drawing.Point(40, 432);
            this.pbPlayListScrollBar.Name = "pbPlayListScrollBar";
            this.pbPlayListScrollBar.Size = new System.Drawing.Size(53, 264);
            this.pbPlayListScrollBar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPlayListScrollBar.TabIndex = 29;
            this.pbPlayListScrollBar.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(0, 432);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(40, 264);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 28;
            this.pictureBox3.TabStop = false;
            // 
            // pbPlayListTopScrollBtn
            // 
            this.pbPlayListTopScrollBtn.Image = ((System.Drawing.Image)(resources.GetObject("pbPlayListTopScrollBtn.Image")));
            this.pbPlayListTopScrollBtn.Location = new System.Drawing.Point(0, 354);
            this.pbPlayListTopScrollBtn.Name = "pbPlayListTopScrollBtn";
            this.pbPlayListTopScrollBtn.Size = new System.Drawing.Size(114, 78);
            this.pbPlayListTopScrollBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPlayListTopScrollBtn.TabIndex = 27;
            this.pbPlayListTopScrollBtn.TabStop = false;
            // 
            // pbPriceList
            // 
            this.pbPriceList.Image = ((System.Drawing.Image)(resources.GetObject("pbPriceList.Image")));
            this.pbPriceList.Location = new System.Drawing.Point(0, 774);
            this.pbPriceList.Name = "pbPriceList";
            this.pbPriceList.Size = new System.Drawing.Size(744, 252);
            this.pbPriceList.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPriceList.TabIndex = 33;
            this.pbPriceList.TabStop = false;
            // 
            // axShockwaveFlash1
            // 
            this.axShockwaveFlash1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axShockwaveFlash1.Enabled = true;
            this.axShockwaveFlash1.Location = new System.Drawing.Point(0, 0);
            this.axShockwaveFlash1.Name = "axShockwaveFlash1";
            this.axShockwaveFlash1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axShockwaveFlash1.OcxState")));
            this.axShockwaveFlash1.Size = new System.Drawing.Size(1280, 258);
            this.axShockwaveFlash1.TabIndex = 18;
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1284, 785);
            this.Controls.Add(this.pbPriceList);
            this.Controls.Add(this.progressBarPlayList);
            this.Controls.Add(this.pbPlayListItems);
            this.Controls.Add(this.pbPlayListBottomScrollBtn);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pbPlayListScrollBar);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pbPlayListTopScrollBtn);
            this.Controls.Add(this.pbOrder);
            this.Controls.Add(this.pbTopTen);
            this.Controls.Add(this.axShockwaveFlash1);
            this.Controls.Add(this.pbTitles);
            this.DoubleBuffered = true;
            this.Name = "MainWnd";
            this.Text = "MusicBox";
            ((System.ComponentModel.ISupportInitialize)(this.pbTitles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTopTen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayListItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayListBottomScrollBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayListScrollBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlayListTopScrollBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPriceList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbTitles;
        //private //musicbox.Play.PlayListCtrl pictureBox8;
        private AxShockwaveFlashObjects.AxShockwaveFlash axShockwaveFlash1;
        private System.Windows.Forms.PictureBox pbTopTen;
        private System.Windows.Forms.PictureBox pbOrder;
        private System.Windows.Forms.ProgressBar progressBarPlayList;
        private System.Windows.Forms.PictureBox pbPlayListItems;
        private System.Windows.Forms.PictureBox pbPlayListBottomScrollBtn;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pbPlayListScrollBar;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pbPlayListTopScrollBtn;
        private System.Windows.Forms.PictureBox pbPriceList;

    }
}

