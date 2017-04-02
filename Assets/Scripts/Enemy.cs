using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField]
    float maxHealthPoints = 100f;
    [SerializeField]
    float attackRadius = 5f;
    [SerializeField]
    float moveRadius = 15f;
    [SerializeField]
    float damagePerShot = 9f;
    [SerializeField]
    float secondsBetweenShots = 0.5f;
    [SerializeField]
    GameObject projectileToUse;
    [SerializeField]
    GameObject projectileSocket;
    [SerializeField]
    Vector3 aimOffset = new Vector3(0, 1, 0);
    
    bool isAttacking = false;

    private float currentHealthPoints;
    AICharacterControl aiCharacterControl = null;
    GameObject player = null;

    public float healthAsPercentage
    {
        get { return currentHealthPoints / maxHealthPoints; }
    }


    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

        if (currentHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
        currentHealthPoints = maxHealthPoints;
	}
	
	// Update is called once per frame
	void Update () {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if(distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;

            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots); //TODO switch to couroutine
            
        }

        if (distanceToPlayer > attackRadius)
        {
            isAttacking = false;
            CancelInvoke();
        }



        if (distanceToPlayer <= moveRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }




    }

    void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.damageCaused = damagePerShot;

        Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255f, 0f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveRadius);
    }

}
