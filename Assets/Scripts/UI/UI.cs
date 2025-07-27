using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillToolTip;
    public UI_SkillTree skillTree;
    private bool _skillTreeEnabled;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
    }

    public void ToggleSkillTreeUI()
    {
        _skillTreeEnabled = !_skillTreeEnabled;
        skillTree.gameObject.SetActive(_skillTreeEnabled);
        skillToolTip.ShowToolTip(false,null);
    }
}
