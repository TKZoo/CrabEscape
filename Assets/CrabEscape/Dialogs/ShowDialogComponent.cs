using System;
using UnityEngine;

public class ShowDialogComponent : MonoBehaviour
{
    [SerializeField] private Mode _mode;
    [SerializeField] private DialogData _bound;
    [SerializeField] private DialogDef _external;

    private DialogBoxController _dialogBox;

    public void ShowDialog()
    {
        Debug.Log(_dialogBox);
        if (_dialogBox == null)
        {
            _dialogBox = FindObjectOfType<DialogBoxController>();
            _dialogBox.ShowDialog(Data);
        }
        else if (_dialogBox._isDialogActive && _dialogBox != null)
        {
            _dialogBox.OnSkip();
        }
        else
        {
            _dialogBox.ShowDialog(Data);
        }
    }

    public void ShowDialog(DialogDef def)
    {
        _external = def;
        ShowDialog();
    }
    
    public DialogData Data
    {
        get
        {
            switch (_mode)
            {
                case Mode.Bound:
                    return _bound;
                case Mode.External:
                    return _external.Data;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public enum Mode
    {
        Bound,
        External
    }
}
