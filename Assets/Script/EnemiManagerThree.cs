using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class EnemiMovementThree : MonoBehaviour, IStunnable
{
    public GameObject enemyThree;
    private float deadTimer;
    public float velocidad;
    private int direccionX;
    private float limiteX;
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
    private bool veoPlayer;
    public TextMeshPro textMeshPro;

    private float angle = 60f;  // Ángulo de visión
    private float distance = 8f;  // Distancia máxima de visión
    private int segments = 30;  // Cantidad de segmentos para dibujar el cono
    private Color normalColor = Color.green;
    private Color pilladoColor = Color.red;
    private LineRenderer lineRenderer;

    public bool activo;
    void Start()
    {
        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
        RotateToPlayer();
        DrawVision();
        CheckDeadTimer();
        FaceTextToCamera();
    }

    void InitializeEnemy()
    {
        deadTimer = 0.5f;
        direccionX = 1;
        velocidad = 2f;
        limiteX = 12.0f;
        veoPlayer = false;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 2;
        lineRenderer.loop = false;
        lineRenderer.startColor = normalColor;
        lineRenderer.endColor = normalColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        activo = true;
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
    void MoveEnemy()
    {
        if (!veoPlayer && activo)
        {
            enemyThree.transform.position += new Vector3(velocidad * direccionX * Time.deltaTime, 0, 0);

            if (enemyThree.transform.position.x >= limiteX)
            {
                enemyThree.transform.position = new Vector3(limiteX, enemyThree.transform.position.y, enemyThree.transform.position.z);
                enemyThree.transform.Rotate(0, 180, 0);
                direccionX = -1;
            }
            else if (enemyThree.transform.position.x <= -limiteX)
            {
                enemyThree.transform.position = new Vector3(-limiteX, enemyThree.transform.position.y, enemyThree.transform.position.z);
                enemyThree.transform.Rotate(0, 180, 0);
                direccionX = 1;
            }
        }
    }

    void RotateToPlayer()
    {
        if (activo)
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

                veoPlayer = true;
            }
        }
    }

    void DrawVision()
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

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void CheckDeadTimer()
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

    void FaceTextToCamera()
    {
        textMeshPro.transform.position = enemyThree.transform.position + new Vector3(0, 3, 0);

        Vector3 directionToCamera = Camera.main.transform.position - textMeshPro.transform.position;
        directionToCamera.y = 0;
        textMeshPro.transform.rotation = Quaternion.LookRotation(directionToCamera);
        textMeshPro.transform.Rotate(0, 180, 0);
    }
}
