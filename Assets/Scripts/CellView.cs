using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CellView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image img;

    private Cell linkedCell;

    // Инициализация
    public void Init(Cell cell)
    {
        linkedCell = cell;
        UpdateValue(cell);
        UpdatePosition(cell);
        // Подписываемся на события
        linkedCell.OnValueChanged.AddListener(UpdateValue);
        linkedCell.OnPositionChanged.AddListener(UpdatePosition);
    }

    // Обновить значение
    private void UpdateValue(Cell cell)
    {
        // Используйте Mathf.Pow вместо Math.Pow
        int displayValue = (int)Mathf.Pow(2, cell.Value); // 2^1 = 2, 2^2 = 4
        valueText.text = displayValue.ToString();

        // Если текст всё равно не виден, добавьте лог:
        Debug.Log($"Text updated: {displayValue}");
        float t = Mathf.Clamp01((cell.Value - 1) / 10f); // Нормализация в диапазон 0-1
    
        // Задаем цвета
        Color startColor = Color.white;
        Color endColor = new Color(139f/255f, 69f/255f, 19f/255f); // Коричневый
        img.color = Color.Lerp(startColor, endColor, t);
    }

    // Обновить позицию
    private void UpdatePosition(Cell cell)
    {
        // Для сетки 4x4 с ячейками размером 1 единицу
        Debug.Log(cell.Position.ToString() + "apsdpaspdpaspd");
        transform.localPosition = new Vector2(
            cell.Position.x  * 200 + 15, // Центрирование для поля 4x4
            cell.Position.y  * 200 + 15
        );
    }
}