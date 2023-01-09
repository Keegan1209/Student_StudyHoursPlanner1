using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ClassLibrary01
{
    public class Class1
    {

        


        //module information
        private  String ModCode;
        private  String ModName;
        private  int ModCredits;
        private double HoursPerWeek;


        public static String modCode { get ; set; }

        public static String modName { get; set; }

        public static int modCredits { get; set; }
        public static double hoursPerWeek { get; set; }





        private double UsNumWeeks;
        private double UsStartDate;
        private double SSHPW;
        private String semDate; 

       

        private double CompStudHours;

        private double RemStudyHours;

        // user input semester start and end date
        public static string SemDate { get; set; }

        public static double usNumWeeks { get; set; }

        public static double usStartDate { get; set; }


        public static double sshpw { get; set; }

        public static double compStudHours { get; set; }

        public static double remStudyHours { get; set; }





        


        public static string userName { get; set; }

        public static string userID { get; set; } 



        public static void SelfStudyHours()
        {

            sshpw = (modCredits * 10 / usNumWeeks) - hoursPerWeek;
        }

        public static void CompStudyHours()
        {
            remStudyHours = sshpw - compStudHours;  
        }






        public Class1(String modCode, String modName, int modCredits, double hoursPerWeek, double sshpw, double remStudyHours, String SemDate)
        {
            this.ModCode = modCode;
            this.ModName = modName;
            this.ModCredits = modCredits;
            this.HoursPerWeek = hoursPerWeek;
            this.SSHPW = sshpw;
            this.RemStudyHours = remStudyHours;
            this.semDate = SemDate; 
            

        }


       
       

       public static List<Class1> ModuleInfoList = new List<Class1>();


     public static List<string> ModCodeLst = new List<string>();


        /// METHOD TO ENCRYPT PASSWORD IN DATABASE. 
        public static string hashPassword(String Password)
        {
            SHA1CryptoServiceProvider HASHPWORD = new SHA1CryptoServiceProvider();

            byte[] Password_bytes = Encoding.ASCII.GetBytes(Password);
            byte[] encrypted_bytes = HASHPWORD.ComputeHash(Password_bytes);
            return Convert.ToBase64String(encrypted_bytes); 

        }



    }

}
