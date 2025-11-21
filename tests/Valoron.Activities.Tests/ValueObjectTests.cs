using Valoron.BuildingBlocks;

namespace Valoron.Activities.Tests;

public class ValueObjectTests
{
    // Test value object implementation with two components
    private class TestValueObject : ValueObject
    {
        public string Property1 { get; }
        public int Property2 { get; }

        public TestValueObject(string property1, int property2)
        {
            Property1 = property1;
            Property2 = property2;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Property1;
            yield return Property2;
        }
    }

    // Test value object with nullable component
    private class TestValueObjectWithNull : ValueObject
    {
        public string? NullableProperty { get; }
        public int Value { get; }

        public TestValueObjectWithNull(string? nullableProperty, int value)
        {
            NullableProperty = nullableProperty;
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return NullableProperty;
            yield return Value;
        }
    }

    [Fact]
    public void Equals_SameComponents_ReturnsTrue()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 42);

        // Act
        var result = vo1.Equals(vo2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_DifferentComponents_ReturnsFalse()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 43);

        // Act
        var result = vo1.Equals(vo2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        // Arrange
        var vo = new TestValueObject("test", 42);

        // Act
        var result = vo.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Arrange
        var vo = new TestValueObject("test", 42);
        var obj = new object();

        // Act
        var result = vo.Equals(obj);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        // Arrange
        var vo = new TestValueObject("test", 42);

        // Act
        var result = vo.Equals(vo);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithNullComponents_ReturnsTrue()
    {
        // Arrange
        var vo1 = new TestValueObjectWithNull(null, 42);
        var vo2 = new TestValueObjectWithNull(null, 42);

        // Act
        var result = vo1.Equals(vo2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_OneNullOneNot_ReturnsFalse()
    {
        // Arrange
        var vo1 = new TestValueObjectWithNull(null, 42);
        var vo2 = new TestValueObjectWithNull("test", 42);

        // Act
        var result = vo1.Equals(vo2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_SameComponents_ReturnsTrue()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 42);

        // Act
        var result = vo1 == vo2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_DifferentComponents_ReturnsFalse()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("different", 42);

        // Act
        var result = vo1 == vo2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorEquals_BothNull_ReturnsTrue()
    {
        // Arrange
        TestValueObject? vo1 = null;
        TestValueObject? vo2 = null;

        // Act
        var result = vo1 == vo2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_OneNull_ReturnsFalse()
    {
        // Arrange
        TestValueObject? vo1 = null;
        var vo2 = new TestValueObject("test", 42);

        // Act
        var result = vo1 == vo2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_SameComponents_ReturnsFalse()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 42);

        // Act
        var result = vo1 != vo2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_DifferentComponents_ReturnsTrue()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 43);

        // Act
        var result = vo1 != vo2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorNotEquals_BothNull_ReturnsFalse()
    {
        // Arrange
        TestValueObject? vo1 = null;
        TestValueObject? vo2 = null;

        // Act
        var result = vo1 != vo2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_OneNull_ReturnsTrue()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        TestValueObject? vo2 = null;

        // Act
        var result = vo1 != vo2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetHashCode_SameComponents_ReturnsSameHashCode()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 42);

        // Act
        var hashCode1 = vo1.GetHashCode();
        var hashCode2 = vo2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_DifferentComponents_ReturnsDifferentHashCode()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 43);

        // Act
        var hashCode1 = vo1.GetHashCode();
        var hashCode2 = vo2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_Consistency_MultipleCalls()
    {
        // Arrange
        var vo = new TestValueObject("test", 42);

        // Act
        var hashCode1 = vo.GetHashCode();
        var hashCode2 = vo.GetHashCode();
        var hashCode3 = vo.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
        Assert.Equal(hashCode2, hashCode3);
    }

    [Fact]
    public void GetHashCode_WithNullComponent_DoesNotThrow()
    {
        // Arrange
        var vo = new TestValueObjectWithNull(null, 42);

        // Act & Assert - Should not throw
        var hashCode = vo.GetHashCode();
        Assert.True(true); // If we get here, no exception was thrown
    }

    [Fact]
    public void GetHashCode_TwoNullComponents_ReturnsSameHashCode()
    {
        // Arrange
        var vo1 = new TestValueObjectWithNull(null, 42);
        var vo2 = new TestValueObjectWithNull(null, 42);

        // Act
        var hashCode1 = vo1.GetHashCode();
        var hashCode2 = vo2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }
}
