using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {
    public Animation doorAnimation;
    private bool isOpen = false;

    public void Open()
    {
        if (isOpen) return;
        doorAnimation.Play("open");
    }
}
