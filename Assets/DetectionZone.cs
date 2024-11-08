using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new List<Collider2D>(); // danh sách các collider trong detection zone
    public Collider2D col;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu collider có tag là "Player"
        if (collision.CompareTag("Player"))
        {
            detectedColliders.Add(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Kiểm tra nếu collider có tag là "Player"
        if (collision.CompareTag("Player"))
        {
            detectedColliders.Remove(collision);
        }
    }
}
