using UnityEngine;

using GameLogic.Entities;
using GameLogic.State;

public class GameState : GameStateWrapper
{
    public Transform moneyPrefab;
    public Transform spawnPoint;
    IStructure spaceship;

    public int countdownAmount = 600;
    public int countdown = 0;

    private void Start()
    {
        spaceship = GameObject.Find("SpaceShip").GetComponent<StructureBehaviour>();
    }

    public void Update()
    {
        countdown--;
        if (countdown <= 0)
        {
            countdown = countdownAmount;

            Vector2 rotation = Quaternion.AngleAxis(Random.Range(0f, (float)(System.Math.PI * 2f)), Vector3.up) * new Vector2(0, 4);

            spawnPoint.position = (Vector2)(spaceship.position) + rotation;

            Instantiate(moneyPrefab, spawnPoint);
        }
    }
}
