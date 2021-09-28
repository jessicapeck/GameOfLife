using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class GameUI : Form
    {
        public GameUI()
        {
            InitializeComponent();
        }

        private int[,] get_initial_2D_array(int num_rows, int num_cols)
        {
            Random randomiser = new Random();
            int bit;

            int[,] current_states = new int[num_rows, num_cols];

            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_cols; col++)
                {
                    if (row == 0 || row == num_rows - 1 || col == 0 || col == num_cols - 1)
                    {
                        bit = -1;
                        current_states[row, col] = bit;
                    }
                    else
                    {
                        bit = randomiser.Next(0, 2);
                        current_states[row, col] = bit;
                    }
                    
                }

            }        

            return current_states;
        }
        
        private int count_neighbours(int[,] current_states, int row_counter, int col_counter, int num_rows, int num_cols)
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
            
        
        private int[,] get_next_states(int[,] current_states, int num_rows, int num_cols)
        {
            int[,] next_states = new int[num_rows, num_cols];
            
            for (int row_counter = 0; row_counter < num_rows; row_counter++)
            {
                for (int col_counter = 0; col_counter < num_cols; col_counter++)
                {
                    int alive_neighbours = count_neighbours(current_states, row_counter, col_counter, num_rows, num_cols);

                    if (alive_neighbours == -1)
                    {
                        next_states[row_counter, col_counter] = -1;
                    }
                    else if ((alive_neighbours < 2) || (alive_neighbours > 3))
                    {
                        next_states[row_counter, col_counter] = 0;
                    }
                    else if ((current_states[row_counter, col_counter] == 0) && (alive_neighbours == 3))
                    {
                        next_states[row_counter, col_counter] = 1;
                    }
                    else if ((current_states[row_counter, col_counter] == 1) && ((alive_neighbours == 2) || (alive_neighbours == 3)))
                    {
                        next_states[row_counter, col_counter] = 1;
                    }
                }
            }

            return next_states;
        }

               
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int num_rows = 50;
            int num_cols = 50;
            
            int[,] current_states = get_initial_2D_array(num_rows, num_cols);         
            
            int pixel_width = panel1.ClientSize.Width / num_cols;
            int pixel_height = panel1.ClientSize.Height / num_rows;

            Graphics g = panel1.CreateGraphics();


            SolidBrush sb_black = new SolidBrush(Color.Black);
            SolidBrush sb_pink = new SolidBrush(Color.LightPink);
            SolidBrush sb_white = new SolidBrush(Color.White);



            while (true)
            {
                int[,] next_states = get_next_states(current_states, num_rows, num_cols);

                for (int row_counter = 0; row_counter < num_cols; row_counter++)
                {
                    for (int col_counter = 0; col_counter < num_rows; col_counter++)
                    {
                        if (next_states[row_counter, col_counter] == 1)
                        {
                            g.FillRectangle(sb_black, col_counter * pixel_width, row_counter * pixel_height, pixel_width, pixel_height);
                        }
                        else if (next_states[row_counter, col_counter] == -1)
                        {
                            g.FillRectangle(sb_pink, col_counter * pixel_width, row_counter * pixel_height, pixel_width, pixel_height);
                        }
                        else
                        {                            
                            g.FillRectangle(sb_white, col_counter * pixel_width, row_counter * pixel_height, pixel_width, pixel_height);
                        }

                    }
                                        
                }

                System.Threading.Thread.Sleep(10);
                current_states = next_states;

                
                                             
                
            }

        }

        
    }
}
