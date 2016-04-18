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
    public partial class Bingo : Form
    {
        private const int NB_COLUMNS = 5;
        private const int NB_ROWS = 5;
        private const int HEADER = 100;

        string[,] strings = new string[NB_ROWS,NB_COLUMNS];
        //FileReader fileReader;

        public Bingo()
        {
            InitializeComponent();
        }

        Panel[,] panels = new Panel[NB_ROWS, NB_COLUMNS];
        bool[,] selectedPanels = new bool[NB_ROWS, NB_COLUMNS];

        private void InitializeForm(object sender, EventArgs e)
        {

            bool isFileRead = FileReader.ReadFile("prout");//Read File

            if (isFileRead)
            {
                for (int i = 0; i < panels.GetLength(0); i++)
                {
                    for (int j = 0; j < panels.GetLength(1); j++)
                    {
                        Panel newPanel = new Panel();
                        newPanel.Width = this.Width / NB_COLUMNS - 3;
                        newPanel.Height = (this.Height - HEADER) / NB_ROWS;
                        newPanel.Margin = new Padding(0, 0, 0, 0);
                        newPanel.Name = i + ";" + j;
                        newPanel.Location = new Point(j * newPanel.Width, i * newPanel.Height + HEADER / 2);

                        Label newLabel = new Label();
                        newLabel.AutoSize = false;
                        newLabel.TextAlign = ContentAlignment.MiddleCenter;
                        newLabel.Dock = DockStyle.Fill;
                        newLabel.Text = i + ";" + j; //Add strings[i,j] from FileReader result here.
                        newLabel.Click += new EventHandler(OnLabelClicked);
                        newLabel.Name = i + ";" + j;

                        panels[i, j] = newPanel;
                        newPanel.Controls.Add(newLabel);
                        this.Controls.Add(newPanel);
                        if (newPanel.Name == NB_ROWS / 2 + ";" + NB_COLUMNS / 2)
                        {
                            ManageClickOnLabel(newLabel);
                        }
                    }
                }
            }
            else
            {
                Label newLabel = new Label();
                newLabel.AutoSize = false;
                newLabel.TextAlign = ContentAlignment.MiddleCenter;
                newLabel.Dock = DockStyle.Fill;
                newLabel.Text = "Game could not be loaded. Check your Data directory for missing or broken files.";
                this.Controls.Add(newLabel);
            }
        }


        private void OnLabelClicked(object sender, EventArgs e)
        {
            ManageClickOnLabel(sender as Label);
        }

        void ManageClickOnLabel(Label label)
        {
            if (!selectedPanels[label.Name[0] - '0', label.Name[2]- '0'])
            {
                label.BackColor = Color.Gold;
            }
            else
            {
                label.BackColor = SystemColors.Control;
            }
            selectedPanels[label.Name[0] - '0', label.Name[2] - '0'] = !selectedPanels[label.Name[0] - '0', label.Name[2] - '0'];
            CheckIfBingo();
        }

        private void CheckIfBingo()
        {
            bool youreWinner = CheckRowsForBingo();
            if (!youreWinner)
            {
                youreWinner = CheckColumnsForBingo();
            }
            if (!youreWinner)
            {
                youreWinner = CheckSlashForBingo();
            }
            if (!youreWinner)
            {
                youreWinner = CheckBackslashForBingo();
            }
            if (youreWinner)
            {
                MessageBox.Show("You have a bingo. Now go be a plebe and tell everybody about it.", "BINGO !", MessageBoxButtons.OK);
            }
        }

        private bool CheckRowsForBingo()
        {
            for (int x = 0; x < NB_ROWS; x++)
            {
                bool rowIsFull = true;
                for (int y = 0; y < NB_COLUMNS; y++)
                {
                    if (!selectedPanels[x, y])
                    {
                        rowIsFull = false;
                    }
                }
                if (rowIsFull)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckColumnsForBingo()
        {
            bool columnIsFull = true;
            for (int x = 0; x < NB_COLUMNS; x++)
            {
                for (int y = 0; y < NB_ROWS; y++)
                {
                    if (!selectedPanels[y, x])
                    {
                        columnIsFull = false;
                    }
                }
                if (columnIsFull)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckSlashForBingo()
        {
            if (NB_ROWS % 2 == 1 && NB_COLUMNS % 2 == 1)
            {
                for (int x = 0; x < NB_COLUMNS; x++)
                {
                    if (!selectedPanels[x, x])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private bool CheckBackslashForBingo()
        {
            if (NB_ROWS % 2 == 1 && NB_COLUMNS % 2 == 1)
            {
                for (int x = 0; x < NB_COLUMNS; x++)
                {
                    if (!selectedPanels[NB_COLUMNS - 1 - x, x])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
