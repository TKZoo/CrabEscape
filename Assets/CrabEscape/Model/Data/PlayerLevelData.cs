using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerLevelData
{
    [SerializeField] private List<LevelProgress> _progress;

    public int GetLevel(statId id)
    {
        var progress = _progress.FirstOrDefault(x => x.Id == id);
        return progress?.Level ?? 0;
    }

    public void LevelUp(statId id)
    {
        var progress = _progress.FirstOrDefault(x => x.Id == id);
        if (progress == null)
        {
            _progress.Add(new LevelProgress(id, 1));
        }
        else 
        {
            progress.Level++;
        }
    }
}

[Serializable]
public class LevelProgress
{
    public statId Id;
    public int Level;

    public LevelProgress(statId id, int level)
    {
        Id = id;
        Level = level;
    }
}