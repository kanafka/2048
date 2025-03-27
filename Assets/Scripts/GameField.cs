using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class GameField : MonoBehaviour
{
    [SerializeField] private int fieldSize = 4; // Размер поля (4x4)
    [SerializeField] private GameObject cellPrefab; // Префаб клетки
    [SerializeField] private RectTransform GameBoard;

    private List<Cell> cells = new List<Cell>();

    // Найти пустую позицию
    public Vector2Int GetEmptyPosition()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();

        // Проверяем все возможные позиции
        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!cells.Exists(c => c.Position == pos))
                    emptyPositions.Add(pos);
            }
        }

        // Возвращаем случайную пустую позицию
        if (emptyPositions.Count == 0)
            return new Vector2Int(-1, -1); // Нет свободных мест

        return emptyPositions[Random.Range(0, emptyPositions.Count)];
    }

    // Создать новую клетку
    public void CreateCell()
    {
        Vector2Int emptyPos = GetEmptyPosition();
        if (emptyPos.x == -1) return; // Мест нет

        // Определяем значение: 1 (90%) или 2 (10%)
        int value = Random.Range(0, 100) < 90 ? 1 : 2;

        // Создаем объект Cell
        Cell newCell = new Cell(emptyPos, value);
        cells.Add(newCell);

        // Создаем префаб клетки на сцене
        GameObject cellObj = Instantiate(cellPrefab, GameBoard);
        cellObj.GetComponent<CellView>().Init(newCell);

        // Позиционируем (пример для 2D)
        Debug.Log(emptyPos);
        newCell.SetPosition(new Vector2(emptyPos.x, emptyPos.y));
    }

    void Start()
    {
        // Пример: создаем две клетки при старте
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