using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void StartPlayText(string text, float duration, Action callback = null)
    {
        StartCoroutine(PlayText(text, duration, callback));
    }

    private IEnumerator PlayText(string text, float duration, Action callback = null)
    {
        _text.text = "";
        var perCharacterDuration = duration / text.Length;
        var waitForPrint = new WaitForSeconds(perCharacterDuration);
        
        foreach (var c in text)
        {
            _text.text += c;
            yield return waitForPrint;
        }
        
        callback?.Invoke();
    }

    public void SetText(string text)
    {
        StopAllCoroutines();
        _text.text = text;
    }
}