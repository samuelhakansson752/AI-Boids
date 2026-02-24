using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Animations;

public class Predator : MonoBehaviour
{
    public List<Boid> AllBoids;

    public Vector3 screenPosition;
    public Vector2 screenSize;
    public Vector3 velocity;
    public float MinMaxY;
    public float maxSpeed;
    public float chaseAttraction;

    public float screenEdgeWeight;

    public bool isActive = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isActive)
        {
            Vector3 acceleration = Vector3.zero;

            acceleration += StayOnScreen() * screenEdgeWeight;
            acceleration += ChaseBoids() * chaseAttraction;

            velocity += acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            transform.position += velocity * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(velocity.normalized);
        }
    }

    private Vector3 ChaseBoids()
    {
        Boid targetBoid = AllBoids[0];
        float prevDist = 0;

        foreach (Boid boid in AllBoids)
        {
            float combDist = 0;

            foreach (Boid neighbor in boid.Neighbors)
            {
                combDist += Vector3.Distance(boid.transform.position, neighbor.transform.position);
            }

            if (combDist < prevDist)
            {
                targetBoid = boid;
            }
            else
            {
                prevDist = combDist;
            }
        }

        Vector3 chaseDirection = targetBoid.transform.position - transform.position;

        return chaseDirection;
    }

    private Vector3 StayOnScreen()
    {
        Vector3 force = Vector3.zero;

        // More Force if postion x/y goes closer to screen max x/y
        force.x += 1 / (screenPosition.x - screenSize.x);
        force.z += 1 / (screenPosition.y - screenSize.y);

        // If position x/y goes closer to screen 0, 0
        force.x += 1 / (screenPosition.x);
        force.z += 1 / (screenPosition.y);

        //-y force if position.y is high and opposite
        force.y += 1 / (MinMaxY - -transform.position.y);
        force.y += 1 / (-MinMaxY - -transform.position.y);

        return force;
    }
}
