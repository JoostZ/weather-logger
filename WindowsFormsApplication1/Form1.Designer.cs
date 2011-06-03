namespace WindowsFormsApplication1
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lbRead = new System.Windows.Forms.ListBox();
            this.lbWrite = new System.Windows.Forms.ListBox();
            this.ws4000HidPort = new WeatherLoggerLib.WS4000HidPort();
            this.btnWrite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(13, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(68, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "WS4000";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // lbRead
            // 
            this.lbRead.FormattingEnabled = true;
            this.lbRead.Location = new System.Drawing.Point(334, 38);
            this.lbRead.Name = "lbRead";
            this.lbRead.Size = new System.Drawing.Size(259, 212);
            this.lbRead.TabIndex = 1;
            // 
            // lbWrite
            // 
            this.lbWrite.FormattingEnabled = true;
            this.lbWrite.Location = new System.Drawing.Point(13, 38);
            this.lbWrite.Name = "lbWrite";
            this.lbWrite.Size = new System.Drawing.Size(315, 212);
            this.lbWrite.TabIndex = 2;
            // 
            // ws4000HidPort
            // 
            this.ws4000HidPort.ProductId = 32801;
            this.ws4000HidPort.VendorId = 6465;
            this.ws4000HidPort.OnSpecifiedDeviceArrived += new System.EventHandler(this.ws4000HidPort_OnSpecifiedDeviceArrived);
            this.ws4000HidPort.OnSpecifiedDeviceRemoved += new System.EventHandler(this.ws4000HidPort_OnSpecifiedDeviceRemoved);
            this.ws4000HidPort.OnDataReceived += new UsbLibrary.DataReceivedEventHandler(this.ws4000HidPort_OnDataReceived);
            this.ws4000HidPort.OnDataSent += new UsbLibrary.DataSentEventHandler(this.ws4000HidPort_OnDataSend);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(228, 13);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 3;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 262);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.lbWrite);
            this.Controls.Add(this.lbRead);
            this.Controls.Add(this.checkBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeatherLoggerLib.WS4000HidPort ws4000HidPort;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ListBox lbRead;
        private System.Windows.Forms.ListBox lbWrite;
        private System.Windows.Forms.Button btnWrite;

    }
}

