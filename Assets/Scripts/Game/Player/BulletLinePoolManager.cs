using UnityEngine;

public class BulletLinePoolManager : MonoBehaviour
{
    public static BulletLinePoolManager Instance;
    public ObjectPool linePool;

    void Awake()
    {
        Instance = this;
    }

    public GameObject GetBulletLine()
    {
        return linePool.Get();
    }

    public void ReturnBulletLine(GameObject obj)
    {
        linePool.ReturnToPool(obj);
    }
}