using UnityEngine;
using System.Collections;

public class TouchChargeDealer : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != Tags.Player) return;
        other.GetComponent<ChargeHolder>().StartChargeByTouch();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != Tags.Player) return;
        other.GetComponent<ChargeHolder>().StopChargeByTouch();
    }

}
