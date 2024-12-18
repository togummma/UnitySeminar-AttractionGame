using UnityEngine;

[ExecuteInEditMode] // エディタでのみ実行
public class RailConnector : MonoBehaviour
{
    public GameObject railPrefab;   // RailのPrefab
    public Transform postParent;   // Postの親オブジェクト（PostPlacerで生成）

    [ContextMenu("Connect Rails")]
    public void ConnectRails()
    {
        if (postParent == null || railPrefab == null) return;

        // 既存のオブジェクトを削除
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // Post間を繋ぐRailを生成
        Transform[] posts = postParent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < posts.Length; i++) // 親自身を含むため1から開始
        {
            Vector3 start = posts[i - 1].position;
            Vector3 end = posts[i].position;
            Vector3 midPoint = (start + end) / 2;
            Vector3 direction = (end - start).normalized;

            float distance = Vector3.Distance(start, end);

            // Railを生成
            GameObject rail = Instantiate(railPrefab, midPoint, Quaternion.LookRotation(direction), transform);
            rail.transform.localScale = new Vector3(1, 1, distance); // 長さを調整
        }
    }
}
