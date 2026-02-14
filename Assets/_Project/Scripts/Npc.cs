using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Npc : MonoBehaviour
{
    private IEnumerator Start()
    {
        StartCoroutine(WaveLoop());
        
        // wait for network to start listening
        yield return new WaitUntil(() => NetworkManager.Singleton.IsListening);
        
        // destroy this script if we not server
        if (!NetworkManager.Singleton.IsServer)
            Destroy(this);
    }

    private IEnumerator WaveLoop()
    {
        // Wave every random seconds
        while (enabled)
        {
            var randSeconds = Random.Range(3f, 6f);
            yield return new WaitForSeconds(randSeconds);
            GetComponent<Animator>().SetTrigger("Wave");
        }
    }
}
