using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_SHARP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("+");
            comboBox1.Items.Add("-");
            comboBox1.Items.Add("*");
            comboBox1.Items.Add("/");
            comboBox1.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {


        }
     

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox1.Text, out double num1))
            {
                MessageBox.Show("第一个数字输入错误");
                return;
            }

            if (!double.TryParse(textBox2.Text, out double num2))
            {
                MessageBox.Show("第二个数字输入错误");
                return;
            }

            string op = comboBox1.SelectedItem.ToString();

            double result = 0;
            switch (op)
            {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    if (num2 == 0)
                    {
                        MessageBox.Show("不能除以零");
                        return;
                    }
                    result = num1 / num2;
                    break;
            }

            label1.Text = "结果: " + result.ToString();
        }
    }
}
