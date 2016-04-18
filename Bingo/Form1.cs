using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bingo
{
    public partial class Form1 : Form
    {
        private const int NB_COLUMNS = 7;
        private const int NB_ROWS = 6;

        public Form1()
        {
            InitializeComponent();
        }

        Panel[,] panels = new Panel[NB_ROWS, NB_COLUMNS];

        private void InitializeForm(object sender, EventArgs e)
        {
            for (int i = 0; i < NB_COLUMNS; i++)
            {
                for (int j = 0; j < NB_ROWS; j++)
                {
                    Panel newPanel = new Panel();
                    newPanel.Width = 64;
                    newPanel.Height = 64;
                    newPanel.Dock = DockStyle.Fill;
                    newPanel.Margin = new Padding(0, 0, 0, 0);
                    newPanel.Click += new EventHandler(OnPanelClicked);
                    newPanel.MouseEnter += new EventHandler(OnPanelMouseEnter);
                    newPanel.MouseLeave += new EventHandler(OnPanelMouseLeave);
                }
            }
        }


        private void OnPanelClicked(object sender, EventArgs e)
        {
            ManageClickOnPanel(sender as Panel);
        }

        private void OnPanelMouseEnter(object sender, EventArgs e)
        {
            ManageMouseOverPanel(sender as Panel);
        }

        private void OnPanelMouseLeave(object sender, EventArgs e)
        {
            ManageMouseLeavingPanel(sender as Panel);
        }

        void ManageClickOnPanel(Panel panel)
        {

        }

        void ManageMouseOverPanel(Panel panel)
        {

        }

        void ManageMouseLeavingPanel(Panel panel)
        {

        }
    }
}
