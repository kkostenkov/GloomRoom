using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Objects.Interactible;

public class ActionKeyController : MonoBehaviour {
    private const float ACTION_DISTANCE = 2.5f;
    private static string ACTION_KEY = "e";
    private Transform selfTransform;
    private Camera cam;
    private Vector2 screenCenter;
    private Ray ray;
    private RaycastHit hit;
    private List<IInteractible> interactInterfaces;
    
	void Start()
    {
        cam = Camera.main;
        selfTransform = GetComponent<Transform>();
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }
	
    void Update()
    {
        if (Input.GetKeyUp(ACTION_KEY))
        {
            print("action key pressed");
            GameObject interactible = GetTargetObject();
            //print(interactible != null);
            // Can it be used?
            InterfaceSearcher.GetInterfaces(out interactInterfaces, interactible);
            foreach (IInteractible face in interactInterfaces)
            {
                face.Activate();
            }
        }
    }


    private GameObject GetTargetObject (){
		// returns object on center of screen
		ray = cam.ScreenPointToRay(screenCenter);
        //Debug.DrawRay(ray.origin, selfTransform.forward, Color.red, 0.5f);
		if (Physics.Raycast(ray, out hit, ACTION_DISTANCE)) {
			return hit.collider.gameObject;
		}
		return null;
	}
}
