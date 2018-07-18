using System;
using RecipeBook.Models;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecipeBook.Tests
{
  [TestClass]
  public class CategoryTests : IDisposable
  {
    public CategoryTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipebook_test;";
    }
    public void Dispose()
    {
      Recipe.DeleteAll();
      Category.DeleteAll();
    }
    [TestMethod]
    public void Save_SavesToDB_CategoryList()
    {
      Category testCategory1 = new Category("Low-Effort");
      List<Category> resultList = new List<Category> {testCategory1};

      testCategory1.Save();
      List<Category> testList = Category.GetAll();

      CollectionAssert.AreEqual(resultList, testList);

    }
    [TestMethod]
    public void GetAll_ReturnsAllCategorysInDB_CategoryList()
    {
      Category testCategory1 = new Category("Low-Effort");
      Category testCategory2 = new Category("Meats");
      List<Category> resultList = new List<Category> {testCategory1, testCategory2};

      testCategory1.Save();
      testCategory2.Save();
      List<Category> testList = Category.GetAll();

      CollectionAssert.AreEqual(resultList, testList);
    }
    [TestMethod]
    public void Find_ReturnsCorrectCategory_Category()
    {
      Category testCategory1 = new Category("Low-Effort");
      Category testCategory2 = new Category("Meats");

      testCategory1.Save();
      testCategory2.Save();
      Category resultCategory = Category.Find(testCategory1.GetId());

      Assert.AreEqual(testCategory1, resultCategory);
    }
    [TestMethod]
    public void AddRecipe_AddsRecipeCategoryConnectionInDB_Recipe()
    {
      Category testCategory1 = new Category("Low-Effort");
      testCategory1.Save();
      Recipe testRecipe1 = new Recipe("Fruit Salad", "Fruit", "Eat the fruit.", 5);
      testRecipe1.Save();

      testCategory1.AddRecipe(testRecipe1);
      List<Recipe> testList = new List<Recipe> {testRecipe1};
      List<Recipe> resultList = testCategory1.GetAllRecipes();

      CollectionAssert.AreEqual(testList, resultList);
    }
    [TestMethod]
    public void GetAllRecipes_ReturnsAllRecipesAssociatedWithCategory_RecipeList()
    {
      Category testCategory1 = new Category("Low-Effort");
      testCategory1.Save();
      Recipe testRecipe1 = new Recipe("Fruit Salad", "Fruit", "Eat the fruit.", 5);
      Recipe testRecipe2 = new Recipe("Scrambled Eggs", "2 Eggs, 1tbspn Milk", "Whip in bowl, heat in pan on medium heat.", 7);
      testRecipe1.Save();
      testRecipe2.Save();

      testCategory1.AddRecipe(testRecipe1);
      testCategory1.AddRecipe(testRecipe2);
      List<Recipe> testList = new List<Recipe> {testRecipe1, testRecipe2};
      List<Recipe> resultList = testCategory1.GetAllRecipes();

      CollectionAssert.AreEqual(testList, resultList);
    }
    [TestMethod]
    public void Update_CorrectlyChangesCategoryNameInDB_String()
    {
      Category testCategory = new Category("Low-Effort");
      testCategory.Save();
      string testString = "Low-Effort, no cook.";

      testCategory.Update(testString);
      string resultString = testCategory.GetName();

      Assert.AreEqual(testString, resultString);
    }
    [TestMethod]
    public void Delete_DeletesSpecificCategory_CategoryList()
    {
      Category testCategory1 = new Category("Low-Effort");
      Category testCategory2 = new Category("Meats");
      List<Category> resultList = new List<Category> {testCategory1};

      testCategory1.Save();
      testCategory2.Save();
      testCategory2.Delete();
      List<Category> testList = Category.GetAll();

      CollectionAssert.AreEqual(resultList, testList);
    }
  }
}
