using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ShootPowerUpTests
{
    private Game game;
    
    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject =
            MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        game = gameGameObject.GetComponent<Game>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(game.gameObject);
    }
    
    // A Test behaves as an ordinary method
    [Test]
    public void PowerUpShouldSpawnAtTheTop()
    {
        GameObject powerUp = game.GetSpawner().SpawnShootPowerUp();
        float powerUpYPos = powerUp.transform.position.y;

        Assert.AreEqual(7.35f, powerUpYPos);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ShootPowerUpShouldMovesDownward()
    {
        GameObject powerUp = game.GetSpawner().SpawnShootPowerUp();
        Vector2 initialPosition = powerUp.transform.position;
        yield return new WaitForSeconds(1);
        Vector2 newPosition = powerUp.transform.position;
        Assert.Less(newPosition.y, initialPosition.y);
    }

    [UnityTest]
    public IEnumerator ShootPowerUpShouldDisappearOnScreenBottom()
    {
        GameObject powerUp = game.GetSpawner().SpawnShootPowerUp();
        powerUp.transform.position = new Vector2(powerUp.transform.position.x, -8);
        yield return new WaitForSeconds(0.1f);
        UnityEngine.Assertions.Assert.IsNull(powerUp);
    }

    [UnityTest]
    public IEnumerator ShootPowerUpCaughtByPlayer()
    {
        GameObject powerUp = game.GetSpawner().SpawnShootPowerUp();
        Vector2 playerPosition  = game.GetShip().transform.position;

        powerUp.transform.position = playerPosition;
        yield return new WaitForSeconds(0.1f);
        Assert.True(game.GetShip().IsShootPowerUpActive);
    }
}
