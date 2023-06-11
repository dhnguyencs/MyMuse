using Microsoft.Data.SqlClient;
using System.Reflection;

namespace FinalProject_340.Utilities
{
    public class SqlDBConnection<TYPE> where TYPE : new()
    {
        public static bool createDatabase(string databaseName, string _connection_string)
        {
            using (SqlConnection connection = new SqlConnection(_connection_string))
            {
                string createDatabaseQuery = $"CREATE DATABASE {databaseName}";
                connection.Open();
                using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        connection.Close();
                        return false;
                    }
                    connection.Close();
                    return true;
                }
            }
        }
        private class Vector2D<TYPE_A, TYPE_B>
        {
            public TYPE_A? type { get; set; }
            public TYPE_B? typeB { get; set; }
        }
        private static Dictionary<string, string> toLowerCaseKey(Dictionary<string, string> thisDict)
        {
            Dictionary<string, string> lo_case_conditions = new Dictionary<string, string>();

            foreach (string condition in thisDict.Keys)
                lo_case_conditions.Add(condition.ToLower(), thisDict[condition]);

            return lo_case_conditions;
        }
        private string _cString;
        private SqlConnection _connection;
        private SqlDataReader? _reader;

        private string _tableName = typeof(TYPE).Name;
        private PropertyInfo[] _props = typeof(TYPE).GetProperties();
        private Type _typeDef = typeof(TYPE);

        private delegate void __VOID__PROP_INFO__TYPE__Type(PropertyInfo info, TYPE model, Type _typeDef);
        private delegate bool __BOOL__PROP_INFO__TYPE(PropertyInfo info, TYPE model);

        private static Dictionary<Type, __VOID__PROP_INFO__TYPE__Type>? _setModel;
        private static Dictionary<Type, string>? _CSHARP_TYPES_TO_SQL_TYPES;

        public string getTableName() { return _tableName; }

        public void createConnection(string? connectionString = null)
        {
            if (connectionString == null) return;
            _cString = connectionString;
            _connection = new SqlConnection(_cString);
        }

        public SqlDBConnection(string? connectionString = null)
        {
            createConnection(connectionString);

            _CSHARP_TYPES_TO_SQL_TYPES = new Dictionary<Type, string>()
            {
                { typeof(int), "INT" }, { typeof(int?), "INT" }, { typeof(long), "BIGINT" }, { typeof(long?), "BIGINT" },
                { typeof(short), "SHORT" }, { typeof(short?), "short" }, { typeof(byte), "TINYINT" }, { typeof(byte?), "TINYINT" },
                { typeof(float), "REAL" }, { typeof(float?), "REAL" }, { typeof(double), "FLOAT" }, { typeof(double?), "FLOAT" },
                { typeof(decimal), "DECIMAL" }, { typeof(decimal?), "DECIMAL" }, { typeof(bool), "BIT" }, { typeof(bool?), "BIT" },
                { typeof(char), "CHAR" }, { typeof(char?), "CHAR" }, { typeof(string), "VARCHAR(4096)" },
                { typeof(DateTime), "DATETIME" }, { typeof(DateTime?), "DATETIME" }
            };

            _setModel = new Dictionary<Type, __VOID__PROP_INFO__TYPE__Type>()
            {
                {
                    typeof(int),
                    delegate(PropertyInfo prop, TYPE newModel, Type _typeDef)
                    {
                        int re;
                        int.TryParse(_reader?[prop.Name].ToString(), out re);
                        _typeDef.GetProperty(prop.Name)?.SetValue(newModel, re, null);
                    }
                },
                {
                    typeof(string),
                    delegate(PropertyInfo prop, TYPE newModel, Type _typeDef)
                    {
                        string ? results = (string)_reader?[prop.Name];
                        if(string.IsNullOrEmpty(results)) return;
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
                        if(string.IsNullOrEmpty(results)) return;
                        _typeDef.GetProperty(prop.Name)?.SetValue(newModel, results?.ToString()?[0], null);
                    }
                },
                {
                    typeof(DateTime),
                    delegate(PropertyInfo prop, TYPE newModel, Type _typeDef)
                    {
                        _typeDef.GetProperty(prop.Name)?.SetValue(newModel, Convert.ToDateTime(_reader?[prop.Name].ToString()), null);
                    }
                }
            };
        }
        private string generateSqlInsertQuery(TYPE model)
        {
            string newSqlQuery = "insert into [" + _tableName + "] (";

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
            Console.WriteLine(newSqlQuery);
            return newSqlQuery;
        }
        private SqlCommand createInsertSqlCommand(TYPE model)
        {
            //create a new query based on the model ie -- insert into table () values ()
            string newSqlQuery = generateSqlInsertQuery(model);

            //Instantiate new SqlCommand class using the sql query and connection
            SqlCommand sqlCommand = new SqlCommand(newSqlQuery, _connection);

            //add all parameters
            foreach (PropertyInfo prop in _props)
            {
                var results = getValue(prop.Name, model);
                if (results != null)
                    sqlCommand.Parameters.AddWithValue("@" + prop.Name, results.typeB?.ToString());
            }

            return sqlCommand;
        }
        public int insertList(List<TYPE> list)
        {
            int failed = 0;

            foreach (TYPE item in list)
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
            catch (Exception e)
            {
                _connection.Close();
                Console.WriteLine(e);
                return false;
            }
            _connection.Close();
            return true;
        }
        private string createRawSqlUpdate(TYPE model, Dictionary<string, string> conditions)
        {
            string sqlRaw = "update [" + _tableName + "] set ";
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
        private string createRawSqlDelete(Dictionary<string, string> conditions)
        {
            string sqlRaw = "delete from [" + _tableName + "] where ";
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

            conditions = toLowerCaseKey(conditions);
            string sqlRaw = createRawSqlDelete(conditions);
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

            conditions = toLowerCaseKey(conditions);
            string sqlRaw = createRawSqlUpdate(model, conditions);
            SqlCommand newCommand = new SqlCommand(sqlRaw, _connection);
            foreach (PropertyInfo prop in _props)
            {
                var results = getValue(prop.Name, model);
                if (results == null) continue;
                newCommand.Parameters.AddWithValue("@" + prop.Name, results.typeB);
                if (conditions.ContainsKey(prop.Name.ToLower())) newCommand.Parameters.AddWithValue("@" + prop.Name + "c", conditions[prop.Name.ToLower()]);

            }
            try
            {
                _connection.Open();
                int re = newCommand.ExecuteNonQuery();
                _connection.Close();
                if (re == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private string generateConditionals(string incompleteSqllQuery, Dictionary<string, string> conditions)
        {
            conditions = toLowerCaseKey(conditions);
            bool f = false;
            if (conditions.Count > 0)
            {
                incompleteSqllQuery += " where ";
            }
            foreach (PropertyInfo prop in _props)
            {
                if (!conditions.ContainsKey(prop.Name.ToLower())) continue;
                f = true;
                incompleteSqllQuery += prop.Name + " = '" + conditions[prop.Name.ToLower()] + "' and ";
            }
            if (f) incompleteSqllQuery = incompleteSqllQuery.Substring(0, incompleteSqllQuery.Length - 4);
            return incompleteSqllQuery;
        }
        public List<TYPE> getList(Dictionary<string, string> conditions, int topX)
        {
            string _topX = "";
            if (topX != 0)
            {
                _topX += "top (" + topX.ToString() + ")";
            }
            List<TYPE> list = new List<TYPE>();
            string newSqlQuery = "Select " + _topX.ToString() + "* from [" + _tableName + "]";
            newSqlQuery = generateConditionals(newSqlQuery, conditions);
            try
            {
                _connection.Open();
                SqlCommand newSqlCommand = new SqlCommand(newSqlQuery, _connection);
                _reader = newSqlCommand.ExecuteReader();

                for (int i = 0; _reader.Read() && i <= topX + 1; i++)
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
                            Console.WriteLine(e.Message);
                            continue;
                        }
                    }
                    list.Add(newEntry);
                }
                _connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return list;
        }
        public TYPE? getFirstOrDefault(Dictionary<string, string> conditions)
        {
            List<TYPE> re = getList(conditions, 1);
            return re.Count > 0 ? re[0] : default;
        }
        public List<TYPE> getList(Dictionary<string, string> conditions) { return getList(conditions, 10); }
        public List<TYPE> getList(int top = 10) { return getList(new Dictionary<string, string>(), top); }
        private void setModel(PropertyInfo prop, TYPE newModel, SqlDataReader _reader)
        {
            _setModel[prop.PropertyType](prop, newModel, _typeDef);
        }
        public bool createTable()
        {
            string createTableQuery = $"create table [{_tableName}](Id int primary key identity not null, ";

            foreach (PropertyInfo prop in _props)
            {
                createTableQuery += $"{prop.Name} {_CSHARP_TYPES_TO_SQL_TYPES[prop.PropertyType]}, ";
            }
            createTableQuery = createTableQuery.Substring(0, createTableQuery.Length - 2) + ")";
            SqlCommand cmd = new SqlCommand(createTableQuery, _connection);
            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }
            try
            {
                _connection.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return true;
        }
        private Vector2D<Type, object>? getValue(string Name, TYPE model)
        {
            object? results = model?.GetType().GetProperty(Name)?.GetValue(model);
            if (results != null && !string.IsNullOrEmpty(results.ToString()))
            {
                Type? typeOfVar = _typeDef.GetProperty(Name)?.GetValue(model)?.GetType();
                return new Vector2D<Type, object> { type = typeOfVar, typeB = typeof(TYPE)?.GetProperty(Name)?.GetValue(model) };
            }
            return null;
        }

    }
}
