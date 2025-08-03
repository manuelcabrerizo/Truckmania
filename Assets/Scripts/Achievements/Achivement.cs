using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Achivement
{
    [field: SerializeField] public AchivementType Type { get; private set; }
    [field:SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Desc { get; private set; }
    [field: SerializeField] public Material Image { get; private set; }

    public bool IsUnlock { get; private set; }

    public Achivement(string name, string desc, Material image, bool isUnlock)
    {
        Name = name;
        Desc = desc;
        Image = image;
        IsUnlock = isUnlock;
    }

    public void Unlock()
    {
        IsUnlock = true;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(Name, IsUnlock ? 1 : 0);
    }
}
