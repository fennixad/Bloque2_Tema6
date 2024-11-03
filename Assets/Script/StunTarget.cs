using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StunTarget : MonoBehaviour
{
    private float distancia;
    private bool canStun;
    private float stunTime;
    private GameObject stunnedTarget;

    void Start()
    {
        distancia = 1.0f;
        canStun = true;
        stunTime = 5.0f;
    }

    void Update()
    {
        HandleStunInput();
        if (stunnedTarget != null)
        {
            UpdateStunTimer();
        }
    }

    void HandleStunInput()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            ProcessStunRaycast();
        }
    }

    void ProcessStunRaycast()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * distancia, Color.red, 1.0f);
        Vector3 posicion = new Vector3(transform.position.x, 1, transform.position.z);

        if (Physics.Raycast(posicion, transform.forward, out hit, distancia))
        {
            if (hit.collider.CompareTag("Enemy") && canStun)
            {
                ExecuteStun(hit.transform.gameObject);
            }
        }
    }

    void ExecuteStun(GameObject target)
    {
        IStunnable stunnable = target.GetComponent<IStunnable>();
        if (stunnable != null)
        {
            stunnable.SetStunned(false);
            stunnedTarget = target;
            stunTime = 5.0f; // Reinicia el tiempo de stun al aplicar el stun
        }
    }

    void UpdateStunTimer()
    {
        stunTime -= Time.deltaTime;
        if (stunTime <= 0f)
        {
            IStunnable stunnable = stunnedTarget.GetComponent<IStunnable>();
            if (stunnable != null)
            {
                stunnable.SetStunned(true);
                stunnedTarget = null; // Libera el objetivo cuando el stun termina
            }
        }
    }
}
