using System;
using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
   private SpriteRenderer _sr;

   [Header("On Taking Damage VFX")] 
   [SerializeField] private Material onDamageMaterial;
   [SerializeField] private float onDamageVfxDuration = .2f;
   private Material _originalMaterial;
   private Coroutine _onDamageVfxCoroutine;

   [Header("On Doing Damage VFX")] 
   [SerializeField] private Color hitVfxColor = Color.white;
   [SerializeField] private GameObject hitVfx;
   
   
   private void Awake()
   {
      _sr = GetComponentInChildren<SpriteRenderer>();
      _originalMaterial = _sr.material;
   }

   public void CreateOnHitVFX(Transform target)
   {
      GameObject vfx = Instantiate(hitVfx, target.position, Quaternion.identity);
      vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;
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
