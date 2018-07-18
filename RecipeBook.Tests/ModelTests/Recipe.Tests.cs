using System;
using RecipeBook.Models;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecipeBook.Tests
{
  [TestClass]
  public class RecipeTests : IDisposable
  {
    public RecipeTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipebook_test;";
    }
    public void Dispose()
    {
      Recipe.DeleteAll();
      Category.DeleteAll();
    }
    [TestMethod]
    public void Save_SavesToDB_RecipeList()
    {
      Recipe testRecipe1 = new Recipe("Fruit Salad", "Fruit", "Eat the fruit.", 5);
      List<Recipe> resultList = new List<Recipe> {testRecipe1};

      testRecipe1.Save();
      List<Recipe> testList = Recipe.GetAll();

      CollectionAssert.AreEqual(resultList, testList);

    }
    [TestMethod]
    public void GetAll_ReturnsAllRecipesInDB_RecipeList()
    {
      Recipe testRecipe1 = new Recipe("Fruit Salad", "Fruit", "Eat the fruit.", 5);
      Recipe testRecipe2 = new Recipe("Scrambled Eggs", "2 Eggs, 1tbspn Milk", "Whip in bowl, heat in pan on medium heat.", 7);
      List<Recipe> resultList = new List<Recipe> {testRecipe1, testRecipe2};

      testRecipe1.Save();
      testRecipe2.Save();
      List<Recipe> testList = Recipe.GetAll();

      CollectionAssert.AreEqual(resultList, testList);
    }
    [TestMethod]
    public void Find_ReturnsCorrectRecipe_Recipe()
    {
      Recipe testRecipe1 = new Recipe("Fruit Salad", "Fruit", "Eat the fruit.", 5);
      Recipe testRecipe2 = new Recipe("Scrambled Eggs", "2 Eggs, 1tbspn Milk", "Whip in bowl, heat in pan on medium heat.", 7);

      testRecipe1.Save();
      testRecipe2.Save();
      Recipe resultRecipe = Recipe.Find(testRecipe1.GetId());

      Assert.AreEqual(testRecipe1, resultRecipe);
    }
    [TestMethod]
    public void AddCategory_CorrectlyAttatchesCategoryToRecipe_Category()
    {
      Recipe testRecipe1 = new Recipe("Fruit Salad", "Fruit", "Eat the fruit.", 5);
      testRecipe1.Save();
      Category testCategory1 = new Category("Low-Effort");
      testCategory1.Save();
      List<Category> testList = new List<Category> {testCategory1};

      testRecipe1.AddCategory(testCategory1);
      List<Category> resultList = testRecipe1.GetAllCategories();

      CollectionAssert.AreEqual(testList, resultList);
    }
    [TestMethod]
    public void GetAllCategories_ReturnsAllCategoriesAttachedToRecipe_CategoryList()
    {
      Recipe testRecipe1 = new Recipe("Fruit Salad", "Fruit", "Eat the fruit.", 5);
      testRecipe1.Save();
      Category testCategory1 = new Category("Low-Effort");
      testCategory1.Save();
      Category testCategory2 = new Category("Meats");
      testCategory2.Save();
      List<Category> testList = new List<Category> {testCategory1, testCategory2};

      testRecipe1.AddCategory(testCategory1);
      testRecipe1.AddCategory(testCategory2);
      List<Category> resultList = testRecipe1.GetAllCategories();

      CollectionAssert.AreEqual(testList, resultList);
    }
  }
}
