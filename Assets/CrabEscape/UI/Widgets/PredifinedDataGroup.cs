using System;
using System.Collections.Generic;
using UnityEngine;

public class PredifinedDataGroup<TDataType, TItemType> : DataGroup<TDataType, TItemType>
    where TItemType : MonoBehaviour, IItemRenderer<TDataType>
{
    public PredifinedDataGroup(Transform container) : base(null, container)
    {
        var items = container.GetComponentsInChildren<TItemType>();
        _createdItem.AddRange(items);
    }

    public override void SetData(IList<TDataType> data)
    {
        if(data.Count > _createdItem.Count)
        {
            throw new IndexOutOfRangeException();
        }
        base.SetData(data);
    }
}