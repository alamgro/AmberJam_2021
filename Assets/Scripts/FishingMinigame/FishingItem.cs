using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingItem : MonoBehaviour
{
    [SerializeField] private int expValue; //Experience given when caught
    [SerializeField] private float minSpeed, maxSpeed; 
    [SerializeField] private float rotationSpeed; 
    [SerializeField] private GameObject particlesExp; 

    private float currentSpeed; 
    private float randRotation; 
    private Vector3 direction; //Movement direction
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        randRotation = Random.Range(10f, 20f);
        //Calculate direction based on the current position
        direction = -(transform.position * Vector2.right).normalized;

        currentSpeed = Random.Range(minSpeed, maxSpeed);
        //Debug.Log(direction, gameObject);
        if(direction.x > 0f)
            sprite.flipX = sprite.flipY = true;

        direction.z = 1f;

    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 40f + Mathf.PingPong(Time.time * rotationSpeed, randRotation));
        //Move
        transform.Translate(direction * Vector2.right * currentSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Bait"))
        {
            FishingController.Instance.LevelExperience += expValue;
            Instantiate(particlesExp, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Edge"))
        {
            Destroy(gameObject);
        }
    }

}
