using Valoron.Activities.Domain;

namespace Valoron.Activities.Tests;

public class ActivityDifficultyTests
{
    #region Constructor Validation Tests

    [Fact]
    public void Create_WithValue1_CreatesValidDifficulty()
    {
        // Act
        var difficulty = ActivityDifficulty.Create(1);

        // Assert
        Assert.NotNull(difficulty);
        Assert.Equal(1, difficulty.Value);
    }

    [Fact]
    public void Create_WithValue10_CreatesValidDifficulty()
    {
        // Act
        var difficulty = ActivityDifficulty.Create(10);

        // Assert
        Assert.NotNull(difficulty);
        Assert.Equal(10, difficulty.Value);
    }

    [Fact]
    public void Create_WithValue5_CreatesValidDifficulty()
    {
        // Act
        var difficulty = ActivityDifficulty.Create(5);

        // Assert
        Assert.NotNull(difficulty);
        Assert.Equal(5, difficulty.Value);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public void Create_WithValidValues_CreatesSuccessfully(int value)
    {
        // Act
        var difficulty = ActivityDifficulty.Create(value);

        // Assert
        Assert.Equal(value, difficulty.Value);
    }

    [Fact]
    public void Create_WithValue0_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ActivityDifficulty.Create(0));
        Assert.Equal("Difficulty must be between 1 and 10", exception.Message);
    }

    [Fact]
    public void Create_WithNegativeValue_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ActivityDifficulty.Create(-1));
        Assert.Equal("Difficulty must be between 1 and 10", exception.Message);
    }

    [Fact]
    public void Create_WithValue11_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ActivityDifficulty.Create(11));
        Assert.Equal("Difficulty must be between 1 and 10", exception.Message);
    }

    [Fact]
    public void Create_WithValue100_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ActivityDifficulty.Create(100));
        Assert.Equal("Difficulty must be between 1 and 10", exception.Message);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(11)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(1000)]
    public void Create_WithInvalidValues_ThrowsArgumentException(int invalidValue)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ActivityDifficulty.Create(invalidValue));
        Assert.Equal("Difficulty must be between 1 and 10", exception.Message);
    }

    #endregion

    #region Preset Tests

    [Fact]
    public void Easy_HasValue1()
    {
        // Act
        var easy = ActivityDifficulty.Easy;

        // Assert
        Assert.NotNull(easy);
        Assert.Equal(1, easy.Value);
    }

    [Fact]
    public void Medium_HasValue5()
    {
        // Act
        var medium = ActivityDifficulty.Medium;

        // Assert
        Assert.NotNull(medium);
        Assert.Equal(5, medium.Value);
    }

    [Fact]
    public void Hard_HasValue8()
    {
        // Act
        var hard = ActivityDifficulty.Hard;

        // Assert
        Assert.NotNull(hard);
        Assert.Equal(8, hard.Value);
    }

    [Fact]
    public void Easy_EqualsCreateWith1()
    {
        // Arrange
        var easy = ActivityDifficulty.Easy;
        var created = ActivityDifficulty.Create(1);

        // Act
        var result = easy.Equals(created);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Medium_EqualsCreateWith5()
    {
        // Arrange
        var medium = ActivityDifficulty.Medium;
        var created = ActivityDifficulty.Create(5);

        // Act
        var result = medium.Equals(created);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Hard_EqualsCreateWith8()
    {
        // Arrange
        var hard = ActivityDifficulty.Hard;
        var created = ActivityDifficulty.Create(8);

        // Act
        var result = hard.Equals(created);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Presets_AreRepeatable()
    {
        // Act
        var easy1 = ActivityDifficulty.Easy;
        var easy2 = ActivityDifficulty.Easy;
        var medium1 = ActivityDifficulty.Medium;
        var medium2 = ActivityDifficulty.Medium;

        // Assert
        Assert.Equal(easy1, easy2);
        Assert.Equal(medium1, medium2);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        // Arrange
        var difficulty1 = ActivityDifficulty.Create(5);
        var difficulty2 = ActivityDifficulty.Create(5);

        // Act
        var result = difficulty1.Equals(difficulty2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var difficulty1 = ActivityDifficulty.Create(5);
        var difficulty2 = ActivityDifficulty.Create(6);

        // Act
        var result = difficulty1.Equals(difficulty2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        // Arrange
        var difficulty = ActivityDifficulty.Create(5);

        // Act
        var result = difficulty.Equals(difficulty);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        // Arrange
        var difficulty = ActivityDifficulty.Create(5);

        // Act
        var result = difficulty.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Arrange
        var difficulty = ActivityDifficulty.Create(5);
        var obj = new object();

        // Act
        var result = difficulty.Equals(obj);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_EasyAndMedium_ReturnsFalse()
    {
        // Arrange
        var easy = ActivityDifficulty.Easy;
        var medium = ActivityDifficulty.Medium;

        // Act
        var result = easy.Equals(medium);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region Operator Equality Tests

    [Fact]
    public void OperatorEquals_SameValue_ReturnsTrue()
    {
        // Arrange
        var difficulty1 = ActivityDifficulty.Create(7);
        var difficulty2 = ActivityDifficulty.Create(7);

        // Act
        var result = difficulty1 == difficulty2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var difficulty1 = ActivityDifficulty.Create(3);
        var difficulty2 = ActivityDifficulty.Create(8);

        // Act
        var result = difficulty1 == difficulty2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_BothNull_ReturnsTrue()
    {
        // Arrange
        ActivityDifficulty? difficulty1 = null;
        ActivityDifficulty? difficulty2 = null;

        // Act
        var result = difficulty1 == difficulty2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_OneNull_ReturnsFalse()
    {
        // Arrange
        ActivityDifficulty? difficulty1 = null;
        var difficulty2 = ActivityDifficulty.Create(5);

        // Act
        var result = difficulty1 == difficulty2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_EasyAndCreateWith1_ReturnsTrue()
    {
        // Arrange
        var easy = ActivityDifficulty.Easy;
        var created = ActivityDifficulty.Create(1);

        // Act
        var result = easy == created;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_EasyAndMedium_ReturnsFalse()
    {
        // Arrange
        var easy = ActivityDifficulty.Easy;
        var medium = ActivityDifficulty.Medium;

        // Act
        var result = easy == medium;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_SameValue_ReturnsFalse()
    {
        // Arrange
        var difficulty1 = ActivityDifficulty.Create(4);
        var difficulty2 = ActivityDifficulty.Create(4);

        // Act
        var result = difficulty1 != difficulty2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_DifferentValue_ReturnsTrue()
    {
        // Arrange
        var difficulty1 = ActivityDifficulty.Create(2);
        var difficulty2 = ActivityDifficulty.Create(9);

        // Act
        var result = difficulty1 != difficulty2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorNotEquals_BothNull_ReturnsFalse()
    {
        // Arrange
        ActivityDifficulty? difficulty1 = null;
        ActivityDifficulty? difficulty2 = null;

        // Act
        var result = difficulty1 != difficulty2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_OneNull_ReturnsTrue()
    {
        // Arrange
        var difficulty1 = ActivityDifficulty.Create(6);
        ActivityDifficulty? difficulty2 = null;

        // Act
        var result = difficulty1 != difficulty2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorNotEquals_MediumAndHard_ReturnsTrue()
    {
        // Arrange
        var medium = ActivityDifficulty.Medium;
        var hard = ActivityDifficulty.Hard;

        // Act
        var result = medium != hard;

        // Assert
        Assert.True(result);
    }

    #endregion

    #region GetHashCode Tests

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var difficulty1 = ActivityDifficulty.Create(6);
        var difficulty2 = ActivityDifficulty.Create(6);

        // Act
        var hashCode1 = difficulty1.GetHashCode();
        var hashCode2 = difficulty2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_DifferentValue_ReturnsDifferentHashCode()
    {
        // Arrange
        var difficulty1 = ActivityDifficulty.Create(3);
        var difficulty2 = ActivityDifficulty.Create(7);

        // Act
        var hashCode1 = difficulty1.GetHashCode();
        var hashCode2 = difficulty2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_Consistency_MultipleCalls()
    {
        // Arrange
        var difficulty = ActivityDifficulty.Create(5);

        // Act
        var hashCode1 = difficulty.GetHashCode();
        var hashCode2 = difficulty.GetHashCode();
        var hashCode3 = difficulty.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
        Assert.Equal(hashCode2, hashCode3);
    }

    [Fact]
    public void GetHashCode_AllValidValues_ProduceDifferentHashCodes()
    {
        // Arrange
        var difficulties = Enumerable.Range(1, 10)
            .Select(i => ActivityDifficulty.Create(i))
            .ToList();

        // Act
        var hashCodes = difficulties.Select(d => d.GetHashCode()).ToList();

        // Assert - All hash codes should be unique
        Assert.Equal(10, hashCodes.Count);
        Assert.Equal(10, hashCodes.Distinct().Count());
    }

    [Fact]
    public void GetHashCode_EasyEqualsCreateWith1()
    {
        // Arrange
        var easy = ActivityDifficulty.Easy;
        var created = ActivityDifficulty.Create(1);

        // Act
        var hashCode1 = easy.GetHashCode();
        var hashCode2 = created.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void Difficulty_ValueIsImmutable()
    {
        // Arrange
        var difficulty = ActivityDifficulty.Create(5);

        // Act & Assert - Value property has only getter, so it cannot be reassigned
        // This test verifies the design by ensuring we can read the property
        Assert.Equal(5, difficulty.Value);

        // If Value were mutable, we would be able to do:
        // difficulty.Value = 10; // This should not compile
    }

    #endregion

    #region Boundary Value Tests

    [Fact]
    public void Create_WithMinimumValue_WorksCorrectly()
    {
        // Act
        var difficulty = ActivityDifficulty.Create(1);

        // Assert
        Assert.Equal(1, difficulty.Value);
    }

    [Fact]
    public void Create_WithMaximumValue_WorksCorrectly()
    {
        // Act
        var difficulty = ActivityDifficulty.Create(10);

        // Assert
        Assert.Equal(10, difficulty.Value);
    }

    [Fact]
    public void Create_JustBelowMinimum_Throws()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ActivityDifficulty.Create(0));
    }

    [Fact]
    public void Create_JustAboveMaximum_Throws()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ActivityDifficulty.Create(11));
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void AllPresets_AreDifferent()
    {
        // Arrange
        var easy = ActivityDifficulty.Easy;
        var medium = ActivityDifficulty.Medium;
        var hard = ActivityDifficulty.Hard;

        // Act & Assert
        Assert.NotEqual(easy, medium);
        Assert.NotEqual(medium, hard);
        Assert.NotEqual(easy, hard);
    }

    [Fact]
    public void AllPresets_HaveCorrectOrdering()
    {
        // Arrange
        var easy = ActivityDifficulty.Easy;
        var medium = ActivityDifficulty.Medium;
        var hard = ActivityDifficulty.Hard;

        // Act & Assert
        Assert.True(easy.Value < medium.Value);
        Assert.True(medium.Value < hard.Value);
    }

    #endregion
}
