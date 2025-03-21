using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private Timer filterTimer;

        private void Form1_Load(object sender, EventArgs e)
        {
            listaParticipantes = ctx.participante.Take(200).ToList();
            foreach (var participante in listaParticipantes)
            {
                participante.pontos = participante.vendas.Count() * 12 + participante.vendas.Sum(x => x.quantidade * x.produto.valor ?? 0) * 24;
            }
            comboBox1.Items.AddRange(ctx.estado.Select(x => x.Sigla).OrderBy(x => x).ToArray());

            filterTimer = new Timer();
            filterTimer.Interval = 700;
            filterTimer.Tick += FilterTimer_Tick;

            LoadLista(listaParticipantes);

        }

        private void FilterTimer_Tick(object sender, EventArgs e)
        {
            filterTimer.Stop();
            ApplyFilter(textBox1.Text);
        }


        private void LoadLista(List<participante> listaParticipantes, bool orderDescending = true, bool orderByPoints = true)
        {
            if (orderDescending)
            {
                listaParticipantes = listaParticipantes.OrderByDescending(x => orderByPoints ? (int?)x.pontos : (object)x.nome).ToList();
            }
            else
            {
                listaParticipantes = listaParticipantes.OrderBy(x => orderByPoints ? (int?)x.pontos : (object)x.nome).ToList();
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

        private void ApplyFilter(string pesquisa)
        {
            var listPar = listaParticipantes.ToList();

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                LoadLista(listaParticipantes);
                return;
            }

            if (pesquisa.StartsWith(">="))
            {
                pesquisa = pesquisa.Replace(">=", "").Trim();
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
            else if (pesquisa.StartsWith("+"))
            {
                pesquisa = pesquisa.Replace("+", "");
                listPar = listPar.Where(x => x.cidade.estado.Sigla.ToLower() == pesquisa.ToLower()).ToList();
            }
            else if (pesquisa.StartsWith(">"))
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
            else if (pesquisa.StartsWith("%") || pesquisa.EndsWith("%"))
            {
                
                if (!string.IsNullOrEmpty(pesquisa.Replace("%", "")))
                {
                    
                    if (pesquisa.StartsWith("%") && pesquisa.EndsWith("%"))
                    {
                        pesquisa = pesquisa.Replace("%", "");
                        listPar = listPar.Where(x => x.nome.ToLower().Contains(pesquisa.ToLower())).ToList();
                    }
                    else if (pesquisa.StartsWith("%"))
                    {
                        pesquisa = pesquisa.Replace("%", "");
                        listPar = listPar.Where(x => x.nome.ToLower().EndsWith(pesquisa.ToLower())).ToList();
                    }
                    else if (pesquisa.EndsWith("%"))
                    {
                        pesquisa = pesquisa.Replace("%", "");
                        listPar = listPar.Where(x => x.nome.ToLower().StartsWith(pesquisa.ToLower())).ToList();
                    }
                }
            }
            else if (pesquisa.StartsWith("!>"))
            {
                pesquisa = pesquisa.Replace("!>", "");
                if (pesquisa.ToLower() == "pa")
                {
                    LoadLista(listPar, false, false);
                }
                else if (pesquisa.ToLower() == "po")
                {
                    LoadLista(listPar);
                }
                return;
            }
            else if (pesquisa.StartsWith("!<"))
            {
                pesquisa = pesquisa.Replace("!<", "");
                if (pesquisa.ToLower() == "pa")
                {
                    LoadLista(listPar, true, false);
                }
                else if (pesquisa.ToLower() == "po")
                {
                    LoadLista(listPar, false);
                }
                return;
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

            LoadLista(listPar);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            filterTimer.Stop();
            filterTimer.Start();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            var lista = listaParticipantes.Where(x => x.cidade.estado.Sigla == comboBox1.SelectedItem.ToString()).ToList();

            LoadLista(lista);
        }
    }
}
