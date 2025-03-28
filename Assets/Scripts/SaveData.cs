using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<CellState> cellStates;
    public int bestScore;
}

[Serializable]
public struct CellState
{
    public int x;
    public int y;
    public int value;

    public CellState(int x, int y, int value)
    {
        this.x = x;
        this.y = y;
        this.value = value;
    }
}