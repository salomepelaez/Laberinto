using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float sightRange = 5f;
    private float timeOnSight = 3f;

    public Transform playerTr;
    private Transform enemyTr;

    public Color warningColor;
    public Color seenColor;

    private void Awake()
    {
        enemyTr = transform;
    }

    void Update()
    {
        PlayerHasBeenSeen(playerTr.position);  
        DetectPlayer();      
    }

    private void DetectPlayer()
    {
        bool playerDetected = PlayerHasBeenSeen(playerTr.position);

        if(playerDetected)
        {
            gameObject.GetComponent<Renderer>().material.color = warningColor;

            if(timeOnSight <= 3)
            {
                timeOnSight += Time.deltaTime;                
            }     

            if(!(timeOnSight >= 3)) return;
                gameObject.GetComponent<Renderer>().material.color = seenColor;
            
        }

        else 
        {
            if(timeOnSight > 0)
            {
                timeOnSight -= Time.deltaTime;
            }

            if(!(timeOnSight <= 0)) return;
                gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }
    }

    private bool PlayerHasBeenSeen(Vector3 playerPosition)
    {
        var displacement = playerPosition - enemyTr.position;
        var distanceToPlayer = displacement.magnitude;

        if(distanceToPlayer <= sightRange)
        {
            var dot = Vector3.Dot(enemyTr.forward, displacement.normalized);
            if(!(dot >= 0.5)) return false;

            var layerMask = 1 << 2;
            layerMask = ~layerMask;

            if(Physics.Raycast(enemyTr.position, displacement.normalized, out var hit, sightRange, layerMask))
            {
                if(hit.collider.GetComponent<Player>())
                {
                    Debug.DrawRay(enemyTr.position, displacement.normalized * hit.distance, Color.red);
                }

                if(!hit.collider.GetComponent<Player>()) return false;

                Debug.DrawRay(enemyTr.position, displacement.normalized * hit.distance, Color.green);
                return true;
            }
        }
        return false;
    }
}
