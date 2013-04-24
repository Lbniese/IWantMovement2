#region Revision info
/*
 * $Author$
 * $Date$
 * $ID: $
 * $Revision$
 * $URL$
 * $LastChangedBy$
 * $ChangesMade: $
 */
#endregion

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace IWantMovement.Settings
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            pgSettings.SelectedObject = IWMSettings.Instance;
            IWMSettings.Instance.Load();
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            IWMSettings.Instance.Save();
            Managers.Movement.ValidatedSettings = false;
        }

        private void pgSettings_Click(object sender, EventArgs e)
        {

        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks! All donations are greatly appreciated.");
            Process.Start("http://bit.ly/YEb4SU");
        }


    }
}
