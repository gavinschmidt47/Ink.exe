using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShieldGenerator : MonoBehaviour
{

    public float totalCharge;
    public float rechargeDelay;
    public Transform playerTransform;
    public GameObject shield;
    private float remainingCharge;
    private float lastHitTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        remainingCharge = totalCharge;
        lastHitTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingCharge > 0)
        {
            RepelEnemies();
            shield.SetActive(true);
            float frac = remainingCharge / totalCharge;
            shield.GetComponent<MeshRenderer>().material.color = new Color(frac, frac, frac, frac);
        }
        else
        {
            shield.SetActive(false);
        }
        if (Time.time > lastHitTime + rechargeDelay)
        {
            remainingCharge += Time.deltaTime * totalCharge;
            remainingCharge = Mathf.Min(remainingCharge, totalCharge);
        }
        shield.transform.position = playerTransform.position;
    }
    void RepelEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(shield.transform.position, enemy.transform.position);
            float radius = shield.transform.localScale.x/2f;
            if (distance < radius)
            {
                lastHitTime = Time.time;
                Vector3 direction = enemy.transform.position - shield.transform.position;
                Vector3 newPosition = direction.normalized * radius + shield.transform.position;
                enemy.transform.position = newPosition;
                remainingCharge -= enemy.GetComponent<EnemyController>().damage * Time.deltaTime;
            }
        }
    }
}
