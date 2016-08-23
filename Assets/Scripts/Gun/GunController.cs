using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {
    private const float RAY_MAX_DISTANCE = 10f;
    [SerializeField] private LineRenderer lazerVisual;
    [SerializeField] private Transform muzzle;
    private const KeyCode fireButton = KeyCode.Mouse0;
    private Ray ray;
    private RaycastHit hit;
	
	void Start () {
        lazerVisual.enabled = false;
	}


    void Update()
    {
        if (Input.GetKey(fireButton))
        {
            Fire();
        }

        else
        {
            lazerVisual.enabled = false;
        }
    }

    private void Fire()
    {
        lazerVisual.SetPosition(0, muzzle.position);
        // Raycast.
        ray = new Ray(muzzle.position, muzzle.forward);
        if (Physics.Raycast(ray, out hit, RAY_MAX_DISTANCE))
        {
            lazerVisual.SetPosition(1, hit.point);
            ChargeHolder chargeHolder = hit.collider.gameObject.GetComponent<ChargeHolder>();
            
            if (chargeHolder != null)
            {
                chargeHolder.TakeCharge();
            }
            
        }
        else
        {
            lazerVisual.SetPosition(1, muzzle.position + muzzle.forward * RAY_MAX_DISTANCE);
        }
        

        lazerVisual.enabled = true;
    }
}
