using UnityEngine;

public class Enemy : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject target;
    public float speed = 5;
    protected float maxYVelocity = -10;
    protected float minYVelocity = -3;
    private PolygonCollider2D polygonCollider2D;
    protected Rigidbody2D rigidbody2D;
    protected bool facingRight = true;

    public static bool gameOver = false;

    void Awake()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // if the game hasn't ended
        if (!gameOver)
        {
            // only change direction of movement if the slime is standing on the tilemap
            if (IsGrounded())
            {
                float v = 0;
                // if the slime is to the left of the target
                if (transform.position.x < target.transform.position.x)
                {
                    facingRight = true;
                    v = 1; // set the direction of movement to right
                }
                // if the slime is to the right of the target
                else if (transform.position.x > target.transform.position.x)
                {
                    facingRight = false;
                    v = -1; // set the direction of movement to left
                }

                // if the slime is bouncing too high, slow his y velocity
                if (rigidbody2D.velocity.y < 0 && rigidbody2D.velocity.y < maxYVelocity)
                {
                    rigidbody2D.velocity = new Vector2(v * speed, maxYVelocity);
                }
                // if the slime is bouncing too low, speed up his y velocity
                else if (rigidbody2D.velocity.y < 0 && rigidbody2D.velocity.y > minYVelocity)
                {
                    rigidbody2D.velocity = new Vector2(v * speed, minYVelocity);
                }
                else
                {
                    rigidbody2D.velocity = new Vector2(v * speed, rigidbody2D.velocity.y);
                }
            }
            CheckShoot();
        }
    }

    // check if the bottom of the enemy is touching the tilemap
    protected bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(polygonCollider2D.bounds.center, polygonCollider2D.bounds.size, 0f, Vector2.down, 0.1f, layerMask);
        return raycastHit2D.collider;
    }

    // do nothing if the enemy is melee only
    virtual protected void CheckShoot()
    {
        return;
    }
}
