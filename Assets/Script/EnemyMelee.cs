using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{

    public Transform target;
    private Vector3 direccion;
    private Vector3 fixDireccion;
    private Quaternion rot;
    private float velRotation = 3f;
    private float distancia;
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
        direccion = transform.position - target.position;
        fixDireccion = new Vector3(direccion.x, 0, direccion.z); 

        distancia = Vector3.Distance(transform.position, target.position);

        if (distancia < 10)
        {
            rot = Quaternion.LookRotation(fixDireccion, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * velRotation);
        }
    }
}
