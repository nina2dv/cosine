using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject deadEffectObj;
    public GameObject itemeffectObj;

    Rigidbody2D rb;
    float angle = 0;

    int xSpeed = 3;
    int ySpeed = 30;

    GameManager gameManager;

    bool isDead = false;

    float hueValue;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        hueValue = UnityEngine.Random.Range(0, 10) / 10.0f;
        SetBackgroundColour();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == true) return;
        MovePlayer();
        GetInput();
    }

    void MovePlayer()
    {
        Vector2 pos = transform.position;
        pos.x = Mathf.Cos(angle) * 5;
        // pos.y = 0;
        transform.position = pos;
        angle += Time.deltaTime * xSpeed;

    }
    void GetInput()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {

            rb.AddForce(new Vector2(0, ySpeed));
        }
        else
        {
            if (rb.velocity.y > 0)
            {
                rb.AddForce(new Vector2(0, -ySpeed/2f));

            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Dead();
        }
        else if (other.gameObject.tag == "Item")
        {
            GetItem(other);
        }
    }

    void GetItem(Collider2D other)
    {
        SetBackgroundColour();

        Destroy(Instantiate(itemeffectObj, other.gameObject.transform.position, Quaternion.identity), 0.5f);
        Destroy(other.gameObject.transform.parent.gameObject);
        gameManager.AddScore();
    }

    void Dead()
    {
        isDead = true;
        StartCoroutine(Camera.main.gameObject.GetComponent<CameraShake>().Shake());

        Destroy(Instantiate(deadEffectObj, transform.position, Quaternion.identity), 0.5f);

        StopPlayer();

        gameManager.CallGameOver();
    }

    void StopPlayer()
    {
        rb.velocity = new Vector2(0, 0);
        rb.isKinematic = true;
    }

    void SetBackgroundColour()
    {
        Camera.main.backgroundColor = Color.HSVToRGB(hueValue, 0.15f, 0.8f);

        hueValue += 0.1f;

        if(hueValue >= 1)
        {
            hueValue = 0;
        }
    }
}
