using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class DevSplineFencePostPlacer : MonoBehaviour
{
    public SplineContainer splineContainer; // スプラインの参照
    public GameObject postPrefab;           // 配置するPostのPrefab
    public float minimumSpacing = 2.0f;     // 最小間隔
    public Vector3 rotationOffset = Vector3.zero; // オブジェクトの向きを調整するためのオフセット（Euler角）

    [ContextMenu("Place Posts")]
    public void PlacePosts()
    {
        if (splineContainer == null || postPrefab == null || minimumSpacing <= 0)
        {
            Debug.LogError("Invalid configuration: Ensure SplineContainer, PostPrefab, and MinimumSpacing are set.");
            return;
        }

        // "Posts" オブジェクトを探すか新しく作成
        Transform postsParent = transform.Find("Posts");
        if (postsParent != null)
        {
            DestroyImmediate(postsParent.gameObject); // 既存の "Posts" を削除
        }

        GameObject postsObject = new GameObject("Posts");
        postsObject.transform.SetParent(transform);
        postsObject.transform.localPosition = Vector3.zero;
        postsObject.transform.localRotation = Quaternion.identity;

        // スプラインデータを取得
        Spline spline = splineContainer.Spline;
        if (spline == null)
        {
            Debug.LogError("SplineContainer does not contain a valid Spline.");
            return;
        }

        // スプライン全体の長さを計算
        float splineLength = SplineUtility.CalculateLength(spline, splineContainer.transform.localToWorldMatrix);

        // 配置するポイントの数を計算
        int numberOfPosts = Mathf.FloorToInt(splineLength / minimumSpacing) + 1;

        // 等間隔で配置
        for (int i = 0; i < numberOfPosts; i++)
        {
            float t = (float)i / (numberOfPosts - 1); // 正規化された位置 [0, 1]
            SplineUtility.Evaluate(
                spline,
                t,
                out float3 localPosition,
                out float3 localTangent,
                out float3 localUpVector
            );

            // ローカル空間の位置と方向をワールド空間に変換
            Vector3 worldPosition = splineContainer.transform.TransformPoint(new Vector3(localPosition.x, localPosition.y, localPosition.z));
            Vector3 worldTangent = splineContainer.transform.TransformDirection(new Vector3(localTangent.x, localTangent.y, localTangent.z));
            Vector3 worldUpVector = splineContainer.transform.TransformDirection(new Vector3(localUpVector.x, localUpVector.y, localUpVector.z));

            // オブジェクトの向きをスプラインに合わせ、回転オフセットを適用
            Quaternion baseRotation = Quaternion.LookRotation(worldTangent, worldUpVector);
            Quaternion adjustedRotation = baseRotation * Quaternion.Euler(rotationOffset);

            // Postを生成
            GameObject postInstance = Instantiate(postPrefab, worldPosition, adjustedRotation, postsObject.transform);

            // 名前に番号を付ける
            postInstance.name = $"Post_{i + 1}";
        }
    }
}
