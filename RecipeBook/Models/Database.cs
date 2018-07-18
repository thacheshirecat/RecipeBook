using System;
using MySql.Data.MySqlClient;
using RecipeBook;

namespace RecipeBook
{
    public class DB
    {
      public static MySqlConnection Connection()
      {
        MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
        return conn;
      }
    }
}
