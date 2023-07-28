using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class GameUI : Form
    {
        // change the number of rows and columns below...
        private int num_rows = 100;
        private int num_cols = 100;

        private SolidBrush sb_black = new SolidBrush(Color.Black);
        private SolidBrush sb_pink = new SolidBrush(Color.LightPink);
        private SolidBrush sb_white = new SolidBrush(Color.White);
        private int[,] current_states;

        public GameUI()
        {
            InitializeComponent();
            current_states = get_initial_2D_array();
            
            // this code uses Reflection to set the DoubleBuffered property of the panel
            // code taken from StackOverflow.com
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, panel1, new object[] { true });
        }

        private int[,] get_initial_2D_array()
        {
            Random randomiser = new Random();
            int bit;

            int[,] state = new int[num_rows, num_cols];

            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_cols; col++)
                {
                    if (row == 0 || row == num_rows - 1 || col == 0 || col == num_cols - 1)
                    {
                        bit = -1;
                        state[row, col] = bit;
                    }
                    else
                    {
                        bit = randomiser.Next(0, 2);
                        state[row, col] = bit;
                    }
                    
                }

            }        

            return state;
        }
        
        private int count_neighbours(int row_counter, int col_counter)
        {
            int sum = 0;

            if (row_counter == 0 || row_counter == num_rows - 1 || col_counter == 0 || col_counter == num_cols - 1)
            {
                sum = -1;
                return sum;
            }
            else
            {
                for (int vertical_shift = -1; vertical_shift < 2; vertical_shift++)
                {
                    for (int horizontal_shift = -1; horizontal_shift < 2; horizontal_shift++)
                    {
                        if (current_states[row_counter + vertical_shift, col_counter + horizontal_shift] != -1)
                        {
                            sum += current_states[row_counter + vertical_shift, col_counter + horizontal_shift];
                        }

                    }
                }

                sum = sum - current_states[row_counter, col_counter];
                return sum;

            }                   

            
        }       
            
        
        private int[,] get_next_states(int[,] current_states)
        {
            int[,] state = new int[num_rows, num_cols];
            
            for (int row_counter = 0; row_counter < num_rows; row_counter++)
            {
                for (int col_counter = 0; col_counter < num_cols; col_counter++)
                {
                    int alive_neighbours = count_neighbours(row_counter, col_counter);

                    if (alive_neighbours == -1)
                    {
                        state[row_counter, col_counter] = -1;
                    }
                    else if ((alive_neighbours < 2) || (alive_neighbours > 3))
                    {
                        state[row_counter, col_counter] = 0;
                    }
                    else if ((current_states[row_counter, col_counter] == 0) && (alive_neighbours == 3))
                    {
                        state[row_counter, col_counter] = 1;
                    }
                    else if ((current_states[row_counter, col_counter] == 1) && ((alive_neighbours == 2) || (alive_neighbours == 3)))
                    {
                        state[row_counter, col_counter] = 1;
                    }
                }
            }

            return state;
        }

               
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
            int pixel_width = panel1.ClientSize.Width / num_cols;
            int pixel_height = panel1.ClientSize.Height / num_rows;


            Graphics targetGraphics = e.Graphics;
                
            for (int row_counter = 0; row_counter < num_rows; row_counter++)
            {
                for (int col_counter = 0; col_counter < num_cols; col_counter++)
                {
                    if (current_states[row_counter, col_counter] == 1)
                    {
                        targetGraphics.FillRectangle(sb_black, col_counter * pixel_width, row_counter * pixel_height, pixel_width, pixel_height);
                    }
                    else if (current_states[row_counter, col_counter] == -1)
                    {
                        targetGraphics.FillRectangle(sb_pink, col_counter * pixel_width, row_counter * pixel_height, pixel_width, pixel_height);
                    }
                    else
                    {                            
                        targetGraphics.FillRectangle(sb_white, col_counter * pixel_width, row_counter * pixel_height, pixel_width, pixel_height);
                    }

                }
                                        
            }                                          
                
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            current_states = get_next_states(current_states);
            panel1.Refresh();

        }
    }

}
