using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameField : MonoBehaviour
{
    [SerializeField] private int fieldSize = 4; // Размер поля (4x4)
    [SerializeField] private GameObject cellPrefab; // Префаб клетки
    [SerializeField] private RectTransform GameBoard; // Родительский объект для клеток
    [SerializeField] private TMPro.TextMeshProUGUI scoreText; // UI для отображения счета
    [SerializeField] private TMPro.TextMeshProUGUI bestScoreText; // UI для отображения счета

    private List<Cell> cells = new List<Cell>();
    private int score = 0;
    private int bestScore = 0;

    private void OnEnable()
    {
        InputManager.OnMove += ProcessMove;
    }

    private void OnDisable()
    {
        InputManager.OnMove -= ProcessMove;
    }

    private void Start()
    {

        SaveData data = SaveManager.LoadGame();
        bestScore = data.bestScore;
        bestScoreText.text = bestScore.ToString();

        CreateCell();
        CreateCell();
        RecalculateScore();
    }

    private void OnApplicationQuit()
    {
        SaveProgress();
    }


    private void SaveProgress()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;

        data.cellStates = new List<CellState>();
        foreach (Cell cell in cells)
        {
            Vector2Int pos = Vector2Int.RoundToInt(cell.Position);
            data.cellStates.Add(new CellState(pos.x, pos.y, cell.Value));
        }
        SaveManager.SaveGame(data);
    }

    public Vector2Int GetEmptyPosition()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();

        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!cells.Exists(c => Vector2Int.RoundToInt(c.Position) == pos))
                    emptyPositions.Add(pos);
            }
        }
        if (emptyPositions.Count == 0)
            return new Vector2Int(-1, -1);

        return emptyPositions[Random.Range(0, emptyPositions.Count)];
    }

    public void CreateCell(int valueOverride = -1)
    {
        Vector2Int emptyPos = GetEmptyPosition();
        if (emptyPos.x == -1) return;


        int value = valueOverride != -1 ? valueOverride : (Random.Range(0, 100) < 90 ? 1 : 2);

        Cell newCell = new Cell(emptyPos, value);
        cells.Add(newCell);

        GameObject cellObj = Instantiate(cellPrefab, GameBoard);
        cellObj.GetComponent<CellView>().Init(newCell);


        newCell.SetPosition(new Vector2(emptyPos.x, emptyPos.y));
    }

    public void ProcessMove(Vector2Int direction)
    {
        bool moved = false;


        cells.Sort((a, b) =>
        {
            if (direction == Vector2Int.right)
                return b.Position.x.CompareTo(a.Position.x);
            if (direction == Vector2Int.left)
                return a.Position.x.CompareTo(b.Position.x);
            if (direction == Vector2Int.up)
                return b.Position.y.CompareTo(a.Position.y);
            if (direction == Vector2Int.down)
                return a.Position.y.CompareTo(b.Position.y);
            return 0;
        });


        foreach (var cell in new List<Cell>(cells))
        {
            Vector2Int currentPos = Vector2Int.RoundToInt(cell.Position);
            Vector2Int targetPos = currentPos;

            while (true)
            {
                Vector2Int nextPos = targetPos + direction;
                if (nextPos.x < 0 || nextPos.x >= fieldSize || nextPos.y < 0 || nextPos.y >= fieldSize)
                    break;

                Cell other = cells.Find(c => Vector2Int.RoundToInt(c.Position) == nextPos);

                if (other == null)
                {
                    targetPos = nextPos;
                    moved = true;
                }
                else if (other.Value == cell.Value)
                {
                    cell.SetValue(cell.Value + 1);
                    other.DestroyCell();
                    cells.Remove(other);
                    targetPos = nextPos;
                    moved = true;
                    break;
                }
                else
                {
                    break;
                }
            }
            if (targetPos != currentPos)
            {
                cell.SetPosition(targetPos);
            }
        }

        if (moved)
        {
            int newValue = (Random.Range(0, 100) < 20) ? 2 : 1;
            CreateCell(newValue);
            RecalculateScore();

            if (IsGameOver())
            {
                Debug.Log("Игра окончена!");

                if (score > bestScore)
                {
                    bestScore = score;
                    Debug.Log("Новый лучший результат: " + bestScore);
                }
                bestScoreText.text = bestScore.ToString();

                SaveProgress();

                ResetGame();
            }
        }
    }


    public void RecalculateScore()
    {
        score = 0;
        foreach (var cell in cells)
        {
            score += (int)Mathf.Pow(2, cell.Value);
        }
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        Debug.Log("Score: " + score);
    }


    private bool IsGameOver()
    {

        if (GetEmptyPosition().x != -1)
            return false;


        foreach (var cell in cells)
        {
            Vector2Int pos = Vector2Int.RoundToInt(cell.Position);
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (var dir in directions)
            {
                Vector2Int neighborPos = pos + dir;
                Cell neighbor = cells.Find(c => Vector2Int.RoundToInt(c.Position) == neighborPos);
                if (neighbor != null && neighbor.Value == cell.Value)
                    return false;
            }
        }
        return true;
    }


    private void ResetGame()
    {

        foreach (Transform child in GameBoard)
        {
            Destroy(child.gameObject);
        }
        cells.Clear();
        score = 0;
        RecalculateScore();

        CreateCell();
        CreateCell();
    }
}
