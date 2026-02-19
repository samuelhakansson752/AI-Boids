using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 velocity;
    public float maxSpeed;
    public List<Boid> Neighbors;

    public float separationRadius;
    public float separationWeight;
    public float alignmentWeight;
    public float cohessionWeight;

    private void Start()
    {
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");

        foreach (GameObject boid in boids)
        {
            if (boid != gameObject)
            {
                Neighbors.Add(boid.GetComponent<Boid>());
            }
        }
    }

    private void Update()
    {
        Vector3 acceleration = Vector3.zero;

        acceleration += CalculateSeparation(Neighbors) * separationWeight;
        acceleration += CalculateAlignment(Neighbors) * alignmentWeight;
        acceleration += CalculateCoheion(Neighbors) * cohessionWeight;

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocity.normalized);
    }

    private Vector3 CalculateSeparation(List<Boid> neighbors)
    {
        Vector3 force = Vector3.zero;

        foreach (Boid boid in neighbors)
        {
            float distance = Vector3.Distance(gameObject.transform.position, boid.transform.position);

            if (distance < separationRadius && distance > 0)
            {
                force += (gameObject.transform.position - boid.transform.position) / distance;
            }
        }

        return force;
    }

    private Vector3 CalculateAlignment(List<Boid> neighbors)
    {
        if (neighbors.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 avgVelocity = Vector3.zero;

        foreach (Boid boid in neighbors)
        {
            avgVelocity += boid.velocity;
        }

        avgVelocity /= neighbors.Count;

        return avgVelocity - velocity;
    }

    private Vector3 CalculateCoheion(List<Boid> neighbors)
    {
        if (neighbors.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 center = Vector3.zero;

        foreach (Boid boid in neighbors)
        {
            center += boid.transform.position;
        }

        center /= neighbors.Count;

        return center - transform.position;
    }
}
