using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField]
    private int vojakSpawnPocetnost;

    [SerializeField]
    private TextMesh pocetnostCounter;

    [SerializeField]
    private GameObject lastVojakSpawned;

    [SerializeField]
    private GameObject vojakPrefab;

    void Start()
    {
        pocetnostCounter.text = "" + vojakSpawnPocetnost;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "VojakStred" && other.transform.parent.gameObject.Equals(lastVojakSpawned))
        {
            if (vojakSpawnPocetnost > 1)
            {
                spawnVojak();
            }
            else
            {
                lastVojakSpawned = null;
            }
            decreaseCounter();
        }
    }

    public void onWrongPlacement(GameObject vojak)
    {
        // ked posledny vojak bol zle umiestneny
        if (vojakSpawnPocetnost == 0)
        {
            spawnVojak();
            increaseCounter();
        }
        // ked vojak neopustil spawnovaci collider (nevyvolal OnTriggerExit)
        else if (vojak.Equals(lastVojakSpawned))
        {
            spawnVojak();
        }
        else
        {
            increaseCounter();
        }
    }

    private void increaseCounter()
    {
        pocetnostCounter.text = "" + (++vojakSpawnPocetnost);

    }

    private void decreaseCounter()
    {
        pocetnostCounter.text = "" + (--vojakSpawnPocetnost);
    }

    private void spawnVojak()
    {
        lastVojakSpawned = Instantiate(vojakPrefab, transform.position, transform.rotation) as GameObject;
        lastVojakSpawned.transform.parent = transform.parent;
        lastVojakSpawned.GetComponent<PlacableObject>().spawning = this;
    }
}
