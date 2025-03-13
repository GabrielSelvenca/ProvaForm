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
using FiorentinoForm.Components;
using FiorentinoForm.Models;
using FiorentinoForm.Properties;

namespace FiorentinoForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            panel1.BackColor = UserData.lightGray;
            label1.ForeColor = UserData.green;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            flowLayoutPanel2.AutoScroll = flowLayoutPanel1.AutoScroll = true;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            Icon = Icon.FromHandle(Resources.logomarca_2.GetHicon());
        }

        MobileALEntities ctx = new MobileALEntities();
        List<participante> listaParticipantes = new List<participante>();
        private void Form1_Load(object sender, EventArgs e)
        {
            listaParticipantes = ctx.participante.Take(200).ToList();
            foreach (var participante in listaParticipantes)
            {
                participante.pontos = participante.vendas.Count() * 12 + participante.vendas.Sum(x => x.quantidade * x.produto.valor ?? 0) * 24;
            }
            comboBox1.Items.AddRange(ctx.estado.Select(x => x.Sigla).OrderBy(x => x).ToArray());


            LoadLista(listaParticipantes);

        }


        private void LoadLista(List<participante> listaParticipantes, Boolean order = false)
        {
            if (!order)
            {
                listaParticipantes = listaParticipantes.OrderByDescending(x => x.pontos).ToList();
            }


            flowLayoutPanel1.Controls.Clear();

            int posicao = 1;
            foreach (var item in listaParticipantes)
            {
                item.posicao = posicao;
                posicao++;
                flowLayoutPanel1.Controls.Add(new UserControl1(item));
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var listPar = listaParticipantes.ToList();
            bool orderByPoints = false;

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                LoadLista(listaParticipantes);
                return;
            }

            string pesquisa = textBox1.Text;
            if (pesquisa.StartsWith(">"))
            {
                pesquisa = pesquisa.Replace(">", "");
                if (int.TryParse(pesquisa, out int valor))
                {
                    listPar = listPar.Where(x => x.pontos > valor).ToList();
                }
            }
            else if (pesquisa.StartsWith("<"))
            {
                pesquisa = pesquisa.Replace("<", "");
                if (int.TryParse(pesquisa, out int valor))
                {
                    listPar = listPar.Where(x => x.pontos < valor).ToList();
                }
            }
            else if (pesquisa.StartsWith("+"))
            {
                pesquisa = pesquisa.Replace("+", "");
                listPar = listPar.Where(x => x.cidade.estado.Sigla.ToLower() == pesquisa.ToLower()).ToList();
            }
            else if (pesquisa.StartsWith(">="))
            {
                pesquisa = pesquisa.Replace(">=", "");
                if (int.TryParse(pesquisa, out int valor))
                {
                    listPar = listPar.Where(x => x.pontos >= valor).ToList();
                }
            }
            else if (pesquisa.StartsWith("<="))
            {
                pesquisa = pesquisa.Replace("<=", "");
                if (int.TryParse(pesquisa, out int valor))
                {
                    listPar = listPar.Where(x => x.pontos <= valor).ToList();
                }
            }
            else if (pesquisa.StartsWith("%") || pesquisa.EndsWith("%"))
            {
                pesquisa = pesquisa.Replace("%", "");
                if (!string.IsNullOrEmpty(pesquisa))
                {
                    if (pesquisa.StartsWith("%") && pesquisa.EndsWith("%"))
                    {
                        listPar = listPar.Where(x => !string.IsNullOrEmpty(x.nome) && x.nome.ToLower().Contains(pesquisa.ToLower())).ToList();
                    }
                    else if (pesquisa.StartsWith("%"))
                    {
                        listPar = listPar.Where(x => !string.IsNullOrEmpty(x.nome) && x.nome.ToLower().EndsWith(pesquisa.ToLower())).ToList();
                    }
                    else if (pesquisa.EndsWith("%"))
                    {
                        listPar = listPar.Where(x => !string.IsNullOrEmpty(x.nome) && x.nome.ToLower().StartsWith(pesquisa.ToLower())).ToList();
                    }
                }
            }
            else if (pesquisa.StartsWith("!>"))
            {
                pesquisa = pesquisa.Replace("!>", "");
                if (pesquisa.ToLower() == "pa")
                {
                    listPar = listPar.OrderBy(x => x.nome).ToList();
                    orderByPoints = true;
                }
                else if (pesquisa.ToLower() == "po")
                {
                    listPar = listPar.OrderBy(x => x.pontos).ToList();
                    orderByPoints = true;
                }
            }
            else if (pesquisa.StartsWith("!<"))
            {
                pesquisa = pesquisa.Replace("!<", "");
                if (pesquisa.ToLower() == "pa")
                {
                    listPar = listPar.OrderByDescending(x => x.nome).ToList();
                    orderByPoints = true;
                }
                else if (pesquisa.ToLower() == "po")
                {
                    orderByPoints = false;
                }
            }
            else if (pesquisa.All(char.IsDigit))
            {
                var pontos = int.Parse(pesquisa);
                listPar = listPar.Where(x => x.pontos == pontos).ToList();
            }
            else
            {
                listPar = listPar.Where(x => x.nome.ToLower().Contains(pesquisa.ToLower())).ToList();
            } 


            LoadLista(listPar, orderByPoints);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            var lista = listaParticipantes.Where(x => x.cidade.estado.Sigla == comboBox1.SelectedItem.ToString()).ToList();

            LoadLista(lista);
        }
    }
}
