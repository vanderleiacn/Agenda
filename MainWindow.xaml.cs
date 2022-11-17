using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Agenda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string operacao;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btSalvar_Click(object sender, RoutedEventArgs e)
        {
            
            //gravar no banco de dados
            if (operacao == "inserir")
            {
                //contato com os dados da tela
                contato c = new contato();
                c.nome = txtNome.Text;
                c.email = txtEmail.Text;
                c.telefone = txtTelefone.Text;

                using (agendaEntities ctx = new agendaEntities())  // ctx representa o banco de dados
                {

                    ctx.contatos.Add(c); // representa todos os dados que estao na tabela contatos
                    ctx.SaveChanges();
                }
            }
            if (operacao == "alterar")
            {
                using (agendaEntities ctx = new agendaEntities())
                {
                    contato c = ctx.contatos.Find(Convert.ToInt32(txtID.Text));
                    if (c != null)
                    {
                        c.nome = txtNome.Text;
                        c.email = txtEmail.Text;
                        c.telefone = txtTelefone.Text;
                        ctx.SaveChanges();
                    }
                }
            }
            this.ListarContatos();
            this.AlteraBotoes(1);
            this.LimpaCampos();
        }
        private void LimpaCampos()
        {
            txtID.IsEnabled = true;
            txtID.Clear();
            txtEmail.Clear();
            txtNome.Clear();
            txtTelefone.Clear();
        }

        private void btInserir_Click(object sender, RoutedEventArgs e)
        {
            this.operacao = "inserir";
            this.AlteraBotoes(2);
            this.LimpaCampos();
            txtID.IsEnabled = false;
        }

        private void ListarContatos()
        {
            using (agendaEntities ctx = new agendaEntities())  
            {
                int a = ctx.contatos.Count();
                lbQtdContatos.Content = " Contatos existentes: " + a.ToString();
                var consulta = ctx.contatos;
                dgDados.ItemsSource = consulta.ToList();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ListarContatos();
            this.AlteraBotoes(1);
        }
        private void AlteraBotoes(int op)
        {
            btAlterar.IsEnabled = false;
            btInserir.IsEnabled = false;
            btExcluir.IsEnabled = false;
            btCancelar.IsEnabled = false;
            btLocalizar.IsEnabled = false;
            btSalvar.IsEnabled = false;
            if(op == 1)
            { //ativar as opçoes iniciais
                btInserir.IsEnabled = true;
                btLocalizar.IsEnabled = true;
            }
            if(op == 2)
            {//inserir um valor
                btCancelar.IsEnabled= true;
                btSalvar.IsEnabled = true;

            }
            if (op == 3)
            {
                btAlterar.IsEnabled = true;
                btExcluir.IsEnabled = true;
            }
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.AlteraBotoes(1);
            this.LimpaCampos();
        }

        private void btLocalizar_Click(object sender, RoutedEventArgs e)
        {
            if(txtID.Text.Trim().Count() > 0)
            {
                //buscar pelo código
                try
                {
                    int id = Convert.ToInt32(txtID.Text);
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        // var consulta = ctx.contatos;
                        // dgDados.ItemsSource = consulta.ToList();
                        contato c = ctx.contatos.Find(id);
                        dgDados.ItemsSource = new contato[1] { c };
                    }
                   
                }
                catch { }
            }
            if (txtNome.Text.Trim().Count() > 0)
            {
                try
                {
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        
                        var consulta = from c in ctx.contatos
                                       where c.nome.Contains(txtNome.Text)
                                       select c;
                        dgDados.ItemsSource = consulta.ToList();
                    }
                }
                catch { }
            }

            if (txtEmail.Text.Trim().Count() > 0)
            {
                try
                {
                    using (agendaEntities ctx = new agendaEntities())
                    {
                        
                        var consulta = from c in ctx.contatos
                                       where c.email.Contains(txtEmail.Text)
                                       select c;
                        dgDados.ItemsSource = consulta.ToList();
                    }
                }
                catch { }
            }
            if (txtTelefone.Text.Trim().Count() > 0)
            {
                try
                {
                    using (agendaEntities ctx = new agendaEntities())
                    {

                        var consulta = from c in ctx.contatos
                                       where c.telefone.Contains(txtTelefone.Text)
                                       select c;
                        dgDados.ItemsSource = consulta.ToList();
                    }
                }
                catch { }
            }

        }

        private void dgDados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dgDados.SelectedIndex >= 0)
            {
                //contato c = (contato)dgDados.Items[dgDados.SelectedIndex];
                contato c = (contato)dgDados.SelectedItem;
               
                txtID.Text = c.id.ToString();
                txtNome.Text = c.nome;
                txtEmail.Text = c.email;
                txtTelefone.Text = c.telefone;
                this.AlteraBotoes(3);
            }
        }

        private void btAlterar_Click(object sender, RoutedEventArgs e)
        {
            this.operacao = "alterar";
            this.AlteraBotoes(2);
            txtID.IsEnabled = false;
        }

        private void btExcluir_Click(object sender, RoutedEventArgs e)
        {
            using (agendaEntities ctx = new agendaEntities())
            {
                contato c = ctx.contatos.Find(Convert.ToInt32(txtID.Text));
                if(c != null)
                {
                    ctx.contatos.Remove(c);
                    ctx.SaveChanges();
                }
                this.ListarContatos();
                this.AlteraBotoes(1);
                this.LimpaCampos();
            }
            
        }
    }
}
