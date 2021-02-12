using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();
        public int cash_now = 100;
        public int cash_now_too = 100;
        public string game_over = "Game over";
        public int time_now = 0;
        public int m = 0;
        public int s;

        public Form1()
        {
            InitializeComponent();
            foreach (CheckBox cb in panel1.Controls)
                field.Add(cb, new Cell());
            cash.Text = cash_now.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            if (cb.Checked) Plant(cb);
            else Harvest(cb);
            if (cash_now < 0)
                //cash.Text = game_over;
                MessageBox.Show(String.Format(game_over));
            else
            {
                cash.Text = cash_now.ToString();
                if (cash_now - cash_now_too < 0)
                    index.Text = "-" + (cash_now_too - cash_now).ToString();
                else
                    index.Text = "+" + (cash_now - cash_now_too).ToString();
            }
            cash_now_too = cash_now;
    }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (CheckBox cb in panel1.Controls)
                NextStep(cb);
            time_now += timer1.Interval;
            s = time_now / 1000;
            if (m > 0)
            {
                s = Math.Abs(m * 60 - s);
            }
            if (s > 0 && s % 60 == 0)
            {
                s = 0;
                m += 1;
            }
            string timer = "";

            if (m < 10)
            {
                timer += "0" + m.ToString();
            }
            else
            {
                timer += m.ToString();
            }

            timer += ":";

            if (s < 10)
            {
                timer += "0" + s.ToString();
            }
            else
            {
                timer += s.ToString();
            }
            time.Text = timer;
        }

        private void Plant(CheckBox cb)
        {
            cash_now += field[cb].money;
            field[cb].Plant();
            UpdateBox(cb);

        }

        private void Harvest(CheckBox cb)
        {
            cash_now += field[cb].money;
            field[cb].Harvest();
            UpdateBox(cb);
        }

        private void NextStep(CheckBox cb)
        {
            field[cb].NextStep();
            UpdateBox(cb);
        }

        private void UpdateBox(CheckBox cb)
        {
            Color c = Color.White;
            switch (field[cb].state)
            {
                case CellState.Planted:
                    c = Color.Black;
                    break;
                case CellState.Green:
                    c = Color.Green;
                    break;
                case CellState.Immature:
                    c = Color.Yellow;
                    break;
                case CellState.Mature:
                    c = Color.Red;
                    break;
                case CellState.Overgrow:
                    c = Color.Brown;
                    break;
            }
            cb.BackColor = c;
        }

        private void SpeedCount()
        {

        }

        private void speed_ValueChanged(object sender, EventArgs e)
        {
            decimal number_of_speed = speed.Value;
            double for_value1 = decimal.ToDouble(number_of_speed);
            int for_value2 = (int)for_value1;
            switch (for_value2)
            {
                case 1:
                    timer1.Interval = 175;
                    break;
                case 2:
                    timer1.Interval = 150;
                    break;
                case 3:
                    timer1.Interval = 125;
                    break;
                case 4:
                    timer1.Interval = 100;
                    break;
                case 5:
                    timer1.Interval = 75;
                    break;
                case 6:
                    timer1.Interval = 50;
                    break;
                case 7:
                    timer1.Interval = 25;
                    break;

            }
        }
    }

    enum CellState
    {
        Empty,
        Planted,
        Green,
        Immature,
        Mature,
        Overgrow
    }

    class Cell
    {
        public CellState state = CellState.Empty;
        public int progress = 0;
        public int money = -2;

        private const int prPlanted = 20;
        private const int prGreen = 100;
        private const int prImmature = 120;
        private const int prMature = 140;
        private const int money_for_empty = -2;
        private const int money_for_immature = 3;
        private const int money_for_mature = 5;
        private const int money_for_overgrow = -1;
        private const int money_for_planted = 0;
        private const int money_for_green = 0;

        public void Plant()
        {
            money = money_for_planted;
            state = CellState.Planted;
            progress = 1;
        }

        public void Harvest()
        {
            money = money_for_empty;
            state = CellState.Empty;
            progress = 0;
        }

        public void NextStep()
        {
            if ((state != CellState.Empty) && (state != CellState.Overgrow))
            {
                progress++;
                if (progress < prPlanted)
                {
                    state = CellState.Planted;
                    money = money_for_planted;
                }
                else if (progress < prGreen)
                {
                    state = CellState.Green;
                    money = money_for_green;
                }
                else if (progress < prImmature)
                {
                    state = CellState.Immature;
                    money = money_for_immature;
                }
                else if (progress < prMature)
                {
                    state = CellState.Mature;
                    money = money_for_mature;
                }
                else
                {
                    state = CellState.Overgrow;
                    money = money_for_overgrow;
                }
            }
        }
    }
}