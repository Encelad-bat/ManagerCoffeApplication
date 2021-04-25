using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SellerCoffeApplication.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Dapper;

namespace ManagerCoffeApplication.ViewModel
{
    class CoffeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public ObservableCollection<Coffe> Drinks { get; set; }

        private Coffe selectedCoffe;

        public Coffe SelectedCoffe
        {
            get { return selectedCoffe; }
            set { selectedCoffe = value; OnPropertyChanged("SelectedCoffe"); }
        }


        private RelayCommand addCommand;
        public RelayCommand AddCommand {

            get
            {
                return addCommand ?? (addCommand = new RelayCommand(added =>
                {
                    if ((added as Coffe) != null)
                    {
                        Drinks.Add(added as Coffe);
                        using (IDbConnection connection = new SqlConnection("Data Source=SQL5103.site4now.net;Initial Catalog=DB_A717C9_MAIN;User Id=DB_A717C9_MAIN_admin;Password=Gorb_bc24"))
                        {
                            connection.Open();

                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    connection.Execute("INSERT INTO Coffe (Price,Quanity,Type) VALUES(@Price,@Quanity,@Type)", new { Price = (added as Coffe).Price, Quanity = (added as Coffe).Quanity, Type = (added as Coffe).Type }, transaction);
                                    transaction.Commit();
                                }
                                catch
                                {
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                }));
            }
                
        }

        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {

            get
            {
                return editCommand ?? (editCommand = new RelayCommand(edited =>
                {
                    if ((edited as Coffe) != null)
                    {
                        int id = Drinks.IndexOf((edited as Coffe));
                        Drinks.Remove((edited as Coffe));
                        using (IDbConnection connection = new SqlConnection("Data Source=SQL5103.site4now.net;Initial Catalog=DB_A717C9_MAIN;User Id=DB_A717C9_MAIN_admin;Password=Gorb_bc24"))
                        {
                            connection.Open();

                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    connection.Execute("UPDATE Coffe Price=@Price,Quanity=@Quanity,Type=@Type WHERE Id=@Id", new { Price = (edited as Coffe).Price, Quanity = (edited as Coffe).Quanity, Type = (edited as Coffe).Type, Id= (edited as Coffe).Id }, transaction);
                                    transaction.Commit();
                                }
                                catch
                                {
                                    transaction.Rollback();
                                }
                            }
                        }
                        Drinks.Insert(id, (edited as Coffe));
                    }
                }));
            }

        }

        public CoffeViewModel()
        {
            using (IDbConnection connection = new SqlConnection("Data Source=SQL5103.site4now.net;Initial Catalog=DB_A717C9_MAIN;User Id=DB_A717C9_MAIN_admin;Password=Gorb_bc24"))
            {
                Drinks = new ObservableCollection<Coffe>(connection.Query<Coffe>("SELECT * FROM Coffe;").ToList());
            }
        }
    }
}
