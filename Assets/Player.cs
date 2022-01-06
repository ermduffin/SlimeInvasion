using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask layerMask;
    private float speed = 10, jumpspeed = 14;
    private BoxCollider2D boxCollider2D;

    private Animator animator;
    AnimatorStateInfo stateInfo;
    int shootStateHash = Animator.StringToHash("Layer2.PlayerShoot");

    private bool facingRight = true;
    public Vector2 startPosition;

    public GameObject _rock;

    public GameController gameController;

    // cooldown for being damaged
    private int cooldownTime = 2;
    private bool canBeDamaged = true;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    void Update()
    {
        // if moving, play the run animation
        if (GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            animator.Play("PlayerRun",0);
        }
        // if not moving, play the idle animation
        else
        {
            animator.Play("PlayerIdle",0);
        }

        // if jump button pressed and the player is standing on the tilemap, jump
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && IsGrounded())
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpspeed);
        }

        // if left mouse button pressed, shoot
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        // if in the shoot animation state, set Shoot to false
        stateInfo = animator.GetCurrentAnimatorStateInfo(1);
        if (stateInfo.fullPathHash == shootStateHash)
        {
            animator.SetBool("Shoot", false);
        }
    }

    void FixedUpdate()
    {
        float v = Input.GetAxisRaw("Horizontal");
        // move the player horizontally
        GetComponent<Rigidbody2D>().velocity = new Vector2(v * speed, GetComponent<Rigidbody2D>().velocity.y);

        // rotate to the left if moving left
        if (facingRight && GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            facingRight = false;
            transform.Rotate(new Vector3(0, 180, 0));
        }
        // rotate to the right if moving right
        else if (!facingRight && GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            facingRight = true;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    // check if the bottom of the enemy is touching the tilemap
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.5f, layerMask);
        return raycastHit2D.collider;
    }

    // throw a rock
    private void Shoot()
    {
        // set the direction of the throw
        if (!facingRight)
            _rock.GetComponent<RockProjectile>().facingRight = false;
        else
            _rock.GetComponent<RockProjectile>().facingRight = true;

        // start the throwing animation
        animator.SetBool("Shoot", true);

        // create a new rock at the player's position
        Instantiate(_rock, new Vector2(transform.position.x, transform.position.y - 4.5f), new Quaternion());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // if the player touches the portal
        if (col.gameObject.name == "Portal")
        {
            // load the next level
            gameController.nextLevel();
        //  if the player touches an enemy
        } else if (col.gameObject.name == "Enemy(Clone)" || col.gameObject.name == "FireEnemy(Clone)")
        {
            // if canBeDamaged is not on cooldown
            if (canBeDamaged)
            {
                // damage the player
                gameController.PlayerDamaged();
                // start the canBeDamaged cooldown
                StartCoroutine(Cooldown());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //  if the player touches a fireball
        if (col.gameObject.name == "FireProjectile(Clone)")
        {
            // if canBeDamaged is not on cooldown
            if (canBeDamaged)
            {
                // destroy the fireball
                Destroy(col.gameObject);
                // damage the player
                gameController.PlayerDamaged();
                // start the canBeDamaged cooldown
                StartCoroutine(Cooldown());
            }
        }
    }

    // disable the player taking damage until after the cooldown timer is done waiting
    IEnumerator Cooldown()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(cooldownTime);
        canBeDamaged = true;
    }
}
