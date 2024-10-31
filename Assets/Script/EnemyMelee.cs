using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public Transform target;
    private Vector3 sentidoPlayerEnemy;
    private Vector3 direccionFromEnemy;
    private Vector3 sentidoPlayerEnemyIgnoringHeight;
    private Vector3 direccionFromEnemyIgnoringHeight;
    private Vector3 direccionFinal;
    private Quaternion rotacion;
    private float distancia;
    private float angulo;
    [SerializeField] private float rotationSpeed = 3.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EnemyRotation();
    }

    void EnemyRotation()
    {
        sentidoPlayerEnemy = target.position - transform.position;
        direccionFromEnemy = transform.forward;

        sentidoPlayerEnemyIgnoringHeight = new Vector3(sentidoPlayerEnemy.x, 0, sentidoPlayerEnemy.z);
        direccionFromEnemyIgnoringHeight = new Vector3(direccionFromEnemy.x, 0, direccionFromEnemy.z);

        distancia = Vector3.Distance(target.position, transform.position);
        angulo = Vector3.Angle(direccionFromEnemyIgnoringHeight, sentidoPlayerEnemyIgnoringHeight);

        if (distancia < 8 && angulo < 60)
        {
            rotacion = Quaternion.LookRotation(new Vector3(sentidoPlayerEnemy.x, 0, sentidoPlayerEnemy.z), transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * rotationSpeed);
        }
    }
}
