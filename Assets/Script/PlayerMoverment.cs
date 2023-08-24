using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private float dirX = 0f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private AudioSource jumpSoundEffect;


    private enum MovementState {idle, running, jumping, falling}
    private MovementState state = MovementState.idle;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        // Gọi hàm CheckGround để kiểm tra va chạm với mặt đất
        bool isGrounded = CheckGround();

        // Xử lý các tác động như nhảy và chuyển đổi trạng thái hoạt hình dựa vào isGrounded
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState(isGrounded);
    }


    private bool CheckGround()
    {
        float raycastDistance = 0.1f; // Khoảng cách raycast từ phía dưới nhân vật xuống để kiểm tra mặt đất
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y - sprite.bounds.extents.y - 0.05f);

        // Thực hiện Raycast và kiểm tra va chạm với đối tượng có tag "Ground"
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance);
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            return true;
        }

        return false;
    }

    private void UpdateAnimationState(bool isGrounded)
    {
        MovementState state; 


        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }
}
