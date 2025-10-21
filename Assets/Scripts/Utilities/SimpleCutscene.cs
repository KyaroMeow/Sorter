using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StartCutscene : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneSlide
    {
        public Sprite slide;
        public float fadeInDuration = 1f;
    }

    [Header("CUTSCENE SETTINGS")]
    public CutsceneSlide[] slides;
    [SerializeField] private string[] dialogue1;
    [SerializeField] private string[] dialogue2;
    public StartDialog startDialog;
    
    [Header("AUDIO SETTINGS")]
    public AudioClip firstAudioClip;
    public AudioClip secondAudioClip;
    public int firstSoundStartSlide = 0;
    public int secondSoundStartSlide = 0;
    
    public float fadeDuration = 1f;

    [Header("REFERENCES")]
    public Image displayImage;
    public AudioSource audioSource;

    [Header("ON CUTSCENE END")]
    public UnityEvent onCutsceneEnd;
    
    private int currentSlide = 0;
    private bool firstSoundPlayed = false;
    private bool secondSoundPlayed = false;
    private bool waitingForInput = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        StartCoroutine(PlayCutscene());
    }

    void Update()
    {
        // Обрабатываем нажатие только когда ждем ввода
        if (waitingForInput && Input.GetMouseButtonDown(0))
        {
            waitingForInput = false;
            ShowNextSlide();
        }
    }

    private IEnumerator PlayCutscene()
    {
        // Начинаем с черного экрана
        displayImage.color = Color.black;
        displayImage.sprite = null;

        // Показываем первый слайд
        yield return StartCoroutine(FadeFromBlackToSlide(slides[0].slide));
        
        // Проверяем звуки для первого слайда
        CheckAndPlaySounds();
        
        // Ждем нажатия игрока для продолжения
        waitingForInput = true;
    }

    private void ShowNextSlide()
    {
        currentSlide++;
        
        if (currentSlide < slides.Length)
        {
            StartCoroutine(ShowSlideWithTransition());
        }
        else
        {
            // Завершаем катсцену
            StartCoroutine(FinishCutscene());
        }
    }

    private IEnumerator ShowSlideWithTransition()
    {
        // Плавный переход к черному
        yield return StartCoroutine(FadeToBlack());
        
        // Плавный переход к следующему слайду
        yield return StartCoroutine(FadeFromBlackToSlide(slides[currentSlide].slide));
        
        // Проверяем звуки для текущего слайда
        CheckAndPlaySounds();
        
        // Снова ждем нажатия игрока
        waitingForInput = true;
    }

    // Плавный переход от черного к слайду
    private IEnumerator FadeFromBlackToSlide(Sprite slide)
    {
        // Устанавливаем новое изображение (пока черное)
        displayImage.sprite = slide;
        displayImage.color = Color.black;

        // Плавное появление слайда из черного
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            displayImage.color = Color.Lerp(Color.black, Color.white, progress);
            yield return null;
        }

        displayImage.color = Color.white;
    }

    // Плавный переход от слайда к черному
    private IEnumerator FadeToBlack()
    {
        float timer = 0f;
        Color startColor = displayImage.color;
        
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            displayImage.color = Color.Lerp(startColor, Color.black, progress);
            yield return null;
        }

        displayImage.color = Color.black;
    }

    // Плавный переход от черного к прозрачному (в самом конце)
    private IEnumerator FadeFromBlackToClear()
    {
        displayImage.color = Color.black;
        displayImage.sprite = null;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            displayImage.color = new Color(0, 0, 0, 1 - progress);
            yield return null;
        }

        displayImage.color = new Color(0, 0, 0, 0);
    }

    private void CheckAndPlaySounds()
    {
        // Проверяем первый звук
        if (!firstSoundPlayed && currentSlide >= firstSoundStartSlide)
        {
            PlayFirstSound();
            if (startDialog != null && dialogue1 != null && dialogue1.Length > 0)
            {
                startDialog.StartDialogue(dialogue1);
            }
            firstSoundPlayed = true;
        }
        
        // Проверяем второй звук
        if (!secondSoundPlayed && currentSlide >= secondSoundStartSlide)
        {
            PlaySecondSound();
            secondSoundPlayed = true;
        }
    }

    private void PlayFirstSound()
    {
        if (firstAudioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(firstAudioClip);
        }
    }

    private void PlaySecondSound()
    {
        if (secondAudioClip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(secondAudioClip);
        }
    }

    private IEnumerator FinishCutscene()
    {
        // Затемнение в конце
        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(0.5f);
        
        // Разтемнение к прозрачному
        yield return StartCoroutine(FadeFromBlackToClear());

        // Вызываем действие после катсцены
        onCutsceneEnd?.Invoke();
        startDialog.StartDialogue(dialogue2);
    }
}