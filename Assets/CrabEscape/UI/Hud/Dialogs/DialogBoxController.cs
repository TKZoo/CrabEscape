using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _container;
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _charImage;

    [Space] [SerializeField] private float _textSpeed = 0.09f;

    [Header("Sounds")] [SerializeField] private AudioClip _typingSfx;
    [SerializeField] private AudioClip _openSfx;
    [SerializeField] private AudioClip _closeSfx;

    private DialogData _dialogData;
    private int _currentSentence;
    private AudioSource _sfxSource;
    private Coroutine _typingRoutine;
    
    public bool _isDialogActive = false;

    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    private void Start()
    {
        _sfxSource = AudioUtils.FindSfxSource();
    }

    public void ShowDialog(DialogData dialogData)
    {
        _dialogData = dialogData;
        _currentSentence = 0;
        _text.text = string.Empty;

        _container.SetActive(true);
        _sfxSource.PlayOneShot(_openSfx);
        _animator.SetBool(IsOpen, true);
    }

    private IEnumerator TypeDialogText()
    {   
        _text.text = string.Empty;
        var sentence = _dialogData.Sentences[_currentSentence];
        foreach (var letter in sentence)
        {
            _text.text += letter;
            _sfxSource.PlayOneShot(_typingSfx);
            yield return new WaitForSeconds(_textSpeed);
        }

        _typingRoutine = null;
    }

    public void OnSkip()
    {
        if(_typingRoutine == null) return;

        StopTypeAnimation();
        _text.text = _dialogData.Sentences[_currentSentence];
    }

    private void StopTypeAnimation()
    {
        if (_typingRoutine != null)
        {
            StopCoroutine(_typingRoutine);
            _typingRoutine = null;
        }
    }

    public void OnContinue()
    {
        StopTypeAnimation();
        _currentSentence++;

        var isDialogCompleted = _currentSentence >= _dialogData.Sentences.Length;
        if (isDialogCompleted)
        {
            HideDialogWindow();
        }
        else
        {
            OnDialogWindowAppear();
        }
    }

    private void HideDialogWindow()
    {
        _isDialogActive = false;
        _animator.SetBool(IsOpen, false);
        _sfxSource.PlayOneShot(_closeSfx);
    }

    private void OnDialogWindowAppear()
    {
        _isDialogActive = true;
        _charImage.sprite = _dialogData.NarratorSprite[_currentSentence];
        _charImage.SetNativeSize();
        _typingRoutine = StartCoroutine(TypeDialogText());
    }

    private void OnDialogWindowClosed()
    {
    }
}