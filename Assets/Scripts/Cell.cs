using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Cell
{

    public Vector2 Position { get; private set; }

    public int Value { get; private set; }


    public UnityEvent<Cell> OnValueChanged = new UnityEvent<Cell>();
    public UnityEvent<Cell> OnPositionChanged = new UnityEvent<Cell>();


    public Cell(Vector2Int position, int value)
    {
        Position = position;
        Value = value;
    }

    public void SetValue(int newValue)
    {
        Value = newValue;
        OnValueChanged.Invoke(this);
    }

    public void SetPosition(Vector2 newPosition)
    {
        Position = newPosition;
        OnPositionChanged.Invoke(this);
    }
}