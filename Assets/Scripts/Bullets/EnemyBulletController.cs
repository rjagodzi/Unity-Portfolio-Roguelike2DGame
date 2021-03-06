using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    private Vector3 playerDirection;

    [SerializeField] int bulletDamageAmmount;

    // Start is called before the first frame update
    void Start()
    {
        playerDirection = FindObjectOfType<PlayerController>().transform.position - transform.position;
        playerDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += playerDirection * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealthHandler>().DamagePlayer(bulletDamageAmmount);
        }
            Destroy(gameObject);
    }
}
