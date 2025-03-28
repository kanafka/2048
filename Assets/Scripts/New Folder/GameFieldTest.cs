using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Reflection;

public class GameFieldTests
{
    private GameObject gameFieldObject;
    private GameField gameField;

    
    [SetUp]
    public void SetUp()
    {
        GameObject gameFieldObj = new GameObject("GameField");
        gameField = gameFieldObj.AddComponent<GameField>();
        // Создаем объект с компонентом GameField
        gameFieldObject = new GameObject("GameField");
        gameFieldObject.AddComponent<RectTransform>();
        gameField = gameFieldObject.AddComponent<GameField>();

        // Создаем временный объект для GameBoard и назначаем его через Reflection
        GameObject boardObj = new GameObject("GameBoard");
        boardObj.transform.SetParent(gameFieldObject.transform);
        RectTransform boardRect = boardObj.AddComponent<RectTransform>();
        FieldInfo gameBoardField = typeof(GameField).GetField("GameBoard", BindingFlags.NonPublic | BindingFlags.Instance);
        gameBoardField.SetValue(gameField, boardRect);

        // Создаем объект для ScoreText
        GameObject scoreObj = new GameObject("ScoreText");
        scoreObj.transform.SetParent(gameFieldObject.transform);
        TextMeshProUGUI scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
        FieldInfo scoreTextField = typeof(GameField).GetField("scoreText", BindingFlags.NonPublic | BindingFlags.Instance);
        scoreTextField.SetValue(gameField, scoreText);

        // Создаем временный префаб для клеток
        GameObject cellPrefab = new GameObject("CellPrefab");
        cellPrefab.AddComponent<CellView>(); // минимально добавляем компонент
        FieldInfo cellPrefabField = typeof(GameField).GetField("cellPrefab", BindingFlags.NonPublic | BindingFlags.Instance);
        cellPrefabField.SetValue(gameField, cellPrefab);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameFieldObject);
    }

    [Test]
    public void GetEmptyPosition_ShouldReturnValidPosition_OnEmptyField()
    {
        // При пустом поле всегда есть свободное место
        Vector2Int pos = gameField.GetEmptyPosition();
        Assert.IsTrue(pos.x >= 0 && pos.y >= 0, "Позиция должна быть в пределах поля");
    }
    

    [Test]
    public void RecalculateScore_ShouldSumValuesCorrectly()
    {
        // Arrange: очистим список клеток и добавим тестовые клетки
        FieldInfo cellsField = typeof(GameField).GetField("cells", BindingFlags.NonPublic | BindingFlags.Instance);
        List<Cell> cellsList = cellsField.GetValue(gameField) as List<Cell>;
        cellsList.Clear();
        // Создаем клетку со значением 1: отображаемое значение 2^1 = 2
        cellsList.Add(new Cell(new Vector2Int(0, 0), 1));
        // Создаем клетку со значением 2: отображаемое значение 2^2 = 4
        cellsList.Add(new Cell(new Vector2Int(0, 1), 2));

        // Act
        gameField.RecalculateScore();

        // Assert: получаем текст из scoreText
        FieldInfo scoreTextField = typeof(GameField).GetField("scoreText", BindingFlags.NonPublic | BindingFlags.Instance);
        TextMeshProUGUI scoreText = scoreTextField.GetValue(gameField) as TextMeshProUGUI;
        int displayedScore = int.Parse(scoreText.text.Replace("Score: ", ""));
        // Ожидаем 2 + 4 = 6
        Assert.AreEqual(6, displayedScore, "Счет рассчитан неверно");
    }

    [Test]
    public void IsGameOver_ShouldReturnTrue_When_NoMovesAvailable()
    {
        // Arrange: заполним поле клетками уникальными значениями, чтобы не было ни пустых мест, ни возможных слияний
        FieldInfo cellsField = typeof(GameField).GetField("cells", BindingFlags.NonPublic | BindingFlags.Instance);
        List<Cell> cellsList = cellsField.GetValue(gameField) as List<Cell>;
        cellsList.Clear();

        // Заполняем поле (4x4) клетками с уникальными значениями
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                cellsList.Add(new Cell(new Vector2Int(x, y), (x * 4 + y) + 1));
            }
        }
        // Получаем приватный метод IsGameOver через Reflection
        MethodInfo isGameOverMethod = typeof(GameField).GetMethod("IsGameOver", BindingFlags.NonPublic | BindingFlags.Instance);
        bool result = (bool)isGameOverMethod.Invoke(gameField, null);

        // Assert
        Assert.IsTrue(result, "Метод IsGameOver должен вернуть true, если ходов нет");
    }
}
