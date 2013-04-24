using System;
using System.Windows.Forms;
using IWantMovement.Settings;

namespace IWantMovement
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
        }


    }
}
