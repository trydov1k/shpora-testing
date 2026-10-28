using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;

public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Перепишите код на использование Fluent Assertions.

        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.Excluding((IMemberInfo member) => member.Name == nameof(Person.Id)));

        // Мое решение лучше, чем решение в тесте CheckCurrentTsar_WithCustomEquality, потому что при добавлении новых свойств 
        // объекта тестирования в моем коде ничего менять не надо будет, максимум, если это поле сравнивать не надо. А в том решении
        // придется постоянно менять тесты при каждом изменении кода. 
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
        // Главный недостаток такого подхода заключается в том, что при провале теста тестировщик получит практически 0 информации.
        // Не понятно в каком именно поле произошла ошибка. Также, в отличие от изначального подхода тестирования, этот подход
        // рекурсивно проверяет совпадение отцов, то есть мы проверяем не только ли отец короля совпадает, но и его дедушка, прадедушка ...
        // не сказать, что это плохо, но это отличается от изначального подхода, в котором сверялись только с отцом.
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}