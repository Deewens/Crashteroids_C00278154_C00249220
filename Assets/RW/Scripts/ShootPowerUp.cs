using System;
using UnityEngine;


public class ShootPowerUp : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private float maxY = -5;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.down * (Time.deltaTime * speed));
        if (transform.position.y < maxY)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "ShipModel")
        {
            Ship player = collision.gameObject.GetComponent<Ship>();
            player.ActivateShootPowerUp();

            Destroy(gameObject);
        }
    }
}