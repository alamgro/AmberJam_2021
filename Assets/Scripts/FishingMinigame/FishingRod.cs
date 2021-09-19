using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[SelectionBase]
public class FishingRod : MonoBehaviour
{
    [SerializeField] private float speed; //Fishin rod speed
    [SerializeField] private Transform positionBait; //reference to the bait Transform
    [SerializeField] private Transform positionBaitLimit; //The limit of the bait in Y position
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Image forceIndicatorUI;
    [SerializeField] private Transform crosshair;
    [SerializeField] private float idleTime; //The time that it takes to return the bait to the rod
    [SerializeField] private float maxTimePressing; //The maximum time that it takes to fill the force indicator
    private Rigidbody2D rb;
    private Collider2D baitCollider;
    private Vector3 baitTarget;
    private float forceTimer = 0f;
    private float idleTimer = 0f;
    private bool isThrowing = false;
    private bool resetBait = false;
    private bool increaseIndicator = true; //Defines if the force indicator increases or decreases

    void Start()
    {
        baitCollider = positionBait.GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        positionBait.position = lineRenderer.transform.position; //Set bait to its initial position
        crosshair.position = positionBait.position;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if (!isThrowing)
        {
            rb.velocity = Vector2.right * speed * Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                increaseIndicator = true;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                if (increaseIndicator)
                {
                    forceTimer += Time.deltaTime;
                    if (forceTimer >= maxTimePressing) 
                        increaseIndicator = false;
                }
                else
                {
                    forceTimer -= Time.deltaTime;
                    if (forceTimer <= 0f)
                        increaseIndicator = true;
                }

                MoveCrosshair(forceTimer / maxTimePressing);
                forceIndicatorUI.fillAmount = forceTimer / maxTimePressing; //Update Force Indicator on UI
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                MoveCrosshair(0f);

                rb.velocity = Vector2.zero; //Stop fishing rod

                isThrowing = true;
                baitCollider.enabled = false;
                increaseIndicator = false;

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
                positionBait.position = Vector2.MoveTowards(positionBait.position, baitTarget, 15.0f * Time.deltaTime);

                resetBait = positionBait.position == baitTarget;
            }
            else if(positionBait.position != lineRenderer.transform.position)
            {
                //wait to start returning
                if(idleTimer >= idleTime)
                {
                    //Return bait to the fishing rod
                    positionBait.position = Vector2.MoveTowards(positionBait.position, lineRenderer.transform.position, 15.0f * Time.deltaTime);
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

    private void MoveCrosshair(float _force)
    {
        float finalY = (positionBaitLimit.position.y - positionBait.position.y); //Calculate reachable distance
        finalY *= _force; //Adjust position relative to force applied
        finalY -= Mathf.Abs(positionBait.position.y);
        //finalY = finalY < 0.5f ? 0.5f : finalY;

        crosshair.position = new Vector2(transform.position.x, finalY);
    }

}

