using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Generates new Terrain if character is wallked a certain distance.
public class TerrainGenerator : MonoBehaviour
{

    public GameObject TilePrefab;
    public GameObject character;
    public GameObject RechargePrefab;
    private GameObject recharge;
    private Vector3 controllerPos;
    private int TileVersionCounter;
    private string tVersion;
    private float xPosRecharPoint;
    private float zPosRecharPoint;
    private float yPosRecharPoint;
    private int TileSize = 10;
    [SerializeField] private int radiusX = 10;
    [SerializeField] private int radiusZ = 10;
    private Dictionary<string, Vector3> Storage;

    void Start()
    {

        controllerPos = GameObject.Find("SwarmController").GetComponent<controllerPos>().getPos();            // Position of controller at Start
        Storage = new Dictionary<string, Vector3>();
        for(int x = -radiusX; x <= radiusX; x++)
        {
            for(int z = -radiusZ; z <= radiusZ; z++)
            {
                Vector3 place = new Vector3(x*TileSize + controllerPos.x, 0, z* TileSize + controllerPos.z);        // radius X/Z determines how far Terrain is rendered and Size

                Instantiate(TilePrefab, place, Quaternion.identity);
                tVersion = "Tile:   " + " x:_ " + place.x.ToString() + " y:_ " + place.y.ToString() + " z:_ " + place.z.ToString();
                Storage.Add(tVersion, place);

            }
        }
        placeRechargePoint();
    }

    public void placeRechargePoint()
    {
        // 1 RechargePoint near controller at start 
        xPosRecharPoint = Random.Range(controllerPos.x + TileSize * 5, controllerPos.x + 7 * TileSize);         // random x pos between 5 Tiles and 8 Tiles around player
        zPosRecharPoint = Random.Range(controllerPos.z + TileSize * 5, controllerPos.z + 7 * TileSize);         // also for z pos
        yPosRecharPoint = controllerPos.y;                                                                      // same height as player
        Vector3 vectorRP = new Vector3(xPosRecharPoint, yPosRecharPoint, zPosRecharPoint);
        recharge = Instantiate(RechargePrefab, vectorRP, Quaternion.identity);
        recharge.transform.Rotate(-90, 0, 0);
        string stringRP = "RechargePoint";
        Storage.Add(stringRP, vectorRP);
    }

    public bool checkStorage()
    {
        // only 1 Rechargepoint at the time
        if (Storage.ContainsKey("RechargePoint"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void removeRechargePoint()
    {
        Storage.Remove("RechargePoint");
        if (recharge != null)
        {
            Destroy(recharge);
        }
    }

    void Update()
    {
        float movedDist = Vector3.Distance(character.transform.position, controllerPos);
        if (movedDist >= TileSize)                                                          // If Character is moved a TileSize wide from previous position, create new Tiles
        {
            controllerPos = character.transform.position;
            // Calc the proper Tile in which character is now(after walking)
            int PosXinNewTile = (int)(Mathf.Floor(character.transform.position.x / TileSize) * TileSize);
            int PosYinNewTile = (int)(Mathf.Floor(character.transform.position.z / TileSize) * TileSize);

            for (int x = -radiusX; x <= radiusX; x++)
            {
                for (int z = -radiusZ; z <= radiusZ; z++)
                {

                    Vector3 place = new Vector3(x * TileSize + PosXinNewTile, 0, z * TileSize + PosYinNewTile);
                    tVersion = "Tile:   " + " x:_ " + place.x.ToString() + " y:_ " + place.y.ToString() + " z:_ " + place.z.ToString();
                    if (!Storage.ContainsKey(tVersion))            // Checks if Key is not in TileStorage and adding to it.
                    {
                        Instantiate(TilePrefab, place, Quaternion.identity);

                        Storage.Add(tVersion, place);
                    }
                }
            }

        }
        recharge.transform.Rotate(0, 0 , 55 * Time.deltaTime);      // rotate the prefab overtime
    }



}