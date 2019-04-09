using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Initialse the boids
public class SwarmFactory : MonoBehaviour
    
{

    public GameObject boidPrefab;
    public List<GameObject> boidList;
    private GameObject boid;
    [SerializeField] private int swarmSize = 20;
    [SerializeField] private float minX = -20;
    [SerializeField] private float maxX = 20;
    [SerializeField] private float minY = 3;
    [SerializeField] private float maxY = 20;
    [SerializeField] private float minZ = -20;
    [SerializeField] private float maxZ = 20;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(" Have FUN! ");
        boidList = new List<GameObject>();
        for (int i = 0; i < swarmSize; i++)
        {
            boid = Instantiate(boidPrefab);
            boidList.Add(boid);
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            float z = Random.Range(minZ, maxZ);

            boidList[i].transform.localPosition = new Vector3(x, y, z);

        }

    }

    public void removeBoid()
    {
        // If health is below 0, in sequence boids get destroyed
        if (boidList.Count > 0)
        {
                GameObject deadBoid = boidList[swarmSize-1];
                boidList.Remove(deadBoid);
                Debug.Log("Your boid dies !!");
                Destroy(deadBoid);
                swarmSize--;
        }else
        {
            Debug.Log("There are no boids left! \n ++++++++++++++++++++++++ RESTART THE GAME ++++++++++++++++++++++++ ");
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
