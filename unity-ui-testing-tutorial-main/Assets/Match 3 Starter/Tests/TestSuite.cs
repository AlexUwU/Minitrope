using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
public class TestSuite
{
    private BoardManager boardManager;
    public List<Sprite> candies;
    [SetUp]
    public void SetUp()
    {
        GameObject canvas = new GameObject("Canvas", typeof(Canvas));
        GameObject boardManagerObject = new GameObject("BoardManager", typeof(BoardManager));
        boardManager = boardManagerObject.GetComponent<BoardManager>();

        Assert.IsNotNull(boardManager, "BoardManager no se inicializó correctamente.");

        boardManager.xSize = 5;
        boardManager.ySize = 5;

        boardManager.candies = new List<Sprite>
        {
            Resources.Load<Sprite>("Sprites/Characters/Blue"),
            Resources.Load<Sprite>("Sprites/Characters/Green"),
            Resources.Load<Sprite>("Sprites/Characters/Multi"),
            Resources.Load<Sprite>("Sprites/Characters/Purple"),
            Resources.Load<Sprite>("Sprites/Characters/Yellow"),
            Resources.Load<Sprite>("Sprites/Characters/Red"),
        };

        foreach (var candy in boardManager.candies)
        {
            Assert.IsNotNull(candy, "Una de las sprites no se cargó correctamente.");
        }

        boardManager.tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");

        Assert.IsNotNull(boardManager.tilePrefab, "El tilePrefab no se cargó correctamente.");
    }

    [Test]
    public void CombinarTresFichas()
    {
        Sprite[,] grid = new Sprite[5, 5];
        grid[0, 0] = boardManager.candies[0];
        grid[1, 0] = boardManager.candies[0];
        grid[2, 0] = boardManager.candies[0];
        grid[3, 0] = boardManager.candies[1];
        grid[4, 0] = boardManager.candies[1];

        for (int x = 0; x < 5; x++)
        {
            for (int y = 1; y < 5; y++)
            {
                grid[x, y] = boardManager.candies[Random.Range(0, boardManager.candies.Count)];
            }
        }

        boardManager.InitializeBoard(grid);

        List<GameObject> matches = boardManager.FindMatches(new Vector2Int(0, 0));

        Assert.IsTrue(matches.Count >= 3, "No se detectó la combinación de 3 fichas.");
    }
    [Test]
    public void CombinacionEspecialDeCuatroFichas()
    {
        Sprite[,] grid = new Sprite[5, 5];
        grid[0, 0] = boardManager.candies[0];
        grid[1, 0] = boardManager.candies[0];
        grid[2, 0] = boardManager.candies[0];
        grid[3, 0] = boardManager.candies[0];
        grid[4, 0] = boardManager.candies[1];

        for (int x = 0; x < 5; x++)
        {
            for (int y = 1; y < 5; y++)
            {
                grid[x, y] = boardManager.candies[Random.Range(0, boardManager.candies.Count)];
            }
        }

        boardManager.InitializeBoard(grid);

        List<GameObject> matches = boardManager.FindMatches(new Vector2Int(0, 0));

        Assert.IsTrue(matches.Count >= 4, "No se detectó la combinación de 4 fichas.");
    }

    [Test]
    public void SinCombinacionesDisponibles()
    {
        Sprite[,] grid = new Sprite[5, 5];

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                grid[x, y] = boardManager.candies[Random.Range(0, boardManager.candies.Count)];
            }
        }

        boardManager.InitializeBoard(grid);

        List<GameObject> matches = boardManager.FindMatches(new Vector2Int(0, 0));

        Assert.IsTrue(matches.Count == 0, "Se detectaron combinaciones cuando no debería haber ninguna.");
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(boardManager.gameObject);
    }
}