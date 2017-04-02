using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CameraRaycaster))]
public class CursorAffordance: MonoBehaviour {

    [SerializeField]
    Texture2D walkCursor = null;
    [SerializeField]
    Texture2D enemyCursor = null;
    [SerializeField]
    Texture2D otherCursor = null;
    [SerializeField]
    Vector2 cursorHotspot = new Vector2(0, 0);

    //TODO solve fight between serialize and const
    [SerializeField]
    const int walkableLayerNumber = 8;
    [SerializeField]
    const int enemyLayerNumber = 9;

    CameraRaycaster cameraRaycaster;


	// Use this for initialization
	void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChange; //registering as an observer
	}
	

	void OnLayerChange (int newLayer) {
                
        switch (newLayer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(otherCursor, cursorHotspot, CursorMode.Auto);
                return;
        }

    }
    //TODO consider de-registering OnLayerChanged on leaving all game scenes
}
