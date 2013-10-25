namespace TLM.Ui
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label11 = new System.Windows.Forms.Label();
            this.BTRun = new System.Windows.Forms.Button();
            this.BTCreateNet = new System.Windows.Forms.Button();
            this.TBBoundRight = new System.Windows.Forms.TextBox();
            this.TBBoundLeft = new System.Windows.Forms.TextBox();
            this.TBBoundBot = new System.Windows.Forms.TextBox();
            this.TBBoundTop = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TBN = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TBC = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TBFreq = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TBEr = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TBZ0 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TBDl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TBSigma = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBSizeY = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBSizeX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TBInputFunc = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TBInputFunc);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.BTRun);
            this.splitContainer1.Panel1.Controls.Add(this.BTCreateNet);
            this.splitContainer1.Panel1.Controls.Add(this.TBBoundRight);
            this.splitContainer1.Panel1.Controls.Add(this.TBBoundLeft);
            this.splitContainer1.Panel1.Controls.Add(this.TBBoundBot);
            this.splitContainer1.Panel1.Controls.Add(this.TBBoundTop);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.TBN);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.TBC);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.TBFreq);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.TBEr);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.TBZ0);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.TBDl);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.TBSigma);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.TBSizeY);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.TBSizeX);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(832, 419);
            this.splitContainer1.SplitterDistance = 245;
            this.splitContainer1.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 314);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Input Function";
            // 
            // BTRun
            // 
            this.BTRun.Location = new System.Drawing.Point(94, 358);
            this.BTRun.Name = "BTRun";
            this.BTRun.Size = new System.Drawing.Size(75, 23);
            this.BTRun.TabIndex = 24;
            this.BTRun.Text = "Run";
            this.BTRun.UseVisualStyleBackColor = true;
            this.BTRun.Click += new System.EventHandler(this.BTRun_Click);
            // 
            // BTCreateNet
            // 
            this.BTCreateNet.Location = new System.Drawing.Point(12, 358);
            this.BTCreateNet.Name = "BTCreateNet";
            this.BTCreateNet.Size = new System.Drawing.Size(75, 23);
            this.BTCreateNet.TabIndex = 23;
            this.BTCreateNet.Text = "Create Net";
            this.BTCreateNet.UseVisualStyleBackColor = true;
            this.BTCreateNet.Click += new System.EventHandler(this.BTCreateNet_Click);
            // 
            // TBBoundRight
            // 
            this.TBBoundRight.Location = new System.Drawing.Point(159, 274);
            this.TBBoundRight.Name = "TBBoundRight";
            this.TBBoundRight.Size = new System.Drawing.Size(61, 20);
            this.TBBoundRight.TabIndex = 22;
            this.TBBoundRight.Text = "-0.17157";
            // 
            // TBBoundLeft
            // 
            this.TBBoundLeft.Location = new System.Drawing.Point(25, 274);
            this.TBBoundLeft.Name = "TBBoundLeft";
            this.TBBoundLeft.Size = new System.Drawing.Size(61, 20);
            this.TBBoundLeft.TabIndex = 21;
            this.TBBoundLeft.Text = "-0.17157";
            // 
            // TBBoundBot
            // 
            this.TBBoundBot.Location = new System.Drawing.Point(92, 287);
            this.TBBoundBot.Name = "TBBoundBot";
            this.TBBoundBot.Size = new System.Drawing.Size(61, 20);
            this.TBBoundBot.TabIndex = 20;
            this.TBBoundBot.Text = "1";
            // 
            // TBBoundTop
            // 
            this.TBBoundTop.Location = new System.Drawing.Point(92, 261);
            this.TBBoundTop.Name = "TBBoundTop";
            this.TBBoundTop.Size = new System.Drawing.Size(61, 20);
            this.TBBoundTop.TabIndex = 19;
            this.TBBoundTop.Text = "1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 244);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Boundaries";
            // 
            // TBN
            // 
            this.TBN.Location = new System.Drawing.Point(53, 215);
            this.TBN.Name = "TBN";
            this.TBN.Size = new System.Drawing.Size(100, 20);
            this.TBN.TabIndex = 17;
            this.TBN.Text = "43";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 218);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "N Iter.";
            // 
            // TBC
            // 
            this.TBC.Location = new System.Drawing.Point(53, 189);
            this.TBC.Name = "TBC";
            this.TBC.Size = new System.Drawing.Size(100, 20);
            this.TBC.TabIndex = 15;
            this.TBC.Text = "300E6";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "C";
            // 
            // TBFreq
            // 
            this.TBFreq.Location = new System.Drawing.Point(53, 163);
            this.TBFreq.Name = "TBFreq";
            this.TBFreq.Size = new System.Drawing.Size(100, 20);
            this.TBFreq.TabIndex = 13;
            this.TBFreq.Text = "10E9";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Freq.";
            // 
            // TBEr
            // 
            this.TBEr.Location = new System.Drawing.Point(53, 137);
            this.TBEr.Name = "TBEr";
            this.TBEr.Size = new System.Drawing.Size(100, 20);
            this.TBEr.TabIndex = 11;
            this.TBEr.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Er";
            // 
            // TBZ0
            // 
            this.TBZ0.Location = new System.Drawing.Point(53, 111);
            this.TBZ0.Name = "TBZ0";
            this.TBZ0.Size = new System.Drawing.Size(100, 20);
            this.TBZ0.TabIndex = 9;
            this.TBZ0.Text = "377";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Z0";
            // 
            // TBDl
            // 
            this.TBDl.Location = new System.Drawing.Point(53, 85);
            this.TBDl.Name = "TBDl";
            this.TBDl.Size = new System.Drawing.Size(100, 20);
            this.TBDl.TabIndex = 7;
            this.TBDl.Text = "1E-3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Delta L";
            // 
            // TBSigma
            // 
            this.TBSigma.Location = new System.Drawing.Point(53, 59);
            this.TBSigma.Name = "TBSigma";
            this.TBSigma.Size = new System.Drawing.Size(100, 20);
            this.TBSigma.TabIndex = 5;
            this.TBSigma.Text = "5E-15";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Sigma";
            // 
            // TBSizeY
            // 
            this.TBSizeY.Location = new System.Drawing.Point(53, 33);
            this.TBSizeY.Name = "TBSizeY";
            this.TBSizeY.Size = new System.Drawing.Size(100, 20);
            this.TBSizeY.TabIndex = 3;
            this.TBSizeY.Text = "2E-2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Size Y";
            // 
            // TBSizeX
            // 
            this.TBSizeX.Location = new System.Drawing.Point(53, 7);
            this.TBSizeX.Name = "TBSizeX";
            this.TBSizeX.Size = new System.Drawing.Size(100, 20);
            this.TBSizeX.TabIndex = 1;
            this.TBSizeX.Text = "10E-2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Size X";
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.StatusBar.Location = new System.Drawing.Point(0, 397);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(832, 22);
            this.StatusBar.TabIndex = 1;
            this.StatusBar.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // TBInputFunc
            // 
            this.TBInputFunc.Location = new System.Drawing.Point(13, 332);
            this.TBInputFunc.Name = "TBInputFunc";
            this.TBInputFunc.Size = new System.Drawing.Size(207, 20);
            this.TBInputFunc.TabIndex = 26;
            this.TBInputFunc.Text = "Sin(2*[Pi]*[f0]*([k]+1)*[dT])";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 419);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBN;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TBC;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TBFreq;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TBEr;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TBZ0;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TBDl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBSigma;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBSizeY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBSizeX;
        private System.Windows.Forms.TextBox TBBoundRight;
        private System.Windows.Forms.TextBox TBBoundLeft;
        private System.Windows.Forms.TextBox TBBoundBot;
        private System.Windows.Forms.TextBox TBBoundTop;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button BTRun;
        private System.Windows.Forms.Button BTCreateNet;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TBInputFunc;
    }
}

