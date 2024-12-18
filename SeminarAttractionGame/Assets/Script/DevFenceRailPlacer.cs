using UnityEngine;
using System.Linq; // LINQを使用

[ExecuteInEditMode]
public class DevFenceRailPlacer : MonoBehaviour
{
    public Transform postsParent;  // Postsオブジェクトの参照
    public GameObject railPrefab;  // Railのプレハブ
    public Vector3 rotationOffset = Vector3.zero; // 方向補正のためのオフセット (Euler角)
    public Vector3 scaleAdjustment = Vector3.one; // スケール補正

    [ContextMenu("Place Rails")]
    public void PlaceRails()
    {
        if (postsParent == null || railPrefab == null)
        {
            Debug.LogError("Invalid configuration: Ensure PostsParent and RailPrefab are set.");
            return;
        }

        // "Rails" オブジェクトを探すか新しく作成
        Transform railsParent = transform.Find("Rails");
        if (railsParent != null)
        {
            DestroyImmediate(railsParent.gameObject); // 既存の "Rails" を削除
        }

        GameObject railsObject = new GameObject("Rails");
        railsObject.transform.SetParent(transform);
        railsObject.transform.localPosition = Vector3.zero;
        railsObject.transform.localRotation = Quaternion.identity;

        // Postオブジェクトを名前でソート
        var sortedPosts = postsParent.Cast<Transform>()
            .Where(post => post.name.StartsWith("Post_"))
            .OrderBy(post => int.Parse(post.name.Replace("Post_", "")))
            .ToList();

        int postCount = sortedPosts.Count;
        if (postCount < 2)
        {
            Debug.LogError("Not enough Posts to place Rails. At least 2 Posts are required.");
            return;
        }

        // Railプレハブの元の長さを取得
        float railOriginalLength = GetRailOriginalLength(railPrefab);

        if (railOriginalLength <= 0)
        {
            Debug.LogError("Failed to determine Rail prefab's original length. Ensure it has a BoxCollider.");
            return;
        }

        // Post間にRailを配置
        for (int i = 0; i < postCount - 1; i++)
        {
            Transform postA = sortedPosts[i];
            Transform postB = sortedPosts[i + 1];

            // Post間の距離と方向を計算
            Vector3 positionA = postA.position;
            Vector3 positionB = postB.position;
            Vector3 direction = (positionB - positionA).normalized;
            float distance = Vector3.Distance(positionA, positionB);

            // Railを生成
            GameObject railInstance = Instantiate(railPrefab, railsObject.transform);

            // Railの位置と回転を設定
            railInstance.transform.position = positionA;
            Quaternion baseRotation = Quaternion.LookRotation(direction, Vector3.up);
            railInstance.transform.rotation = baseRotation * Quaternion.Euler(rotationOffset);

            // Railのスケールを設定 (X方向をPost間の距離に合わせる)
            Vector3 scale = railInstance.transform.localScale;
            scale.x = distance / railOriginalLength; // プレハブの元の長さで補正
            scale.x *= scaleAdjustment.x; // 手動スケール調整
            scale.y *= scaleAdjustment.y;
            scale.z *= scaleAdjustment.z;
            railInstance.transform.localScale = scale;

            // 名前を設定
            railInstance.name = $"Rail_{i + 1}";
        }
    }

    private float GetRailOriginalLength(GameObject railPrefab)
    {
        // BoxColliderから元の長さを取得
        var boxCollider = railPrefab.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            return boxCollider.size.x; // X軸方向のサイズを取得
        }

        // BoxColliderがない場合は0を返す
        return 0;
    }
}
