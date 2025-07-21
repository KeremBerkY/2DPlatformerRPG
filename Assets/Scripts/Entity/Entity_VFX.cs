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

   [Header("On Doing Damage VFX")] 
   [SerializeField] private Color chillVfx = Color.cyan;
   private Color _originalHitVfxColor;
   private Coroutine statusVfxCo;
   
   private void Awake()
   {
      _entity = GetComponent<Entity>();
      _sr = GetComponentInChildren<SpriteRenderer>();
      _originalMaterial = _sr.material;
      _originalHitVfxColor = hitVfxColor;
   }

   public void PlayOnStatusVfx(float duration, ElementType element)
   {
      if (element == ElementType.Ice)
         StartCoroutine(PlayStatusVfxCo(duration, chillVfx));
   }

   private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
   {
      float tickInterval = .25f;
      float timeHasPassed = 0;

      Color lightColor = Color.Lerp(effectColor, Color.white, 0.7f);
      Color darkColor = Color.Lerp(effectColor, Color.black, 0.4f);

      bool toggle = false;

      while (timeHasPassed < duration)
      {
         _sr.color = toggle ? lightColor : darkColor;
         toggle = !toggle;

         yield return new WaitForSeconds(tickInterval);
         timeHasPassed = timeHasPassed + tickInterval;
      }

      _sr.color = Color.white;
   }
   
   public void CreateOnHitVFX(Transform target, bool isCrit)
   {
      GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
      GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
      vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;

      if (_entity.facingDir == -1 && isCrit)
         vfx.transform.Rotate(0, 180, 0);
   }

   public void UpdateOnHitColor(ElementType element)
   {
      if (element == ElementType.Ice)
         hitVfxColor = chillVfx;

      if (element == ElementType.None)
         hitVfxColor = _originalHitVfxColor;
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
