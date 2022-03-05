public abstract class PrefsPersistentProperty<TPropertyType> : PersistantProperty<TPropertyType>
{
    protected string Key;
    
    protected PrefsPersistentProperty(TPropertyType defValue, string key) : base(defValue)
    {
        Key = key;
    }

}
