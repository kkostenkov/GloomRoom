using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour {
    [SerializeField ] private Animation animationComponent;
    private Dictionary<string, string> animationNames;
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

	void Start () {
        animationComponent["idle"].wrapMode = WrapMode.Loop;
        animationComponent.Play("idle", PlayMode.StopAll);
        
    }
	
	
	void Update () {
        if (animationNames == null) return;
	}


    private void OnOvercharge()
    {
        animationComponent["idle"].wrapMode = WrapMode.ClampForever;
        animationComponent.Play("collapse", PlayMode.StopAll);
    }

}
