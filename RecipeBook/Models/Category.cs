using System;
using RecipeBook;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace RecipeBook.Models
{
  public class Category
  {
    private int _id;
    private string _name;

    public Category(string newName, int id = 0)
    {
      _name = newName;
      _id = id;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public override bool Equals(System.Object otherObject)
    {
      if(!(otherObject is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherObject;
        bool IdEquality = (this.GetId() == newCategory.GetId());
        bool NameEquality = (this.GetName() == newCategory.GetName());
        return (IdEquality && NameEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO categories (name) VALUES (@CategoryName)";

      cmd.Parameters.Add(new MySqlParameter("@CategoryName", _name));
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<Category> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Category> allCategorys = new List<Category>{};
      while(rdr.Read())
      {
        int CategoryId = rdr.GetInt32(0);
        string CategoryName = rdr.GetString(1);
        Category newCategory = new Category(CategoryName, CategoryId);
        allCategorys.Add(newCategory);
      }

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allCategorys;
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM categories; DELETE FROM recipes_categories;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public static Category Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories WHERE id = @CategoryId;";

      cmd.Parameters.Add(new MySqlParameter("@CategoryId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int CategoryId = 0;
      string CategoryName = "";

      while(rdr.Read())
      {
        CategoryId = rdr.GetInt32(0);
        CategoryName = rdr.GetString(1);
      }

      Category foundCategory = new Category(CategoryName, CategoryId);

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return foundCategory;
    }
    public void AddRecipe(Recipe newRecipe)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO recipes_categories (recipe_id, category_id) VALUES (@RecipeId, @CategoryId);";

      cmd.Parameters.Add(new MySqlParameter("@RecipeId", newRecipe.GetId()));
      cmd.Parameters.Add(new MySqlParameter("@CategoryId", _id));

      cmd.ExecuteNonQuery();

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Recipe> GetAllRecipes()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT recipes.* FROM categories
                          JOIN recipes_categories ON (categories.id = recipes_categories.category_id)
                          JOIN recipes ON (recipes_categories.recipe_id = recipes.id)
                          WHERE categories.id = @CategoryId;";

      cmd.Parameters.Add(new MySqlParameter("@CategoryId", _id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Recipe> allRecipes = new List<Recipe> {};

      while(rdr.Read())
      {
        int RecipeId = rdr.GetInt32(0);
        string RecipeName = rdr.GetString(1);
        string RecipeIngredients = rdr.GetString(2);
        string RecipeInstructions = rdr.GetString(3);
        int RecipeRating = rdr.GetInt32(4);
        Recipe newRecipe = new Recipe(RecipeName, RecipeIngredients, RecipeInstructions, RecipeRating, RecipeId);
        allRecipes.Add(newRecipe);
      }

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }

      return allRecipes;
    }
    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE categories SET name = @newName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      cmd.Parameters.Add(new MySqlParameter("@newName", newName));

      cmd.ExecuteNonQuery();
      _name = newName;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public void Delete()
    {
      
    }
  }
}
