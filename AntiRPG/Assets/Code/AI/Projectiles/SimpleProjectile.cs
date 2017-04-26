using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{

    public float Damage = 10f;
    public float Speed = 1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            player.Damage(Damage);
            Destroy(gameObject);

        }
    }
}
