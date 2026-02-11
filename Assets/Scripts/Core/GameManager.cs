using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Current Game State")] public GameObject currentItem;
    public int currentMistakes = 0;
    public int totalItemsProcessed = 0;
    public float currentTime = 0;
    public bool isGameStarted = false;
    public bool isTimerWork = false;

    public ItemSpawner itemSpawner;
    public Hands hands;
    public ScanUI scanUI;
    public GameObject scaner;
    public GameObject scanerOnTable;
    public Lights lights;
    public Slider volumeSlider;
    public AnomallyController anomallyController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AudioManager.Instance.Play("DroneSound");
        CutsceneManager.Instance.PlayStartCutscene(() =>
        {
            AudioManager.Instance.Play("Wake up");
            AudioManager.Instance.Stop("DroneSound");
            AudioManager.Instance.Play("Conveyor");
        });
    }

    void Update()
    {
        if (isGameStarted)
        {
            UpdateTimer();
        }
    }

    public void StartTimer()
    {
        isTimerWork = true;
    }

    public void StartGame()
    {
        isGameStarted = true;
        isTimerWork = true;
        SpawnItem();
    }

    public void StartAnomally()
    {
        anomallyController.StartAnomally();
    }

    private void UpdateTimer()
    {
        if (isTimerWork && SettingManager.Instance.timer)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                WrongSort();
            }
        }
    }

    public void SetVolume()
    {
        SettingManager.Instance.volumeValue = volumeSlider.value;
    }

    public void ToggleScaner()
    {
        scaner.SetActive(!scaner.activeSelf);
        scanerOnTable.SetActive(!scaner.activeSelf);
    }

    public void ToggleScanerOff()
    {
        scaner.SetActive(false);
        scanerOnTable.SetActive(true);
    }

    public void SortItem(bool selectedVariant)
    {
        if (currentItem == null) return;
        hands.PlayPressButton();
        bool itemVariant = currentItem.GetComponent<Item>().isDefective;
        if (selectedVariant == itemVariant)
        {
            CorrectSort();
        }
        else
        {
            WrongSort();
        }
    }

    public void ShowScanResult()
    {
        scanUI.ShowResult(currentItem.GetComponent<Item>().barcodeShowsGood);
    }

    public void CorrectSort()
    {
        AudioManager.Instance.Play("CorrectSort");
        lights.ChangeColorGreen();
        totalItemsProcessed++;
        if (currentItem != null) Destroy(currentItem);
        StartTimer();
        SpawnItem();
    }

    public void WrongSort()
    {
        AudioManager.Instance.Play("IncorrectSort");
        lights.ChangeColorRed();
        totalItemsProcessed++;
        currentMistakes++;
        CheckForDamage();
        if (currentMistakes > SettingManager.Instance.currentDifficulty.maxMistakes)
        {
            GameOver();
        }

        Destroy(currentItem);
        SpawnItem();
    }

    private void CheckForDamage()
    {
        int mistakesPerDamage = GetMistakesPerDamage();
        if (currentMistakes % mistakesPerDamage == 0)
        {
            hands.PlayTakeDamage();
        }
    }

    private int GetMistakesPerDamage()
    {
        switch (SettingManager.Instance.currentDifficulty.difficultyName)
        {
            case "EASY":
                return 3;
            case "NORMAL":
                return 2;
            case "HARD":
                return 1;
            default:
                return 3;
        }
    }

    public void BadEnd()
    {
        CutsceneManager.Instance.PlayLooseCutscene(() => SceneManager.LoadScene(0));
    }

    public void SpawnItem()
    {
        if (totalItemsProcessed == SettingManager.Instance.currentDifficulty.anomalyItemNum)
        {
            itemSpawner.SpawnAnomalyItem();
        }
        else if (totalItemsProcessed == SettingManager.Instance.currentDifficulty.bombNum)
        {
            itemSpawner.SpawnBomb();
        }
        else
        {
            itemSpawner.SpawnItem();
            currentTime = SettingManager.Instance.currentDifficulty.timePerItem;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}