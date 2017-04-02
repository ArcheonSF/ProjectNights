using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed;
    [HideInInspector]
    public float damageCaused;

    private void OnCollisionEnter(Collision collider)
    {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));
        
        if(damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
        Destroy(gameObject,0.01f);
    }

}
