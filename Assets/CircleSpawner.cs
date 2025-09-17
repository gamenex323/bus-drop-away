using UnityEngine;
using System.Collections.Generic;

public class CircleSpawner : MonoBehaviour
{


    public GameObject characterPrefab;   // Prefab with PlayerMovement + MeshRenderer
    public Transform centerPoint;        // The circle center
    public float radius = 5f;            // Circle radius
    public float spacingAngle = 12f;     // Circle spacing (degrees between characters)
    public float startingAngle = 0f;     // First character angle


    public List<Player> players;
    private Player[] playersArray;

    private void Start()
    {
        SpawnCharacters();
    }

    public void SpawnCharacters()
    {
        if (characterPrefab == null || centerPoint == null) return;

        // Get all PlayerMovement components under this spawner
        PlayerMovement[] players = GetComponentsInChildren<PlayerMovement>();
        if (players.Length == 0) return;

        // --- Build full list of seats ---
        List<GameManager.ColorData> seatList = new List<GameManager.ColorData>();
        foreach (var player in players)
        {
            for (int i = 0; i < player.seatCapacity; i++)
            {
                seatList.Add(player.vehicleColor);
            }
        }

        // --- Apply mode rules ---
        switch (GameManager.instance.currentMode)
        {
            case GameMode.Easy:
                // No shuffle (keep order as is)
                break;

            case GameMode.Medium:
                // Shuffle all seats randomly
                Shuffle(seatList);
                break;

            case GameMode.Hard:
                // Sort/group by color
                Shuffle(seatList);
                break;
        }

        // --- Spawn ---
        int circleIndex = 0;
        foreach (var color in seatList)
        {
            float angle = (startingAngle + circleIndex * spacingAngle) * Mathf.Deg2Rad;

            float x = Mathf.Cos(angle) * radius;
            float z = -Mathf.Sin(angle) * radius;

            Vector3 spawnPos = new Vector3(centerPoint.position.x + x,
                                           centerPoint.position.y,
                                           centerPoint.position.z + z);

            GameObject newChar = Instantiate(characterPrefab, spawnPos, Quaternion.identity, transform);

            // Face tangent of circle
            Vector3 dir = (spawnPos - centerPoint.position).normalized;
            Vector3 tangent = new Vector3(dir.z, 0, -dir.x);
            newChar.transform.rotation = Quaternion.LookRotation(tangent, Vector3.up);

            // Assign color
            newChar.GetComponent<Player>().playerColor = color;

            circleIndex++;
        }


        playersArray = transform.GetComponentsInChildren<Player>();
        ConvertPlayerArrayToList();
    }

    void ConvertPlayerArrayToList()
    {
        foreach (var player in playersArray)
        {
            players.Add(player);
        }
    }

    // Fisher–Yates shuffle
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

    public void TakeOffThePlayer(ColorData targetColor)
    {
       
    }
}
