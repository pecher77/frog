using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowMovement : BaseMovement
{

    public bool useWeapon = false;
    public float forceToPlayerX = 5.0f;
    public float forceToPlayerY = 5.0f;

    private bool _shouldHitPlayer = true;

    public WeaponManager weaponManager;
    public override void Start()
    {
        base.Start();

        if (useWeapon)
        {
            StartCoroutine(routine: UseWeapon("Egg"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _shouldHitPlayer)
        {
            //collision.gameObject.GetComponent<PlayerMovement>().AddEnemyForce(new Vector2(forceToPlayerX, forceToPlayerY));
            _shouldHitPlayer = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _shouldHitPlayer = true;
        }
    }

    public IEnumerator UseWeapon(string name)
    {
        while (true)
        {
            //TODO
            var weapon = weaponManager.FindWeaponByName(name);
            if (WeaponEgg.CanUse(transform))
            {
                Instantiate(weapon, new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z), Quaternion.identity);
                yield return new WaitForSeconds(2.0f);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }

        }
       
    }
}
