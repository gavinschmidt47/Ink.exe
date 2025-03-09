using UnityEngine;

public class Attack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            Destroy(other.gameObject);
        }
    }
}
