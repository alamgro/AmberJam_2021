using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] fishes;
    [SerializeField] private Collider2D spawnArea;
    [SerializeField] private float xOffset; //Offsets for the object when it's instantiated
    private Camera cam;
    private float posOffset; //Final x position with the offset applied
    private float timerSpawn;


    void Start()
    {
        cam = Camera.main;
        posOffset = (cam.orthographicSize * cam.aspect) + xOffset; // half the horizontal size of the camera
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        timerSpawn += Time.deltaTime;

        if(timerSpawn >= 0.5f)
        {
            int randIndex = Random.Range(0, 15);
            //Special fish spawns with 1/15 of propability
            if(randIndex != 0)
                Instantiate(fishes[0], RandomPosition(), fishes[0].transform.rotation); //Normal fish
            else
                Instantiate(fishes[1], RandomPosition(), fishes[1].transform.rotation); //Special fish

            timerSpawn = 0f;
        }
    }

    private Vector2 RandomPosition()
    {
        int randSide = Random.Range(0, 2);
        Vector3 spawnPos;

        if (randSide == 0)
        {
            //Spawn lado izquierdo
            spawnPos = Vector3.left * posOffset;
        }
        else
        {
            //Spawn lado derecho
            spawnPos = Vector3.right * posOffset;
        }

        spawnPos.y += Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);

        return spawnPos;
    }
}
