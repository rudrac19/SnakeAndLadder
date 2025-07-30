using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerSystem : MonoBehaviour
{
    public DiceRollingSystem diceRollingSystem;
    public RectTransform player1;
    public RectTransform player2;

    public int[] playerPositions = new int[2]; // 0 = player1, 1 = player2
    private int currentPlayerIndex = 0;
    private bool hasRolled = false;

    public Vector2[] boardCoordinates = new Vector2[101];

    // Snakes and Ladders
    private readonly int[] snakeHeads = { 47, 86, 91, 99 };
    private readonly int[] snakeTails = { 16, 52, 53, 58 };
    private readonly int[] ladderBottoms = { 2, 8, 11, 33 };
    private readonly int[] ladderTops = { 57, 15, 70, 76 };

    void Start()
    {
        GenerateBoardCoordinates();
        ResetPlayers();
    }

    void ResetPlayers()
    {
        playerPositions[0] = 0;
        playerPositions[1] = 0;
        player1.anchoredPosition = boardCoordinates[0];
        player2.anchoredPosition = boardCoordinates[0];
    }

    void GenerateBoardCoordinates()
    {
        int index = 0;
        bool leftToRight = true;

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                int actualX = leftToRight ? col : 11 - col;

                float x = -550 + actualX * 100;        // 100 units per square horizontally
                float y = -450f + row * 100;         // 110 units per row vertically (bottom to top)

                boardCoordinates[index] = new Vector2(x, y);
                index++;
            }

            leftToRight = !leftToRight; // Flip direction every row
        }
        boardCoordinates[10] = new Vector2(450, -450);
        boardCoordinates[20] = new Vector2(-450, -350);
        boardCoordinates[30] = new Vector2(450, -250);
        boardCoordinates[40] = new Vector2(-450, -150);
        boardCoordinates[50] = new Vector2(450, -50);
        boardCoordinates[60] = new Vector2(-450, 50);
        boardCoordinates[70] = new Vector2(450, 150);
        boardCoordinates[80] = new Vector2(-450, 250);
        boardCoordinates[90] = new Vector2(450, 350);
        boardCoordinates[100] = new Vector2(-450, 450);
    }
   



    // Called when the dice is rolled
    public void OnDiceRolled(int rolledValue)
    {
        if (hasRolled) return; // Prevent multiple rolls
        diceRollingSystem.randomInteger = rolledValue;
        hasRolled = true;
    }

    // Called when the Move button is pressed
    public void MoveCurrentPlayer()
    {
        if (!hasRolled)
        {
            Debug.Log("You must roll the dice before moving!");
            return;
        }

        int steps = diceRollingSystem.randomInteger;
        int player = currentPlayerIndex;

        int newPosition = playerPositions[player] + steps;

        if (newPosition > 100)
        {
            Debug.Log("Move exceeds board. Turn skipped.");
            hasRolled = false;
            currentPlayerIndex = 1 - currentPlayerIndex;
            return;
        }

        playerPositions[player] = newPosition;

        // Apply snake
        for (int i = 0; i < snakeHeads.Length; i++)
        {
            if (playerPositions[player] == snakeHeads[i])
            {
                Debug.Log("Player hit a snake!");
                playerPositions[player] = snakeTails[i];
                break;
            }
        }

        // Apply ladder
        for (int i = 0; i < ladderBottoms.Length; i++)
        {
            if (playerPositions[player] == ladderBottoms[i])
            {
                Debug.Log("Player climbed a ladder!");
                playerPositions[player] = ladderTops[i];
                break;
            }
        }

        // Final safety clamp (in case)
        playerPositions[player] = Mathf.Clamp(playerPositions[player], 0, 99);

        // Move the player's UI
        Vector2 newPos = boardCoordinates[playerPositions[player]];
        if (player == 0)
            StartCoroutine(AnimateMove(player1, newPos));
        else
            StartCoroutine(AnimateMove(player2, newPos));

        IEnumerator AnimateMove(RectTransform playerTransform, Vector2 targetPosition, float speed = 600f)
        {
            while (Vector2.Distance(playerTransform.anchoredPosition, targetPosition) > 0.1f)
            {
                playerTransform.anchoredPosition = Vector2.MoveTowards(
                    playerTransform.anchoredPosition,
                    targetPosition,
                    speed * Time.deltaTime
                );
                yield return null;
            }

            playerTransform.anchoredPosition = targetPosition;

            // Win check after move completes
            int player = (playerTransform == player1) ? 0 : 1;
            if (playerPositions[player] == 100)
            {
                Debug.Log($"Player {player + 1} wins!");
                // Optional: Disable UI here
            }
        }



        // End turn
        hasRolled = false;
        currentPlayerIndex = 1 - currentPlayerIndex;
    }
}
