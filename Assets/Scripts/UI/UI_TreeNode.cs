using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI _ui;
    private RectTransform _rect;
    
    [SerializeField] private Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockedColorHex = "#605E5E";
    private Color lastColor;
    public bool isUnlocked;
    public bool isLocked;

    private void Awake()
    {
        _ui = GetComponentInParent<UI>();
        _rect = GetComponent<RectTransform>();
        
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    private void UnLock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);

        // TODO: Find Player_SkillManager
            // Unlock skill on skill manager
            // skill manager unlock skill from skill data skill type
    }
    
    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        return true;
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            UnLock();
        else
            Debug.Log("Cannot be unlocked!");
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.skillToolTip.ShowToolTip(true, _rect, skillData);
        
        if (isUnlocked == false)
            UpdateIconColor(Color.white* .9f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.skillToolTip.ShowToolTip(false, _rect);

        if (isUnlocked == false)
            UpdateIconColor(lastColor);
    }

    public Color GetColorByHex(string colorHex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(colorHex, out color))
            return color;
        else
            return Color.white;
    }
    
    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
