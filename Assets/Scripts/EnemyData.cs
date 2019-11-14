using System.Collections;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public int width;
    public int height;
    public Tile[] tiles;
}

[System.Serializable]
public class Tile
{
    public int x;
    public int y;
    public string material;

}

[System.Serializable]
public class Data
{
    public int number_of_levels;
    public Level[] levels;
}

[System.Serializable]
public class Level
{
    public int number_of_grids;
    public Grid[] grids;
}

[System.Serializable]
public class Grid
{
    public Policko[] policko;
}

[System.Serializable]
public class Policko
{
    public int policko_id;
    public int vojak;
}



[System.Serializable]
public class EnemyDataController : MonoBehaviour
{
    public void LoadData()
    {
        //string path = "Assets/Database/data.json";
        //StreamReader reader = new StreamReader(path);
        //string json = reader.ReadToEnd();
        //Enemy data = JsonUtility.FromJson<Enemy>(json);

        //Debug.Log(data.height);
        //Debug.Log(data.width);
        //Debug.Log(data.tiles[0].y);


        // load everything

        string path = "Assets/Database/real.json";
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        Data data = JsonUtility.FromJson<Data>(json);

        // Pristup k zakladnej strukture
        Debug.Log(data.number_of_levels); //OK


        // Pristup k levelom
        //current_level musim poznat
        //Debug.Log(data.levels[current_level - 1])

        Debug.Log(data.levels[0]);   // OK
        Level level = data.levels[0];


        // Pristup ku gridom konkretneho levelu
        Debug.Log(level.number_of_grids);
        // Vygeneruj random cislo od 0 do number_of_grids - 1
        Debug.Log(level.grids[0]);  //akoze vygenerovana 0 -> 1. grid.
        Policko[] policko = level.grids[0].policko;

        // mam vygenerovany grid, v ktorom je 25 policok. Teraz cez ne budem loopovat a vytvarat objekty na ploche.
        foreach (var tmp in policko)
        {
            //zisti aky typ vojaka spawnut
            //zisti kam ho spawnut
            //spawni.



            //Instantiate(vojakPrefab, transform.position, transform.rotation)ô
            //Debug.Log(tmp.policko_id);
            //Debug.Log(tmp.vojak);
        }





    }
}