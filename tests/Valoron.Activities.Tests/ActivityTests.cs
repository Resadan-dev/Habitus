using Valoron.Activities.Domain;
using Valoron.Activities.Domain.Events;

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
        var measurement = ActivityMeasurement.CreateBinary();

        // Act
        var activity = new Activity(id, title, category, difficulty, measurement);

        // Assert
        Assert.Equal(id, activity.Id);
        Assert.Equal(title, activity.Title);
        Assert.Equal(category, activity.Category);
        Assert.Equal(difficulty, activity.Difficulty);
        Assert.Equal(measurement, activity.Measurement);
    }

    [Fact]
    public void Constructor_WithValidParameters_SetsCreatedAtToCurrentTime()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Clean the kitchen";
        var category = ActivityCategory.Environment;
        var difficulty = ActivityDifficulty.Medium;
        var measurement = ActivityMeasurement.CreateBinary();
        var beforeCreation = DateTime.UtcNow;

        // Act
        var activity = new Activity(id, title, category, difficulty, measurement);
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
        var measurement = ActivityMeasurement.CreateBinary();

        // Act
        var activity = new Activity(id, title, category, difficulty, measurement);

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
        var measurement = ActivityMeasurement.CreateBinary();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Activity(id, title, category, difficulty, measurement));
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
        var measurement = ActivityMeasurement.CreateBinary();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Activity(id, title, category, difficulty, measurement));
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
        var measurement = ActivityMeasurement.CreateBinary();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Activity(id, title, category, difficulty, measurement));
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
        var measurement = ActivityMeasurement.CreateBinary();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Activity(id, title, category, difficulty, measurement));
        Assert.Equal("Title can't be empty", exception.Message);
    }

    #endregion

    #region LogProgress Method Tests (formerly Complete)

    [Fact]
    public void LogProgress_OnBinaryActivity_MarksAsCompleted()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        activity.LogProgress(1);

        // Assert
        Assert.True(activity.IsCompleted);
        Assert.NotNull(activity.CompletedAt);
    }

    [Fact]
    public void LogProgress_OnBinaryActivity_SetsCompletedAtToCurrentTime()
    {
        // Arrange
        var activity = CreateValidActivity();
        var beforeCompletion = DateTime.UtcNow;

        // Act
        activity.LogProgress(1);
        var afterCompletion = DateTime.UtcNow;

        // Assert
        Assert.NotNull(activity.CompletedAt);
        Assert.True(activity.CompletedAt >= beforeCompletion);
        Assert.True(activity.CompletedAt <= afterCompletion);
    }

    [Fact]
    public void LogProgress_CalledTwiceOnBinary_IsIdempotent()
    {
        // Arrange
        var activity = CreateValidActivity();
        activity.LogProgress(1);
        var firstCompletedAt = activity.CompletedAt;

        // Add small delay to ensure time would change if CompletedAt was being reset
        Thread.Sleep(10);

        // Act
        activity.LogProgress(1);
        var secondCompletedAt = activity.CompletedAt;

        // Assert
        Assert.Equal(firstCompletedAt, secondCompletedAt);
    }

    [Fact]
    public void LogProgress_CalledMultipleTimes_DoesNotChangeCompletedAt()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        activity.LogProgress(1);
        var originalCompletedAt = activity.CompletedAt;

        Thread.Sleep(10);
        activity.LogProgress(1);
        Thread.Sleep(10);
        activity.LogProgress(1);

        // Assert
        Assert.Equal(originalCompletedAt, activity.CompletedAt);
    }

    [Fact]
    public void IsCompleted_BeforeLogProgress_ReturnsFalse()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        var isCompleted = activity.IsCompleted;

        // Assert
        Assert.False(isCompleted);
    }

    [Fact]
    public void IsCompleted_AfterLogProgress_ReturnsTrue()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        activity.LogProgress(1);

        // Assert
        Assert.True(activity.IsCompleted);
    }

    #endregion

    #region Measurement Tests

    [Fact]
    public void LogProgress_OnQuantifiableActivity_UpdatesProgress()
    {
        // Arrange
        var measurement = ActivityMeasurement.CreateQuantifiable(MeasureUnit.Count, 20);
        var activity = CreateValidActivity(measurement: measurement);

        // Act
        activity.LogProgress(10);

        // Assert
        Assert.False(activity.IsCompleted);
        Assert.Equal(10, activity.Measurement.CurrentValue);
        Assert.Equal(0.5m, activity.Measurement.CompletionPercentage());
    }

    [Fact]
    public void LogProgress_Regression_RemovesCompletedAt()
    {
        // Arrange
        var measurement = ActivityMeasurement.CreateQuantifiable(MeasureUnit.Count, 20);
        var activity = CreateValidActivity(measurement: measurement);
        activity.LogProgress(20); // Complete it first

        // Act
        activity.LogProgress(-10); // Regress

        // Assert
        Assert.False(activity.IsCompleted);
        Assert.Null(activity.CompletedAt);
        Assert.Equal(10, activity.Measurement.CurrentValue);
    }

    [Fact]
    public void LogProgress_OnQuantifiableActivity_CompletesWhenTargetReached()
    {
        // Arrange
        var measurement = ActivityMeasurement.CreateQuantifiable(MeasureUnit.Count, 20);
        var activity = CreateValidActivity(measurement: measurement);

        // Act
        activity.LogProgress(20);

        // Assert
        Assert.True(activity.IsCompleted);
        Assert.NotNull(activity.CompletedAt);
        Assert.Equal(20, activity.Measurement.CurrentValue);
    }

    [Fact]
    public void LogProgress_OnQuantifiableActivity_CanOverAchieve()
    {
        // Arrange
        var measurement = ActivityMeasurement.CreateQuantifiable(MeasureUnit.Count, 20);
        var activity = CreateValidActivity(measurement: measurement);

        // Act
        activity.LogProgress(25);

        // Assert
        Assert.True(activity.IsCompleted);
        Assert.Equal(25, activity.Measurement.CurrentValue);
        Assert.Equal(1.0m, activity.Measurement.CompletionPercentage()); // Capped at 1.0 for percentage
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
        activity.LogProgress(1);
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
        activity.LogProgress(1);

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
        activity.LogProgress(1);

        // Assert - Completed state
        Assert.True(activity.IsCompleted);
        Assert.NotNull(activity.CompletedAt);
    }

    [Fact]
    public void Activity_CompletedState_LocksModifications()
    {
        // Arrange
        var activity = CreateValidActivity();
        activity.LogProgress(1);

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
        activity.LogProgress(1);

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
        activity.LogProgress(1);

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

            activity.LogProgress(1);
            Assert.True(activity.IsCompleted);
        }
    }

    #endregion

    #region Domain Event Tests

    [Fact]
    public void LogProgress_WhenCompleted_RaisesActivityCompletedEvent()
    {
        // Arrange
        var activity = CreateValidActivity();

        // Act
        activity.LogProgress(1);

        // Assert
        var completedEvent = activity.DomainEvents.OfType<ActivityCompletedEvent>().SingleOrDefault();
        Assert.NotNull(completedEvent);
        Assert.Equal(activity.Id, completedEvent.ActivityId);
        Assert.Equal(activity.ResourceId, completedEvent.ResourceId);
    }

    [Fact]
    public void LogProgress_WhenNotCompleted_DoesNotRaiseActivityCompletedEvent()
    {
        // Arrange
        var measurement = ActivityMeasurement.CreateQuantifiable(MeasureUnit.Count, 20);
        var activity = CreateValidActivity(measurement: measurement);

        // Act
        activity.LogProgress(10);

        // Assert
        var completedEvent = activity.DomainEvents.OfType<ActivityCompletedEvent>().SingleOrDefault();
        Assert.Null(completedEvent);
    }

    [Fact]
    public void Constructor_RaisesActivityCreatedEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "New Activity";
        var category = ActivityCategory.Environment;
        var difficulty = ActivityDifficulty.Medium;
        var measurement = ActivityMeasurement.CreateBinary();
        var resourceId = Guid.NewGuid();

        // Act
        var activity = new Activity(id, title, category, difficulty, measurement, resourceId);

        // Assert
        var createdEvent = activity.DomainEvents.OfType<ActivityCreatedEvent>().SingleOrDefault();
        Assert.NotNull(createdEvent);
        Assert.Equal(id, createdEvent.ActivityId);
        Assert.Equal(title, createdEvent.Title);
        Assert.Equal(category.Code, createdEvent.Category);
        Assert.Equal(difficulty.Value, createdEvent.Difficulty);
        Assert.Equal(resourceId, createdEvent.ResourceId);
    }

    #endregion

    #region Entity Identity Tests

    [Fact]
    public void Activity_WithSameId_AreEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var measurement = ActivityMeasurement.CreateBinary();
        var activity1 = new Activity(id, "Activity 1", ActivityCategory.Body, ActivityDifficulty.Easy, measurement);
        var activity2 = new Activity(id, "Activity 2", ActivityCategory.Social, ActivityDifficulty.Hard, measurement);

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
        ActivityDifficulty? difficulty = null,
        ActivityMeasurement? measurement = null)
    {
        var id = Guid.NewGuid();
        category ??= ActivityCategory.Environment;
        difficulty ??= ActivityDifficulty.Medium;
        measurement ??= ActivityMeasurement.CreateBinary();

        return new Activity(id, title, category, difficulty, measurement);
    }

    #endregion
}
