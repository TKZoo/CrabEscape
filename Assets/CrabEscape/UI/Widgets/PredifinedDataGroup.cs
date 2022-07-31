using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PredifinedDataGroup<TDataType, TItemType> : DataGroup<TDataType, TItemType>
    where TItemType : MonoBehaviour, IItemRenderer<TDataType>
{
    public PredifinedDataGroup(Transform container) : base(null, container)
    {
        var items = container.GetComponentsInChildren<TItemType>();
        CreatedItems.AddRange(items);
    }

    public override void SetData(IList<TDataType> data)
    {
        if(data.Count > CreatedItems.Count)
        {
            throw new IndexOutOfRangeException();
        }
        base.SetData(data);
    }
}