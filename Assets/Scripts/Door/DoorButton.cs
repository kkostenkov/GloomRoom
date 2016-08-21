using UnityEngine;
using System.Collections;
using Objects.Interactible;

public class DoorButton : Button {
    public Animation doorAnimation;

    public override void Activate()
    {
        if (isActive) return;
        doorAnimation.Play("open");
        isActive = true;
    }
}
