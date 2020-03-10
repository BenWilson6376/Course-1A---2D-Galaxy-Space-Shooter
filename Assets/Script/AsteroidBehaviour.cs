using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 3;
    [SerializeField]
    private GameObject explosion;
    private Player player;
    private SpawnManager _spawnManager;


    //check for laser collision (Trigger)
    //instantiate explosion at the position of the asteroid
    //destroy the explosion after 3 seconds


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate object on the z axis
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser") {
            Instantiate(explosion, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }
        if (other.tag == "Player")
            player.Damage();
    }

}
