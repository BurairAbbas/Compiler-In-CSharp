namespace CC_Compiler_In_WFA
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
            this.Codetb = new System.Windows.Forms.TextBox();
            this.GenerateToken = new System.Windows.Forms.Button();
            this.tokentb = new System.Windows.Forms.TextBox();
            this.Syntaxtb = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Codetb
            // 
            this.Codetb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Codetb.Location = new System.Drawing.Point(30, 12);
            this.Codetb.Multiline = true;
            this.Codetb.Name = "Codetb";
            this.Codetb.Size = new System.Drawing.Size(435, 267);
            this.Codetb.TabIndex = 0;
            // 
            // GenerateToken
            // 
            this.GenerateToken.Location = new System.Drawing.Point(550, 183);
            this.GenerateToken.Name = "GenerateToken";
            this.GenerateToken.Size = new System.Drawing.Size(140, 51);
            this.GenerateToken.TabIndex = 1;
            this.GenerateToken.Text = "Compile";
            this.GenerateToken.UseVisualStyleBackColor = true;
            this.GenerateToken.Click += new System.EventHandler(this.GenerateToken_Click);
            // 
            // tokentb
            // 
            this.tokentb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tokentb.Location = new System.Drawing.Point(776, 12);
            this.tokentb.Multiline = true;
            this.tokentb.Name = "tokentb";
            this.tokentb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tokentb.Size = new System.Drawing.Size(396, 426);
            this.tokentb.TabIndex = 2;
            // 
            // Syntaxtb
            // 
            this.Syntaxtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Syntaxtb.Location = new System.Drawing.Point(30, 313);
            this.Syntaxtb.Multiline = true;
            this.Syntaxtb.Name = "Syntaxtb";
            this.Syntaxtb.Size = new System.Drawing.Size(435, 125);
            this.Syntaxtb.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 450);
            this.Controls.Add(this.Syntaxtb);
            this.Controls.Add(this.tokentb);
            this.Controls.Add(this.GenerateToken);
            this.Controls.Add(this.Codetb);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Codetb;
        private System.Windows.Forms.Button GenerateToken;
        private System.Windows.Forms.TextBox tokentb;
        private System.Windows.Forms.TextBox Syntaxtb;
    }
}

