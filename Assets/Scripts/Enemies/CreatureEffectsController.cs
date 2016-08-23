using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureEffectsController : MonoBehaviour {
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] ParticleEmitter explosionVisual;
    private AudioSource audioSource;
    [SerializeField] private Animation animationComponent;
    private Dictionary<string, string> animationNames;
    ChargeHolder chargeHolder;
    WarriorBehaviour behaviour;

    void OnEnable()
    {
        chargeHolder = GetComponent<ChargeHolder>();
        if (chargeHolder != null)
        {
            chargeHolder.Overcharged += OnOvercharge;
        }
        behaviour = GetComponent<WarriorBehaviour>();
        if (behaviour != null)
        {
            behaviour.Moved += OnMoved;
            behaviour.Stopped += OnStopped;
            behaviour.Calm += OnCalm;
            behaviour.Angry += OnAngry;
        }
    }
    void OnDisable()
    {
        if (chargeHolder != null) chargeHolder.Overcharged -= OnOvercharge;
        if (behaviour != null)
        {
            behaviour.Moved -= OnMoved;
            behaviour.Stopped -= OnStopped;
        }
    }

	void Start () {
        audioSource = GetComponent<AudioSource>();
        animationComponent["idle"].wrapMode = WrapMode.Loop;
        animationComponent.Play("idle", PlayMode.StopAll);
        animationComponent["collapse"].wrapMode = WrapMode.ClampForever;
        animationComponent["walk"].wrapMode = WrapMode.Loop;
    }	
	
	void Update () {
        if (animationNames == null) return;
	}


    private void OnMoved()
    {
        animationComponent.Play("walk", PlayMode.StopAll);
    }

    private void OnStopped()
    {
        animationComponent.Play("idle", PlayMode.StopAll);
    }
    private void OnCalm()
    {
        animationComponent["walk"].speed = 1;
    }
    private void OnAngry()
    {
        animationComponent["walk"].speed = 3;
    }
    private void OnOvercharge()
    {
        audioSource.clip = explosionSound;
        audioSource.Play();
        animationComponent.Play("collapse", PlayMode.StopAll);
    }

}
