using System;
using UnityEngine;

public class AudioSettingsComponent : MonoBehaviour
{
    [SerializeField] private SoundSettings _mode;
    [SerializeField] private AudioSource _source;
    private FloatPersistentProperty _model;
    
    private void Start()
    {
        _model = FindProperty();
        _model.OnChanged += OnSoundSettingsChanged;
        OnSoundSettingsChanged(_model.Value, _model.Value);
    }

    private void OnSoundSettingsChanged(float newvalue, float oldvalue)
    {
        _source.volume = newvalue;
    }

    private FloatPersistentProperty FindProperty()
    {
        switch (_mode)
        {
            case SoundSettings.Music:
                return GameSettings.I.Music;
            case SoundSettings.Sfx:
                return GameSettings.I.Sfx;
        }

        throw new AggregateException("Undefined mode");
    }

    private void OnDestroy()
    {
        _model.OnChanged -= OnSoundSettingsChanged;
    }
}
