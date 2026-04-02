# Тема 5 - Windows Forms приложение с тестированием

## Описание
Windows Forms приложение с полноценным покрытием unit-тестами, демонстрирующее принципы разработки через тестирование (TDD - Test-Driven Development).

## Функциональность
Приложение представляет собой Windows Forms проект с интегрированной системой автоматического тестирования, обеспечивающей надёжность и качество кода.

## Возможности
- Windows Forms пользовательский интерфейс
- Универсальный класс-утилита (Class1)
- Полное покрытие unit-тестами
- Автоматизированное тестирование

## Технические детали
- **Платформа**: .NET Framework 4.7.2
- **Технология**: Windows Forms
- **Тестовый фреймворк**: MSTest
- **IDE**: Visual Studio

## Структура проекта
```
Тема 5/
├── WindowsFormsApp1/             # Основное приложение
│   ├── Form1.cs                  # Главная форма
│   ├── Form1.Designer.cs
│   ├── Class1.cs                 # Класс с бизнес-логикой
│   ├── Program.cs                # Точка входа
│   ├── WindowsFormsApp1.csproj   # Файл проекта
│   └── Properties/               # Свойства проекта
├── UnitTestProject1/             # Проект с тестами
│   ├── UnitTest1.cs              # Unit-тесты
│   ├── UnitTestProject1.csproj   # Файл проекта тестов
│   └── Properties/
│       └── AssemblyInfo.cs
├── packages/                     # NuGet пакеты
│   └── MSTest.*                  # Пакеты для тестирования
└── WindowsFormsApp1.sln          # Решение Visual Studio
```

## Компоненты решения

### WindowsFormsApp1
Основное приложение с пользовательским интерфейсом.

**Основные файлы:**
- `Form1.cs` - Главная форма приложения
- `Class1.cs` - Класс с основной функциональностью

### UnitTestProject1
Проект с автоматическими тестами для проверки функциональности.

**Тестовые файлы:**
- `UnitTest1.cs` - Набор unit-тестов

## Тестирование

### Используемые атрибуты MSTest
- `[TestClass]` - Обозначает класс с тестами
- `[TestMethod]` - Обозначает метод-тест
- `[TestInitialize]` - Инициализация перед каждым тестом
- `[TestCleanup]` - Очистка после каждого теста

### Методы проверки (Assert)
- `Assert.AreEqual()` - Проверка равенства
- `Assert.IsTrue()` - Проверка истинности
- `Assert.IsFalse()` - Проверка ложности
- `Assert.IsNotNull()` - Проверка на null
- `Assert.ThrowsException()` - Проверка исключений

## Запуск приложения
1. Открыть решение `WindowsFormsApp1.sln` в Visual Studio
2. Убедиться, что установлены NuGet пакеты MSTest
3. Собрать решение (Build → Build Solution)
4. Запустить приложение (F5)

## Запуск тестов

### Через Visual Studio
1. Открыть Test Explorer (Test → Test Explorer)
2. Собрать решение (Ctrl+Shift+B)
3. Запустить все тесты:
   - Нажать "Run All" в Test Explorer
   - Или использовать Ctrl+R, A

### Через командную строку
```bash
cd WindowsFormsApp1
dotnet test
```

### Через Developer Command Prompt
```cmd
cd Тема 5
vstest.console.exe UnitTestProject1\bin\Debug\UnitTestProject1.dll
```

## Просмотр результатов тестов
- **Зелёная галочка**: Тест пройден успешно
- **Красный крестик**: Тест провален
- **Жёлтый предупреждающий знак**: Тест пропущен

Test Explorer показывает:
- Количество пройденных тестов
- Количество проваленных тестов
- Время выполнения каждого теста
- Сообщения об ошибках (для проваленных тестов)

## Принципы разработки

### TDD (Test-Driven Development)
1. Написать тест (который изначально провалится)
2. Написать минимальный код для прохождения теста
3. Рефакторинг кода
4. Повторить цикл

### AAA Pattern (Arrange-Act-Assert)
```csharp
[TestMethod]
public void TestMethod_Scenario_ExpectedBehavior()
{
    // Arrange - подготовка данных
    var testObject = new Class1();

    // Act - выполнение действия
    var result = testObject.Method();

    // Assert - проверка результата
    Assert.AreEqual(expectedValue, result);
}
```

## Преимущества тестирования
- Раннее обнаружение ошибок
- Уверенность в работоспособности кода
- Безопасный рефакторинг
- Документация кода через тесты
- Упрощение отладки
- Повышение качества кода

## Покрытие кода тестами
Для анализа покрытия можно использовать:
- Visual Studio Enterprise (встроенный Code Coverage)
- dotCover (JetBrains)
- OpenCover + ReportGenerator

## Best Practices
1. **Именование тестов**: `MethodName_Scenario_ExpectedBehavior`
2. **Один Assert на тест**: Каждый тест проверяет одну вещь
3. **Независимость**: Тесты не должны зависеть друг от друга
4. **Быстрота**: Тесты должны выполняться быстро
5. **Повторяемость**: Тесты дают одинаковый результат

## Пример структуры теста
```csharp
[TestClass]
public class UnitTest1
{
    private Class1 _testObject;

    [TestInitialize]
    public void Setup()
    {
        _testObject = new Class1();
    }

    [TestMethod]
    public void Method_ValidInput_ReturnsExpectedValue()
    {
        // Arrange
        var input = 10;

        // Act
        var result = _testObject.Method(input);

        // Assert
        Assert.AreEqual(20, result);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _testObject = null;
    }
}
```

## Непрерывная интеграция (CI)
Тесты можно интегрировать в CI/CD pipeline:
- Azure DevOps
- GitHub Actions
- Jenkins
- TeamCity

## Автор
Практическая работа по программированию - Unit-тестирование
