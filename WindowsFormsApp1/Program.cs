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
            Task t = new Task(Datareaderthread);
            t.Start();
            Application.Run(new Form1());
            
            //create a new thread for the data reader
            
            
            
        }

        public static void Datareaderthread()
        {
            
            SerialPort mySerialPort = new SerialPort("COM4");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                
            mySerialPort.Open();
            
        }

    private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadExisting();
        Logarithmic_resistance_to_temp(indata);


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
            string query = "INSERT INTO table_temperature ('meleg') VALUES ('" + indata + "')";
            //Iamhere();
        }

        public static void Iamhere()
        {
            MessageBox.Show("I am here now");
        }




        private static string Logarithmic_resistance_to_temp(string indata)
        {
            //trying to determine the resistance of the thermistor
            string temperature;
            double thermistor_ressist;
            int read_voltage;
            temperature = null;
            //the resostance of the known resisstor:
            int known_res;
            
            known_res = 3000;

            int.TryParse(indata, out read_voltage);
            // R2 = R1 * (1023.0 / (float)Vo - 1.0)
            thermistor_ressist =( known_res * ((1023.0 / (read_voltage)) - 1.0));
            //we now have the resistance
            MessageBox.Show(thermistor_ressist.ToString());            
            /*
            logR2 = log(R2);
            T = (1.0 / (c1 + c2 * logR2 + c3 * logR2 * logR2 * logR2));
            T = T - 273.15;
            T = (T * 9.0) / 5.0 + 32.0;

            */



            return temperature;
        }

        public static void Test()
        {
            
        }
    }
}
