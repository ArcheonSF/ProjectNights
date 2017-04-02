using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField]
    float maxHealthPoints = 100f;
    [SerializeField]
    int enemyLayer = 9;
    [SerializeField]
    float damagePerHit = 10f;
    [SerializeField]
    float minTimeBetweenHits = 0.5f;
    [SerializeField]
    float maxAttackRange = 2f;

    private float currentHealthPoints;
    float lastHitTime = 0f;

    GameObject currentTarget;
    CameraRaycaster cameraRaycaster;



    public float healthAsPercentage
    {
        get { return currentHealthPoints / maxHealthPoints; }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

        if (currentHealthPoints <=0)
        {
            //Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        currentHealthPoints = maxHealthPoints;
    }
	
    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;
            currentTarget = enemy;

            //check enemy is in range
            if (Vector3.Distance(enemy.transform.position, transform.position) > maxAttackRange)
            {
                    return;
            }

            var enemyComponent = enemy.GetComponent<Enemy>();

            if (Time.time - lastHitTime > minTimeBetweenHits)
            {
                print("In Range, Hitting");
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
            

        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
