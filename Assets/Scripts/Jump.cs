using UnityEngine;
using UnityEngine.U2D;

public class Jump : MonoBehaviour
{
    public float jumpForce = 5;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private int jumpCount = 0;
    public int maxJumps = 2;
    public Animator collectAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (isGrounded || jumpCount < maxJumps)
            {
                jump();
                jumpCount++;
            }
        }
    }

    void jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; // R�initialiser le compteur de sauts lorsqu'on touche le sol
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Collect"){
            ScoreManager.AddPoints(1);
            col.gameObject.GetComponent<Animator>().SetBool("Collect", true);
        }
    }
}
