using Valoron.Activities.Domain;

namespace Valoron.Activities.Tests;

public class ActivityTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_CreatesActivity()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Clean the kitchen";
        var category = ActivityCategory.Environment;
        var difficulty = ActivityDifficulty.Medium;

        // Act
        var activity = new Activity(id, title, category, difficulty);

        // Assert
        Assert.Equal(id, activity.Id);
        Assert.Equal(title, activity.Title);
        Assert.Equal(category, activity.Category);
        Assert.Equal(difficulty, activity.Difficulty);
    }

    [Fact]
    public void Constructor_WithValidParameters_SetsCreatedAtToCurrentTime()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Clean the kitchen";
        var category = ActivityCategory.Environment;
        var difficulty = ActivityDifficulty.Medium;
        var beforeCreation = DateTime.UtcNow;

        // Act
        var activity = new Activity(id, title, category, difficulty);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(activity.CreatedAt >= beforeCreation);
        Assert.True(activity.CreatedAt <= afterCreation);
    }

    [Fact]
    public void Constructor_WithValidParameters_InitiallyNotCompleted()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Clean the kitchen";
        var category = ActivityCategory.Environment;
        var difficulty = ActivityDifficulty.Medium;

        // Act
        var activity = new Activity(id, title, category, difficulty);

        // Assert
        Assert.False(activity.IsCompleted);
        Assert.Null(activity.CompletedAt);
    }

    [Fact]
    public void Constructor_WithNullTitle_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        string title = null!;
        var category = ActivityCategory.Environment;
        var difficulty = ActivityDifficulty.Medium;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Activity(id, title, category, difficulty));
        Assert.Equal("Title can't be empty", exception.Message);
    }

    [Fact]
    public void Constructor_WithEmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "";
        var category = ActivityCategory.Environment;
        var difficulty = ActivityDifficulty.Medium;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Activity(id, title, category, difficulty));
        Assert.Equal("Title can't be empty", exception.Message);
    }

    [Fact]
    public void Constructor_WithWhitespaceTitle_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "   ";
        var category = ActivityCategory.Environment;
        var difficulty = ActivityDifficulty.Medium;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Activity(id, title, category, difficulty));
        Assert.Equal("Title can't be empty", exception.Message);
    }

    [Fact]
    public void Constructor_WithTabsAndNewlines_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "\t\n\r";
        var category = ActivityCategory.Environment;
        var difficulty = ActivityDifficulty.Medium;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Activity(id, title, category, difficulty));
        Assert.Equal("Title can't be empty", exception.Message);
    }

    #endregion

    #region Complete Method Tests

    [Fact]
    public void Complete_OnNewActivity_MarksAsCompleted()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        activity.Complete();

        // Assert
        Assert.True(activity.IsCompleted);
        Assert.NotNull(activity.CompletedAt);
    }

    [Fact]
    public void Complete_OnNewActivity_SetsCompletedAtToCurrentTime()
    {
        // Arrange
        var activity = CreateValidActivity();
        var beforeCompletion = DateTime.UtcNow;

        // Act
        activity.Complete();
        var afterCompletion = DateTime.UtcNow;

        // Assert
        Assert.NotNull(activity.CompletedAt);
        Assert.True(activity.CompletedAt >= beforeCompletion);
        Assert.True(activity.CompletedAt <= afterCompletion);
    }

    [Fact]
    public void Complete_CalledTwice_IsIdempotent()
    {
        // Arrange
        var activity = CreateValidActivity();
        activity.Complete();
        var firstCompletedAt = activity.CompletedAt;

        // Add small delay to ensure time would change if CompletedAt was being reset
        Thread.Sleep(10);

        // Act
        activity.Complete();
        var secondCompletedAt = activity.CompletedAt;

        // Assert
        Assert.Equal(firstCompletedAt, secondCompletedAt);
    }

    [Fact]
    public void Complete_CalledMultipleTimes_DoesNotChangeCompletedAt()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        activity.Complete();
        var originalCompletedAt = activity.CompletedAt;

        Thread.Sleep(10);
        activity.Complete();
        Thread.Sleep(10);
        activity.Complete();

        // Assert
        Assert.Equal(originalCompletedAt, activity.CompletedAt);
    }

    [Fact]
    public void IsCompleted_BeforeComplete_ReturnsFalse()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        var isCompleted = activity.IsCompleted;

        // Assert
        Assert.False(isCompleted);
    }

    [Fact]
    public void IsCompleted_AfterComplete_ReturnsTrue()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        activity.Complete();

        // Assert
        Assert.True(activity.IsCompleted);
    }

    #endregion

    #region UpdateDifficulty Method Tests

    [Fact]
    public void UpdateDifficulty_OnIncompleteActivity_UpdatesDifficulty()
    {
        // Arrange
        var activity = CreateValidActivity(difficulty: ActivityDifficulty.Easy);
        var newDifficulty = ActivityDifficulty.Hard;

        // Act
        activity.UpdateDifficulty(newDifficulty);

        // Assert
        Assert.Equal(newDifficulty, activity.Difficulty);
    }

    [Fact]
    public void UpdateDifficulty_MultipleTimesOnIncompleteActivity_UpdatesSuccessfully()
    {
        // Arrange
        var activity = CreateValidActivity(difficulty: ActivityDifficulty.Easy);

        // Act
        activity.UpdateDifficulty(ActivityDifficulty.Medium);
        activity.UpdateDifficulty(ActivityDifficulty.Hard);
        activity.UpdateDifficulty(ActivityDifficulty.Create(7));

        // Assert
        Assert.Equal(7, activity.Difficulty.Value);
    }

    [Fact]
    public void UpdateDifficulty_OnCompletedActivity_ThrowsInvalidOperationException()
    {
        // Arrange
        var activity = CreateValidActivity(difficulty: ActivityDifficulty.Easy);
        activity.Complete();
        var newDifficulty = ActivityDifficulty.Hard;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            activity.UpdateDifficulty(newDifficulty));
        Assert.Equal("Impossible to change the difficulty of an ended activity", exception.Message);
    }

    [Fact]
    public void UpdateDifficulty_OnCompletedActivity_DoesNotChangeDifficulty()
    {
        // Arrange
        var originalDifficulty = ActivityDifficulty.Easy;
        var activity = CreateValidActivity(difficulty: originalDifficulty);
        activity.Complete();

        // Act
        try
        {
            activity.UpdateDifficulty(ActivityDifficulty.Hard);
        }
        catch (InvalidOperationException)
        {
            // Expected exception
        }

        // Assert
        Assert.Equal(originalDifficulty, activity.Difficulty);
    }

    #endregion

    #region State Transition Tests

    [Fact]
    public void Activity_StateTransition_FromNewToCompleted()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act - Initial state
        Assert.False(activity.IsCompleted);
        Assert.Null(activity.CompletedAt);

        // Act - Transition to completed
        activity.Complete();

        // Assert - Completed state
        Assert.True(activity.IsCompleted);
        Assert.NotNull(activity.CompletedAt);
    }

    [Fact]
    public void Activity_CompletedState_LocksModifications()
    {
        // Arrange
        var activity = CreateValidActivity();
        activity.Complete();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            activity.UpdateDifficulty(ActivityDifficulty.Hard));
    }

    #endregion

    #region Integration Tests (Multiple Operations)

    [Fact]
    public void Activity_CanUpdateDifficultyMultipleTimesBeforeCompletion()
    {
        // Arrange
        var activity = CreateValidActivity(difficulty: ActivityDifficulty.Easy);

        // Act
        activity.UpdateDifficulty(ActivityDifficulty.Medium);
        activity.UpdateDifficulty(ActivityDifficulty.Hard);
        activity.Complete();

        // Assert
        Assert.True(activity.IsCompleted);
        Assert.Equal(ActivityDifficulty.Hard, activity.Difficulty);
    }

    [Fact]
    public void Activity_CompletedAtIsAfterCreatedAt()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        Thread.Sleep(10); // Ensure some time passes
        activity.Complete();

        // Assert
        Assert.True(activity.CompletedAt > activity.CreatedAt);
    }

    [Fact]
    public void Activity_DifferentCategoriesAndDifficulties_AllWork()
    {
        // Arrange & Act
        var activities = new[]
        {
            CreateValidActivity(category: ActivityCategory.Environment, difficulty: ActivityDifficulty.Easy),
            CreateValidActivity(category: ActivityCategory.Body, difficulty: ActivityDifficulty.Medium),
            CreateValidActivity(category: ActivityCategory.Social, difficulty: ActivityDifficulty.Hard),
            CreateValidActivity(category: ActivityCategory.Learning, difficulty: ActivityDifficulty.Create(6))
        };

        // Assert
        foreach (var activity in activities)
        {
            Assert.NotNull(activity);
            Assert.False(activity.IsCompleted);

            activity.Complete();
            Assert.True(activity.IsCompleted);
        }
    }

    #endregion

    #region Entity Identity Tests

    [Fact]
    public void Activity_WithSameId_AreEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var activity1 = new Activity(id, "Activity 1", ActivityCategory.Body, ActivityDifficulty.Easy);
        var activity2 = new Activity(id, "Activity 2", ActivityCategory.Social, ActivityDifficulty.Hard);

        // Act
        var areEqual = activity1.Equals(activity2);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Activity_WithDifferentIds_AreNotEqual()
    {
        // Arrange
        var activity1 = CreateValidActivity();
        var activity2 = CreateValidActivity();

        // Act
        var areEqual = activity1.Equals(activity2);

        // Assert
        Assert.False(areEqual);
    }

    #endregion

    #region Helper Methods

    private Activity CreateValidActivity(
        string title = "Test Activity",
        ActivityCategory? category = null,
        ActivityDifficulty? difficulty = null)
    {
        var id = Guid.NewGuid();
        category ??= ActivityCategory.Environment;
        difficulty ??= ActivityDifficulty.Medium;

        return new Activity(id, title, category, difficulty);
    }

    #endregion
}
