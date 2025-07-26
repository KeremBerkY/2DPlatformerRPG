using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI _ui;
    private RectTransform _rect;
    private UI_SkillTree _skillTree;

    [Header("Unlock Details")] 
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;
    
    [Header("Skill details")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#605E5E";
    private Color lastColor;

    private void Awake()
    {
        _ui = GetComponentInParent<UI>();
        _rect = GetComponent<RectTransform>();
        _skillTree = GetComponentInParent<UI_SkillTree>();
        
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    private void UnLock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        _skillTree.RemoveSkillPoints(skillData.cost);
        LockConflictNodes();

        // TODO: Find Player_SkillManager
            // Unlock skill on skill manager
            // skill manager unlock skill from skill data skill type
    }
    
    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        if (_skillTree.EnoughSkillPoints(skillData.cost) == false)
            return false;

        foreach (var node in neededNodes)
        {
            if (node.isUnlocked == false)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
                return false;
        }
        
        return true;
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
            node.isLocked = true;
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
        _ui.skillToolTip.ShowToolTip(true, _rect, this);
        
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
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
