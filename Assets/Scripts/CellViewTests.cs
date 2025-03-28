using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CellViewTests
{
    private GameObject cellViewObj;
    private CellView cellView;
    private GameObject textObj;
    private GameObject imageObj;

    [SetUp]
    public void SetUp()
    {
        // Создаем объект с компонентом CellView
        cellViewObj = new GameObject("CellViewTest");
        cellView = cellViewObj.AddComponent<CellView>();

        // Создаем объект для TextMeshProUGUI и добавляем его
        textObj = new GameObject("ValueText");
        textObj.transform.SetParent(cellViewObj.transform);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        // Через SerializeField поле можно назначить через инспектор, эмулируем это:
        typeof(CellView)
            .GetField("valueText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(cellView, tmp);

        // Создаем объект для Image и добавляем его
        imageObj = new GameObject("Image");
        imageObj.transform.SetParent(cellViewObj.transform);
        Image img = imageObj.AddComponent<Image>();
        typeof(CellView)
            .GetField("img", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(cellView, img);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(cellViewObj);
    }

    [Test]
    public void Init_Should_UpdateValueTextAndColor()
    {
        // Arrange: создаем клетку с определенным значением (например, 3)
        Cell cell = new Cell(new Vector2Int(0, 0), 3);
        // Act: инициализируем CellView
        cellView.Init(cell);
        // Перед обновлением значение уже обновляется при вызове Init
        // Ожидаем, что отображаемое значение = Mathf.Pow(2, cell.Value) = 2^3 = 8
        TextMeshProUGUI tmp = (TextMeshProUGUI)typeof(CellView)
            .GetField("valueText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(cellView);
        Assert.AreEqual("8", tmp.text, "Значение текстового поля должно быть '8'");

        // Также проверяем, что Image.color вычислен через Lerp.
        // Для cell.Value = 3, t = (3 - 1) / 10 = 0.2
        Image img = (Image)typeof(CellView)
            .GetField("img", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(cellView);
        Color expectedColor = Color.Lerp(Color.white, new Color(139f/255f, 69f/255f, 19f/255f), 0.2f);
        Assert.AreEqual(expectedColor, img.color, "Цвет должен быть вычислен через Lerp");
    }

    [Test]
    public void UpdatePosition_Should_SetCorrectLocalPosition()
    {
        // Arrange: создаем клетку с позицией (1,2)
        Cell cell = new Cell(new Vector2Int(0, 0), 1);
        cellView.Init(cell);
        // Act: изменяем позицию клетки
        Vector2 newPos = new Vector2(1, 2);
        cell.SetPosition(newPos);
        // Ожидаем, что позиция объекта обновилась согласно формуле:
        // localPosition.x = cell.Position.x * 200 + 15, localPosition.y = cell.Position.y * 200 + 15
        Vector3 expected = new Vector3(1 * 200 + 15, 2 * 200 + 15, 0);
        Assert.AreEqual(expected, cellViewObj.transform.localPosition, "Позиция должна обновляться согласно формуле");
    }
}
