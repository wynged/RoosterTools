namespace Ductulator
{
    partial class Ductulator
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SideBText = new System.Windows.Forms.TextBox();
            this.SideAText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EDText = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FixA = new System.Windows.Forms.CheckBox();
            this.FixB = new System.Windows.Forms.CheckBox();
            this.FixED = new System.Windows.Forms.CheckBox();
            this.Flow = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.button1.Location = new System.Drawing.Point(137, 244);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(24, 244);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "SetED";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "B";
            // 
            // SideBText
            // 
            this.SideBText.Location = new System.Drawing.Point(75, 41);
            this.SideBText.Name = "SideBText";
            this.SideBText.Size = new System.Drawing.Size(44, 20);
            this.SideBText.TabIndex = 4;
            this.SideBText.Leave += new System.EventHandler(this.SideBText_TextChanged);
            // 
            // SideAText
            // 
            this.SideAText.Location = new System.Drawing.Point(12, 41);
            this.SideAText.Name = "SideAText";
            this.SideAText.Size = new System.Drawing.Size(44, 20);
            this.SideAText.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Eq Diam";
            // 
            // EDText
            // 
            this.EDText.Location = new System.Drawing.Point(135, 41);
            this.EDText.Name = "EDText";
            this.EDText.Size = new System.Drawing.Size(44, 20);
            this.EDText.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FixED);
            this.groupBox1.Controls.Add(this.FixB);
            this.groupBox1.Controls.Add(this.FixA);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.EDText);
            this.groupBox1.Controls.Add(this.SideBText);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SideAText);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 91);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dimensions";
            // 
            // FixA
            // 
            this.FixA.AutoSize = true;
            this.FixA.Location = new System.Drawing.Point(12, 67);
            this.FixA.Name = "FixA";
            this.FixA.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.FixA.Size = new System.Drawing.Size(39, 17);
            this.FixA.TabIndex = 3;
            this.FixA.Text = "Fix";
            this.FixA.UseVisualStyleBackColor = true;
            // 
            // FixB
            // 
            this.FixB.AutoSize = true;
            this.FixB.Location = new System.Drawing.Point(80, 67);
            this.FixB.Name = "FixB";
            this.FixB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.FixB.Size = new System.Drawing.Size(39, 17);
            this.FixB.TabIndex = 3;
            this.FixB.Text = "Fix";
            this.FixB.UseVisualStyleBackColor = true;
            // 
            // FixED
            // 
            this.FixED.AutoSize = true;
            this.FixED.Location = new System.Drawing.Point(140, 67);
            this.FixED.Name = "FixED";
            this.FixED.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.FixED.Size = new System.Drawing.Size(39, 17);
            this.FixED.TabIndex = 3;
            this.FixED.Text = "Fix";
            this.FixED.UseVisualStyleBackColor = true;
            // 
            // Flow
            // 
            this.Flow.Location = new System.Drawing.Point(12, 109);
            this.Flow.Name = "Flow";
            this.Flow.Size = new System.Drawing.Size(200, 100);
            this.Flow.TabIndex = 9;
            this.Flow.TabStop = false;
            this.Flow.Text = "Flow";
            // 
            // Ductulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 282);
            this.Controls.Add(this.Flow);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Ductulator";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox SideAText;
        private System.Windows.Forms.TextBox SideBText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox EDText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox FixA;
        private System.Windows.Forms.CheckBox FixED;
        private System.Windows.Forms.CheckBox FixB;
        private System.Windows.Forms.GroupBox Flow;
    }
}

