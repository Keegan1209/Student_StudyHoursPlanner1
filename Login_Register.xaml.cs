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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Security.Cryptography;
using ClassLibrary01;


namespace POE_PROG6212
{
    
    public partial class Login_Register : Window
    {

        
        public Login_Register()
        {

            InitializeComponent();
        }



        // login method if username exists in database. 
         private void btnLogin_click(object sender, RoutedEventArgs e)
        {

            try
            {
                // sql database connection string
                SqlConnection con = new SqlConnection(@"Data Source=KEEGANSPC\SQLEXPRESS02;Initial Catalog=PROG2_POE;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");

                // Relative sql script 
                // SqlConnection con = new SqlConnection(@"Keegan Frank 20103712 -PROG6212 POE TASK 2\PROG POE TASK 2\POE_PROG6212\bin\Debug\Keegan Frank 20103712 SQL PROG POE TASK 2.sql");


                con.Open();

                // select all items from database table USER1 where the usernames and passwords are equal to the user input username and password.
                String findDetails = "SELECT * FROM [dbo].[USER1] WHERE  USERNAME=@USERNAME AND PASSWORD = @PASSWORD ";
                SqlCommand cmd2 = new SqlCommand(findDetails, con);

                
                
                cmd2.Parameters.AddWithValue("@USERNAME", txtUserName.Text);
                // encrypts password in the database. 
                cmd2.Parameters.AddWithValue("@PASSWORD", Class1.hashPassword(txtPassword.Password));
                cmd2.ExecuteNonQuery();
                int Count = Convert.ToInt32(cmd2.ExecuteScalar());
                // assign primary key value of USER1 table to a variable userName in ClassLibrary01
                Class1.userName = txtUserName.Text;


                con.Close();


               

                if(Count>0)
                {

                    MainWindow mw = new MainWindow();
                    mw.Show();

                    this.Close(); 
                   

                    
                }
                else
                {
                    MessageBox.Show("username or password is incorrect", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                }



            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.ToString());

            }



        }
       
        // Register new username and password to the database. 
        private void btnRegister_click(object sender, RoutedEventArgs e)
        {

            try
            {

                SqlConnection con = new SqlConnection(@"Data Source=KEEGANSPC\SQLEXPRESS02;Initial Catalog=PROG2_POE;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
                //SqlConnection con = new SqlConnection(@"Keegan Frank 20103712 -PROG6212 POE TASK 2\PROG POE TASK 2\POE_PROG6212\bin\Debug\Keegan Frank 20103712 SQL PROG POE TASK 2.sql");




                con.Open();

                // insert username and password into databse table USER1
                String addDetails = "INSERT INTO [dbo].[USER1] VALUES (@USERNAME, @PASSWORD)";

                SqlCommand cmd = new SqlCommand(addDetails, con);

                

                cmd.Parameters.AddWithValue("@USERNAME", txtUserName.Text);
                // INSERTED PASSWORD ENCRYPTED IN DATABASE. 
                cmd.Parameters.AddWithValue("@PASSWORD", Class1.hashPassword(txtPassword.Password));
               
                cmd.ExecuteNonQuery();

                con.Close();
                
                MessageBox.Show("WELCOME! " + txtUserName.Text + " You have succesfully registered to the system."+"\nClick LOGIN to gain access","Successful", MessageBoxButton.OK, MessageBoxImage.None );

            }

            // catch error
            catch 

            {
                txtUserName.Text = "";
                txtPassword.Password = "";
                
                // notify user that username is taken to ensure all usernames are unique to a sepcific user. 
                MessageBox.Show("USERNAME is taken, please enter a new one ", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);


               

            }







        }


        private void LoginRegisterMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        // close window method
        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult prompt = MessageBox.Show("Are you sure you want to exit?", "Notice", MessageBoxButton.YesNo, MessageBoxImage.Warning); // exit message 

            if (prompt == MessageBoxResult.Yes)
            {

                Close();
            }
            else if (prompt == MessageBoxResult.No)
            {
                MessageBox.Show("Press OK to resume", "Continue App", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}
