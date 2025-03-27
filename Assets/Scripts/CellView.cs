using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CellView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image img;

    private Cell linkedCell;


    public void Init(Cell cell)
    {
        linkedCell = cell;
        UpdateValue(cell);
        UpdatePosition(cell);
        linkedCell.OnValueChanged.AddListener(UpdateValue);
        linkedCell.OnPositionChanged.AddListener(UpdatePosition);
    }

    private void UpdateValue(Cell cell)
    {
        int displayValue = (int)Mathf.Pow(2, cell.Value);
        valueText.text = displayValue.ToString();


        float t = Mathf.Clamp01((cell.Value - 1) / 10f);
    

        Color startColor = Color.white;
        Color endColor = new Color(139f/255f, 69f/255f, 19f/255f);
        img.color = Color.Lerp(startColor, endColor, t);
    }


    private void UpdatePosition(Cell cell)
    {


        transform.localPosition = new Vector2(
            cell.Position.x  * 200 + 15,
            cell.Position.y  * 200 + 15
        );
    }
}