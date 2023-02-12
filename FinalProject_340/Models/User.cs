using Microsoft.Data.SqlClient;
using FinalProject_340;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace FinalProject_340.Models
{
    public class Users
    {
        public static Users retrieveUser_SQL(string UUID)
        {
            SqlCommand newSqlCommand = new SqlCommand();
            //SqlConnection appData = new SqlConnection(FinalProject_340.Properties.Resource.appData);
            SqlConnection appData = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=userDB_340;Connect Timeout=100;");
            SqlDataReader dataReader;
            Users user = new Users();
            try
            {
                appData.Open();
                newSqlCommand = new SqlCommand($"select top(1) * from users where UUID = {UUID}", appData);
                dataReader = newSqlCommand.ExecuteReader();
                dataReader.Read();
                user.UUID  = (string)dataReader.GetValue(1);
                user.FIRST_NAME = (string)dataReader.GetValue(2);
                user.LAST_NAME = (string)dataReader.GetValue(3);
                appData.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;


        }

        public string UUID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name Cannot Be Blank!")]
        public string FIRST_NAME { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name Cannot Be Blank!")]
        public string LAST_NAME { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email Required!")]
        public string? EMAIL { get; set; }

        private IDictionary<string, string> _listSet;

        public void helloWorld()
        {
            System.Console.WriteLine("Hello World");
        }
        public Users() { }
        public Users(string UUID)
        {
            Users newUser = retrieveUser_SQL(UUID);
            this.UUID = UUID;
            this.FIRST_NAME = newUser.FIRST_NAME;
            this.LAST_NAME = newUser.LAST_NAME;
        }
        public Users(string EMAIL, string PASSWORD)
        {
            UUID = (EMAIL + PASSWORD).toHash();
        }
        public Users(string _EMAIL, string FIRSTNAME, string LASTNAME, string _PASSWORD)
        {
            FIRST_NAME = FIRSTNAME; 
            LAST_NAME = LASTNAME;
            EMAIL = _EMAIL;
            UUID = (_EMAIL + _PASSWORD).toHash();
        }
        public IDictionary<string, string> getList()
        {
            return _listSet;
        }
    }
}
