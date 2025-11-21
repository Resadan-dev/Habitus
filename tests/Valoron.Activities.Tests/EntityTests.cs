using Valoron.BuildingBlocks;

namespace Valoron.Activities.Tests;

public class EntityTests
{
    // Test entity implementation for testing purposes
    private class TestEntity : Entity
    {
        public TestEntity() : base() { }
        public TestEntity(Guid id) : base(id) { }
    }

    [Fact]
    public void Constructor_WithGuid_SetsId()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var entity = new TestEntity(id);

        // Assert
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void IsTransient_WithEmptyGuid_ReturnsTrue()
    {
        // Arrange
        var entity = new TestEntity(Guid.Empty);

        // Act
        var result = entity.IsTransient();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsTransient_WithValidGuid_ReturnsFalse()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity.IsTransient();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity.Equals(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());
        var obj = new object();

        // Act
        var result = entity.Equals(obj);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_SameIdSameType_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_DifferentIdSameType_ReturnsFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_BothTransient_ReturnsFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.Empty);
        var entity2 = new TestEntity(Guid.Empty);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_OneTransientOneNot_ReturnsFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.Empty);
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_BothNull_ReturnsTrue()
    {
        // Arrange
        TestEntity? entity1 = null;
        TestEntity? entity2 = null;

        // Act
        var result = entity1 == entity2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_LeftNull_ReturnsFalse()
    {
        // Arrange
        TestEntity? entity1 = null;
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1 == entity2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_RightNull_ReturnsFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        TestEntity? entity2 = null;

        // Act
        var result = entity1 == entity2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_SameId_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var result = entity1 == entity2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_DifferentId_ReturnsFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1 == entity2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_BothTransient_ReturnsFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.Empty);
        var entity2 = new TestEntity(Guid.Empty);

        // Act
        var result = entity1 == entity2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_BothNull_ReturnsFalse()
    {
        // Arrange
        TestEntity? entity1 = null;
        TestEntity? entity2 = null;

        // Act
        var result = entity1 != entity2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_OneNull_ReturnsTrue()
    {
        // Arrange
        TestEntity? entity1 = null;
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1 != entity2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorNotEquals_SameId_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var result = entity1 != entity2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_DifferentId_ReturnsTrue()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1 != entity2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetHashCode_TransientEntity_UsesBaseHashCode()
    {
        // Arrange
        var entity = new TestEntity(Guid.Empty);

        // Act
        var hashCode1 = entity.GetHashCode();
        var hashCode2 = entity.GetHashCode();

        // Assert - Same entity should return same hash code
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_SameIdAndType_ReturnsSameHashCode()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_DifferentId_ReturnsDifferentHashCode()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_Consistency_MultipleCalls()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act
        var hashCode1 = entity.GetHashCode();
        var hashCode2 = entity.GetHashCode();
        var hashCode3 = entity.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
        Assert.Equal(hashCode2, hashCode3);
    }
}
