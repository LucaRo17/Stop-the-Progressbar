using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fill_the_Progressbar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Shown += new EventHandler(Form1_Shown);

            // To report progress from the background worker we need to set this property
            backgroundWorker1.WorkerReportsProgress = true;
            // This event will be raised on the worker thread when the worker starts
            backgroundWorker1.DoWork += new DoWorkEventHandler(BackgroundWorker1_DoWork);
            // This event will be raised when we call ReportProgress
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker1_ProgressChanged);
        }
        void Form1_Shown(object sender, EventArgs e)
        {
            // Start the background worker
            backgroundWorker1.RunWorkerAsync();
        }
        // On worker thread so do our thing!
        void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Your background task goes here
            for (int i = 0; i <= 100; i++)
            {
                // Report progress to 'UI' thread
                backgroundWorker1.ReportProgress(i);
                // Simulate long task
                System.Threading.Thread.Sleep(100);
            }
        }
        // Back on the 'UI' thread so we can update the progress bar
        void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            progressBar1.Value = e.ProgressPercentage;
        }

        void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            _ = progressBar1.Value == 99;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            MessageBox.Show("Well done! You won the game!", "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();

            if (progressBar1.Value > 98)
            {
                MessageBox.Show("Sorry, you lost. Open the game again to play.", "Game over", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            MessageBox.Show("Sorry, you lost. Open the game again to play.", "Game over", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }
}