using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using System;
using System.Dynamic;
using System.Numerics;
using System.Reflection;
using System.Xml.Linq;

namespace FinalProject_340.Models
{
    public class SqlDBConnection<TYPE> where TYPE : new()
    {
        private String          _cString;
        private SqlConnection   _connection;
        private SqlCommand      _command;
        private SqlDataReader   _reader;

        private String          _tableName  = typeof(TYPE).Name;
        private PropertyInfo[]  _props      = typeof(TYPE).GetProperties();

        public SqlDBConnection(string connectionString)
        {
            _cString    = connectionString;
            _connection = new SqlConnection(connectionString);
            _command    = new SqlCommand();
        }
        public String generateSqlInsertQuery(TYPE model)
        {
            String newSqlQuery = "insert into " + _tableName + " (";

            foreach (PropertyInfo prop in _props)
            {
                var results = getValue(prop.Name, model);
                if (results != null)
                {
                    newSqlQuery += prop.Name + ", ";
                }
            }
            newSqlQuery = newSqlQuery.Substring(0, newSqlQuery.Length - 2);
            newSqlQuery += " ) values ( ";

            foreach (PropertyInfo prop in _props)
            {
                var results = getValue(prop.Name, model);
                if (results != null)
                {
                    newSqlQuery += "@" + prop.Name.ToString() + ", ";
                }
            }
            newSqlQuery = newSqlQuery.Substring(0, newSqlQuery.Length - 2);
            newSqlQuery += " )";
            System.Console.WriteLine(newSqlQuery);
            return newSqlQuery;
        }
        public SqlCommand createSqlCommand(TYPE model)
        {
            //create a new query based on the model ie -- insert into table () values ()
            String newSqlQuery = generateSqlInsertQuery(model);
            
            //Instantiate new SqlCommand class using the sql query and connection
            SqlCommand sqlCommand = new SqlCommand(newSqlQuery, _connection);

            //add all parameters
            foreach (PropertyInfo prop in _props)
            {
                var results = getValue(prop.Name, model);
                if(results != null) 
                    sqlCommand.Parameters.AddWithValue("@" + prop.Name, results.typeB?.ToString());
            }

            return sqlCommand;
        }
        public bool insertIntoTable(TYPE newModel)
        {
            SqlCommand newSqlCommand = createSqlCommand(newModel);
            _connection.Open();
            int i = newSqlCommand.ExecuteNonQuery();
            _connection.Close();
            if(i == 0)
            {
                System.Console.WriteLine("Error inserting SQL entry!");
                return false;
            }

            return true;
        }
        public TYPE getFirst(Dictionary<string, string> conditions)
        {
            TYPE newEntry = new TYPE();
            String newSqlQuery = "Select top(1) * from " + _tableName;
            bool f = false;
            if (conditions.Count > 0)
            {
                newSqlQuery += " where ";
            }
            foreach(PropertyInfo prop in _props)
            {
                if (conditions.ContainsKey(prop.Name))
                {
                    f = true;
                    newSqlQuery += prop.Name + " = '" + conditions[prop.Name] + "' and ";
                }
            }
            if(f) newSqlQuery = newSqlQuery.Substring(0, newSqlQuery.Length - 4);
            System.Console.WriteLine(newSqlQuery);

            try
            {
                _connection.Open();
                SqlCommand newSqlCommand = new SqlCommand(newSqlQuery, _connection);
                _reader = newSqlCommand.ExecuteReader();
                _reader.Read();
                foreach(PropertyInfo prop in _props)
                {
                    //System.Console.WriteLine(prop.PropertyType);
                    try
                    {
                        setModel(prop, newEntry);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e);
                    }
                }
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return newEntry;
        }
        private void setModel(PropertyInfo prop, TYPE newModel)
        {
            System.Console.WriteLine(_reader[prop.Name] + " " + newModel.GetType().GetProperty(prop.Name).ToString());
            var results = _reader[prop.Name];
            if (prop.PropertyType.ToString().Contains("Int"))
            {
                newModel.GetType().GetProperty(prop.Name).SetValue(newModel, (int)results, null);
            }
            else if (prop.PropertyType.ToString().Contains("String"))
            {
                newModel.GetType().GetProperty(prop.Name).SetValue(newModel, results.ToString(), null);
            }
            else if (prop.PropertyType.ToString().Contains("Single"))
            {
                float re;
                float.TryParse(_reader[prop.Name].ToString(), out re);
                newModel.GetType().GetProperty(prop.Name).SetValue(newModel, re, null);
            }
            else if (prop.PropertyType.ToString().Contains("Char"))
            {
                newModel.GetType().GetProperty(prop.Name).SetValue(newModel, _reader[prop.Name].ToString()[0], null);
            }
            else if (prop.PropertyType.ToString().Contains("Date"))
            {
                newModel.GetType().GetProperty(prop.Name).SetValue(newModel, Convert.ToDateTime(_reader[prop.Name].ToString()), null);
            }
        }
        public Vector2D<Type, Object>? getValue(String Name, TYPE model) {
            //int ? isNull =  i : (int?)null;
            Object ? results = model?.GetType().GetProperty(Name)?.GetValue(model);
            if (results != null && !string.IsNullOrEmpty(results.ToString()))
            {
                Type ? typeOfVar = typeof(TYPE).GetProperty(Name)?.GetValue(model)?.GetType();
                return new Vector2D<Type, Object> { type = typeOfVar, typeB = typeof(TYPE)?.GetProperty(Name)?.GetValue(model) };
            }
            return null;
        }

    }
}
