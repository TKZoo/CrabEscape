using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagePerksWindow : AnimatedWindow
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _useButton;
    [SerializeField] private ItemWidget _price;
    [SerializeField] private TMP_Text _info;
    [SerializeField] private Transform _perksContainer;

    private PredifinedDataGroup<string, PerkWidget> _dataGroup;
    private CompositeDisposable _trash = new CompositeDisposable();

    protected override void Start()
    {
        _dataGroup = new PredifinedDataGroup<string, PerkWidget>(_perksContainer);
    }
}
