using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
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

    [UnityTest]
    public IEnumerator AsteroidsMoveDown()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        float initialYPos = asteroid.transform.position.y;
        yield return new WaitForSeconds(0.1f);

        Assert.Less(asteroid.transform.position.y, initialYPos);
    }

    //[UnityTest]
    //public IEnumerator GameOverOccursOnAsteroidCollision()
    //{
    //    GameObject asteroid = game.GetSpawner().SpawnAsteroid();
    //    asteroid.transform.position = game.GetShip().transform.position;
    //    yield return new WaitForSeconds(0.1f);

    //    Assert.True(game.isGameOver);
    //}

    [UnityTest]
    public IEnumerator NewGameRestartsGame()
    {
        //1
        game.isGameOver = true;
        game.NewGame();
        //2
        Assert.False(game.isGameOver);
        yield return null;
    }

    [UnityTest]
    public IEnumerator LaserMovesUp()
    {
        // 1
        GameObject laser = game.GetShip().SpawnLaser();
        // 2
        float initialYPos = laser.transform.position.y;
        yield return new WaitForSeconds(0.1f);
        // 3
        Assert.Greater(laser.transform.position.y, initialYPos);
    }
    
    [UnityTest]
    public IEnumerator LaserDestroysAsteroid()
    {
        // 1
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;
        GameObject laser = game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        // 2
        UnityEngine.Assertions.Assert.IsNull(asteroid);
    }
    
    [UnityTest]
    public IEnumerator DestroyedAsteroidRaisesScore()
    {
        // 1
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;
        GameObject laser = game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        // 2
        Assert.AreEqual(game.score, 1);
    }

    [UnityTest]
    public IEnumerator GameStartsWithZeroScore()
    {
        game.score = 2;
        game.NewGame();

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(game.score, 0);
    }

    [UnityTest]
    public IEnumerator MovingLeftAndRightWorks()
    {
        var initalPos = game.GetShip().transform.position;
        
        game.GetShip().MoveLeft();
        yield return new WaitForSeconds(0.1f);
        Assert.Less(game.GetShip().transform.position.x, initalPos.x);
        
        initalPos = game.GetShip().transform.position;
        
        game.GetShip().MoveRight();
        yield return new WaitForSeconds(0.1f);
        Assert.Greater(game.GetShip().transform.position.x, initalPos.x);
    }

    [UnityTest]
    public IEnumerator CheckIfTextUpdates()
    {
        game.NewGame();
        var expectedLives = game.lives;
        yield return new WaitForSeconds(0.1f);
        string lifeText = game.GetLifeText();
        Assert.True(lifeText.EndsWith(expectedLives.ToString()));

        Game.LoseLife();
        expectedLives--;

        yield return new WaitForSeconds(0.1f);
        lifeText = game.GetLifeText();
        Assert.True(lifeText.EndsWith(expectedLives.ToString()));
    }

    [UnityTest]
    public IEnumerator CheckIfAsteroidCollisionApplies()
    {
        game.NewGame();
        var expectedLives = game.lives - 1;
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = game.GetShip().transform.position;
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(expectedLives, game.lives);
        UnityEngine.Assertions.Assert.IsNull(asteroid);
    }

    [UnityTest]
    public IEnumerator CheckIfNoLivesIsGameOver()
    {
        game.lives = 1;
        Game.LoseLife();
        yield return new WaitForSeconds(0.1f);
        Assert.True(game.isGameOver);
    }
}