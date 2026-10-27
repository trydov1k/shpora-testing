using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void CreateNumberValidator_ThrowsArgumentException_OnNegativePrecision()
    {
        var action = () => new NumberValidator(-1, 2, true);
        action.Should().Throw<ArgumentException>();
    }
    
    [Test]
    public void CreateNumberValidator_ThrowsArgumentException_OnNegativeScale()
    {
        var action = () => new NumberValidator(1, -1, true);
        action.Should().Throw<ArgumentException>();
    }
    
    [Test]
    public void CreateNumberValidator_ThrowsArgumentException_OnScaleGreaterThanPrecision()
    {
        var action = () => new NumberValidator(1, 2, true);
        action.Should().Throw<ArgumentException>();
    }


    [Test]
    public void CreateNumberValidator_ThrowsArgumentException_OnScaleEqualToPrecision()
    {
        var action = () => new NumberValidator(1, 1, true);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_OnNullString()
    {
        var validator = new NumberValidator(17, 2, true);
        
        var result = validator.IsValidNumber(null!);
        result.Should().BeFalse();
    }
    
    [Test]
    public void IsValidNumber_ReturnsFalse_OnEmptyString()
    {
        var validator = new NumberValidator(17, 2, true);
        
        var result = validator.IsValidNumber("");
        result.Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_OnZeroWithFractionalPart()
    {
        var validator = new NumberValidator(17, 2, true);
        var result = validator.IsValidNumber("0.0");
        result.Should().BeTrue();
    }
    
    [Test]
    public void IsValidNumber_ReturnsTrue_OnIntegerZero()
    {
        var validator = new NumberValidator(17, 2, true);
        var result = validator.IsValidNumber("0");
        result.Should().BeTrue();
    }
    
    [Test]
    public void IsValidNumber_ReturnsFalse_WhenDigitCountExceedsPrecision()
    {
        var validator = new NumberValidator(3, 2, true);
        var result = validator.IsValidNumber("00.00");
        result.Should().BeFalse();
    }
    
    [Test]
    public void IsValidNumber_ReturnsFalse_OnNegativeZero_WhenOnlyPositiveEnabled()
    {
        var validator = new NumberValidator(17, 2, true);
        var result = validator.IsValidNumber("-0.00");
        result.Should().BeFalse();
    }
    
    [Test]
    public void IsValidNumber_ReturnsTrue_OnNegativeZero_WhenOnlyPositiveDisabled()
    {
        var validator = new NumberValidator(17, 2, false);
        var result = validator.IsValidNumber("-0.00");
        result.Should().BeTrue();
    }
    
    [Test]
    public void IsValidNumber_ReturnsTrue_WhenNumberWithPlusSignFitsPrecision()
    {
        var validator = new NumberValidator(4, 2, true);
        var result = validator.IsValidNumber("+1.23");
        result.Should().BeTrue();
    }
    
    [Test]
    public void IsValidNumber_ReturnsFalse_WhenNumberWithPlusSignExceedsPrecision()
    {
        var validator = new NumberValidator(3, 2, true);
        var result = validator.IsValidNumber("+1.23");
        result.Should().BeFalse();
    }
    
    [Test]
    public void IsValidNumber_ReturnsFalse_WhenFractionalDigitCountExceedsScale()
    {
        var validator = new NumberValidator(17, 2, true);
        var result = validator.IsValidNumber("0.000");
        result.Should().BeFalse();
    }
    
    [Test]
    public void IsValidNumber_ReturnsFalse_OnNonNumericString()
    {
        var validator = new NumberValidator(3, 2, true);
        var result = validator.IsValidNumber("a.sd");
        result.Should().BeFalse();
    }
}