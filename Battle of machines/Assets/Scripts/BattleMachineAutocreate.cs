using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BattleMachineAutocreate : MonoBehaviour
{
    [NonSerialized]
    public bool isEnemy = false;
    public GameObject car;
    public float time = 5f;

    private void Start()
    {
        StartCoroutine(SpawnCar());
    }

    IEnumerator SpawnCar()
    {
        yield return new WaitUntil(() => StartGame.IsGameStarted);

        for (int i = 1; i <= 3; i++)
        {
            /*
                Fix me: Correct vehicle rotation based on the hangar + 
                randomized spawn location to prevent vehicles from overlapping.
            */
            yield return new WaitForSeconds(time);
            Vector3 pos = new Vector3(
                transform.GetChild(0).position.x + UnityEngine.Random.Range(3f, 7f),
                transform.GetChild(0).position.y,
                transform.GetChild(0).position.z + UnityEngine.Random.Range(3f, 7f)
                );
            GameObject spawn = Instantiate(car, pos, Quaternion.identity);

            if (isEnemy)
            {
                spawn.tag = "Enemy";
                Interlocked.Increment(ref StartGame.enemies);
            }
            else
            {
                Interlocked.Increment(ref StartGame.machines);
            }
        }
    }
}
