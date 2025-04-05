using Framework;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [TitleGroup("Components")]
    [SerializeField, Required]
    private SpriteRenderer _sr;

    [TitleGroup("Components")]
    [SerializeField, Required]
    private Animator _animator;

    [TitleGroup("Components")]
    [SerializeField, Required]
    private Rigidbody2D _rb;

    [TitleGroup("Swim")]
    [SerializeField]
    private float _swimHorizontalStrength = 10;

    [TitleGroup("Swim")]
    [SerializeField]
    private float _idleSwimVerticalStrength = 10;

    [TitleGroup("Swim")]
    [SerializeField]
    private float _movingSwimVerticalStrength = 10;

    [TitleGroup("Swim")]
    [ShowInInspector, HideInEditorMode]
    private Vector2 _swimDirection;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        bool isSwimming = false;

        this.UpdateSwimDirection();

        isSwimming |= Input.GetKey(KeyCode.X);
        if (Input.GetKeyDown(KeyCode.X))
        {
            this._animator.SetTrigger("Swimming");
        }
        
        if (this._swimDirection != Vector2.zero)
        {
            this._sr.flipX = this._swimDirection.x < 0;
        }
        
        this._animator.SetBool("IsSwimming", isSwimming);
    }

    private void UpdateSwimDirection()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this._swimDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this._swimDirection = Vector2.right;
        }
        else
        {
            this._swimDirection = Vector2.zero;
        }
    }
    

    private void Swim()
    {
        // cancel the fall speed
        float velocityY = Math.Max(0, this._rb.velocity.y);

        this._rb.velocity = new Vector2(0, velocityY);

        // Jump higher when not "moving"
        float verticalStrength = this._swimDirection != Vector2.zero ? this._movingSwimVerticalStrength : this._idleSwimVerticalStrength;

        Vector2 swimVerticalForce =  verticalStrength * Vector2.up;
        Vector2 swimHorizontalForce = this._swimHorizontalStrength * this._swimDirection;

        Vector2 swimForce = swimHorizontalForce + swimVerticalForce;

        this._rb.AddForce(swimForce);
        this._animator.ResetTrigger("Swimming");
    }
}
