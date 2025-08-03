using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAchivement : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI tile;
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private GameObject off;

    public void SetAchivement(Achivement achivement)
    {
        image.material = achivement.Image;
        tile.text = achivement.Name;
        desc.text = achivement.Desc;
        if (off != null)
        {
            if (achivement.IsUnlock)
            { 
                off.SetActive(false);
            }
        }
    }
}
