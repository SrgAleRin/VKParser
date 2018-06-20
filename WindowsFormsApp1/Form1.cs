using System;
using System.Windows.Forms;
using VkParser.Logic;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private AppLogic _mainLogic;
        private volatile int _mainCount = 0;
        private volatile int _linksCount = 0;
        private volatile int _imagesCount = 0;
        private delegate void ParameterDelegate(int value);

        public Form1()
        {
            InitializeComponent();
            this._mainLogic = new AppLogic();
            this._mainLogic.MoveNext += this.SafeMoveNext;
        }

        private void SafeMoveNext(object sender, int e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ParameterDelegate(this.MoveNext), new object[] { e });
                return;
            }
            MoveNext(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this._mainLogic.Login(this.textBox1.Text, this.textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this._mainCount = 0;
            this._linksCount = 0;
            this._imagesCount = 0;
            this.label9.Text = "Начато";
            this._mainLogic.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this._mainLogic.Stop();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void MoveNext(int e)
        {
            switch (e)
            {
                case 1:
                    this.label4.Text = this._mainCount++.ToString();
                    break;
                case 2:
                    this.label5.Text = this._linksCount++.ToString();
                    break;
                case 3:
                    this.label6.Text = this._imagesCount++.ToString();
                    break;
                case 4:
                    this.label9.Text = "Завершено";
                    break;
                default:
                    break;
            }
        }
    }
}
