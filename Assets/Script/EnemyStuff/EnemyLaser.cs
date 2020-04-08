using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    //keep public
    public float speed = 8.0f;
    private Player _playerReference;

    private void Start()
    {
        _playerReference = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (!_playerReference.timeStopActive)
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (_playerReference.timeStopActive)
            transform.Translate(Vector3.up * speed * Time.deltaTime * 0);

        if (transform.position.y > 10 || transform.position.y < -10) {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Player>().Damage();
            collision.gameObject.GetComponent<Player>().GetComponent<AudioSource>().Play();
        }
    }
}
