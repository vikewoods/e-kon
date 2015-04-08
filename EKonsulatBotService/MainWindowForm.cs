using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EKonsulatBotService
{
    public partial class MainWindowForm : Form
    {
        public Checker checker = new Checker();

        public MainWindowForm()
        {
            InitializeComponent();
            proxyType.SelectedIndex = 0;
            proxyType.SelectedIndexChanged += ProxyTypeOnSelectedIndexChanged;
        }

        

        private void startBtn_Click(object sender, EventArgs e)
        {      
            checker.DoRequest();
        }
  
        private void stopBtn_Click(object sender, EventArgs e)
        {

        }

        private void restartBtn_Click(object sender, EventArgs e)
        {

        }

        private void proxyBtn_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value > 0 && trackBar1.Value < 180)
            {
                timer1.Interval = trackBar1.Value*1000;
            }
            else
            {
                timer1.Interval = 6000;
            }
            label9.Text = trackBar1.Value.ToString();
        }

        private void ProxyTypeOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            Globals.ProxyType = proxyType.SelectedIndex;
            //richTextBox1.AppendText("Proxy type has been chaged to " + Globals.ProxyType.ToString());
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
