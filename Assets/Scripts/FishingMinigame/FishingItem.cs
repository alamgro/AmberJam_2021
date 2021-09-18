using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingItem : MonoBehaviour
{
    [SerializeField] private float speed; 
    [SerializeField] private int expValue; //Experience given when caught
    private Vector3 direction; //Movement direction

    void Start()
    {
        //Calculate direction based on the current position
        direction = -(transform.position * Vector2.right).normalized;
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        //Move
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Bait"))
        {
            FishingController.Instance.LevelExperience += expValue;
            
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Edge"))
        {
            Destroy(gameObject);
        }


    }
}
