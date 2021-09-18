using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[SelectionBase]
public class FishingRod : MonoBehaviour
{
    [SerializeField] private Transform positionBait; //reference to the bait Transform
    [SerializeField] private Transform positionBaitLimit; //The limit of the bait in Y position
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image forceIndicatorUI;
    [SerializeField] private float idleTime; //The time that it takes to return the bait to the rod
    [SerializeField] private float maxTimePressing; //The maximum time that it takes to fill the force indicator
    private Collider2D baitCollider;
    private Vector3 baitTarget;
    private float forceTimer = 0f;
    private float idleTimer = 0f;
    private bool isThrowing = false;
    private bool resetBait = false;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        baitCollider = positionBait.GetComponent<Collider2D>();

        positionBait.position = lineRenderer.transform.position; //Set bait to its initial position
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if (!isThrowing)
        {
            if (Input.GetMouseButton(0))
            {
                forceTimer += Time.deltaTime;
                if (forceTimer >= maxTimePressing) forceTimer = 0f;

                forceIndicatorUI.fillAmount = forceTimer / maxTimePressing; //Update Force Indicator on UI

                transform.position = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y);
                //transform.position = Vector2.MoveTowards(transform.position, new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y), 5f * Time.deltaTime);

                positionBait.position = lineRenderer.transform.position; //Reset bait to its initial position

            }
            else if (Input.GetMouseButtonUp(0))
            {
                isThrowing = true;
                baitCollider.enabled = false;

                baitTarget = GetBaitTrajectory(forceTimer / maxTimePressing);

                //Reset timer and force indicator
                forceTimer = forceIndicatorUI.fillAmount = 0f;
            }
        }
        else
        {
            if (!resetBait && positionBait.position != baitTarget)
            {
                //Move bait to target
                positionBait.position = Vector2.MoveTowards(positionBait.position, baitTarget, 7.0f * Time.deltaTime);

                resetBait = positionBait.position == baitTarget;
            }
            else if(positionBait.position != lineRenderer.transform.position)
            {
                //wait to start returning
                if(idleTimer >= 0.5f)
                {
                    //Return bait to the fishing rod
                    positionBait.position = Vector2.MoveTowards(positionBait.position, lineRenderer.transform.position, 7.0f * Time.deltaTime);
                    baitCollider.enabled = true;
                }
                else
                    idleTimer += Time.deltaTime;
            }
            else
            {
                //Restart fishing rod parameters
                baitCollider.enabled = false;
                idleTimer = 0f;
                resetBait = false;
                isThrowing = false;
            }
        }

    }

    private void FixedUpdate()
    {
        lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
        lineRenderer.SetPosition(1, positionBait.position);
    }

    private Vector3 GetBaitTrajectory(float _force)
    {
        float randX = positionBait.position.x + (Random.Range(-1f, 1f) * _force); //Generate a slight random deviation
        float finalY = positionBaitLimit.position.y - positionBait.position.y; //Calculate max reachable distance
        finalY *= _force; //Adjust final throwing force
        finalY = finalY < 0.5f ? 0.5f : finalY; //The mínimum distance would be 0.5

        return new Vector2(randX, positionBait.position.y + finalY); //Return position where the bait will land
    }

}

