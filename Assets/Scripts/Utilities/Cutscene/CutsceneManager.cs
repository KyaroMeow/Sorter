using System;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    
    [SerializeField] private Cutscene startCutscene;
    [SerializeField] private Cutscene beglecCutscene;
    [SerializeField] private Cutscene looseCutscene;
    
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

    public void PlayStartCutscene(Action callback = null)
    {
        startCutscene.Play(callback);
    }

    public void PlayBeglecCutscene(Action callback = null)
    {
        beglecCutscene.Play(callback);
    }
    
    public void PlayLooseCutscene(Action callback = null)
    {
        looseCutscene.Play(callback);
    }

}