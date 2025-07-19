using System;
using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
   private SpriteRenderer _sr;
   private Entity _entity;

   [Header("On Taking Damage VFX")] 
   [SerializeField] private Material onDamageMaterial;
   [SerializeField] private float onDamageVfxDuration = .2f;
   private Material _originalMaterial;
   private Coroutine _onDamageVfxCoroutine;

   [Header("On Doing Damage VFX")] 
   [SerializeField] private Color hitVfxColor = Color.white;
   [SerializeField] private GameObject hitVfx;
   [SerializeField] private GameObject critHitVfx;
   
   
   private void Awake()
   {
      _entity = GetComponent<Entity>();
      _sr = GetComponentInChildren<SpriteRenderer>();
      _originalMaterial = _sr.material;
   }

   public void CreateOnHitVFX(Transform target, bool isCrit)
   {
      GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
      GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
      vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;

      if (_entity.facingDir == -1 && isCrit)
         vfx.transform.Rotate(0, 180, 0);
   }
   
   public void PlayOnDamageVfx()
   {
      if (_onDamageVfxCoroutine != null)
         StopCoroutine(_onDamageVfxCoroutine);
      
      _onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
   }

   private IEnumerator OnDamageVfxCo()
   {
      _sr.material = onDamageMaterial;

      yield return new WaitForSeconds(onDamageVfxDuration);
      _sr.material = _originalMaterial;
   }
}
