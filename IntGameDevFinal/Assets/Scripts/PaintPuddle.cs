using UnityEngine;

public class PaintPuddle : MonoBehaviour
{
    public enum PaintType
    {
        SpeedUp,
        SlowDown,
        DamageUp
    }

    public PaintType paintType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (paintType == PaintType.SpeedUp)
            {
                player.playerSave.speed *= 2;
            }
            else if (paintType == PaintType.SlowDown)
            {
                player.playerSave.speed /= 2;
            }
            else if (paintType == PaintType.DamageUp)
            {
                player.playerSave.damage *= 2;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (paintType == PaintType.SpeedUp)
            {
                player.playerSave.speed /= 2;
            }
            else if (paintType == PaintType.SlowDown)
            {
                player.playerSave.speed *= 2;
            }
            else if (paintType == PaintType.DamageUp)
            {
                player.playerSave.damage /= 2;
            }
        }
    }
}
