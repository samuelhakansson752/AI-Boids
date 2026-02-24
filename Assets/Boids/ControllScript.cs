using System.Collections.Generic;
using UnityEngine;

public class ControllScript : MonoBehaviour
{
    public Camera screen;

    public List<Boid> AllBoids;
    public List<Predator> Predators;

    public float maxSpeed;
    public float MinMaxY;

    public float screenEdgeWeight;
    public float separationRadius;
    public float separationWeight;
    public float alignmentWeight;
    public float cohessionWeight;
    public float predatorAvoidance;
    public float predDistanceWeight;

    public bool initialized;

    private void Start()
    {
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");
        GameObject[] preds = GameObject.FindGameObjectsWithTag("Predator");

        foreach (GameObject boid in boids)
        {
            AllBoids.Add(boid.GetComponent<Boid>());
        }
        foreach (GameObject predator in preds)
        {
            Predators.Add(predator.GetComponent<Predator>());
        }
    }

    private void Update()
    {
        foreach (Boid boid in AllBoids)
        {
            boid.screenSize.x = screen.pixelWidth;
            boid.screenSize.y = screen.pixelHeight;
            boid.screenPosition = screen.WorldToScreenPoint(boid.transform.position);
            boid.MinMaxY = MinMaxY;
            boid.maxSpeed = maxSpeed;
            boid.screenEdgeWeight = screenEdgeWeight;
            boid.separationRadius = separationRadius;
            boid.separationWeight = separationWeight;
            boid.alignmentWeight = alignmentWeight;
            boid.cohessionWeight = cohessionWeight;
            boid.predatorAvoidance = predatorAvoidance;
            boid.predDistanceWeight = predDistanceWeight;
}

        foreach (Predator pred in Predators)
        {
            pred.MinMaxY = MinMaxY;
            pred.screenSize.x = screen.pixelWidth;
            pred.screenSize.y = screen.pixelHeight;
            pred.screenPosition = screen.WorldToScreenPoint(pred.transform.position);
            pred.AllBoids = AllBoids;
        }

        if (!initialized)
        {
            initialized = !initialized;
            foreach (Boid boid in AllBoids)
            {
                boid.isActive = true;
            }
            foreach (Predator pred in Predators)
            {
                pred.isActive = true;
            }
        }
    }
}
