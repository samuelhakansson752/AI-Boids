using System.Collections.Generic;
using UnityEngine;

public class ControllScript : MonoBehaviour
{
    public Camera screen;

    public List<Boid> AllBoids;

    public float maxSpeed;

    public float screenEdgeWeight;
    public float separationRadius;
    public float separationWeight;
    public float alignmentWeight;
    public float cohessionWeight;

    private void Start()
    {
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");

        foreach (GameObject boid in boids)
        {
            AllBoids.Add(boid.GetComponent<Boid>());
        }
    }

    private void Update()
    {
        foreach (Boid boid in AllBoids)
        {
            boid.screenSize.x = screen.pixelWidth;
            boid.screenSize.y = screen.pixelHeight;
            boid.screenPosition = screen.WorldToScreenPoint(boid.transform.position);
            boid.maxSpeed = maxSpeed;
            boid.screenEdgeWeight = screenEdgeWeight;
            boid.separationRadius = separationRadius;
            boid.separationWeight = separationWeight;
            boid.alignmentWeight = alignmentWeight;
            boid.cohessionWeight = cohessionWeight;
        }
    }
}
