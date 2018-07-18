using System;
using RecipeBook;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace RecipeBook.Models
{
  public class Recipe
  {
    private int _id;
    private string _name;
    private string _ingredients;
    private string _instructions;
    private int _rating;

    public Recipe(string newName, string newIngredients, string newInstructions, int newRating, int id = 0)
    {
      _ingredients = newIngredients;
      _name = newName;
      _instructions = newInstructions;
      _rating = newRating;
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
    public string GetIngredients()
    {
      return _ingredients;
    }
    public string GetInstructions()
    {
      return _instructions;
    }
    public int GetRating()
    {
      return _rating;
    }
    public override bool Equals(System.Object otherObject)
    {
      if(!(otherObject is Recipe))
      {
        return false;
      }
      else
      {
        Recipe newRecipe = (Recipe) otherObject;
        bool IdEquality = (this.GetId() == newRecipe.GetId());
        bool NameEquality = (this.GetName() == newRecipe.GetName());
        bool IngredientsEquality = (this.GetIngredients() == newRecipe.GetIngredients());
        bool InstructionsEquality = (this.GetInstructions() == newRecipe.GetInstructions());
        bool RatingEquality = (this.GetRating() == newRecipe.GetRating());
        return (IdEquality && NameEquality && IngredientsEquality && InstructionsEquality && RatingEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetIngredients().GetHashCode();
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO recipes (name, ingredients, instructions, rating) VALUES (@RecipeName, @RecipeIngredients, @RecipeInstructions, @RecipeRating)";

      cmd.Parameters.Add(new MySqlParameter("@RecipeName", _name));
      cmd.Parameters.Add(new MySqlParameter("@RecipeIngredients", _ingredients));
      cmd.Parameters.Add(new MySqlParameter("@RecipeInstructions", _instructions));
      cmd.Parameters.Add(new MySqlParameter("@RecipeRating", _rating));
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<Recipe> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipes;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Recipe> allRecipes = new List<Recipe>{};
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
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM recipes;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public static Recipe Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipes WHERE id = @RecipeId;";

      cmd.Parameters.Add(new MySqlParameter("@RecipeId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int RecipeId = 0;
      string RecipeName = "";
      string RecipeIngredients = "";
      string RecipeInstructions = "";
      int RecipeRating = 0;

      while(rdr.Read())
      {
        RecipeId = rdr.GetInt32(0);
        RecipeName = rdr.GetString(1);
        RecipeIngredients = rdr.GetString(2);
        RecipeInstructions = rdr.GetString(3);
        RecipeRating = rdr.GetInt32(4);
      }

      Recipe foundRecipe = new Recipe(RecipeName, RecipeIngredients, RecipeInstructions, RecipeRating, RecipeId);

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }

      return foundRecipe;
    }
    public void AddCategory(Category newCategory)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO recipes_categories (recipe_id, category_id) VALUES (@RecipeId, @CategoryId);";

      cmd.Parameters.Add(new MySqlParameter("@RecipeId", _id));
      cmd.Parameters.Add(new MySqlParameter("@CategoryId", newCategory.GetId()));

      cmd.ExecuteNonQuery();

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Category> GetAllCategories()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT categories.* FROM recipes
                          JOIN recipes_categories ON (recipes.id = recipes_categories.recipe_id)
                          JOIN categories ON (recipes_categories.category_id = categories.id)
                          WHERE recipes.id = @RecipeId;";

      cmd.Parameters.Add(new MySqlParameter("@RecipeId", _id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Category> allCategories = new List<Category> {};

      while(rdr.Read())
      {
        int CategoryId = rdr.GetInt32(0);
        string CategoryName = rdr.GetString(1);
        Category newCategory = new Category(CategoryName, CategoryId);
        allCategories.Add(newCategory);
      }

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }

      return allCategories;
    }
    public void Edit(string newName, string newIngredients, string newInstructions, int newRating)
    {

    }
  }
}
