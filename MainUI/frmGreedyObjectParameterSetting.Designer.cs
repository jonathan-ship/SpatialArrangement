namespace Eoba.Shipyard.ArrangementSimulator.MainUI
{
    partial class frmGreedyObjectParameterSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tboxNumofTwist = new System.Windows.Forms.TextBox();
            this.tboxNumofAdjacentNum = new System.Windows.Forms.TextBox();
            this.tboxAdjacentLength = new System.Windows.Forms.TextBox();
            this.tboxDistanceofCentroid = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Twist 갯수";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "인접한 변의 갯수";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "인접한 변의 길이";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "무게중심 사이의 거리";
            // 
            // tboxNumofTwist
            // 
            this.tboxNumofTwist.Location = new System.Drawing.Point(184, 30);
            this.tboxNumofTwist.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tboxNumofTwist.Name = "tboxNumofTwist";
            this.tboxNumofTwist.Size = new System.Drawing.Size(61, 25);
            this.tboxNumofTwist.TabIndex = 4;
            this.tboxNumofTwist.Text = "1";
            this.tboxNumofTwist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tboxNumofAdjacentNum
            // 
            this.tboxNumofAdjacentNum.Location = new System.Drawing.Point(184, 80);
            this.tboxNumofAdjacentNum.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tboxNumofAdjacentNum.Name = "tboxNumofAdjacentNum";
            this.tboxNumofAdjacentNum.Size = new System.Drawing.Size(61, 25);
            this.tboxNumofAdjacentNum.TabIndex = 5;
            this.tboxNumofAdjacentNum.Text = "1";
            this.tboxNumofAdjacentNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tboxAdjacentLength
            // 
            this.tboxAdjacentLength.Location = new System.Drawing.Point(184, 128);
            this.tboxAdjacentLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tboxAdjacentLength.Name = "tboxAdjacentLength";
            this.tboxAdjacentLength.Size = new System.Drawing.Size(61, 25);
            this.tboxAdjacentLength.TabIndex = 6;
            this.tboxAdjacentLength.Text = "1";
            this.tboxAdjacentLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tboxDistanceofCentroid
            // 
            this.tboxDistanceofCentroid.Location = new System.Drawing.Point(184, 178);
            this.tboxDistanceofCentroid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tboxDistanceofCentroid.Name = "tboxDistanceofCentroid";
            this.tboxDistanceofCentroid.Size = new System.Drawing.Size(61, 25);
            this.tboxDistanceofCentroid.TabIndex = 7;
            this.tboxDistanceofCentroid.Text = "1";
            this.tboxDistanceofCentroid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tboxDistanceofCentroid);
            this.groupBox1.Controls.Add(this.tboxAdjacentLength);
            this.groupBox1.Controls.Add(this.tboxNumofAdjacentNum);
            this.groupBox1.Controls.Add(this.tboxNumofTwist);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(267, 222);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "목적함수의 계수 설정 (0~1)";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(49, 245);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 29);
            this.button1.TabIndex = 9;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(174, 245);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 29);
            this.button2.TabIndex = 10;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmGreedyObjectParameterSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 295);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmGreedyObjectParameterSetting";
            this.Text = "EVA - Greedy Object Function Parameter Setting";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tboxNumofTwist;
        private System.Windows.Forms.TextBox tboxNumofAdjacentNum;
        private System.Windows.Forms.TextBox tboxAdjacentLength;
        private System.Windows.Forms.TextBox tboxDistanceofCentroid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}