using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LocalizeText : MonoBehaviour
{
    [SerializeField] private string _key;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        LocalizationManager.I.OnLocaleChanged += OnLocaleChanged;
        Localize();
    }

    private void OnLocaleChanged()
    {
        Localize();
    }

    private void Localize()
    {
        _text.text = LocalizationManager.I.Localize(_key);
    }
}
