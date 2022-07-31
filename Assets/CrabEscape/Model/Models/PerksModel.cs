using System;

public class PerksModel : IDisposable
{
    private readonly PlayerData _playerData;

    public PerksModel(PlayerData playerdata)
    {
        _playerData = playerdata;
    }
    
    public void Dispose()
    {
    }
}