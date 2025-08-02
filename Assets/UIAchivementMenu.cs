using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAchivementMenu : MonoBehaviour
{
    [SerializeField] private AchievementSystem achivementSystem;
    [SerializeField] private GameObject grid;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject prefab;

    private List<GameObject> achivements = new List<GameObject>();

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(OnBackButtonClick);
    }

    private void Start()
    {
        ShowUpdateAchivements();
    }

    private void OnEnable()
    {
        ShowUpdateAchivements();
        GameEventManager.Instance.AddListener<JoystickOrKeyboardUseEvent>(OnJoystickAndKeyboardUse);
        OnJoystickAndKeyboardUse(null);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<JoystickOrKeyboardUseEvent>(OnJoystickAndKeyboardUse);
    }

    private void ShowUpdateAchivements()
    {
        RemoveOldAchivements();
        foreach (Achivement achivement in achivementSystem.Achivements.Values)
        {
            GameObject achivementView = Instantiate(prefab, grid.transform);
            UIAchivement ui = achivementView.GetComponent<UIAchivement>();
            ui.SetAchivement(achivement);
            achivements.Add(achivementView);
        }
    }

    private void RemoveOldAchivements()
    {
        foreach (GameObject go in achivements)
        {
            Destroy(go);
        }
        achivements.Clear();

    }

    private void OnBackButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(AchivementBackButtonClickEvent.GetEvent());
    }

    private void OnJoystickAndKeyboardUse(GameEvent gameEvent)
    {
        EventSystem.current.firstSelectedGameObject = backButton.gameObject;
        backButton.Select();
    }

}
