using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using System;
using System.Dynamic;
using System.Numerics;
using System.Reflection;

namespace FinalProject_340.Models
{
    public class SqlDBConnection<TYPE> where TYPE : new()
    {
        private String _cString;
        private SqlConnection _connection;
        private SqlCommand _command;

        public SqlDBConnection(string connectionString)
        {
            this._cString = connectionString;
            _connection = new SqlConnection(connectionString);
            _command = new SqlCommand();
        }

        public SqlCommand generateInsertQuery(TYPE model)
        {
            PropertyInfo[] props = typeof(TYPE).GetProperties();

            String _tableName = typeof(TYPE).Name;

            String newSqlQuery = "insert into " + _tableName + " (";

            foreach (PropertyInfo prop in props)
            {
                //System.Console.WriteLine(prop.Name + " " + prop.PropertyType.ToString());
                newSqlQuery += prop.Name + ", ";
            }
            newSqlQuery = newSqlQuery.Substring(0, newSqlQuery.Length - 2);
            newSqlQuery += " ) values ( ";

            foreach (PropertyInfo prop in props)
            {
                newSqlQuery += "@" + prop.Name.ToString() + ", ";
            }
            newSqlQuery = newSqlQuery.Substring(0, newSqlQuery.Length - 2);
            newSqlQuery += " )";
            System.Console.WriteLine(newSqlQuery);
            
            SqlCommand sqlCommand = new SqlCommand(newSqlQuery, _connection);

            foreach (PropertyInfo prop in props)
            {
                var results = getValue(prop.Name, model);
                //System.Console.WriteLine(results.type.ToString() + " " + results.typeB.ToString());
                if(results.typeB.ToString() != null) sqlCommand.Parameters.AddWithValue("@" + prop.Name, results.typeB.ToString());
            }

            return sqlCommand;
        }
        public bool insertIntoTable(TYPE newModel)
        {
            System.Console.WriteLine();
            PropertyInfo[] props = typeof(TYPE).GetProperties();

            String _tableName = typeof(TYPE).Name;
            SqlCommand newSqlCommand = generateInsertQuery(newModel);
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
        private bool insertIntoTable(string SQL)
        {

            return true;
        }

        public Vector2D<Type, Object> getValue(String Name, TYPE model) {
            Type typeOfVar = typeof(TYPE).GetProperty(Name).GetValue(model).GetType();
            return new Vector2D<Type, Object> { type = typeOfVar, typeB = typeof(TYPE).GetProperty(Name).GetValue(model) };
        }

    }
}
