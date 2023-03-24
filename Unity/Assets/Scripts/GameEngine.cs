using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;
using UnityEditor.Build.Reporting;

public class GameEngine : MonoBehaviour
{
    public GameObject pigModel;
    public GameObject tileModel;
    public GameObject towerModel;

    private Tower tower;
    private Enemy enemy;

    //maak hier private class variable, LETOP dit wordt een ARRAY van GameObject(en):
    //1 (acces=private, Type= GameObject[], name=path)
    //gebruik nog geen new
    private GameObject[] path;

    double GetDist(GameObject A, GameObject B)
    {
        float dx = A.transform.position.x - B.transform.position.x;
        float dy = A.transform.position.y - B.transform.position.y;
        float dz = A.transform.position.z - B.transform.position.z;


        float powered = (dx * dx) + (dy * dy) + (dz * dz);
        double dist = Math.Sqrt(powered);
        Debug.Log(dist);
        return dist;

    }
    void MoveEnemy(Enemy enemy)
        {
            if (enemy.to >= path.Length)
            {
                return;
            }
        
            GameObject from = path[enemy.from];
            GameObject to = path[enemy.to];
            

            float dx = to.transform.position.x - from.transform.position.x;
            float dy = to.transform.position.y - from.transform.position.y;
            float dz = to.transform.position.z - from.transform.position.z;

            Debug.Log(dx + " " + dy + " " + dz);

            enemy.obj.transform.position += new Vector3(dx, dy, dz) * Time.deltaTime;
            double dist = GetDist(enemy.obj, to);
            
            if (dist < 0.01)
        {
            Debug.Log("GOTO NEXT!");
            enemy.to++;
            enemy.from++;
            enemy.obj.transform.position = to.transform.position;
        }

            

    }
    void Start()
    {
        int x = 0;
        int z = 0;
        int size = 2;


    //maak hier een lokale variable,
    //1 (Type= RelAdd[], name=pathplus)
    //maak deze meteen ook met new aan, 10 groot graag
    RelAdd[] pathplus = new RelAdd[] {

            new RelAdd() {x=0,z=0 },
            new RelAdd() {x=0,z=1 },
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=0,z=-1 },
            new RelAdd() {x=0,z=-1 },
            new RelAdd() {x=0,z=-1 },
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=1,z=0},
            new RelAdd() {x=1,z=0},
            };

    //geef path (class variable) hier een waarde met new, even groot als pathplus! 
    //- gebruik pathplus.Length voor de grote!
    path = new GameObject[pathplus.Length];

        //nu maken we een for loop over pathplus 
        for (int i = 0;i < pathplus.Length ; i++)
        {
            RelAdd step = pathplus[i];
            x += step.x * size;
            z += step.z * size;

            path[i] = Instantiate(tileModel, new Vector3(x, 0, z), Quaternion.identity);
            


            
        }
        //zet binnen de forloop deze code neer:
        //- loop tot pathplus.Length!
        GameObject enemyStart = path[0];
        GameObject enemyObj = Instantiate(pigModel,new Vector3(0, 0, 0), Quaternion.identity);
        enemy = new Enemy(enemyObj);//deze werkt nog niet ga naar Enemy.cs en los de ??? op
        enemy.from = 0;
        enemy.to = 1;

        GameObject towerPlace = path[4];
        GameObject onTile = Instantiate(tileModel, towerPlace.transform.position + new Vector3(0, 0, 2), Quaternion.identity);
        GameObject towerObj = Instantiate(towerModel, onTile.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
        tower = new Tower(towerObj, 5, onTile);//deze werkt nog niet ga naar Tower.cs en los de ??? op

        

    }

    void Update()
    {
        if (GetDist(tower.obj, enemy.obj) <= tower.detectRange)
        {
            Debug.Log("near!");
        }
        MoveEnemy(enemy);
    }


}