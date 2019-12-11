using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float sightRange = 3f;
    private float timeOnSight = 3f;

    public Transform playerTr;
    private Transform enemyTr;

    public GameObject warning = null;
    public GameObject detect = null;

    void Start()
    {
        warning.SetActive(false);
        detect.SetActive(false);
    }

    private void Awake()
    {
        enemyTr = transform;
    }

    void Update()
    {
        PlayerHasBeenSeen(playerTr.position);
    }

    private void DetectPlayer(bool playerHasBeenSeen)
    {
        if(playerHasBeenSeen)
        {
            warning.SetActive(true);
            detect.SetActive(false);

            if(timeOnSight <= 3)
            {
                timeOnSight += Time.deltaTime;
            }

            if(!(timeOnSight >= 3)) return;
            warning.SetActive(false);
            detect.SetActive(true);
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
                Debug.DrawRay(enemyTr.position, displacement.normalized * hit.distance, Color.red);
                Debug.Log("tas muerto");

                if(!hit.collider.GetComponent<Player>()) return false;

                Debug.DrawRay(enemyTr.position, displacement.normalized * hit.distance, Color.green);
                return true;
            }
        }
        return false;
    }
}
