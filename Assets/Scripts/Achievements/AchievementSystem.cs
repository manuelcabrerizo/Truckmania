using System.Collections.Generic;
using UnityEngine;

public enum AchivementType
{ 
    // Level complete event should always be on top 
    LEVEL1_COMPLETE,
    LEVEL2_COMPLETE,
    LEVEL3_COMPLETE,

    BACKFLIP,
    DOUBLE_BACKFLIP,
    SUBMARINE,

    MONSTER_HUNTER,
    MONSTER_SLAYER,
    BROKE
}

public class AchievementSystem : MonoBehaviour
{
    [SerializeField] private AchivementSO data;
    private Dictionary<AchivementType, Achivement> achivements = new Dictionary<AchivementType, Achivement>();
    public Dictionary<AchivementType, Achivement> Achivements => achivements;

    private void Awake()
    {
        foreach (Achivement achivement in data.achivements)
        {
            bool unlock = false;
            if (PlayerPrefs.HasKey(achivement.Name))
            {
                unlock = PlayerPrefs.GetInt(achivement.Name) == 1;
            }
            PlayerPrefs.SetInt(achivement.Name, unlock ? 1 : 0);
            achivements.Add(achivement.Type, new Achivement(achivement.Name, achivement.Desc, achivement.Image, unlock));
        }
        GameEventManager.Instance.AddListener<UnlockAchivementEvent>(Unlock);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<UnlockAchivementEvent>(Unlock);
        foreach (Achivement achivement in achivements.Values)
        {
            achivement.Save();
        }
    }

    public void Unlock(GameEvent gameEvent)
    {
        UnlockAchivementEvent e = (UnlockAchivementEvent)gameEvent;
        if (achivements.ContainsKey(e.type))
        { 
            Achivement achivement = achivements[e.type];
            if (!achivement.IsUnlock)
            {
                achivement.Unlock();
                GameEventManager.Instance.TriggerEvent(ShowAchivementUnlockUIEvent.GetEvent(achivement));
            }
        }
    }
}
