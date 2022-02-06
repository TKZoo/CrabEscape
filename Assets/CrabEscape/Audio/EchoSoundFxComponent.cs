using UnityEngine;
using UnityEngine.Audio;

public class EchoSoundFxComponent : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot _echoOff;
    [SerializeField] private AudioMixerSnapshot _echoOn;
    [SerializeField] private float _transitionTime;

    public void SetEchoSFXOn()
    {
        _echoOn.TransitionTo(_transitionTime);
    }
    
    public void SetEchoSFXOff()
    {
        _echoOff.TransitionTo(_transitionTime);
    }

}
