using System;
using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity _entity;

    private void Awake()
    {
        _entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        _entity.OnFlipped += HandleFlip;
    }

    private void OnDisable()
    {
        _entity.OnFlipped -= HandleFlip;
    }

    private void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
