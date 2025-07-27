using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI _ui;
    private RectTransform _rect;
    private UI_SkillTree _skillTree;
    private UI_TreeConnectHandler _connectHandler;

    [Header("Unlock Details")] 
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked; // This variable use for lock a node. Used to lock conflict node of chosen node.
    
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
        _connectHandler = GetComponent<UI_TreeConnectHandler>();
        
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));
            
        _skillTree.AddSkillPoints(skillData.cost);
        _connectHandler.UnlockConnectionImage(false);
    }

    private void UnLock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();
        
        _skillTree.RemoveSkillPoints(skillData.cost);
        _connectHandler.UnlockConnectionImage(true);

        _skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeType);
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
        else if (isLocked)
            _ui.skillToolTip.LockedSkillEffect();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.skillToolTip.ShowToolTip(true, _rect, this);

        if (isUnlocked || isLocked)
            return;
        
        ToggleNodeHighlight(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.skillToolTip.ShowToolTip(false, _rect);

        if (isUnlocked || isLocked)
            return;
    
        ToggleNodeHighlight(false);
    }
    
    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlightColor = Color.white * .9f; highlightColor.a = 1;
        Color colorToApply = highlight ? highlightColor : lastColor;
        
        UpdateIconColor(colorToApply);
    }

    public Color GetColorByHex(string colorHex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(colorHex, out color))
            return color;
        else
            return Color.white;
    }

    private void OnDisable()
    {
        if (isLocked)
            UpdateIconColor(GetColorByHex(lockedColorHex));
        
        if (isUnlocked)
            UpdateIconColor(Color.white);
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
