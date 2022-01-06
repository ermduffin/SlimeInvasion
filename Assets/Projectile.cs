using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float speed = 8;
    public bool facingRight = true;

    void Start()
    {
        if (facingRight)
            // fire the projectile to the right
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        else
            // fire the projectile to the left
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // if the projectile collides with the tilemap or the portal, destroy it
        if (col.gameObject.name == "Tilemap_ForeGround" || col.gameObject.name == "Tilemap_ForeGround_Back"
            || col.gameObject.name == "Portal")
        {
            Destroy(gameObject);
        }
    }
}
