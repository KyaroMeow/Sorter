using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

[System.Serializable]
public class CutsceneStep
{
    public Sprite image;
    public AudioClip audioClip;
    [TextArea]
    public string subtitle;
    public float autoAdvanceDuration;
    public float subtitleDuration;
    public float fadeDuration = 0.5f;
}

public class Cutscene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image cutsceneImage;
    [SerializeField] private Image fadeBlackImage;
    [SerializeField] private DialogText dialogText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private List<CutsceneStep> cutsceneSteps;
    
    private bool _advance;

    public void Play(Action onComplete = null)
    {
        cutsceneImage.raycastTarget = true;
        StartCoroutine(PlayCutscene(onComplete));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _advance = true;
    }

    private IEnumerator PlayCutscene(Action onComplete = null)
    {
        foreach (var step in cutsceneSteps)
        {

            if (step.image)
            {
                UnHideImages();
                cutsceneImage.sprite = step.image;
                yield return fadeBlackImage.DOFade(0f, step.fadeDuration).WaitForCompletion();
            }
            else
                HideImages();

            if (step.audioClip)
                 audioSource.PlayOneShot(step.audioClip);
            
            if (step.subtitle != null)
                yield return HandleSubtitle(step);

            yield return WaitForAdvance(step.autoAdvanceDuration);
            
            if (step.image) 
                yield return fadeBlackImage.DOFade(1f, step.fadeDuration).WaitForCompletion();
            
            dialogText.SetText("");
            audioSource.Stop();
            _advance = false;
        }
        
        onComplete?.Invoke();
        cutsceneImage.enabled = false;
        
        yield return fadeBlackImage.DOFade(0f, 1f).WaitForCompletion();
        cutsceneImage.raycastTarget = false;
    }

    private void ShowChoice()
    {
        choicePanel.SetActive(true);
    }

    private void HideImages()
    {
        fadeBlackImage.color = new Color(1, 1, 1, 0);
        cutsceneImage.color = new Color(1, 1, 1, 0);
    }
    
    private void UnHideImages()
    {
        fadeBlackImage.color = new Color(0, 0, 0, 1);
        cutsceneImage.color = new Color(1, 1, 1, 1);
    }

    private IEnumerator HandleSubtitle(CutsceneStep step)
    {
        dialogText.StartPlayText(step.subtitle, step.subtitleDuration, () => { _advance = true; });
        yield return WaitForAdvance();
        dialogText.SetText(step.subtitle);
        _advance = false;
    }

    private IEnumerator WaitForAdvance(float autoAdvanceDuration = 0f)
    {
        while (!_advance)
            if (autoAdvanceDuration != 0f)
            {
                yield return new WaitForSeconds(autoAdvanceDuration);
                _advance = true;
            }
            else 
                yield return null;
    }
}