using UnityEngine;
using UnityEngine.AI;

public static class ClearProcess
{
    public static void Do()
    {
        Debug.Log("ClearProcess.Do() called");
        StopPlayer();
        StopEnemies();
    }

    public static void StopPlayer()
    {
        RabbitMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<RabbitMovement>();
        if (playerMovement != null)
        {
            playerMovement.Stop();
            Debug.Log("Player movement stopped.");
        }
        else
        {
            Debug.LogWarning("Player movement script not found.");
        }
    }

     public static void StopEnemies()
    {
        Stop_EnemyMovement_withNavMeshandRigidbody();
    }

    public static void Stop_EnemyMovement_withNavMeshandRigidbody(){
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            var _EnemyMovement_withNavMeshandRigidbody = enemy.GetComponent<EnemyMovement_withNavMeshandRigidbody>();
            if (_EnemyMovement_withNavMeshandRigidbody != null)
            {
                _EnemyMovement_withNavMeshandRigidbody.Stop(); // 停止処理を呼び出し
            }
        }
    }
}
