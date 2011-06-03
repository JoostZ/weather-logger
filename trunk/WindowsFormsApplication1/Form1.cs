using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WeatherLoggerLib;
using UsbLibrary;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ws4000HidPort.RegisterHandle(Handle);
        }

        protected override void WndProc(ref Message m)
        {
            ws4000HidPort.ParseMessages(ref m);
            base.WndProc(ref m);	// pass message on to base form
        }


        private void ws4000HidPort_OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            lbWrite.Items.Add("Help");
            Invalidate();
        }

        private void ws4000HidPort_OnSpecifiedDeviceRemoved(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            Invalidate();
        }

        private void ws4000HidPort_OnDataSend(object sender, UsbLibrary.DataSentEventArgs args)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new DataSentEventHandler(ws4000HidPort_OnDataSend), new object[] { sender, args });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                try
                {
                    String value = "";
                    for (int i = 1; i < 9; i++)
                    {
                        value += args.data.Buffer[i].ToString("X2") + " ";
                    }
                    lbWrite.Items.Add(value);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        SnapshotReader reader;

        private void btnWrite_Click(object sender, EventArgs e)
        {
            //RequestBuffer buffer = new RequestBuffer(this.ws4000HidPort.SpecifiedDevice);
            //buffer.Offset = 0;
            //buffer.Send();
            WS4000Device device = ws4000HidPort.SpecifiedDevice;
            reader = new SnapshotReader(device);
            device.BufferReceived += handleBufferReceived;
            //device.getMemory(0, 64);
            reader.readSnapshots();
        }

        private void handleBufferReceived(object sender, BufferReceivedEventArgs args)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new WS4000Device.BufferReceivedEventHandler(handleBufferReceived), new object[] { sender, args });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                int numBytes = args.Data.Buffer.Length;
                int numBlocks = numBytes / 0x10;
                for (int j = 0; j < numBytes / 0x10; j++)
                {
                    String value = "";
                    for (int i = 0; i < 16; i++)
                    {
                        value += args.Data.Buffer[i + 16 * j].ToString("X2") + " ";
                    }
                    lbRead.Items.Add(value);
                }
            }
        }

        private void ws4000HidPort_OnDataReceived(object sender, UsbLibrary.DataReceivedEventArgs args)
        {
            //if (InvokeRequired)
            //{
            //    try
            //    {
            //        Invoke(new DataReceivedEventHandler(ws4000HidPort_OnDataReceived), new object[] { sender, args });
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.ToString());
            //    }
            //}
            //else
            //{
            //    String value = "";
            //    for (int i = 1; i < 9; i++)
            //    {
            //        value += args.data.Buffer[i].ToString("X2") + " ";
            //    }
            //    lbRead.Items.Add(value);
            //}
        }
            
    }
}
