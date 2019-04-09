using UnityEngine;
using UnityEngine.UI;

// Basic UI for the Healthbar
public class HealthPoints : MonoBehaviour {
    private bool newRPoint;
    private TerrainGenerator tg;
    private SwarmFactory sf;
    private Vector3 charPos;
    private Vector3 vectorRP;
    private GameObject swarmController;

    public Image currentHealthBar;
    public Text ratioText;
    [SerializeField] public float damage = 1.5f;        // change higher to make it more difficult
    private float hitpoint = 100;
    private float maxHitpoint = 100;

    // Use this for initialization
    void Start () {
        tg = GameObject.Find("TerainController").GetComponent<TerrainGenerator>();
        sf = GameObject.Find("Swarm Factory").GetComponent<SwarmFactory>();
        InvokeRepeating("DoT", 1, 0.5f);
        UpdateHealthbar();


    }

    private void UpdateHealthbar()
    {
        float ratio = hitpoint / maxHitpoint;
        currentHealthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        ratioText.text = (ratio * 100).ToString("0") + "%";
    }

    private void DoT()
    {
        //Damage over Time
        hitpoint -= damage;
        if(hitpoint < 0)
        {

            sf.removeBoid();
            hitpoint = 0;

        }
        UpdateHealthbar();
    }

    private void heal()
    {
        // called if controller reached the recharge point
        Debug.Log(" Healed by 15 Units");
        hitpoint += 15;
        if(hitpoint > maxHitpoint)
        {
            hitpoint = maxHitpoint;
        }
        UpdateHealthbar();
    }
	
	// Update is called once per frame
	void Update () {
        charPos = GameObject.Find("SwarmController").GetComponent<controllerPos>().getPos();            // Position of controller at Start
        vectorRP = GameObject.Find("RechargePoint(Clone)").transform.position;                          // Position of recharge point     


        if (Vector3.Distance(charPos, vectorRP) < 7)                                                    // If swarm is near the recharge point
        {
            heal();
            tg.removeRechargePoint();
            if (tg.checkStorage() == true)
            {
                tg.placeRechargePoint();
            }
        }
	}
}
