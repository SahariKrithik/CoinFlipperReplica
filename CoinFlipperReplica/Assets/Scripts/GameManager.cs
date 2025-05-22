using UnityEngine;
using UnityEngine.UI;
using TMPro; // <-- Add this for TextMesh Pro

public class GameManager : MonoBehaviour
{
    public TMP_Text balanceText, resultText;           // Updated
    public TMP_InputField betInput;                    // Updated
    public Button headsButton, tailsButton;
    public Image coinImage;
    public Sprite headsSprite, tailsSprite;

    private int balance = 1000;

    void Start()
    {
        UpdateBalanceText();
        headsButton.onClick.AddListener(() => FlipCoin(0));
        tailsButton.onClick.AddListener(() => FlipCoin(1));
    }

    void FlipCoin(int playerChoice)
    {
        int bet;

        if (!int.TryParse(betInput.text, out bet) || bet <= 0 || bet > balance)
        {
            resultText.text = "Invalid Bet!";
            return;
        }

        int result = Random.Range(0, 2); // 0 = Heads, 1 = Tails
        coinImage.sprite = (result == 0) ? headsSprite : tailsSprite;

        if (playerChoice == result)
        {
            balance += bet;
            resultText.text = "You Win!";
        }
        else
        {
            balance -= bet;
            resultText.text = "You Lose!";
        }

        UpdateBalanceText();
    }

    void UpdateBalanceText()
    {
        balanceText.text = "Balance: $" + balance;
    }
}
