using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace WindowsFormsApp1
{

          
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>


        

        [STAThread]
        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Thread t = new Thread(Datareader);
            t.Start();
            Datawriter("234343");
        }


        static void Datareader()
        {

            SerialPort mySerialPort = new SerialPort("COM1");
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            try
            {
                mySerialPort.Open();

            }
            catch
            {
                Console.WriteLine("Wrong port..." + mySerialPort.PortName);
            }

        }


        private static void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Console.WriteLine("Data Received:");
            //Console.Write(indata);

            if (indata != null)
            {
                indata = Logarithmic_resistancecheck(indata);
            }
            

            Datawriter(indata);
        }

       
        public static bool Mysqlconnecter()
        { 

            MySqlConnection connection;
            string server;
            string database;
            string uid;
            string password;

            server = "localhost";
            database = "ardruino";
            uid = "root";
            password = "cybear";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }


        }

        private static void Datawriter(string indata)
        {
            if (!Mysqlconnecter())
            {
                MessageBox.Show("Nem sikerült csaltakozni az adatbázishoz");

            }
            else
            {
                Updatequerry(indata);
            }


        }

        private static void Updatequerry(string indata)
        {
            // string query = "INSERT INTO table_temperature ('Temeprature') VALUES ('" + indata.ToString() + "')";
            string query = "INSERT INTO table_temperature ('Temeprature') VALUES ('15')";
            //Iamhere();
        }

        public static void Iamhere()
        {
            MessageBox.Show("I am here now");
        }




        private static string Logarithmic_resistancecheck(string indata)
        {
            string temperature = null;

            return temperature;
        }

        public static void Testinsert()
        {

            MySqlConnection connection;
            string server;
            string database;
            string uid;
            string password;

            server = "localhost";
            database = "ardruino";
            uid = "root";
            password = "cybear";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            
            connection = new MySqlConnection(connectionString);
            connection.Open();

            string indata = "453";
            string query = "INSERT INTO table_temperature (ido, meleg) VALUES ('"+ DateTime.Now.ToString() +"',"+ indata+")";

            Clipboard.SetText(query);
            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //Execute command
            cmd.ExecuteNonQuery();

                
            
        }
    }
}
