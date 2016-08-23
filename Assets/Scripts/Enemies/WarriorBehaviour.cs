using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WarriorBehaviour : MonoBehaviour {
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
    private bool turnAllowed;
    private float turnSpeed = 1f;

    private const float checkTimeout = 0.1f; // seconds before another collision check
    private float untillCheck;
    private float collisionCheckDistance = 3f;
    private Ray ray;
    private RaycastHit hit;

    void Start () {
        mood = NpcMood.Calm;
        selfTransform = GetComponent<Transform>();
        GenerateDirectionsToCheck();
        movementAllowed = true;
        turnAllowed = true;
        StartCoroutine(CalmBehaviour());
        ChooseMovementDirection();

    }
	
	
	void Update () {
        //Move();
        //Turn();
        untillCheck -= Time.deltaTime;
    }
    /*
    private void Move()
    {
        if (movementAllowed)
        {
            
        }
    }

    private void Turn()
    {
        if (turnAllowed)
        {
            
        }
    }
    */

    private Vector3 lastMovement = Vector3.zero;
    private IEnumerator CalmBehaviour()
    {
        while (true)
        {
            while (mood != NpcMood.Calm) yield return null;
            // Beheviour description.
            // Check player visibility (if near).
            // Choose movement dir.
            // Move
            Vector3 normalizedDirection = Vector3.Normalize(movementDirection);
            float turnAngle = Vector3.Angle(selfTransform.forward, movementDirection);
            if (movementAllowed)// & (turnAngle < 2))
            {
                float moveSpeed = (mood == NpcMood.Calm) ? calmSpeed : angrySpeed;
                print(movementDirection);
                Debug.DrawRay(selfTransform.position, movementDirection, Color.green, 0.1f);
                selfTransform.position = Vector3.MoveTowards(selfTransform.position, selfTransform.position + movementDirection, moveSpeed * Time.deltaTime);
            }
            print("1");
            lastMovement = movementDirection;
            // Rotate.
            //selfTransform.Rotate(Vector3.up, turnAngle * Time.deltaTime);
            yield return null;
        }
    }
    
    void OnTriggerStay(Collider col)
    {
        // Check only once a while.
        if (untillCheck > 0)
        {
            return;
        }
        // Reset check timer.
        untillCheck = checkTimeout;

        if (col.tag == "Player")
        {
            movementDirection = col.transform.position - selfTransform.position;
            return;
        }
        ChooseMovementDirection();

    }

    private void ChooseMovementDirection()
    {
        // Fing not blocked ways.
        directionsWithObstacles.Clear();
        safeDirections.Clear();
        for (int i = 0; i < directions.Count; i++)
        {
            ray = new Ray(selfTransform.position, directions[i]);
            if (Physics.Raycast(ray, out hit, collisionCheckDistance))
            {
                // Something is hit.
                directionsWithObstacles.Add(directions[i]);
                //Debug.DrawRay(selfTransform.position, directions[i], Color.red, 0.1f);
                //MarkNeighboursUnsafe(i, directions);
            }
        }
        //Debug.DrawRay(selfTransform.position, closestHitDirection, Color.red);
        for (int i = 0; i < directions.Count; i++)
        {
            if (directionsWithObstacles.Contains(directions[i])) continue;
            safeDirections.Add(directions[i]);
        }
        //safeDirections = new List<Vector3>(directions.Except(directionsWithObstacles));
        foreach(Vector3 dir in safeDirections)
        {
            Debug.DrawRay(selfTransform.position, dir, Color.yellow, 0.1f);
        }
        movementAllowed = !(safeDirections.Count == 0); // Check if unit is blocked.
        if (!movementAllowed) return;
        // Continue moving to previous direction if it is unblocked.
        if (safeDirections.Contains(lastMovement))
        {
            movementDirection = lastMovement;
            Debug.DrawRay(selfTransform.position, lastMovement, Color.red, 1f);
            print("moving to last");
        }
        else
        {
            // Move to random unblocked direction.
            int choise = Random.Range(0, safeDirections.Count - 1);
            movementDirection = new Vector3(safeDirections[choise].x, 0, safeDirections[choise].z);
            Debug.LogWarning(choise);
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
        /*
        foreach (Vector3 dir in checkDirections) {
            GameObject.Instantiate(testCube, body.position + dir, Quaternion.identity);
        }
        */
    }
}
