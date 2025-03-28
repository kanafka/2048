using NUnit.Framework;
using UnityEngine;

public class CellTests
{
    [Test]
    public void SetValue_ShouldTriggerOnValueChanged()
    {
        // Arrange
        Cell cell = new Cell(new Vector2Int(0, 0), 1);
        int updatedValue = 0;
        cell.OnValueChanged.AddListener((updatedCell) => updatedValue = updatedCell.Value);
        
        // Act
        cell.SetValue(2);
        
        // Assert
        Assert.AreEqual(2, updatedValue, "OnValueChanged событие не вызвалось или передало неверное значение");
    }

    [Test]
    public void SetPosition_ShouldTriggerOnPositionChanged()
    {
        // Arrange
        Cell cell = new Cell(new Vector2Int(0, 0), 1);
        Vector2 updatedPosition = Vector2.zero;
        cell.OnPositionChanged.AddListener((updatedCell) => updatedPosition = updatedCell.Position);
        Vector2 newPosition = new Vector2(1, 1);
        
        // Act
        cell.SetPosition(newPosition);
        
        // Assert
        Assert.AreEqual(newPosition, updatedPosition, "OnPositionChanged событие не вызвалось или передало неверное значение");
    }
}