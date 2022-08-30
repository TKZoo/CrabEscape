using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternComponent : MonoBehaviour
{
    public bool lanternOn = false;
    private Light2D _lightSource;
    private GameSession _session;
    private float _defaultLanternIntesity;
    private int LanternFuel => _session.PlayerData.Inventory.Count("Lantern");

    private bool outOfFuel = false;
    
    private void Awake()
    {
        _lightSource = GetComponent<Light2D>();
        _session = FindObjectOfType<GameSession>();
        _defaultLanternIntesity = _lightSource.intensity;
    }

    private void Start()
    {
        if (lanternOn)
        {
            UseLantern();
        }
        else
        {
            TurnOffLantern();
        }
    }

    public void UseLantern()
    {
        gameObject.SetActive(true);
        StartCoroutine(LanternUse());
    }

    public void TurnOffLantern()
    {
        gameObject.SetActive(false);
        lanternOn = false;
        StopCoroutine(LanternUse());
    }
    
    private IEnumerator LanternUse()
    {
        lanternOn = true;
        for (int i = 2; i <= LanternFuel;)
        {
            
            yield return new WaitForSeconds(0.1f);
            if (LanternFuel <= 5)
            {
                outOfFuel = true;
            }
            if (LanternFuel > 5)
            {
                outOfFuel = false;
            }
            yield return new WaitForSeconds(1f);
            _session.PlayerData.Inventory.Remove("Lantern", 1);
        }

        TurnOffLantern();
    }

    private void Update()
    {
        if (outOfFuel)
        {
            var time = Time.deltaTime / 5;
            var lightIntensity = LanternFuel * _lightSource.intensity;
            _lightSource.intensity -= lightIntensity * time;
        }
        else
        {
            _lightSource.intensity = _defaultLanternIntesity;
        }
    }
}