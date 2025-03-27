using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class GameField : MonoBehaviour
{
    [SerializeField] private int fieldSize = 4; // Размер поля (4x4)
    [SerializeField] private GameObject cellPrefab; // Префаб клетки
    [SerializeField] private RectTransform GameBoard;

    private List<Cell> cells = new List<Cell>();

    public Vector2Int GetEmptyPosition()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();

        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!cells.Exists(c => c.Position == pos))
                    emptyPositions.Add(pos);
            }
        }
        if (emptyPositions.Count == 0)
            return new Vector2Int(-1, -1);

        return emptyPositions[Random.Range(0, emptyPositions.Count)];
    }

    public void CreateCell()
    {
        Vector2Int emptyPos = GetEmptyPosition();
        if (emptyPos.x == -1) return;

        int value = Random.Range(0, 100) < 90 ? 1 : 2;

        Cell newCell = new Cell(emptyPos, value);
        cells.Add(newCell);

        GameObject cellObj = Instantiate(cellPrefab, GameBoard);
        cellObj.GetComponent<CellView>().Init(newCell);

        Debug.Log(emptyPos);
        newCell.SetPosition(new Vector2(emptyPos.x, emptyPos.y));
    }

    void Start()
    {
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
        CreateCell();
    }
}