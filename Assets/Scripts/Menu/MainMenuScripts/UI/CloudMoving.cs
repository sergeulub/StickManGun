using UnityEngine;

public class CloudMoving : MonoBehaviour
{
    [Header("Начальная и конечная позиция")]
    public Vector3 startPosition;
    public Vector3 endPosition;

    [Header("Скорость облака")]
    public float minSpeed = 0.5f;
    public float maxSpeed = 2.0f;

    [Header("Размер облака")]
    public float minScale = 0.5f;
    public float maxScale = 1f;

    [Header("Дальность движения влево")]
    public float travelDistance = 1800f;

    private float currentSpeed;
    private float currentScale;
    private Vector3 currentStartPos;

    void Start()
    {
        currentStartPos = startPosition;
        SetRandom();
    }

    void Update()
    {
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        if (transform.localPosition.x <= endPosition.x)
        {
            // Сброс позиции и новая скорость
            transform.position = currentStartPos;
            SetRandom();
        }
    }

    void SetRandom()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        currentScale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(currentScale, currentScale, 1);
    }

    [ContextMenu("Set2")]
    public void Set()
    {
        startPosition.y = transform.position.y;
        startPosition.x = 900;
        endPosition.y = transform.position.y;
        endPosition.x = -900;
    }
}

