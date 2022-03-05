using System;
using UnityEngine.Events;

public static class UnityEventExtensions
{
    static public IDisposable Subscribe(this UnityEvent unityEvent, UnityAction call)
    {
        unityEvent.AddListener(call);
        return new ActionDisposable(() => unityEvent.RemoveListener(call));
    }
    
    static public IDisposable Subscribe<TType>(this UnityEvent<TType> unityEvent, UnityAction<TType> call)
    {
        unityEvent.AddListener(call);
        return new ActionDisposable(() => unityEvent.RemoveListener(call));
    }
}
