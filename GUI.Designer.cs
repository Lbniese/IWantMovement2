namespace IWantMovement
{
    partial class GUI
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
            this.chkMovement = new System.Windows.Forms.CheckBox();
            this.chkFacing = new System.Windows.Forms.CheckBox();
            this.chkTargeting = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkMovement
            // 
            this.chkMovement.AutoSize = true;
            this.chkMovement.Location = new System.Drawing.Point(19, 26);
            this.chkMovement.Name = "chkMovement";
            this.chkMovement.Size = new System.Drawing.Size(76, 17);
            this.chkMovement.TabIndex = 0;
            this.chkMovement.Text = "Movement";
            this.chkMovement.UseVisualStyleBackColor = true;
            // 
            // chkFacing
            // 
            this.chkFacing.AutoSize = true;
            this.chkFacing.Location = new System.Drawing.Point(19, 49);
            this.chkFacing.Name = "chkFacing";
            this.chkFacing.Size = new System.Drawing.Size(58, 17);
            this.chkFacing.TabIndex = 1;
            this.chkFacing.Text = "Facing";
            this.chkFacing.UseVisualStyleBackColor = true;
            // 
            // chkTargeting
            // 
            this.chkTargeting.AutoSize = true;
            this.chkTargeting.Location = new System.Drawing.Point(19, 72);
            this.chkTargeting.Name = "chkTargeting";
            this.chkTargeting.Size = new System.Drawing.Size(71, 17);
            this.chkTargeting.TabIndex = 2;
            this.chkTargeting.Text = "Targeting";
            this.chkTargeting.UseVisualStyleBackColor = true;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 115);
            this.Controls.Add(this.chkTargeting);
            this.Controls.Add(this.chkFacing);
            this.Controls.Add(this.chkMovement);
            this.Name = "GUI";
            this.Text = "I Want Movement";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public static System.Windows.Forms.CheckBox chkMovement;
        public static System.Windows.Forms.CheckBox chkFacing;
        public static System.Windows.Forms.CheckBox chkTargeting;

    }
}