using System;
using UnityEngine;
using UnityEngine.UI;

public class UsedPerkWidget : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _cooldownFiller;

    private GameSession _session;
    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
    }

    public void Set(PerkDef perkDef)
    {
        _icon.sprite = perkDef.Icon;
    }

    private void Update()
    {
        var cooldown = _session.PerksModel.Cooldown;
        _cooldownFiller.fillAmount = cooldown.RemainingTime / cooldown.Value;
    }
}