using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generate the perlin noise and tree destribution of single mesh
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] public float NoiseScale;
    [SerializeField] public float TerrainHeight;
    [SerializeField] public int seed = 777;       // avoid symmetry, set value between 10 and 10.000
    public GameObject treePrefab;
    private Vector3 place;




    // Start is called before the first frame update
    void Start()
    {

        Mesh terrainTile = this.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = terrainTile.vertices;          // store all vertices in an array

        for (int i = 0; i < vertices.Length; i++)
        {
            //___ Calc the y properties foreach vertice with an offset, so that each vertices dont look the same
            vertices[i].y = Mathf.PerlinNoise(((vertices[i].x + this.transform.position.x)/NoiseScale)+ seed,            
                ((vertices[i].z + this.transform.position.z)/ NoiseScale)+ seed) * TerrainHeight;       // *amp/gain
            
            place = new Vector3(vertices[i].x + this.transform.position.x, vertices[i].y, vertices[i].z + this.transform.position.z);
            if(vertices[i].y > 2.2f && vertices[i].y < 3.0f)   // middle part of perlin noise(values)
            {
                // distribute trees, slightly
                place.x += Random.Range(1.0f, 2.0f);
                place.z += Random.Range(1.0f, 2.0f);
                float rotation = Random.Range(0, 360f);
                
                GameObject pine = Instantiate(treePrefab, place , Quaternion.identity);
                pine.transform.Rotate(new Vector3(-90.0f, rotation), Space.World);
            }
            
        }


        terrainTile.vertices = vertices;            // set "old" vertices to new calculated
        terrainTile.RecalculateBounds();            // After modyfing vertices, should call this function
        terrainTile.RecalculateNormals();           // -...-
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
