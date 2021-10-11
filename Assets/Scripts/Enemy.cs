using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 100;
    [Header("Projectile")]
    [SerializeField] float minTimeBetweenShots = .2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyLaser;
    [SerializeField] float projectileSpeed = -10f;
    [Header("Death")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [Header("Sound")]
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] AudioClip enemyShootingSFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;
    [SerializeField] [Range(0, 1)] float shootingSoundVolume = 1f;
    float shotCounter;


    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(
            enemyLaser,
            transform.position,
            Quaternion.identity
            ) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(enemyShootingSFX, Camera.main.transform.position, shootingSoundVolume);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        HandleDamage(damageDealer);
    }

    private void HandleDamage(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Explosion();
            FindObjectOfType<GameStatus>().AddScore(scoreValue);
        }
    }

    private void Explosion()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(enemyDeathSFX, Camera.main.transform.position, deathSoundVolume);
    }
}