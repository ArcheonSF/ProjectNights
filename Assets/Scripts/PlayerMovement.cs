using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster = null;
    Vector3 currentDestination, clickPoint;
    bool isInDirectMode = false;
    AICharacterControl aiCharacterControl = null;
    GameObject walkTarget = null;

    //TODO solve fight between serialize and const
    [SerializeField]
    const int walkableLayerNumber = 8;
    [SerializeField]
    const int enemyLayerNumber = 9;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        aiCharacterControl = GetComponent<AICharacterControl>();

        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
        walkTarget = new GameObject("walkTarget");
    }

    void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
    {
        switch(layerHit)
        {
            case enemyLayerNumber:
                //navigate to the enemy
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                //navigate to point on the ground
                //TODO consider whether we really want to do this for player movement
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;
            default:
                Debug.LogWarning("Don't know how to handle mouse click for player movement");
                return;
        }
    }

    //TODO make this get called again
    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

         

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.transform.right;

        thirdPersonCharacter.Move(movement, false, false);

    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }


}

