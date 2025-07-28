using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class VFX_AutoController : MonoBehaviour
{
    private SpriteRenderer _sr;
    
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    [Space]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private bool randomRotation = true;

    [Header("Fade effect")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1;

    [Header("Random Rotation")] 
    [SerializeField] private float minRotation = 0;
    [SerializeField] private float maxRotation = 360; 
    
    [Header("Random Position")] 
    [SerializeField] private float xMinOffset = -.3f;
    [SerializeField] private float xMaxOffset = .3f;
    [Space] 
    [SerializeField] private float yMinOffset = -.3f;
    [SerializeField] private float yMaxOffset = .3f;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (canFade)
            StartCoroutine(FadeCo());
        
        ApplyRandomOffset();
        ApplyRandomRotation();
        
        if (autoDestroy)
            Destroy(gameObject, destroyDelay);
    }

    private IEnumerator FadeCo()
    {
        Color targetColor = Color.white;

        while (targetColor.a > 0)
        {
            targetColor.a = targetColor.a - (fadeSpeed * Time.deltaTime);
            _sr.color = targetColor;
            yield return null;
        }

        _sr.color = targetColor;
    }

    private void ApplyRandomOffset()
    {
        if (randomOffset == false)
            return;

        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position = transform.position + new Vector3(xOffset, yOffset);
    }

    private void ApplyRandomRotation()
    {
        if (randomRotation == false)
            return;

        float zRotation = Random.Range(minRotation, maxRotation);
        transform.Rotate(0,0,zRotation);
    }
}
