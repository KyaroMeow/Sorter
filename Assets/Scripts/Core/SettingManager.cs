using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;
    [Header("Доступные сложности")]
    public List<Difficult> availableDifficulties;

    [Header("Текущая сложность")]
    public Difficult currentDifficulty;

    [Header("Настройки по умолчанию")]
    public string defaultDifficultyName = "NORMAL";

    [Header("Настройка звука")]
    public float volumeValue;

    [Header("Таймер")]
    public bool timer = true;

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
        SetDifficulty(defaultDifficultyName);
    }
    void Update()
    {
        AudioListener.volume = volumeValue;
    }
    public void SetDifficulty(string diffName)
    {
        var difficulty = availableDifficulties.Find(d =>
            d.difficultyName.ToUpper() == diffName.ToUpper());

        if (difficulty != null)
        {
            currentDifficulty = difficulty;
            Debug.Log($"Сложность изменена на: {currentDifficulty.difficultyName}");
        }
        else
        {
            Debug.LogWarning($"Сложность {diffName} не найдена");
        }
    }

}
