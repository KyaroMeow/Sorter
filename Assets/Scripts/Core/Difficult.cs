using UnityEngine;

[CreateAssetMenu(fileName = "Difficult", menuName = "Scriptable Objects/Difficult")]
public class Difficult : ScriptableObject
{
    [Header("Название")]
    public string difficultyName = "NORMAL";

    [Header("Время и лимиты")]
    public float timePerItem = 60f;      // Время на товар
    public int maxMistakes = 10;         // Макс ошибок

    [Header("Тайминги аномалий")]
    public int anomalyItemNum = 20;      // Какой в очереди куб
    public int bombNum = 40;            // Какая в очереди бомба

    [Header("Шансы дефектов (0-1)")]
    [Range(0f, 1f)] public float noBarcodeChance = 0.1f;     // Нет штрихкода
    [Range(0f, 1f)] public float wrongBarcodeChance = 0.1f;  // Неправильный штрихкод
    [Range(0f, 1f)] public float defectChance = 0.1f;        // Кляксы
    [Range(0f, 1f)] public float scratchesChance = 0.1f;     // Царапины

    [TextArea(2, 4)]
    public string description;
}
