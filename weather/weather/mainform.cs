using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace weather
{
    public partial class all_weather_btn : Form
    {
        public all_weather_btn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form = new allweather();
            form.StartPosition = FormStartPosition.CenterParent;
            form.Show();
        }
    }
}
