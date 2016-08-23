using UnityEngine;
using System.Collections;

public class TouchChargeDealer : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != Tags.Player) return;
        other.GetComponent<ChargeHolder>().TakeCharge();
    }

}
