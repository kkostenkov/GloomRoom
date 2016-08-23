using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WarriorBehaviour : MonoBehaviour {
    public Transform player;
    private const float playerDetectionRadius = 12f;
    private enum NpcMood { Calm, Angry, Dead };
    private NpcMood mood;
    private Transform selfTransform;
    private const int directionsToCheckCount = 12;    
    private List<Vector3> directions = new List<Vector3>();
    private List<Vector3> directionsWithObstacles = new List<Vector3>();
    private List<Vector3> safeDirections = new List<Vector3>();
    private Vector3 movementDirection;
    private bool movementAllowed;
    private float calmSpeed = 1f;
    private float angrySpeed = 3f;
    private float turnSpeed = 60f;

    private const float checkTimeout = 0.1f; // seconds before another collision check
    private float untillCheck;
    private float collisionCheckDistance = 3f;
    private Ray ray;
    private RaycastHit hit;

    public delegate void ActionHandler();
    public ActionHandler Moved;
    public ActionHandler Stopped;
    public ActionHandler Calm;
    public ActionHandler Angry;

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
        mood = NpcMood.Calm;
        Calm();
        selfTransform = GetComponent<Transform>();
        GenerateDirectionsToCheck();
        movementAllowed = true;
        Moved();
        StartCoroutine(CalmBehaviour());
        StartCoroutine(AngryBehaviour());
        ChooseMovementDirection();
    }

    private void OnOvercharge()
    {
        mood = NpcMood.Dead;
        movementAllowed = false;
    }

    void Update () {
        untillCheck -= Time.deltaTime;
    }

    private Vector3 lastMovement = Vector3.zero;
    private IEnumerator CalmBehaviour()
    {
        while (true)
        {
            while (mood != NpcMood.Calm) yield return null;
            // Check player visibility (if near).
            if (Vector3.Distance(selfTransform.position, player.position) < playerDetectionRadius)
            {
                ray = new Ray(selfTransform.position, player.position - selfTransform.position);
                if (Physics.Raycast(ray, out hit, playerDetectionRadius))
                {
                    if (hit.transform.tag == Tags.Player)
                    {
                        print("player detected");
                        mood = NpcMood.Angry;
                        Angry();
                        continue;
                    }
                }
            }
            // Move
            //Debug.DrawRay(selfTransform.position, movementDirection, Color.green, 0.1f);
            Vector3 normalizedDirection = Vector3.Normalize(movementDirection);
            float angleToTarget = Vector3.Angle(selfTransform.forward, movementDirection);
            if (movementAllowed & (angleToTarget < 0.5f))
            {
                float moveSpeed = (mood == NpcMood.Calm) ? calmSpeed : angrySpeed;
                selfTransform.position = Vector3.MoveTowards(selfTransform.position, selfTransform.position + movementDirection, moveSpeed * Time.deltaTime);
            }
            lastMovement = movementDirection;
            // Turn.
            float turnAngle = turnSpeed * Time.deltaTime;
            selfTransform.Rotate(Vector3.up, Mathf.Clamp(turnAngle, 0, angleToTarget));
            yield return null;
        }
    }

    private IEnumerator AngryBehaviour()
    {
        while (true)
        {
            while (mood != NpcMood.Angry) yield return null;
            if (player == null)
            {
                mood = NpcMood.Calm;
                Calm();
                continue;
            }
            // Move.
            Vector3 destination = new Vector3(player.position.x, selfTransform.position.y, player.position.z);
            selfTransform.position = Vector3.MoveTowards(selfTransform.position, destination, angrySpeed * Time.deltaTime);
            // Turn.
            float angleToTarget = Vector3.Angle(selfTransform.forward, movementDirection);
            float turnAngle = turnSpeed * Time.deltaTime;
            selfTransform.Rotate(Vector3.up, Mathf.Clamp(turnAngle, 0, angleToTarget));
            yield return null;
        }
    }

    void OnTriggerStay(Collider col)
    {
        // Check only once a while.
        if (untillCheck > 0 || mood == NpcMood.Dead)
        {
            return;
        }
        // Reset check timer.
        untillCheck = checkTimeout;
        ChooseMovementDirection();
    }

    private void ChooseMovementDirection()
    {
        // Find not blocked ways.
        directionsWithObstacles.Clear();
        safeDirections.Clear();
        for (int i = 0; i < directions.Count; i++)
        {
            ray = new Ray(selfTransform.position, directions[i]);
            if (Physics.Raycast(ray, out hit, collisionCheckDistance))
            {
                // Something is hit.
                directionsWithObstacles.Add(directions[i]);
            }
        }
        safeDirections = new List<Vector3>(directions.Except(directionsWithObstacles));
        // Check if unit is blocked.
        if (safeDirections.Count == 0)
        {
            movementAllowed = false;
            Stopped();
            return;
        }
        else {
            if (!movementAllowed) Moved();
            movementAllowed = true;
        }
        // Continue moving to previous direction if it is unblocked.
        if (safeDirections.Contains(lastMovement))
        {
            movementDirection = lastMovement;
        }
        else
        {
            // Move to random unblocked direction.
            int choise = Random.Range(0, safeDirections.Count - 1);
            movementDirection = new Vector3(safeDirections[choise].x, 0, safeDirections[choise].z);
        }
    }
    
    private void GenerateDirectionsToCheck()
    {
        directions.Clear();
        float angle;
        for (int i = 0; i < directionsToCheckCount; i++)
        {
            angle = 360 / directionsToCheckCount * i;
            directions.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)));
        }
    }
}
