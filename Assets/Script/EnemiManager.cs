using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemiMovement : MonoBehaviour
{
    public GameObject enemyOne;
    public GameObject enemyTwo;
    public GameObject enemyThree;
    public float velocidad;
    private int direccion;
    private float limite;
    void Start()
    {
        direccion = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        Enemy1();
        Enemy3();
    }

    void WhatEnemyAmI()
    {
        
    }

    void Enemy1()
    {
        velocidad = 3f;
        limite = 12.0f;

        Transform transform = enemyOne.transform;

        transform.position += new Vector3(velocidad * direccion * Time.deltaTime, 0, 0);

        if (transform.position.x >= limite)
        {
            transform.position = new Vector3(limite, transform.position.y, transform.position.z);
            direccion = -1; 
        }
        else if (transform.position.x <= -limite)
        {
            transform.position = new Vector3(-limite, transform.position.y, transform.position.z);
            direccion = 1; 
        }
    }

    void Enemy2()
    {
        enemyOne.GetComponent<Transform>().position = new Vector3(0, 0, 0);
    }

    void Enemy3()
    {

    }
}
