using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform spawnPos;
    public GameObject[] prefabs;
    [Header("Spawn Percentage muss zusammen 100 ergeben")]
    public int[] spawnPercentage;
    public float spawnTime;
    void Start()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn() 
    { 
        yield return new WaitForSeconds(spawnTime);

        Instantiate(prefabs[GetRandomSpawn()], spawnPos.position, spawnPos.rotation);

        StartCoroutine(Spawn());
    }
    private int GetRandomSpawn()
    {
        float random = Random.Range(0f, 1f);
        float numforadding = 0;
        float total = 0;
        for (int i = 0; i < spawnPercentage.Length; i++)
        {
            total += spawnPercentage[i];
        }
        for(int i= 0; i < prefabs.Length; i++)
        {
            if (spawnPercentage[i] / total + numforadding >= random)
            {
                return i;
            }
            else
            {
                numforadding += spawnPercentage[i]/total;
            }
        }
        return 0;
    }
}
