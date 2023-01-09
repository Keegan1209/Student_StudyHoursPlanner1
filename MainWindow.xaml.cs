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
using ClassLibrary01;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Data;

namespace POE_PROG6212
{
    /// KEEGAN FRANK 20103712
    /// BCAD G1 YEAR2
    /// THIS CODE WILL HELP USERS MANAGE THIER TIME STUDYING FOR SEMESTER MODULES. IT WILL PROMPT THE USER TO ENTER DETAILS ABOUT EACH MODULE
    /// AND WILL HELP MANAGE THE USERS TIME STUDYING FOR EACH MODULE WEEKLY
    /// </FILE COPY OF ORININAL LOCATED IN REPOS>
    public partial class MainWindow : Window
    {
        // connection string
        String Connection_String = ConfigurationManager.ConnectionStrings["SqlConnection1"].ConnectionString;



        public MainWindow()
        {
            InitializeComponent();

            MessageBox.Show("Logged in as "+ Class1.userName , "Welcome Back!", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        public void dataBindGrid()
        {
            
            SqlConnection Conn = new SqlConnection(Connection_String);
           

            Conn.Open();

            // REMOVED CE
            SqlDataAdapter Adapt = new SqlDataAdapter("SELECT * FROM STUDYINFO1 WHERE STUDYINFO1.USERNAME = '" + Class1.userName + "'", Conn);


            DataSet Bind = new DataSet();

            Adapt.Fill(Bind, "MyDataBinding");

            ModuleDG.DataContext = Bind;

            Conn.Close();

        }


       

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {


            ValidateUserInput();



            try
            {



                SqlConnection Conn = new SqlConnection(Connection_String);


                Conn.Open();



                // assign textblock values to  classlibrary variables to be used in calculations. 
                Class1.modCode = txtModCode.Text;
                Class1.modName = txtModName.Text;
                Class1.modCredits = int.Parse(txtModCredits.Text);
                Class1.hoursPerWeek = double.Parse(txtHoursPerWeek.Text);
                Class1.usNumWeeks = double.Parse(txtNumWeeks.Text);
                Class1.compStudHours = double.Parse(txtStudyH.Text);

                // equations method from class library 
                Class1.SelfStudyHours();
                Class1.CompStudyHours();

                
                // Assign SQL table values to  class library variables.
                String PopulateDG = @"INSERT INTO STUDYINFO1 (USERNAME, SEMESTER_DATE, MOD_CODE, MOD_NAME, MOD_CREDITS, MOD_HPW, MOD_NUMWEEKS, STUD_HPW, SELF_STUDHPW, CALC_HPW)
                                        VALUES (@userName, @SemDate, @modCode, @modName, @modCredits, @hoursperweek, @usNumWeeks, @compStudyHours, @sshpw, @remStudyHours)";


                SqlCommand cmd = new SqlCommand(PopulateDG, Conn);

                // inserts class library variables into database columns. 
                cmd.Parameters.AddWithValue("@userName", Class1.userName);
                cmd.Parameters.AddWithValue("@SemDate", Class1.SemDate);
                cmd.Parameters.AddWithValue("@modCode", Class1.modCode);
                cmd.Parameters.AddWithValue("@modName", Class1.modName);
                cmd.Parameters.AddWithValue("@modCredits", Class1.modCredits);
                cmd.Parameters.AddWithValue("@hoursperweek", Class1.hoursPerWeek);
                cmd.Parameters.AddWithValue("@usNumWeeks", Class1.usNumWeeks);
                cmd.Parameters.AddWithValue("@compStudyHours", Class1.compStudHours);
                cmd.Parameters.AddWithValue("@sshpw", Class1.sshpw);
                cmd.Parameters.AddWithValue("@remStudyHours", Class1.remStudyHours);





                cmd.ExecuteNonQuery();

                MessageBox.Show("Rows Succesfully added","Successful", MessageBoxButton.OK);

                //stores new info in datagrid
                this.dataBindGrid();



                // clear textboxes 

                txtModCode.Text = "";
                txtModName.Text = "";
                txtModCredits.Text = "";
                txtHoursPerWeek.Text = "";
                txtNumWeeks.Text = "";
                txtStudyH.Text = "";


            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.ToString());

            }




        }





    

        // update datagrid entries method
        private void btnUPDATE_Click(object sender, RoutedEventArgs e)
        {

            ValidateUserInput(); 

            try
            {
                
                SqlConnection con = new SqlConnection(Connection_String);

                con.Open();

                // assign textblock values to  classlibrary variables to be used in calculations. 
                Class1.modCode = txtModCode.Text;
                Class1.modName = txtModName.Text;
                Class1.modCredits = int.Parse(txtModCredits.Text);
                Class1.hoursPerWeek = double.Parse(txtHoursPerWeek.Text);
                Class1.usNumWeeks = double.Parse(txtNumWeeks.Text);
                Class1.compStudHours = double.Parse(txtStudyH.Text);
                // equations method from class library 
                Class1.SelfStudyHours();
                Class1.CompStudyHours();

                // UPDATE query to update data in datagrid where module codes are equal. 
                String UpdateInfo = @"UPDATE STUDYINFO1 SET SEMESTER_DATE ='" + SelectDate.ToString() + "',MOD_CODE='" + txtModCode.Text + "',MOD_NAME='" + txtModName.Text

                                     + "',MOD_CREDITS='" + int.Parse(txtModCredits.Text) + "',MOD_HPW='" + double.Parse(txtHoursPerWeek.Text) + "',MOD_NUMWEEKS='" + double.Parse(txtNumWeeks.Text)
                                     + "',STUD_HPW='" + double.Parse(txtStudyH.Text) + "',SELF_STUDHPW='" + Class1.sshpw.ToString() + "',CALC_HPW='" + Class1.remStudyHours.ToString()
                                     + "'WHERE MOD_CODE ='" + txtModCode.Text + "'";

                SqlCommand updateCmd = new SqlCommand(UpdateInfo, con);

                updateCmd.ExecuteNonQuery();


                // binds input to datagrid
                this.dataBindGrid(); 

            }
            catch 

            {

                MessageBox.Show("column name does not exist", "Error" , MessageBoxButton.OK, MessageBoxImage.Error);

            }




        }


        // exit application
        private void btnFinish_Click(object sender, RoutedEventArgs e)
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


        // dragable window
        private void mainWinMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ModuleDG_Loaded(object sender, RoutedEventArgs e)
        {
            dataBindGrid(); 
        }



        // validates user input
        public void ValidateUserInput()
        {
            // checks if user inputs are valid 

            if (String.IsNullOrEmpty(txtModName.Text) || String.IsNullOrEmpty(txtModCode.Text) || String.IsNullOrEmpty(txtHoursPerWeek.Text) || String.IsNullOrEmpty(txtModCredits.Text))
            {
                MessageBox.Show("Please fill in all fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);

                return;

            }
            // use of regex to check if numbers are in range and user only enters numerical digits
            else if (!Regex.Match(txtModCredits.Text, @"^[0-9,]*$").Success)
            {
                MessageBox.Show("Module Credits Must be Numerical Digit", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                txtModCredits.Focus();
                return;

            }
            else if (!Regex.Match(txtNumWeeks.Text, @"^[0-9,]*$").Success)
            {
                MessageBox.Show("Number of weeks Must be Numerical Digit", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNumWeeks.Focus();
                return;

            }
            else if (!Regex.Match(txtHoursPerWeek.Text, @"^[0-9,]*$").Success)
            {
                MessageBox.Show("Hours Must be Numerical Digit", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                txtModCredits.Focus();
                return;

            }
            else if (!Regex.Match(txtStudyH.Text, @"^[0-9,]*$").Success)
            {
                MessageBox.Show("Hours Must be Numerical Digit", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                txtModCredits.Focus();
                return;

            }


            else
            {
                if (SelectDate.SelectedDate.HasValue)
                {
                    Class1.SemDate = SelectDate.SelectedDate.Value.ToString("MM/dd/yyyy");

                }
                else
                {
                    MessageBox.Show("You have to select a semester start date", "Warning", MessageBoxButton.OK, MessageBoxImage.Error); // error message to be show if a user does not enter a semester start date
                }

            }
        }






    }




    }




