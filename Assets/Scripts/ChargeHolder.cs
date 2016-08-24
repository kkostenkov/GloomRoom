using UnityEngine;
using System.Collections;

public class ChargeHolder : MonoBehaviour {
    [SerializeField] private float overchargeTime = 3f; // Time till overcharge in seconds
    private bool isChargingRemotely = false;
    private bool isChargingByTouch = false;
    private float chargeTimer;
    private bool overcharged = false;

    public delegate void ChargeHandler();
    public ChargeHandler Overcharged;

    void Start () {
        chargeTimer = 0;
    }
	
	
	void Update () {

        if (overcharged) return;
        if (chargeTimer >= overchargeTime)
        {
            overcharged = true;
            Overcharged();            
        }
        
        // Add time to charge timer.
        if (isChargingRemotely || isChargingByTouch)
        {
            chargeTimer += Time.deltaTime;
            isChargingRemotely = false;
        } else 
        {
            // Reset charge timer if this frame was no charge;
            chargeTimer = 0;
        }
	}

    public void TakeCharge()
    {
        isChargingRemotely = true;
    }
    public void StartChargeByTouch()
    {
        isChargingByTouch = true;
    }
    public void StopChargeByTouch()
    {
        isChargingByTouch = false;
    }


}
