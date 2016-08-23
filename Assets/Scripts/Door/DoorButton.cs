using UnityEngine;
using System.Collections;
using Objects.Interactible;

public class DoorButton : Button {
    public Animation doorAnimation;

    void Start()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public override void Activate()
    {
        if (isActive) return;
        doorAnimation.Play("open");
        isActive = true;
        GetComponent<Renderer>().material.color = Color.green;
    }
}
