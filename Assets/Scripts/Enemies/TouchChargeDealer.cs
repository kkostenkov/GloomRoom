using UnityEngine;
using System.Collections;

public class TouchChargeDealer : MonoBehaviour {
    private bool isAlive = true;
    ChargeHolder chargeHolder;

    void OnEnable()
    {
        chargeHolder = GetComponent<ChargeHolder>();
        if (chargeHolder != null)
        {
            chargeHolder.Overcharged += OnOvercharge;
        }
    }
    void OnDisable()
    {
        if (chargeHolder != null) chargeHolder.Overcharged -= OnOvercharge;
    }

    private void OnOvercharge()
    {
        isAlive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAlive) return;
        if (other.tag != Tags.Player) return;
        other.GetComponent<ChargeHolder>().StartChargeByTouch();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != Tags.Player) return;
        other.GetComponent<ChargeHolder>().StopChargeByTouch();
    }

}
