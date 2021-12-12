using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]

public class SpriteAnimationComponent : MonoBehaviour
{
    [SerializeField] private int _frameRate;      
    [SerializeField] private AnimationClips[] clips;

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
        _nextFrameTime = Time.time + _secondsPerFrame;
    }

    private void Update()
    {
        if(_isPlaying == false ||_nextFrameTime > Time.time)
        {
            return;
        }        
        if (_currentSpriteIndex >= clips[_currentClip].sprites.Length)
        {
            if (clips[_currentClip].loop)
            {
                _currentSpriteIndex = 0;
            }
            else
            {
                _isPlaying = false;
                clips[_currentClip].onComplete?.Invoke();
                if (clips[_currentClip].allowNextClip)
                {
                    _currentSpriteIndex = 0;
                    _currentClip = (int)Mathf.Repeat(_currentClip + 1, clips.Length);
                }
                return;
            }
        }
        _renderer.sprite = clips[_currentClip].sprites[_currentSpriteIndex];
        _nextFrameTime += _secondsPerFrame;
        _currentSpriteIndex++;        
    }

    public void SetClip(string clipName)
    {
        for(int i = 0; i < clips.Length; i++)
        {
            if (clipName == clips[i].clipName)
            {
                _currentClip = i;
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

        public string clipName => _clipName;
        public Sprite[] sprites => _sprites;
        public bool loop => _loop;
        public bool allowNextClip => _allowNextClip;
        public UnityEvent onComplete => _onComplete;
    }    
}
