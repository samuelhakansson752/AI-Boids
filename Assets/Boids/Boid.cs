using NUnit.Framework;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 screenPosition;
    public Vector2 screenSize;
    public Vector3 velocity;
    public float MinMaxY;
    public float maxSpeed;
    public List<Boid> Neighbors;
    public List<Predator> Predators;

    public float screenEdgeWeight;
    public float separationRadius;
    public float separationWeight;
    public float alignmentWeight;
    public float cohessionWeight;
    public float predatorAvoidance;
    public float predDistanceWeight;

    public bool isActive = false;

    private void Start()
    {
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");
        GameObject[] preds = GameObject.FindGameObjectsWithTag("Predator");

        foreach (GameObject boid in boids)
        {
            if (boid != gameObject)
            {
                Neighbors.Add(boid.GetComponent<Boid>());
            }
        }

        foreach (GameObject predator in preds)
        {
            Predators.Add(predator.GetComponent<Predator>());
        }
    }

    private void Update()
    {
        if (isActive)
        {
            Vector3 acceleration = Vector3.zero;

            acceleration += CalculateSeparation(Neighbors) * separationWeight;
            acceleration += CalculateAlignment(Neighbors) * alignmentWeight;
            acceleration += CalculateCoheion(Neighbors) * cohessionWeight;
            acceleration += StayOnScreen() * screenEdgeWeight;
            acceleration += AvoidPredator() * predatorAvoidance;

            velocity += acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            transform.position += velocity * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(velocity.normalized);
        }
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

    private Vector3 AvoidPredator()
    {
        Vector3 avoidance = Vector3.zero;

        foreach (Predator pred in Predators)
        {
            float dist = Vector3.Distance(pred.transform.position, transform.position);

            avoidance += pred.velocity * (predDistanceWeight / dist);
        }

        return avoidance;
    }
}
