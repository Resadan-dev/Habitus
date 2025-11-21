using Valoron.Activities.Domain;

namespace Valoron.Activities.Tests;

public class ActivityCategoryTests
{
    #region Factory Property Tests

    [Fact]
    public void Environment_ReturnsValidCategory()
    {
        // Act
        var category = ActivityCategory.Environment;

        // Assert
        Assert.NotNull(category);
        Assert.Equal("ENV", category.Code);
        Assert.Equal("Environment", category.Name);
    }

    [Fact]
    public void Body_ReturnsValidCategory()
    {
        // Act
        var category = ActivityCategory.Body;

        // Assert
        Assert.NotNull(category);
        Assert.Equal("BODY", category.Code);
        Assert.Equal("Body", category.Name);
    }

    [Fact]
    public void Nutrition_ReturnsValidCategory()
    {
        // Act
        var category = ActivityCategory.Nutrition;

        // Assert
        Assert.NotNull(category);
        Assert.Equal("NUTR", category.Code);
        Assert.Equal("Nutrition", category.Name);
    }

    [Fact]
    public void Hygiene_ReturnsValidCategory()
    {
        // Act
        var category = ActivityCategory.Hygiene;

        // Assert
        Assert.NotNull(category);
        Assert.Equal("HYG", category.Code);
        Assert.Equal("Hygiene", category.Name);
    }

    [Fact]
    public void Social_ReturnsValidCategory()
    {
        // Act
        var category = ActivityCategory.Social;

        // Assert
        Assert.NotNull(category);
        Assert.Equal("SOC", category.Code);
        Assert.Equal("Social", category.Name);
    }

    [Fact]
    public void Admin_ReturnsValidCategory()
    {
        // Act
        var category = ActivityCategory.Admin;

        // Assert
        Assert.NotNull(category);
        Assert.Equal("ADM", category.Code);
        Assert.Equal("Administrative", category.Name);
    }

    [Fact]
    public void Learning_ReturnsValidCategory()
    {
        // Act
        var category = ActivityCategory.Learning;

        // Assert
        Assert.NotNull(category);
        Assert.Equal("LRN", category.Code);
        Assert.Equal("Learning", category.Name);
    }

    [Fact]
    public void Project_ReturnsValidCategory()
    {
        // Act
        var category = ActivityCategory.Project;

        // Assert
        Assert.NotNull(category);
        Assert.Equal("PROJ", category.Code);
        Assert.Equal("Project", category.Name);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_SameCode_ReturnsTrue()
    {
        // Arrange
        var category1 = ActivityCategory.Body;
        var category2 = ActivityCategory.Body;

        // Act
        var result = category1.Equals(category2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_DifferentCode_ReturnsFalse()
    {
        // Arrange
        var category1 = ActivityCategory.Body;
        var category2 = ActivityCategory.Social;

        // Act
        var result = category1.Equals(category2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        // Arrange
        var category = ActivityCategory.Environment;

        // Act
        var result = category.Equals(category);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        // Arrange
        var category = ActivityCategory.Environment;

        // Act
        var result = category.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Arrange
        var category = ActivityCategory.Environment;
        var obj = new object();

        // Act
        var result = category.Equals(obj);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region Operator Equality Tests

    [Fact]
    public void OperatorEquals_SameCategory_ReturnsTrue()
    {
        // Arrange
        var category1 = ActivityCategory.Body;
        var category2 = ActivityCategory.Body;

        // Act
        var result = category1 == category2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_DifferentCategories_ReturnsFalse()
    {
        // Arrange
        var category1 = ActivityCategory.Body;
        var category2 = ActivityCategory.Social;

        // Act
        var result = category1 == category2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_BothNull_ReturnsTrue()
    {
        // Arrange
        ActivityCategory? category1 = null;
        ActivityCategory? category2 = null;

        // Act
        var result = category1 == category2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_OneNull_ReturnsFalse()
    {
        // Arrange
        ActivityCategory? category1 = null;
        var category2 = ActivityCategory.Body;

        // Act
        var result = category1 == category2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_SameCategory_ReturnsFalse()
    {
        // Arrange
        var category1 = ActivityCategory.Social;
        var category2 = ActivityCategory.Social;

        // Act
        var result = category1 != category2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_DifferentCategories_ReturnsTrue()
    {
        // Arrange
        var category1 = ActivityCategory.Body;
        var category2 = ActivityCategory.Social;

        // Act
        var result = category1 != category2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorNotEquals_BothNull_ReturnsFalse()
    {
        // Arrange
        ActivityCategory? category1 = null;
        ActivityCategory? category2 = null;

        // Act
        var result = category1 != category2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_OneNull_ReturnsTrue()
    {
        // Arrange
        var category1 = ActivityCategory.Body;
        ActivityCategory? category2 = null;

        // Act
        var result = category1 != category2;

        // Assert
        Assert.True(result);
    }

    #endregion

    #region GetHashCode Tests

    [Fact]
    public void GetHashCode_SameCategory_ReturnsSameHashCode()
    {
        // Arrange
        var category1 = ActivityCategory.Body;
        var category2 = ActivityCategory.Body;

        // Act
        var hashCode1 = category1.GetHashCode();
        var hashCode2 = category2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_DifferentCategories_ReturnsDifferentHashCodes()
    {
        // Arrange
        var category1 = ActivityCategory.Body;
        var category2 = ActivityCategory.Social;

        // Act
        var hashCode1 = category1.GetHashCode();
        var hashCode2 = category2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_Consistency_MultipleCalls()
    {
        // Arrange
        var category = ActivityCategory.Environment;

        // Act
        var hashCode1 = category.GetHashCode();
        var hashCode2 = category.GetHashCode();
        var hashCode3 = category.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
        Assert.Equal(hashCode2, hashCode3);
    }

    [Fact]
    public void GetHashCode_AllCategories_ProduceDifferentHashCodes()
    {
        // Arrange
        var categories = new[]
        {
            ActivityCategory.Environment,
            ActivityCategory.Body,
            ActivityCategory.Nutrition,
            ActivityCategory.Hygiene,
            ActivityCategory.Social,
            ActivityCategory.Admin,
            ActivityCategory.Learning,
            ActivityCategory.Project
        };

        // Act
        var hashCodes = categories.Select(c => c.GetHashCode()).ToList();

        // Assert - All hash codes should be unique
        Assert.Equal(hashCodes.Count, hashCodes.Distinct().Count());
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ReturnsName()
    {
        // Arrange
        var category = ActivityCategory.Body;

        // Act
        var result = category.ToString();

        // Assert
        Assert.Equal("Body", result);
    }

    [Fact]
    public void ToString_AllCategories_ReturnCorrectNames()
    {
        // Arrange & Act & Assert
        Assert.Equal("Environment", ActivityCategory.Environment.ToString());
        Assert.Equal("Body", ActivityCategory.Body.ToString());
        Assert.Equal("Nutrition", ActivityCategory.Nutrition.ToString());
        Assert.Equal("Hygiene", ActivityCategory.Hygiene.ToString());
        Assert.Equal("Social", ActivityCategory.Social.ToString());
        Assert.Equal("Administrative", ActivityCategory.Admin.ToString());
        Assert.Equal("Learning", ActivityCategory.Learning.ToString());
        Assert.Equal("Project", ActivityCategory.Project.ToString());
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void Category_PropertiesAreImmutable()
    {
        // Arrange
        var category = ActivityCategory.Body;

        // Act & Assert - Properties have only getters, so they cannot be reassigned
        // This test verifies the design by ensuring we can read the properties
        Assert.Equal("BODY", category.Code);
        Assert.Equal("Body", category.Name);

        // If properties were mutable, we would be able to do:
        // category.Code = "XXX"; // This should not compile
        // category.Name = "YYY"; // This should not compile
    }

    #endregion

    #region All Categories Test

    [Fact]
    public void AllCategories_AreDistinct()
    {
        // Arrange
        var categories = new[]
        {
            ActivityCategory.Environment,
            ActivityCategory.Body,
            ActivityCategory.Nutrition,
            ActivityCategory.Hygiene,
            ActivityCategory.Social,
            ActivityCategory.Admin,
            ActivityCategory.Learning,
            ActivityCategory.Project
        };

        // Act
        var codes = categories.Select(c => c.Code).ToList();
        var names = categories.Select(c => c.Name).ToList();

        // Assert - All codes and names should be unique
        Assert.Equal(8, codes.Count);
        Assert.Equal(8, codes.Distinct().Count());
        Assert.Equal(8, names.Distinct().Count());
    }

    [Fact]
    public void AllCategories_HaveValidCodeFormat()
    {
        // Arrange
        var categories = new[]
        {
            ActivityCategory.Environment,
            ActivityCategory.Body,
            ActivityCategory.Nutrition,
            ActivityCategory.Hygiene,
            ActivityCategory.Social,
            ActivityCategory.Admin,
            ActivityCategory.Learning,
            ActivityCategory.Project
        };

        // Act & Assert - All codes should be uppercase and 3-4 characters
        foreach (var category in categories)
        {
            Assert.NotNull(category.Code);
            Assert.NotEmpty(category.Code);
            Assert.True(category.Code.Length >= 3 && category.Code.Length <= 4);
            Assert.Equal(category.Code, category.Code.ToUpper());
        }
    }

    #endregion
}
