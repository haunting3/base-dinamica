using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SbsSW.SwiPlCs;

namespace InterfaceProlog
{
    public partial class formBD : Form
    {
        public formBD()
        {
            InitializeComponent();
        }

        public void AtualizarList()
        {
            listView1.Clear();

            listView1.View = View.Details;
            listView1.Columns.Add("Nome", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Poder", 60, HorizontalAlignment.Left);
            PlQuery poder = new PlQuery("poder(X,Y)");
            foreach (PlQueryVariables var in poder.SolutionVariables)
            {
                ListViewItem item = new ListViewItem(new[] { var["X"].ToString(), var["Y"].ToString() });
                listView1.Items.Add(item);
            }
        }

        private void formBD_Load(object sender, EventArgs e)
        {
            Environment.SetEnvironmentVariable("SWI_HOME_DIR", @"prolog");
            Environment.SetEnvironmentVariable("Path", @"prolog");
            Environment.SetEnvironmentVariable("Path", @"prolog\bin");
            string[] p = { "-q", "-f", @"regras.pl" };
            PlEngine.Initialize(p);
            PlQuery c = new PlQuery("carrega");
            c.NextSolution();

            AtualizarList();
        }

        private void formBD_FormClosing(object sender, FormClosingEventArgs e)
        {
            PlQuery q = new PlQuery("salva");
            q.NextSolution();
            PlEngine.PlCleanup();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == String.Empty || textBox4.Text == String.Empty)
            {
                MessageBox.Show("Campo em branco.");
            }
            else
            {
                PlQuery add = new PlQuery("adiciona(Nome,Poder)");
                add.Variables["Nome"].Unify(textBox3.Text.ToLower());
                add.Variables["Poder"].Unify(textBox4.Text);
                add.NextSolution();

                AtualizarList();

                MessageBox.Show("Personagem adicionado.");
                textBox3.Text = "";
                textBox4.Text = "";
                textBox3.Focus();
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == String.Empty)
            {
                MessageBox.Show("Campo em branco.)");
            }
            else
            {
                PlQuery remove = new PlQuery("remove(Nome)");
                remove.Variables["Nome"].Unify(textBox5.Text.ToLower());
                remove.NextSolution();

                AtualizarList();

                MessageBox.Show("Personagem removido.");
                textBox5.Text = "";
                textBox5.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlQuery nome = new PlQuery("poder(Nome,_)");
            PlQuery nome2 = new PlQuery("poder(Nome2,_)");
            nome.Variables["Nome"].Unify(textBox1.Text.ToLower());
            nome2.Variables["Nome2"].Unify(textBox2.Text.ToLower());

            if (nome.NextSolution() == true && nome2.NextSolution() == true)
            {
                PlQuery batalha = new PlQuery("comp(Nome, Nome2)");
                batalha.Variables["Nome"].Unify(textBox1.Text.ToLower());
                batalha.Variables["Nome2"].Unify(textBox2.Text.ToLower());

                if (batalha.NextSolution() == true)
                {
                    label3.Text = batalha.Variables["Nome"].ToString().ToUpper() + " derrotou " + batalha.Variables["Nome2"].ToString().ToUpper();
                }
                else
                {
                    label3.Text = batalha.Variables["Nome2"].ToString().ToUpper() + " derrotou " + batalha.Variables["Nome"].ToString().ToUpper();
                }
                textBox1.Text = "";
                textBox2.Text = "";
                textBox1.Focus();
            }
            else
            {
                MessageBox.Show("Personagem não existe");
            }
        }
    }
}
