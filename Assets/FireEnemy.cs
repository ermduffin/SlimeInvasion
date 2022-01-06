using System.Collections;
using UnityEngine;

public class FireEnemy : Enemy
{
    public GameObject _fire;
    private int cooldownTime = 2;
    private bool canShoot = true;

    // check if the enemy is ready to shoot a fireball
    override protected void CheckShoot()
    {
        if (canShoot)
        {
            // shoot a fireball
            Shoot();
            // wait to shoot another fireball until after a cooldown
            StartCoroutine(Cooldown());
        }
    }

    // shoot a fireball
    private void Shoot()
    {
        // set whether the fireball will shoot to the right or left of the enemy
        if (!facingRight)
            _fire.GetComponent<Projectile>().facingRight = false;
        else
            _fire.GetComponent<Projectile>().facingRight = true;

        // create a new fireball at the enemy's position
        Instantiate(_fire, new Vector2(transform.position.x, transform.position.y), new Quaternion());
    }

    // disable shooting for the enemy until after the cooldown timer is done waiting
    IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldownTime);
        canShoot = true;
    }
}
