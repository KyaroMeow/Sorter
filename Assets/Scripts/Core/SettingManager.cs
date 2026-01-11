using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;
    public float volumeValue;
    public float timePerItem;
    public int maxMistakes;
    public int anomalyItemNum;
    public int BombNum;
    public float noBarcodeChance;
    public float wrongBarcodeChance;
    public float defectChance;
    public float scratchesChance;
    public bool timer;
    public string currentDifficulty;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SetDifficult("NORMAL");
    }
    void Update()
    {
        AudioListener.volume = volumeValue;
    }
    public void SetDifficult(string diffName)
    {
        switch (diffName)
        {
            case "EASY":
                currentDifficulty = "EASY";
                scratchesChance = 0.2f;
                wrongBarcodeChance = 0.1f;
                defectChance = 0.1f;
                noBarcodeChance = 0.1f;
                anomalyItemNum = 10;
                BombNum = 20;
                timePerItem = 90f;
                maxMistakes = 15;
                break;
            case "NORMAL":
                currentDifficulty = "NORMAL";
                scratchesChance = 0.1f;
                wrongBarcodeChance = 0.1f;
                defectChance = 0.1f;
                noBarcodeChance = 0.1f;
                anomalyItemNum = 20;
                BombNum = 40;
                timePerItem = 60f;
                maxMistakes = 10;
                break;
            case "HARD":
                currentDifficulty = "HARD";
                scratchesChance = 0.3f;
                wrongBarcodeChance = 0.2f;
                defectChance = 0.1f;
                noBarcodeChance = 0.1f;
                anomalyItemNum = 30;
                BombNum = 50;
                timePerItem = 30f;
                maxMistakes = 5;
                break;
        }
    }

}
