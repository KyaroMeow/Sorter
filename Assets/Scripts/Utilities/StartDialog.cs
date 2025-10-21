using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StartDialog : MonoBehaviour
{
    [Header("UI REFERENCES")]
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    
    [Header("SETTINGS")]
    public float textSpeed = 0.05f;
    public float autoCloseDelay = 1f; // Задержка перед авто-закрытием
    
    [Header("EVENTS")]
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;
    
    private string[] currentDialogue;
    private int currentLine = 0;
    private bool isTyping = false;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;
    private Coroutine autoCloseCoroutine;

    void Update()
    {
        if (!isDialogueActive) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Пропуск анимации текста
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);
                    
                dialogueText.text = currentDialogue[currentLine];
                isTyping = false;
                
                // Отменяем предыдущий таймер авто-закрытия
                if (autoCloseCoroutine != null)
                    StopCoroutine(autoCloseCoroutine);
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    public void StartDialogue(string[] dialogueLines)
    {
        Debug.Log("Work");
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogWarning("Dialogue lines are empty!");
            return;
        }

        // Активируем канвас
        if (dialogueCanvas != null)
        {
            Debug.Log("setActive");
            dialogueCanvas.SetActive(true);
        }
        else
            Debug.LogError("Dialogue Canvas is not assigned!");

        currentDialogue = dialogueLines;
        currentLine = 0;
        isDialogueActive = true;

        // Вызываем событие начала диалога
        onDialogueStart?.Invoke();

        // Начинаем печатать текст
        typingCoroutine = StartCoroutine(TypeText(currentDialogue[currentLine]));
        
        Debug.Log("Dialogue started, canvas active: " + (dialogueCanvas != null && dialogueCanvas.activeInHierarchy));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
        
        // После завершения печати текста запускаем таймер авто-закрытия
        if (currentLine == currentDialogue.Length - 1)
        {
            // Это последняя строка - закрываем весь диалог через секунду
            autoCloseCoroutine = StartCoroutine(AutoCloseDialogue());
        }
    }

    

    private IEnumerator AutoCloseDialogue()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        EndDialogue();
    }

    private void ShowNextLine()
    {
        if (currentDialogue == null)
        {
            Debug.LogError("Current dialogue is null!");
            EndDialogue();
            return;
        }

        currentLine++;
        
        if (currentLine < currentDialogue.Length)
        {
            typingCoroutine = StartCoroutine(TypeText(currentDialogue[currentLine]));
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        // Останавливаем все корутины
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
            
        if (autoCloseCoroutine != null)
            StopCoroutine(autoCloseCoroutine);

        // Деактивируем канвас
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
            
        dialogueText.text = "";
        currentDialogue = null;
        currentLine = 0;
        isDialogueActive = false;
        isTyping = false;
        
        // Вызываем событие завершения диалога
        onDialogueEnd?.Invoke();
        
        Debug.Log("Dialogue ended");
    }

    // Метод для принудительного завершения диалога
    public void ForceEndDialogue()
    {
        if (isDialogueActive)
        {
            EndDialogue();
        }
    }

    // Публичные свойства для проверки состояния
    public bool IsDialogueActive => isDialogueActive;
    public bool IsTyping => isTyping;
}