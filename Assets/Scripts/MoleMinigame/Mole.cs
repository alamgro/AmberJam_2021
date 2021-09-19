using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public bool goOut = false;

    [SerializeField] private int expValue;
    [SerializeField] private GameObject particlesExp;

    private float spriteHeight;
    private Vector3 originalPos;
    private Vector3 targetPos;
    private Collider2D collMole;

    void Start()
    {
        spriteHeight = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        originalPos = transform.position;
        targetPos = new Vector2(originalPos.x, originalPos.y + spriteHeight);
        collMole = GetComponent<Collider2D>();
        collMole.enabled = false;
    }

    void Update()
    {
        if (goOut)
        {
            Peekaboo(transform.position, targetPos);
            collMole.enabled = true;
        }
        else
        {
            Peekaboo(transform.position, originalPos);
            collMole.enabled = false;
        }
    }

    public IEnumerator IEPikaboo(float _time)
    {
        goOut = true;
        WhackamoleControler.Instance.MolesOutside++;

        yield return new WaitForSecondsRealtime(_time);
        WhackamoleControler.Instance.MolesOutside--;
        goOut = false;
        //print("Bajar");
    }

    private void Peekaboo(Vector3 _fromPosition, Vector3 _toPosition)
    {
        if (_fromPosition != _toPosition)
        {
            transform.position = Vector2.MoveTowards(_fromPosition, _toPosition, 5.0f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            WhackamoleControler.Instance.LevelExperience += expValue;
            Instantiate(particlesExp, transform.position, Quaternion.identity);
            goOut = false;
        }
    }


}
