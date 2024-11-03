using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.Rendering.HableCurve;

public class EnemiMovementOne : MonoBehaviour, IStunnable
{
    public GameObject enemyOne;
    public float velocidad;
    private int direccionX;
    private float limiteX;
    private float angle;  // Ángulo de visión
    private float distance;  // Distancia máxima de visión
    private int segments;
    private Color normalColor = Color.green;
    private Color pilladoColor = Color.red;
    private float deadTimer;
    public Transform target;
    private Vector3 direccionFromEnemy;
    private bool veoPlayer;
    private LineRenderer lineRenderer;

    private bool activo;

    void Start()
    {
        direccionX = 1;
        velocidad = 1.5f;
        limiteX = 12.0f;
        veoPlayer = false;
        deadTimer = 0.5f;
        veoPlayer = false;
        segments = 30;
        distance = 20f;
        angle = 60f;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 2;  // Puntos para formar el cono
        lineRenderer.loop = false;
        lineRenderer.startColor = normalColor;
        lineRenderer.endColor = normalColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        activo = true;
    }

    void Update()
    {
        Movement();
        DetectionPlayer();
        DrawVisionCone();
    }

    void Movement()
    {
        if (!veoPlayer && activo)
        {
            enemyOne.transform.position += new Vector3(velocidad * direccionX * Time.deltaTime, 0, 0);

            if (enemyOne.transform.position.x >= limiteX)
            {
                enemyOne.transform.position = new Vector3(limiteX, enemyOne.transform.position.y, enemyOne.transform.position.z);
                enemyOne.transform.Rotate(0, 180, 0);
                direccionX = -1;
            }
            else if (enemyOne.transform.position.x <= -limiteX)
            {
                enemyOne.transform.position = new Vector3(-limiteX, enemyOne.transform.position.y, enemyOne.transform.position.z);
                enemyOne.transform.Rotate(0, 180, 0);
                direccionX = 1;
            }
        }
    }


    void DetectionPlayer()
    {
        if (activo)
        {
            float halfAngle = angle / 2;

            for (int i = 0; i <= segments; i++)
            {
                // Calcula la dirección del rayo usando el ángulo actual.
                float currentAngle = -halfAngle + (i * (angle / segments));
                Vector3 rayDirection = Quaternion.Euler(0, currentAngle, 0) * transform.forward;


                RaycastHit hit;
                if (Physics.Raycast(transform.position, rayDirection, out hit, distance))
                {
                    Debug.DrawRay(transform.position, rayDirection * distance, Color.red);
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        veoPlayer = true;
                        DeadTimer();
                    }
                }
            }
        }
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
    void DrawVisionCone()
    {

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
}
