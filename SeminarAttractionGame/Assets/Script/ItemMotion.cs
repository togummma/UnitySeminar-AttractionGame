using UnityEngine;

public class ItemMotion : MonoBehaviour
{
    public float rotationSpeed = 50f; // 回転速度
    public float floatSpeed = 2f;    // 上下の速度
    public float floatHeight = 0.5f; // 上下する高さ

    private Vector3 startPosition;

    void Start()
    {
        // 初期位置を最も低い位置に設定
        startPosition = transform.position;
    }

    void Update()
    {
        // 回転
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // 上下
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight + floatHeight; 
        // floatHeight を加えて、最小値が startPosition.y になるよう調整
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
