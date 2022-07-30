namespace PsiiTrans
{
    partial class MainForm
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
            this.outputTextbox = new System.Windows.Forms.TextBox();
            this.selectProcessButton = new System.Windows.Forms.Button();
            this.selectedProcessLabel = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.ttCombo = new System.Windows.Forms.ComboBox();
            this.connectedLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // outputTextbox
            // 
            this.outputTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTextbox.Font = new System.Drawing.Font("Meiryo", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.outputTextbox.Location = new System.Drawing.Point(12, 34);
            this.outputTextbox.Multiline = true;
            this.outputTextbox.Name = "outputTextbox";
            this.outputTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextbox.Size = new System.Drawing.Size(776, 329);
            this.outputTextbox.TabIndex = 0;
            // 
            // selectProcessButton
            // 
            this.selectProcessButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.selectProcessButton.Location = new System.Drawing.Point(96, 386);
            this.selectProcessButton.Name = "selectProcessButton";
            this.selectProcessButton.Size = new System.Drawing.Size(75, 23);
            this.selectProcessButton.TabIndex = 1;
            this.selectProcessButton.Text = "select";
            this.selectProcessButton.UseVisualStyleBackColor = true;
            this.selectProcessButton.Click += new System.EventHandler(this.selectProcessButton_Click);
            // 
            // selectedProcessLabel
            // 
            this.selectedProcessLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.selectedProcessLabel.AutoSize = true;
            this.selectedProcessLabel.Location = new System.Drawing.Point(198, 391);
            this.selectedProcessLabel.Name = "selectedProcessLabel";
            this.selectedProcessLabel.Size = new System.Drawing.Size(85, 13);
            this.selectedProcessLabel.TabIndex = 2;
            this.selectedProcessLabel.Text = "nothing selected";
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.connectButton.Location = new System.Drawing.Point(96, 415);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // ttCombo
            // 
            this.ttCombo.FormattingEnabled = true;
            this.ttCombo.Location = new System.Drawing.Point(13, 7);
            this.ttCombo.Name = "ttCombo";
            this.ttCombo.Size = new System.Drawing.Size(460, 21);
            this.ttCombo.TabIndex = 4;
            // 
            // connectedLabel
            // 
            this.connectedLabel.AutoSize = true;
            this.connectedLabel.Location = new System.Drawing.Point(198, 420);
            this.connectedLabel.Name = "connectedLabel";
            this.connectedLabel.Size = new System.Drawing.Size(76, 13);
            this.connectedLabel.TabIndex = 5;
            this.connectedLabel.Text = "not connected";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.connectedLabel);
            this.Controls.Add(this.ttCombo);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.selectedProcessLabel);
            this.Controls.Add(this.selectProcessButton);
            this.Controls.Add(this.outputTextbox);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "PsiiTrans";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox outputTextbox;
        private System.Windows.Forms.Button selectProcessButton;
        private System.Windows.Forms.Label selectedProcessLabel;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ComboBox ttCombo;
        private System.Windows.Forms.Label connectedLabel;
    }
}

