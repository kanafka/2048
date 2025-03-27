using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Cell
{
    // Позиция на игровом поле (не координаты в Unity!)
    public Vector2 Position { get; private set; }
    // Значение клетки (1 или 2)
    public int Value { get; private set; }

    // События
    public UnityEvent<Cell> OnValueChanged = new UnityEvent<Cell>();
    public UnityEvent<Cell> OnPositionChanged = new UnityEvent<Cell>();

    // Конструктор
    public Cell(Vector2Int position, int value)
    {
        Position = position;
        Value = value;
    }

    // Изменение значения
    public void SetValue(int newValue)
    {
        Value = newValue;
        OnValueChanged.Invoke(this); // Вызов события
    }

    // Изменение позиции
    public void SetPosition(Vector2 newPosition)
    {
        Position = newPosition;
        OnPositionChanged.Invoke(this); // Вызов события
    }
}