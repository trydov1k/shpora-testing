using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "Constructor_ThrowsArgumentException_OnNegativePrecision")]
    [TestCase(1, -1, true, TestName = "Constructor_ThrowsArgumentException_OnNegativeScale")]
    [TestCase(1, 2, true, TestName = "Constructor_ThrowsArgumentException_OnScaleGreaterThanPrecision")]
    [TestCase(1, 1, true, TestName = "Constructor_ThrowsArgumentException_OnScaleEqualToPrecision")]
    public void CreateNumberValidator_ThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);

        action.Should().Throw<ArgumentException>();
    }

    [TestCase(null, TestName = "IsValidNumber_ReturnsFalse_OnNullString")]
    [TestCase("", TestName = "IsValidNumber_ReturnsFalse_OnEmptyString")]
    public void IsValidNumber_ReturnsFalse_OnMissingValue(string? input)
    {
        var validator = new NumberValidator(17, 2, true);

        var result = validator.IsValidNumber(input!);

        result.Should().BeFalse();
    }

    [TestCase("0.0", TestName = "IsValidNumber_ReturnsTrue_OnZeroWithFractionalPart")]
    [TestCase("0", TestName = "IsValidNumber_ReturnsTrue_OnIntegerZero")]
    public void IsValidNumber_ReturnsTrue_OnZero(string input)
    {
        var validator = new NumberValidator(17, 2, true);

        var result = validator.IsValidNumber(input);

        result.Should().BeTrue();
    }

    [TestCase(true, false,
        TestName = "IsValidNumber_ReturnsFalse_OnNegativeZero_WhenOnlyPositiveEnabled")]
    [TestCase(false, true,
        TestName = "IsValidNumber_ReturnsTrue_OnNegativeZero_WhenOnlyPositiveDisabled")]
    public void IsValidNumber_ChecksNegativeZero(bool onlyPositive, bool expected)
    {
        var validator = new NumberValidator(17, 2, onlyPositive);

        var result = validator.IsValidNumber("-0.00");

        result.Should().Be(expected);
    }

    [TestCase("00.00", 3, false,
        TestName = "IsValidNumber_ReturnsFalse_WhenDigitCountExceedsPrecision")]
    [TestCase("+1.23", 4, true,
        TestName = "IsValidNumber_ReturnsTrue_WhenNumberWithPlusSignFitsPrecision")]
    [TestCase("+1.23", 3, false,
        TestName = "IsValidNumber_ReturnsFalse_WhenNumberWithPlusSignExceedsPrecision")]
    public void IsValidNumber_ChecksPrecision(string input, int precision, bool expected)
    {
        var validator = new NumberValidator(precision, 2, true);

        var result = validator.IsValidNumber(input);

        result.Should().Be(expected);
    }

    [TestCase("0.0", true,
        TestName = "IsValidNumber_ReturnsTrue_WhenFractionalDigitCountIsBelowScale")]
    [TestCase("0.00", true,
        TestName = "IsValidNumber_ReturnsTrue_WhenFractionalDigitCountEqualsScale")]
    [TestCase("0.000", false,
        TestName = "IsValidNumber_ReturnsFalse_WhenFractionalDigitCountExceedsScale")]
    public void IsValidNumber_ChecksScale(string input, bool expected)
    {
        var validator = new NumberValidator(17, 2, true);

        var result = validator.IsValidNumber(input);

        result.Should().Be(expected);
    }

    [TestCase("++0.0",
        TestName = "IsValidNumber_ReturnsFalse_WhenTwoSymbolsPlus")]
    [TestCase("--0.0",
        TestName = "IsValidNumber_ReturnsFalse_WhenTwoSymbolsMinus")]
    [TestCase("+-0.0",
        TestName = "IsValidNumber_ReturnsFalse_WhenTwoSymbolsMixed")]
    [TestCase("-+-0.0",
        TestName = "IsValidNumber_ReturnsFalse_WhenThreeSymbolsMixed")]
    public void IsValidNumber_ChecksOnManySymbolBeforePoint(string input)
    {
        var validator = new NumberValidator(17, 2, false);
        
        var result = validator.IsValidNumber(input);

        result.Should().Be(false);
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_OnNonNumericString()
    {
        var validator = new NumberValidator(3, 2, true);
        var result = validator.IsValidNumber("a.sd");
        result.Should().BeFalse();
    }
}