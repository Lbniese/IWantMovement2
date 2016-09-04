using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace IWantMovement2.Settings
{
    public partial class Gui : Form
    {
        public Gui()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            pgSettings.SelectedObject = IwmSettings.Instance;
            IwmSettings.Instance.Load();
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            IwmSettings.Instance.Save();
        }

        private void pgSettings_Click(object sender, EventArgs e)
        {
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks! All donations are greatly appreciated.");
            Process.Start("http://bit.ly/1nu7Hip");
        }

        private void reportAnIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("There is currently no thread on the forums about this updated version.");
            //Process.Start("https://www.thebuddyforum.com/members/48399-lbniese.html");
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var credits = new Credits();
            credits.Show();
        }
    }
}