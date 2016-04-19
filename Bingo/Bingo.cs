using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private string gameDataPath = "Data/yvon.txt";
        private Random rng = new Random();
        Panel[,] panels;
        bool[,] selectedPanels;

        public Bingo()
        {
            InitializeComponent();
        }

        private void InitializeForm(object sender, EventArgs e)
        {
            System.GC.Collect();
            panels = new Panel[NB_ROWS, NB_COLUMNS];
            selectedPanels = new bool[NB_ROWS, NB_COLUMNS];
            this.Controls.Clear();

            MainMenu mainStrip = new MainMenu();
            MenuItem superGameOption = new MenuItem();
            MenuItem subGameNewOption = new MenuItem();
            MenuItem subGameLoadOption = new MenuItem();

            superGameOption.Text = "Game";
            subGameNewOption.Text = "New Game";
            subGameLoadOption.Text = "Load Bingo Data";

            subGameNewOption.Click += new EventHandler(onNewGameMenuClicked);
            subGameLoadOption.Click += new EventHandler(onLoadDataMenuClicked);

            superGameOption.MenuItems.Add(subGameNewOption);
            superGameOption.MenuItems.Add(subGameLoadOption);
            mainStrip.MenuItems.Add(superGameOption);
            this.Menu = mainStrip;

            List<string> strings = FileReader.ReadFile(gameDataPath);
          
            if (strings != null)
            {
                string freeSpace = strings[0];
                strings.Remove(strings[0]);
                Shuffle(strings);

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
                        newLabel.Click += new EventHandler(OnLabelClicked);
                        newLabel.Name = i + ";" + j;
                        
                        if (newPanel.Name == NB_ROWS / 2 + ";" + NB_COLUMNS / 2)
                        {
                            newLabel.Text = freeSpace;
                            ManageClickOnLabel(newLabel);
                        }
                        else
                        {
                            newLabel.Text = strings[0];
                            strings.Remove(strings[0]);
                        }

                        panels[i, j] = newPanel;
                        newPanel.Controls.Add(newLabel);
                        this.Controls.Add(newPanel);                     
                    }
                }
            }
            else
            {
                Label newLabel = new Label();
                newLabel.AutoSize = false;
                newLabel.TextAlign = ContentAlignment.MiddleCenter;
                newLabel.Dock = DockStyle.Fill;
                newLabel.Text = "A bingo card could not be loaded. Check your Data directory for missing or broken files.";
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
                label.BackColor = SystemColors.ControlLight;
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
                MessageBox.Show("You have a bingo! Now go tell everybody about it.", "BINGO !", MessageBoxButtons.OK);
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
            for (int x = 0; x < NB_COLUMNS; x++)
            {
                bool columnIsFull = true;
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
        }

        private void Shuffle(List<string> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void onLoadDataMenuClicked(object sender, EventArgs e)
        {
            using(OpenFileDialog dataExplorer = new OpenFileDialog())
            {
                dataExplorer.InitialDirectory = "Data";
                dataExplorer.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                dataExplorer.FilterIndex = 2;
                dataExplorer.RestoreDirectory = true;
                if (dataExplorer.ShowDialog() == DialogResult.OK)
                {
                    gameDataPath = dataExplorer.FileName;
                    InitializeForm(sender, e);
                }
            }
        }

        private void onNewGameMenuClicked(object sender, EventArgs e)
        {
            InitializeForm(sender, e);
        }
    }
}
