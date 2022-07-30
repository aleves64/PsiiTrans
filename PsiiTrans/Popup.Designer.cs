namespace PsiiTrans
{
    partial class Popup
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
            this.SystemLabel = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // SystemLabel
            // 
            this.SystemLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemLabel.BackColor = System.Drawing.Color.White;
            this.SystemLabel.Location = new System.Drawing.Point(447, 1);
            this.SystemLabel.Name = "SystemLabel";
            this.SystemLabel.Size = new System.Drawing.Size(24, 24);
            this.SystemLabel.TabIndex = 11;
            this.SystemLabel.Text = "g";
            this.SystemLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contentPanel
            // 
            this.contentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contentPanel.Location = new System.Drawing.Point(1, 1);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(470, 246);
            this.contentPanel.TabIndex = 12;
            // 
            // Popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(472, 247);
            this.Controls.Add(this.SystemLabel);
            this.Controls.Add(this.contentPanel);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Popup";
            this.Text = "Dictionary popup";
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Label SystemLabel;
        public System.Windows.Forms.Panel contentPanel;
    }
}

