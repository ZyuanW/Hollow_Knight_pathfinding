using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;



    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
        InvokeRepeating("UpdatePath", 0f, .5f); // update path every 0.5 seconds
        // seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {

            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // move enemy
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; // direction to next waypoint
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }



    }
}
