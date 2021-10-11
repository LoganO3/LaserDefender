using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = .5f;
    [SerializeField] float health = 200f;

    [Header("Projectile")]
    [SerializeField] GameObject playerLaser;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    [Header("Sound")]
    [SerializeField] AudioClip playerDeathSFX;
    [SerializeField] AudioClip playerShootingSFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;
    [SerializeField] [Range(0, 1)] float shootingSoundVolume = 1f;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
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
            FindObjectOfType<Level>().LoadGameOver();
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(playerDeathSFX, Camera.main.transform.position, deathSoundVolume);
        }
    }
    private void Fire()
    {
       if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void Move()
    { 
        var deltax = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltay = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltax, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltay, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }
    public float GetHealth()
    {
        return health;
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(playerLaser,
                    transform.position,
                    Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(playerShootingSFX, Camera.main.transform.position, shootingSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;

        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

}
