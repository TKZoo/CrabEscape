using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]

public class SpriteAnimationComponent : MonoBehaviour
{
    [SerializeField] private int _frameRate;      
    [SerializeField] private AnimationClips[] _clips;

    private SpriteRenderer _renderer;
    private float _secondsPerFrame;
    private int _currentSpriteIndex;
    private float _nextFrameTime;
    private bool _isPlaying = true;
    private int _currentClip;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _secondsPerFrame = 1f / _frameRate;
        _nextFrameTime = Time.time;
    }

    private void Update()
    {
        if(_isPlaying == false ||_nextFrameTime > Time.time)
        {
            return;
        }        
        if (_currentSpriteIndex >= _clips[_currentClip].Sprites.Length)
        {
            if (_clips[_currentClip].Loop)
            {
                _currentSpriteIndex = 0;
            }
            else
            {
                //_isPlaying = false;
                _clips[_currentClip].OnComplete?.Invoke();
                if (_clips[_currentClip].AllowNextClip)
                {
                    _currentSpriteIndex = 0;
                    _currentClip = (int)Mathf.Repeat(_currentClip + 1, _clips.Length);
                }
                return;
            }
        }
        _renderer.sprite = _clips[_currentClip].Sprites[_currentSpriteIndex];
        _nextFrameTime += _secondsPerFrame;
        _currentSpriteIndex++;        
    }

    public void SetClip(string clipName)
    {
        for(int i = 0; i < _clips.Length; i++)
        {
            if (clipName == _clips[i].ClipName)
            {
                _secondsPerFrame = 1f / _frameRate;
                _nextFrameTime = Time.time + _secondsPerFrame;
                _currentClip = i;
                _currentSpriteIndex = 0;
                _isPlaying = true;
            }          
        }        
    }

    [Serializable]
    public class AnimationClips
    {
        [SerializeField] private string _clipName;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _allowNextClip;
        [SerializeField] private UnityEvent _onComplete;

        public string ClipName => _clipName;
        public Sprite[] Sprites => _sprites;
        public bool Loop => _loop;
        public bool AllowNextClip => _allowNextClip;
        public UnityEvent OnComplete => _onComplete;
    }
}
