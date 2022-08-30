using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShakeEffect : MonoBehaviour
{
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _intensivity = 3f;

    private CinemachineBasicMultiChannelPerlin _cameraNoise;

    private Coroutine _coroutine;

    private void Awake()
    {
        var virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        if (_coroutine != null)
        {
            StopAnimation();
        }
        StartCoroutine(StartShakeAnim());
    }
    
    private IEnumerator StartShakeAnim()
    {
        _cameraNoise.m_FrequencyGain = _intensivity;
        yield return new WaitForSeconds(_shakeDuration);
        StopAnimation();
    }

    private void StopAnimation()
    {
        _cameraNoise.m_FrequencyGain = 0;
        StopCoroutine(StartShakeAnim());
        _coroutine = null;
    }
}
