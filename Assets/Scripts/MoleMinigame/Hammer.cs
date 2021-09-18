using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private Collider2D collHammer;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        collHammer = GetComponent<Collider2D>();
        collHammer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(UseHammer());
        }

        transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private IEnumerator UseHammer()
    {
        collHammer.enabled = true;
        transform.Rotate(Vector3.left * 50f);
        yield return new WaitForSecondsRealtime(0.1f);
        transform.Rotate(Vector3.right * 50f);

        collHammer.enabled = false;
    }
}
