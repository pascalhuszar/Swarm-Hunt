using UnityEngine;
using System.Collections.Generic;

// Regulate the steering Behavoiur
// Included Forces: Separation, Alignment, Cohesion as "Controller Position gravity", Avoidance, away fromn center as antiCenter
public class BoidBehaivour : MonoBehaviour {


    public float neighbourHoodRange = 10;           // scope for boid
    public float separationDist = 10;               // min gap between boids
    private float maxForce = 15;                    // max value for force 
    public float gravForce = 15;                    // beyond, the gravity get stronger
    private float smoothSpeed = 100.0f;
    private Collider[] boids; 
    public Vector3 avgVelo;
    public Vector3 controllerPos;
    public Vector3 gravity;
    private Vector3 acceleration;
    private Vector3 separation;
    private Vector3 cohesion;
    public Vector3 velocity;
    private Vector3 antiCenter;
    public float cohWeight;                 
    public float sepWeight;
    public float aligWeight;
    public float gravWeight;


    private void Start()
    {
        InvokeRepeating("applyForce", 0, 0.5f);
        
    }
  
    void applyForce()      
    {
        velocity = Vector3.zero;
        cohesion = Vector3.zero;
        separation = Vector3.zero;
        avgVelo = Vector3.zero;
        controllerPos = Vector3.zero;
        antiCenter = Vector3.zero;
        gravity = Vector3.zero;

        

        cohWeight = 1.5f;                 //weight the forces..
        sepWeight = 5.0f;
        aligWeight = 5.5f;
        gravWeight =  1.5f;

        int neighbourCounterSep = 0;
        int neighbourCounterCoh = 0;

        boids = Physics.OverlapSphere(transform.position, neighbourHoodRange);

        controllerPos = GameObject.Find("SwarmController").GetComponent<controllerPos>().getPos();      // Position of controller

        foreach (var boid in boids)
        {

            //___ calc sep force in given radius
            float distToOthers = Vector3.Distance(transform.position, boid.transform.position); 
            if ((boid != GetComponent<Collider>()) && (distToOthers < separationDist))  
            {
                separation += (transform.position - boid.transform.position);      // Raynolds "steering formula"
                separation += separation / distToOthers;                           // weight, if near to other boid -> force get stronger
                neighbourCounterSep++;
            }

            //___ calc coh force in given radius
            if (distToOthers > 0 && distToOthers < neighbourHoodRange)
            {
                cohesion += boid.transform.position;
                neighbourCounterCoh++;

            }

            //___ Calc grav force to controller position
            gravity += (controllerPos - transform.position);           
            gravity = Vector3.ClampMagnitude(gravity, maxForce);
            float distToCenter = (transform.position - controllerPos).magnitude;
            if (distToCenter > 10 && distToCenter < 20)
            {
                gravWeight = 5;
            }
            else if (distToCenter > 20)
            {
                gravWeight = 20;
            }

            //____ Calc force away from center
            if (distToCenter > 0 && distToCenter < 5)
            {
                antiCenter = (transform.position - controllerPos);
                Debug.DrawRay(transform.position, antiCenter, Color.black);
            }

            //__ Calc average velocity of local group
            avgVelo += boid.GetComponent<BoidBehaivour>().velocity;


        }

        if(neighbourCounterCoh > 0)
        {
            cohesion = cohesion / neighbourCounterCoh;          // average position of local group
            cohesion = cohesion - transform.position;           // Raynolds "steering formula"
            cohesion = Vector3.ClampMagnitude(cohesion, maxForce);
        }
        if (neighbourCounterSep > 0)
        {
            separation = separation / neighbourCounterSep;
            separation = Vector3.ClampMagnitude(separation, maxForce);

        }


        avgVelo = avgVelo / boids.Length;
        avgVelo = Vector3.ClampMagnitude(avgVelo, maxForce);



        velocity += cohesion*cohWeight + separation*sepWeight + avgVelo*aligWeight + gravity*gravWeight + antiCenter;       //add up all forces to get overall steering behaivour
        velocity = Vector3.ClampMagnitude(velocity, maxForce);
    }


    void Update()
    {
        transform.position += velocity * Time.deltaTime;
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), smoothSpeed * Time.deltaTime);    
        }

        // Debug Ray's
        Debug.DrawRay(transform.position, velocity, Color.white);
        //Debug.DrawRay(transform.position, antiCenter, Color.black);
        //Debug.DrawRay(transform.position, avgVelo, Color.blue);
        //Debug.DrawRay(transform.position, cohesion, Color.magenta);
        //Debug.DrawRay(transform.position, separation, Color.green);
        //Debug.DrawRay(transform.position, gravity, Color.yellow);
        
        

    }

}
