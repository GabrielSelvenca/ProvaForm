using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FiorentinoForm.Models;

namespace FiorentinoForm.Components
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public UserControl1(participante item)
        {
            InitializeComponent();
            Item = item;
            label1.Text = item.posicao.ToString();
            string[] partsName = item.nome.Split(' ');
            label2.Text = partsName[0];
            label3.ForeColor = circleBox1.BackColor = label8.BackColor = UserData.green;
            label3.Text = partsName[1];
            label4.Text = item.cidade.estado.Sigla;
            label5.Text = item.pontos.ToString();
            label8.Text = $"{partsName[0][0]}{partsName[1][0]}";
            string pathBandeira = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bandeiras", $"{item.cidade.estado.Sigla}.png");

            if(File.Exists(pathBandeira))
                pictureBox1.Image = Image.FromFile(pathBandeira);
        }

        public participante Item { get; }

        private void circleBox1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
