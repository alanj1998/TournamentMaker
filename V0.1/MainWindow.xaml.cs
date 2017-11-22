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
using AlanJLibraries;
using System.Data;
using System.Threading;

namespace V0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Random randVar = new Random(); //Static randomiser added at the top so that only one randomiser is created for an instance of the programme
        public int MyProperty;
        public MainWindow()
        {
            InitializeComponent();
        }

        /*
         *  On click method that clears list boxes and randomises groups for the world cup
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string>[] pots = GetPots();

            A.ItemsSource = "";
            b.ItemsSource = "";
            c.ItemsSource = "";
            d.ItemsSource = "";
            ET.ItemsSource = "";
            f.ItemsSource = "";
            g.ItemsSource = "";
            h.ItemsSource = "";

            List<string>[] groups = new List<string>[8];
            for(int i = 0; i < 8; i++)
            {
                groups[i] = new List<string>();
            }

            //Sort the teams into groups randomly
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    int random = Randomizer(pots[j].Count());
                    groups[i].Add(pots[j].ElementAt(random));
                    pots[j].Remove(pots[j].ElementAt(random));
                }
            }

            A.ItemsSource = groups[0];
            b.ItemsSource = groups[1];
            c.ItemsSource = groups[2];
            d.ItemsSource = groups[3];
            ET.ItemsSource = groups[4];
            f.ItemsSource = groups[5];
            g.ItemsSource = groups[6];
            h.ItemsSource = groups[7];
        }

        /*
         *  This method collects all the teams from the database and sort them into pots
         *      dependent on their real life pots
         */
        private List<string>[] GetPots()
        {
            DB database = new DB("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + System.Environment.CurrentDirectory + "\\Tournament.mdf;Integrated Security=True");
            database.AddQuery("SelectTeams", "EXEC SelectRussia2018Teams");
            DataTable dt = new DataTable();
            dt = database.RunQuery("SelectTeams");

            List<string>[] pots = { new List<string>(), new List<string>(), new List<string>(), new List<string>()};

            foreach (DataRow data in dt.Rows)
            {
                switch (data.Field<Int16>(1))
                {
                    case 1:
                        pots[0].Add(data.Field<string>(0));
                        break;
                    case 2:
                        pots[1].Add(data.Field<string>(0));
                        break;
                    case 3:
                        pots[2].Add(data.Field<string>(0));
                        break;
                    case 4:
                        pots[3].Add(data.Field<string>(0));
                        break;
                    default:
                        break;
                }
            }

            return pots;
        }

        /*
         *  This method gets a random number from the amount of countries left in each pot
         *  It is used to simulate the actual countries being taken from a pot before each
         *      world cup.
         */
        private int Randomizer(int max)
        {
            return randVar.Next(max);      
        }
    }

    /*
     *  Class that has to be implemented from Database_Connection abstract class
     */
    public class DB : Database_Connection
    {
        public DB(string connectionString) : base(connectionString) { }

        public override void ErrorMessage(string ErrorType)
        {
            switch(ErrorType)
            {
                case "noQry":
                    MessageBox.Show("");
                    break;
                case "deleteQry":
                    MessageBox.Show("");
                    break;
            }
        }
    }
}
