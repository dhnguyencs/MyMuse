using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace FinalProject_340.Models
{
    public class SqlDBConnection<TYPE> where TYPE : new() 
    {
        private String          _cString;
        private SqlConnection   _connection;
        private SqlDataReader?  _reader;

        private String          _tableName  = typeof(TYPE).Name;
        private PropertyInfo[]  _props      = typeof(TYPE).GetProperties();
        private Type            _typeDef    = typeof(TYPE);

        private delegate void   __VOID__PROP_INFO__TYPE__Type       (PropertyInfo info, TYPE model, Type _typeDef);
        private delegate bool   __BOOL__PROP_INFO__TYPE             (PropertyInfo info, TYPE model);

        private static Dictionary<Type, __VOID__PROP_INFO__TYPE__Type>? _setModel;

        public SqlDBConnection(string connectionString)
        {
            _cString    = connectionString;
            _connection = new SqlConnection(_cString);

            _setModel = new Dictionary<Type, __VOID__PROP_INFO__TYPE__Type>()
            {
                {
                    typeof(int), 
                    delegate(PropertyInfo prop, TYPE newModel, Type _typeDef)
                    {
                        int re;
                        int.TryParse(_reader?[prop.Name].ToString(), out re);
                        _typeDef.GetProperty(prop.Name)?.SetValue(newModel, (int)re, null);
                    }
                },
                {
                    typeof(string), 
                    delegate(PropertyInfo prop, TYPE newModel, Type _typeDef)
                    {
                        string ? results = (string)_reader?[prop.Name];
                        if(String.IsNullOrEmpty(results)) return;
                        _typeDef.GetProperty(prop.Name)?.SetValue(newModel, results?.ToString(), null);
                    } 
                },
                {
                    typeof(float), 
                    delegate(PropertyInfo prop, TYPE newModel, Type _typeDef){
                        float re;
                        float.TryParse(_reader?[prop.Name].ToString(), out re);
                        _typeDef.GetProperty(prop.Name)?.SetValue(newModel, re, null);
                    } 
                },
                {
                    typeof(double), 
                    delegate(PropertyInfo prop, TYPE newModel, Type _typeDef){
                        double re;
                        double.TryParse(_reader?[prop.Name].ToString(), out re);
                        _typeDef.GetProperty(prop.Name)?.SetValue(newModel, re, null);
                    } 
                },
                {
                    typeof(char), 
                    delegate(PropertyInfo prop, TYPE newModel, Type _typeDef){
                        string ? results = (string)_reader?[prop.Name];
                        if(String.IsNullOrEmpty(results)) return;
                        _typeDef.GetProperty(prop.Name)?.SetValue(newModel, results?.ToString()?[0], null);
                    } 
                },
                {   
                    typeof(DateTime), 
                    delegate(PropertyInfo prop, TYPE newModel, Type _typeDef)
                    {
                        _typeDef.GetProperty(prop.Name)?.SetValue(newModel, Convert.ToDateTime(_reader?[prop.Name].ToString()), null);
                    } 
                },
            };
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
        public SqlCommand createInsertSqlCommand(TYPE model)
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
        public int insertList(List<TYPE> list)
        {
            int failed = 0;
            
            foreach(TYPE item in list)
                failed = insertIntoTable(item) ? failed : failed++;
            
            return failed;
        }
        public bool insertIntoTable(TYPE newModel)
        {
            SqlCommand newSqlCommand = createInsertSqlCommand(newModel);
            try
            {
                _connection.Open();
                int i = newSqlCommand.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                _connection.Close();
                System.Console.WriteLine(e);
                return false;
            }
            _connection.Close();
            return true;
        }
        public String createRawSqlUpdate(TYPE model, Dictionary<string, string> conditions)
        {
            String sqlRaw = "update " + _tableName + " set ";
            foreach (PropertyInfo prop in _props)
            {
                var results = getValue(prop.Name, model);
                if (results != null)
                {
                    sqlRaw += prop.Name + " = " + "@" + prop.Name + ", ";
                }
            }
            sqlRaw = sqlRaw.Substring(0, sqlRaw.Length - 2) + " ";
            sqlRaw += "where ";
            foreach (PropertyInfo prop in _props)
            {
                if (conditions.ContainsKey(prop.Name.ToLower()))
                {
                    sqlRaw += prop.Name + " = " + "@" + prop.Name + "c " + " and ";
                }
            }
            sqlRaw = sqlRaw.Substring(0, sqlRaw.Length - 4) + " ";
            return sqlRaw;
        }
        public String createRawSqlDelete(Dictionary<string, string> conditions)
        {
            String sqlRaw = "delete from " + _tableName + " where ";
            foreach (PropertyInfo prop in _props)
            {
                if (conditions.ContainsKey(prop.Name.ToLower()))
                {
                    sqlRaw += prop.Name + " = " + "@" + prop.Name + "c " + " and ";
                }
            }
            sqlRaw = sqlRaw.Substring(0, sqlRaw.Length - 4) + " ";
            return sqlRaw;
        }
        public bool delete(Dictionary<string, string> conditions)
        {
            if (conditions.Count == 0) return false;

            conditions = conditions.toLowerCaseKey();
            String sqlRaw = createRawSqlDelete(conditions);
            SqlCommand newCommand = new SqlCommand(sqlRaw, _connection);
            foreach (PropertyInfo prop in _props)
            {
                if (conditions.ContainsKey(prop.Name.ToLower()))
                    newCommand.Parameters.AddWithValue("@" + prop.Name + "c", conditions[prop.Name.ToLower()]);

            }

            _connection.Open();
            int re = newCommand.ExecuteNonQuery();
            _connection.Close();
            if (re == 0) return false;
            return true;
        }
        public bool update(TYPE model, Dictionary<string, string> conditions)
        {
            if (conditions.Count == 0) return false;

            conditions = conditions.toLowerCaseKey();
            String sqlRaw = createRawSqlUpdate(model, conditions);
            SqlCommand newCommand = new SqlCommand(sqlRaw, _connection);
            foreach(PropertyInfo prop in _props)
            {
                var results = getValue(prop.Name, model);
                if (results != null)
                {
                    newCommand.Parameters.AddWithValue("@" + prop.Name, results.typeB);
                    if (conditions.ContainsKey(prop.Name.ToLower()))
                    {
                        newCommand.Parameters.AddWithValue("@" + prop.Name + "c", conditions[prop.Name.ToLower()]);
                    }
                }

            }

            _connection.Open();
            int re = newCommand.ExecuteNonQuery();
            _connection.Close();
            if (re == 0) return false;
            return true;
        }
        public String generateConditionals(String incompleteSqllQuery, Dictionary<string, string> conditions)
        {
            conditions = conditions.toLowerCaseKey();
            bool f = false;
            if (conditions.Count > 0)
            {
                incompleteSqllQuery += " where ";
            }
            foreach (PropertyInfo prop in _props)
            {
                if (conditions.ContainsKey(prop.Name.ToLower()))
                {
                    f = true;
                    incompleteSqllQuery += prop.Name + " = '" + conditions[prop.Name.ToLower()] + "' and ";
                }
            }
            if (f) incompleteSqllQuery = incompleteSqllQuery.Substring(0, incompleteSqllQuery.Length - 4);
            return incompleteSqllQuery;
        }
        public List<TYPE> getList(Dictionary<string, string> conditions, int topX)
        {
            string _topX = ""; 
            if(topX != 0)
            {
                _topX += "top (" + topX.ToString() + ")";
            }
            List<TYPE> list = new List<TYPE>();
            String newSqlQuery = "Select " + _topX.ToString() + "* from " + _tableName;
            newSqlQuery = generateConditionals(newSqlQuery, conditions);
            try
            {
                _connection.Open();
                SqlCommand newSqlCommand = new SqlCommand(newSqlQuery, _connection);
                _reader = newSqlCommand.ExecuteReader();
                
                for (int i = 0; _reader.Read() && (i <= topX+1); i++)
                {
                    TYPE newEntry = new TYPE();
                    foreach (PropertyInfo prop in _props)
                    {
                        try
                        {
                            setModel(prop, newEntry, _reader);
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine(e);
                        }
                    }
                    list.Add(newEntry);
                }
                _connection.Close();
            }catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return list;
        }
        public TYPE? getFirstOrDefault(Dictionary<string, string> conditions) {
            List<TYPE> re = getList(conditions, 1);
            return re.Count > 0 ? re[0] : default(TYPE);
        }
        public List<TYPE> getList(Dictionary<string, string> conditions) { return getList(conditions, 10); }
        private void setModel(PropertyInfo prop, TYPE newModel, SqlDataReader _reader)
        {
            _setModel[prop.PropertyType](prop, newModel, _typeDef);
        }
        private Vector2D<Type, Object>? getValue(String Name, TYPE model) {
            Object ? results = model?.GetType().GetProperty(Name)?.GetValue(model);
            if (results != null && !string.IsNullOrEmpty(results.ToString()))
            {
                Type ? typeOfVar = _typeDef.GetProperty(Name)?.GetValue(model)?.GetType();
                return new Vector2D<Type, Object> { type = typeOfVar, typeB = typeof(TYPE)?.GetProperty(Name)?.GetValue(model) };
            }
            return null;
        }

    }
}
