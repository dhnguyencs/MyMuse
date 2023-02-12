//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore.Metadata.Conventions;
//using System;
//using System.Dynamic;
//using System.Reflection;

//namespace FinalProject_340.Models
//{
//    public class SqlDBConnection<TYPE>
//    {
//        private dynamic _data = new ExpandoObject();
//        private PropertyInfo[] _props;
//        private String _cString;


//        public SqlDBConnection(String connectionString, TYPE model)
//        {

//            _cString = connectionString;
//            _props = getPropInfo();
//            _tableName = typeof(TYPE).Name;

//            System.Console.WriteLine(_tableName);

//            foreach (PropertyInfo prop in _props)
//            {
//                _types.Add(prop.Name, prop.PropertyType.ToString());
//                System.Console.WriteLine(prop.Name.ToString().ToUpper() + " " + prop.PropertyType);
//            }

//        }
//        public PropertyInfo[] getPropInfo()
//        {
//            return typeof(TYPE).GetProperties();
//        }
//        public bool TestConnection()
//        {
//            return true;
//        }
//        public TYPE test()
//        {
//            dynamic obj = new ExpandoObject();
//            ((IDictionary<String, Object>)obj).Add("_fName", "David");
//            ((IDictionary<String, Object>)obj).Add("_lName", "Nguyen");

//            return (TYPE)obj;
//        }
//    }
//}
