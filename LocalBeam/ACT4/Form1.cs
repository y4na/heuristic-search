using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace ACT4
{
    public partial class Form1 : Form
    {
        int boardSize;
        int numQueens = 6;
        List<SixState> beamStates;
        int beamWidth = 3;
        int moveCounter;

        public Form1()
        {
            InitializeComponent();

            boardSize = pictureBox1.Width / numQueens;
            beamStates = new List<SixState>();

            
            for (int i = 0; i < beamWidth; i++)
            {
                beamStates.Add(randomSixState());
            }

            updateUI();
        }

        private void updateUI()
        {
            pictureBox2.Refresh();

            
            listBox1.Items.Clear();
            foreach (var state in beamStates)
            {
                listBox1.Items.Add("Attacking pairs: " + getAttackingPairs(state));
            }

            label4.Text = "Moves: " + moveCounter;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            if (beamStates.Count > 0)
            {
                drawBoard(e.Graphics, beamStates[0]);
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            
            if (beamStates.Count > 0)
            {
                drawBoard(e.Graphics, beamStates[0]);
            }
        }

        private void drawBoard(Graphics graphics, SixState state)
        {
           
            for (int i = 0; i < numQueens; i++)
            {
                for (int j = 0; j < numQueens; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        graphics.FillRectangle(Brushes.Black, i * boardSize, j * boardSize, boardSize, boardSize);
                    }

                    if (j == state.Y[i])
                    {
                        graphics.FillEllipse(Brushes.Fuchsia, i * boardSize, j * boardSize, boardSize, boardSize);
                    }
                }
            }
        }

        private SixState randomSixState()
        {
            Random rand = new Random();
            return new SixState(rand.Next(numQueens),
                                rand.Next(numQueens),
                                rand.Next(numQueens),
                                rand.Next(numQueens),
                                rand.Next(numQueens),
                                rand.Next(numQueens));
        }

        private int getAttackingPairs(SixState state)
        {
            int attackers = 0;
            for (int i = 0; i < numQueens; i++)
            {
                for (int j = i + 1; j < numQueens; j++)
                {
                    
                    if (state.Y[i] == state.Y[j]) attackers++;
                    
                    if (state.Y[j] == state.Y[i] + j - i) attackers++;
                    if (state.Y[i] == state.Y[j] + j - i) attackers++;
                }
            }
            return attackers;
        }

        private List<SixState> generateSuccessorStates(SixState current)
        {
            List<SixState> successors = new List<SixState>();

            for (int col = 0; col < numQueens; col++)
            {
                for (int row = 0; row < numQueens; row++)
                {
                    if (current.Y[col] != row) 
                    {
                        SixState newState = new SixState(current);
                        newState.Y[col] = row;
                        successors.Add(newState);
                    }
                }
            }

            return successors;
        }

        private void executeBeamSearchStep()
        {
            List<SixState> allSuccessors = new List<SixState>();

            
            foreach (var state in beamStates)
            {
                allSuccessors.AddRange(generateSuccessorStates(state));
            }

            
            allSuccessors = allSuccessors.OrderBy(s => getAttackingPairs(s)).ToList();

            
            beamStates = allSuccessors.Take(beamWidth).ToList();

            moveCounter++;
            updateUI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            executeBeamSearchStep();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            beamStates.Clear();
            for (int i = 0; i < beamWidth; i++)
            {
                beamStates.Add(randomSixState());
            }

            moveCounter = 0;
            updateUI();
            pictureBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            while (beamStates.Count > 0 && getAttackingPairs(beamStates[0]) > 0)
            {
                executeBeamSearchStep();
            }
        }
    }
}
