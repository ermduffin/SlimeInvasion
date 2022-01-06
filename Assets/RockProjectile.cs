using UnityEngine;

public class RockProjectile : Projectile
{
    public GameController gameController;
    void Start()
    {
        speed = 15;
        if (facingRight)
            // fire the projectile to the right
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        else
            // fire the projectile to the left
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // if the projectile collides with the tilemap or the portal, destroy it
        if (col.gameObject.name == "Tilemap_ForeGround" || col.gameObject.name == "Tilemap_ForeGround_Back"
            || col.gameObject.name == "Portal")
        {
            Destroy(gameObject);
        // if the projectile collides with an enemy, destroy the enemy and the projectile
        } else if (col.gameObject.name == "Enemy(Clone)" || col.gameObject.name == "FireEnemy(Clone)")
        {
            Destroy(gameObject);
            Destroy(col.gameObject);
            // adjust the score and enemy count for the enemy killed
            gameController.EnemyKilled();
        }
    }
}
