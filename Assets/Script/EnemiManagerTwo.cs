using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.Rendering.HableCurve;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class EnemiMovementTwo : MonoBehaviour, IStunnable
{
    private bool veoPlayer;
    public float velocidad = 2.0f;
    private Vector3 destino;
    private int etapa = 1;
    public Transform target;
    private LineRenderer lineRenderer;
    private Color normalColor;
    private Color pilladoColor;
    private float angle;
    private int segments;
    private float deadTimer;
    private float distance;
    private bool activo;
    void Start()
    {
        veoPlayer = false;
        destino = new Vector3(0, transform.position.y, transform.position.z);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = false;
        lineRenderer.startColor = normalColor;
        lineRenderer.endColor = normalColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        normalColor = Color.green;
        pilladoColor = Color.red;
        deadTimer = 0.5f;
        segments = 30;
        angle = 60f;
        distance = 3000f;
        activo = true;
    }

    void Update()
    {
        Movimiento();
        Rotacion();
        Detection();
        DrawLine();
        DeadTimer();
    }

    public void SetStunned(bool stunned)
    {
        Debug.Log("Stunned");
        this.activo = stunned;

        if (!activo)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
        }
    }
    void CambiarDestino()
    {
        float posY = transform.position.y;

        switch (etapa)
        {
            case 1:
                destino = new Vector3(0, posY, transform.position.z);

                etapa = 2;
                break;

            case 2:
                destino = new Vector3(0, posY, 0);

                etapa = 3;
                break;

            case 3:
                destino = new Vector3(-10, posY, 0);
                transform.Rotate(0, -90, 0);
                etapa = 4;
                break;

            case 4:
                destino = new Vector3(-10, posY, -7);

                etapa = 1;
                break;
        }
    }
    void Movimiento()
    {
        if (!veoPlayer && activo)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);

            if (Vector3.Distance(transform.position, destino) < 0.1f)
            {
                CambiarDestino();
            }
        }
    }
    void Rotacion()
    {
        if (activo)
        {
            Vector3 direction = (destino - transform.position).normalized; // Obtener la dirección hacia el destino
            if (direction != Vector3.zero) // Evitar la rotación si la dirección es cero
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
            }
        }

    }
    void Detection()
    {
        if (activo)
        {
            Vector3 iniPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Vector3 finPos = new Vector3(target.position.x, target.position.y + 1, target.position.z);
            Vector3 directionToTarget = (finPos - iniPos).normalized;

            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= angle / 2)
            {
                RaycastHit hit;
                if (Physics.Linecast(iniPos, finPos, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        veoPlayer = true;
                    }
                    else if (!hit.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("No tengo linea de vision");
                    }
                }
            }
        }
    }
    void DrawLine()
    {
        if (!activo || !lineRenderer.enabled) return;
        if (veoPlayer)
        {
            lineRenderer.startColor = pilladoColor;
            lineRenderer.endColor = pilladoColor;
        }
        else
        {
            lineRenderer.startColor = normalColor;
            lineRenderer.endColor = normalColor;
        }

        Vector3 startPoint = transform.position;   // Centro del cono (posición del enemigo)
        lineRenderer.positionCount = segments + 2;
        lineRenderer.SetPosition(0, startPoint);    // Empiezo a dibujar desde el centro del cono

        // Ángulo inicial de la visión
        float angleStep = angle / segments;
        float startAngle = -angle / 2;

        // Dibujar los puntos del cono
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
            Vector3 endPoint = startPoint + direction * distance;

            lineRenderer.SetPosition(i + 1, endPoint);
        }

        lineRenderer.SetPosition(segments + 1, startPoint);
    }
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void DeadTimer()
    {
        if (veoPlayer && activo)
        {
            deadTimer -= Time.deltaTime;
            if (deadTimer <= 0)
            {
                RestartScene();
            }
        }
    }
}

