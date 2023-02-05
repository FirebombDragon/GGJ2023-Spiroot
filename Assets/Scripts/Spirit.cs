using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private InputController controls;
    private GameObject player;
    private bool collected = false;
    private bool following = false;

    float distance;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);
        if (collected)
        {
            if (following && distance <= 4)
                following = false;
            if (!following && distance > 4)
                following = true;
        }
        Debug.Log(following);
        if (following)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Spirit Collected");
        collected = true;
    }
}
