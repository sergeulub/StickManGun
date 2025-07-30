using UnityEngine;

public class CloudMoving : MonoBehaviour
{
    [SerializeField] Transform _transform;
    [SerializeField] Vector3 _startPos;
    [SerializeField] Vector3 _endPos;
    public float _speed;

    private void Start()
    {
        Reset();
    }
    private void Update()
    {   
        if (_transform.position.x > _endPos.x)
        {
            Reset();
            _transform.position = _startPos;
        }
        _transform.position += Vector3.right * _speed * Time.deltaTime;
    }
    private void Reset()
    {
        _speed = Random.value / 100f;
        _speed = Mathf.Max(0.0012f, _speed);
        _speed = Mathf.Min(0.0025f, _speed);
    }
    [ContextMenu("Set2")]
    public void Set()
    {   
        _transform = GetComponent<Transform>();
        _startPos.y = transform.position.y;
        _endPos = _startPos + new Vector3(22f,0,0);
    }
}
