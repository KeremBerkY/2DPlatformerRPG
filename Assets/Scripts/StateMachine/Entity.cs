using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }     
    protected StateMachine stateMachine;
   
    
    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;
    
    [Header("Collision detection")] 
    [SerializeField] protected LayerMask WhatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }
    
    // Condition variables
    private bool _isKnocked;
    private Coroutine _knockBackCo;
    private Coroutine _slowDownCo;
    
    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        stateMachine = new StateMachine();

    }
    
    protected virtual void Start()
    {
    }
    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public virtual void EntityDeath()
    {
        
    }

    public virtual void SlowDownEntity(float duration, float slowMultiplier)
    {
        if (_slowDownCo != null)
            StopCoroutine(_slowDownCo);

        _slowDownCo = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }

    protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }
    
    public void ReceiveKnockBack(Vector2 knockBack, float duration)
    {
        if (_knockBackCo != null)
            StopCoroutine(_knockBackCo);

        _knockBackCo = StartCoroutine(KnockBackCo(knockBack, duration));
    }

    private IEnumerator KnockBackCo(Vector2 knockBack ,float duration)
    {
        _isKnocked = true;
        rb.linearVelocity = knockBack;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        _isKnocked = false;
    }
    
    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }
        
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (_isKnocked)
            return;
        
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;
        
        OnFlipped?.Invoke();
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, WhatIsGround);

        if (secondaryWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance,WhatIsGround)
                    && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, WhatIsGround);
        }
        else
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance,
                WhatIsGround);
    }
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0 ));
        
        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0 ));
    }
}
