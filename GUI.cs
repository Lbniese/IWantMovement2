using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            
        }

        public bool EnableMovement { get { return chkMovement.Checked; } }
        public bool EnableFacing { get { return chkFacing.Checked; } }
        public bool EnableTargeting { get { return chkTargeting.Checked; } }
    }
}
