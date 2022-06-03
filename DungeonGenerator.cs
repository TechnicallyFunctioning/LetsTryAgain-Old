using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using MTAssets.EasyMeshCombiner;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject floorContainer;
    [SerializeField] private GameObject wallContainer;
    [SerializeField] private GameObject[] floorTiles;
    [SerializeField] private GameObject[] wallTiles;
    [SerializeField] private GameObject dungeonExit;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int enemySpawnChance;
    [SerializeField] private int maxTiles = 25;

    [SerializeField] private List<Vector3> floorPositions;
    private List<Vector3> floorPositionsFloor;
    private GameObject combinedWalls;
    private TeleportationArea floorTP;

    private BoxCollider boxCollider;
    private RuntimeMeshCombiner meshCombiner;

    private Vector3 currentPosition;
    private int currentTiles;
    private Vector3 front;
    private bool frontEmpty;
    private Vector3 back;
    private bool backEmpty;
    private Vector3 left;
    private bool leftEmpty;
    private Vector3 right;
    private bool rightEmpty;
    private bool boxedIn;
    private int sideRoll = 0;

    private RuntimeMeshCombiner floorCombiner;
    private RuntimeMeshCombiner wallCombiner;

    private void Start()
    {
        GenerateDungeon();
    }
    private void GenerateDungeon()
    {
        boxCollider = GetComponent<BoxCollider>();
        currentPosition = transform.position;

        AssignFloorPositions();
        SpawnFloor();
        Instantiate(dungeonExit, floorPositions[floorPositions.Count - 1], transform.rotation);
        SpawnWalls();
        boxCollider.enabled = false;
        SpawnEnemies();
    }
    private void AssignFloorPositions()
    {
        floorPositions.Add(currentPosition);

        while (!boxedIn && currentTiles < maxTiles)
        {
            //Get positions on all sides based on direction, tile scale, and collider size
            front = currentPosition + Vector3.forward * (boxCollider.size.z * transform.localScale.z);
            back = currentPosition + Vector3.back * (boxCollider.size.z * transform.localScale.z);
            left = currentPosition + Vector3.left * (boxCollider.size.x * transform.localScale.x);
            right = currentPosition + Vector3.right * (boxCollider.size.x * transform.localScale.x);

            //Check which positions are already taken on our list
            if (!floorPositions.Contains(front)) { frontEmpty = true; }
            if (!floorPositions.Contains(back)) { backEmpty = true; }
            if (!floorPositions.Contains(left)) { leftEmpty = true; }
            if (!floorPositions.Contains(right)) { rightEmpty = true; }
            if (!frontEmpty && !backEmpty && !leftEmpty && !rightEmpty) { boxedIn = true; }

            if(back.z < transform.position.z + boxCollider.size.z * transform.localScale.z) { backEmpty = false; }

            //Roll for next position to add to our list
            sideRoll = Random.Range(0, 5);

            //If the position we rolled for is empty, add one to our count, add it to our list, and set it as our current position
            if (sideRoll < 2 && frontEmpty)
            {
                currentTiles++;
                floorPositions.Add(front);
                currentPosition = front;
            }
            else if (sideRoll == 2 && backEmpty)
            {
                currentTiles++;
                floorPositions.Add(back);
                currentPosition = back;
            }
            else if (sideRoll == 3 && leftEmpty)
            {
                currentTiles++;
                floorPositions.Add(left);
                currentPosition = left;
            }
            else if (sideRoll <5 && rightEmpty)
            {
                currentTiles++;
                floorPositions.Add(right);
                currentPosition = right;
            }

        }

        floorPositions = floorPositions.Distinct().ToList();

    }
    private void SpawnFloor()
    {
        for (int i = 0; i < floorPositions.Count; i++)
        {
            GameObject floorTile = Instantiate(floorTiles[Random.Range(0, floorTiles.Length)], floorPositions[i], transform.rotation);
            floorTile.transform.SetParent(floorContainer.transform);
        }
        floorCombiner = floorContainer.GetComponent<RuntimeMeshCombiner>();
        floorCombiner.CombineMeshes();
        floorContainer.GetComponent<NavMeshSurface>().BuildNavMesh();

        floorTP = floorContainer.AddComponent<TeleportationArea>();
        floorTP.interactionLayerMask = LayerMask.GetMask("Teleport");
    }
    private void SpawnWalls()
    {
        //floorPositions.Add(transform.position);

        for (int i = 0; i < floorPositions.Count; i++)
        {
            frontEmpty = false; backEmpty = false; leftEmpty = false; rightEmpty = false;

            //Get positions on all sides based on direction, tile scale, and collider size
            front = floorPositions[i] + Vector3.forward * (boxCollider.size.z * transform.localScale.z);
            back = floorPositions[i] + Vector3.back * (boxCollider.size.z * transform.localScale.z);
            left = floorPositions[i] + Vector3.left * (boxCollider.size.x * transform.localScale.x);
            right = floorPositions[i] + Vector3.right * (boxCollider.size.x * transform.localScale.x);

            //Check which positions are already taken on our list
            if (!floorPositions.Contains(front)) { frontEmpty = true; }
            if (!floorPositions.Contains(back)) { backEmpty = true; }
            if (!floorPositions.Contains(left)) { leftEmpty = true; }
            if (!floorPositions.Contains(right)) { rightEmpty = true; }
            if (!frontEmpty && !backEmpty && !leftEmpty && !rightEmpty) { boxedIn = true; }

            if (back.z < transform.position.z) { backEmpty = false; }

            //if (back.z < transform.position.z + boxCollider.size.z * transform.localScale.z) { backEmpty = false; }

            if (frontEmpty)
            {
                GameObject wallTile = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], floorPositions[i] + Vector3.forward * ((boxCollider.size.z * transform.localScale.z) / 2), transform.rotation * Quaternion.Euler(Vector3.up * 90));
                wallTile.transform.SetParent(wallContainer.transform);
            }
            if (backEmpty)
            {
                GameObject wallTile = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], floorPositions[i] + Vector3.back * ((boxCollider.size.z * transform.localScale.z) / 2), transform.rotation * Quaternion.Euler(Vector3.up * 270));
                wallTile.transform.SetParent(wallContainer.transform);

            }
            if (leftEmpty)
            {
                GameObject wallTile = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], floorPositions[i] + Vector3.left * ((boxCollider.size.x * transform.localScale.x) / 2), transform.rotation);
                wallTile.transform.SetParent(wallContainer.transform);

            }
            if (rightEmpty)
            {
                GameObject wallTile = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], floorPositions[i] + Vector3.right * ((boxCollider.size.x * transform.localScale.x) / 2), transform.rotation * Quaternion.Euler(Vector3.up * 180));
                wallTile.transform.SetParent(wallContainer.transform);
            }
        }
        wallCombiner = wallContainer.GetComponent<RuntimeMeshCombiner>();
        wallCombiner.CombineMeshes();
    }
    private void SpawnEnemies()
    {
        for (int i = 0; i < floorPositions.Count; i++)
        {
            if (Random.Range(0, 10) < enemySpawnChance) { Instantiate(enemies[Random.Range(0, enemies.Length)], floorPositions[i], Quaternion.Euler(0,Random.Range(0 , 360),0)); }
        }
    }
}