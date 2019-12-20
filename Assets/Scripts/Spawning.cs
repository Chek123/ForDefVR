using UnityEngine;

public class Spawning : MonoBehaviour
{
    private int vojakSpawnPocetnost;

    [SerializeField]
    private TextMesh pocetnostCounter;

    [SerializeField]
    private GameObject lastVojakSpawned;

    [SerializeField]
    private GameObject vojakPrefab;

    [SerializeField]
    private int soldierId = 0;

    [SerializeField]
    private WeaponController weaponController;

    void Start()
    {
        Debug.Log(GameManager.Instance.levelData);
        switch(soldierId)
        {
            case 1:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_1;
                break;
            case 2:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_2;
                break;
            case 3:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_3;
                break;
            case 4:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_4;
                break;
            case 5:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_5;
                break;
            case 6:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_6;
                break;
            default:
                Debug.LogError("Unable to load soldier with id" + soldierId);
                vojakSpawnPocetnost = 0;
                break;
        }

        if(vojakSpawnPocetnost == 0)
        {
            GameObject.Destroy(lastVojakSpawned);
            GameObject.Destroy(gameObject);
        }

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
        lastVojakSpawned = GameManager.InstantateScaled(vojakPrefab, transform.position, transform.rotation);
        lastVojakSpawned.transform.parent = transform.parent;
        lastVojakSpawned.GetComponent<PlacableObject>().spawning = this;
        lastVojakSpawned.GetComponent<PlayableObject>().weaponController = this.weaponController;
    }
}
