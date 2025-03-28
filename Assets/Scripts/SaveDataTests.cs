using NUnit.Framework;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveDataAndSaveManagerTests
{
    private string testFilePath;

    [SetUp]
    public void SetUp()
    {
        // Определяем тестовый путь, можно использовать отдельную папку внутри persistentDataPath
        testFilePath = Path.Combine(Application.persistentDataPath, "test_save.dat");
        // Меняем путь в SaveManager через Reflection (если требуется) или переопределяем метод SaveGame/LoadGame для тестирования.
        // Для простоты мы можем временно копировать файл SaveManager
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
    }

    [Test]
    public void SaveAndLoadGame_Should_PreserveSaveData()
    {
        // Arrange: создаем SaveData с несколькими клетками и bestScore
        SaveData originalData = new SaveData();
        originalData.bestScore = 100;
        originalData.cellStates = new List<CellState>
        {
            new CellState(0, 0, 1),
            new CellState(1, 1, 2)
        };

        // Сохраняем в тестовый файл
        // Для тестирования используем SaveManager.SaveGame, предварительно переопределив путь через Reflection
        typeof(SaveManager)
            .GetField("saveFilePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .SetValue(null, testFilePath);
        SaveManager.SaveGame(originalData);

        // Act: загружаем данные
        SaveData loadedData = SaveManager.LoadGame();

        // Assert: проверяем, что значения совпадают
        Assert.AreEqual(originalData.bestScore, loadedData.bestScore, "bestScore не совпадает");
        Assert.AreEqual(originalData.cellStates.Count, loadedData.cellStates.Count, "Количество клеток не совпадает");
        for (int i = 0; i < originalData.cellStates.Count; i++)
        {
            Assert.AreEqual(originalData.cellStates[i].x, loadedData.cellStates[i].x, $"Клетка {i} X не совпадает");
            Assert.AreEqual(originalData.cellStates[i].y, loadedData.cellStates[i].y, $"Клетка {i} Y не совпадает");
            Assert.AreEqual(originalData.cellStates[i].value, loadedData.cellStates[i].value, $"Клетка {i} value не совпадает");
        }
    }
}
