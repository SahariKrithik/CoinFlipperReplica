using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text balanceText, resultText;
    public TMP_InputField betInput;
    public Button headsButton, tailsButton;
    public Image coinImage;
    public Sprite headsSprite, tailsSprite;
    public RectTransform multiplierGroup;

    [Header("Multiplier Buttons")]
    public Button halfXButton;
    public Button oneXButton;
    public Button twoXButton;

    [Header("Multiplier Button Highlight Colors")]
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;

    private int balance = 1000;
    private float selectedMultiplier = -1f;

    private Vector3 multiplierGroupOriginalPos;

    void Start()
    {
        coinImage.rectTransform.localScale = Vector3.one;
        resultText.gameObject.SetActive(false);

        headsButton.interactable = false;
        tailsButton.interactable = false;

        multiplierGroupOriginalPos = multiplierGroup.localPosition;

        UpdateBalanceText();

        headsButton.onClick.AddListener(() => FlipCoin(0));
        tailsButton.onClick.AddListener(() => FlipCoin(1));

        halfXButton.onClick.AddListener(() => SelectMultiplier(0.5f, halfXButton));
        oneXButton.onClick.AddListener(() => SelectMultiplier(1f, oneXButton));
        twoXButton.onClick.AddListener(() => SelectMultiplier(2f, twoXButton));
    }

    void SelectMultiplier(float value, Button selectedButton)
    {
        selectedMultiplier = value;
        resultText.gameObject.SetActive(true);
        resultText.text = $"Multiplier set to {value}x";

        headsButton.interactable = true;
        tailsButton.interactable = true;

        StopAllCoroutines(); // Stop shake immediately
        ResetMultiplierButtonVisuals();

        // Highlight selected using ColorBlock
        SetButtonHighlight(selectedButton, selectedColor);

        // Reset group position
        multiplierGroup.localPosition = multiplierGroupOriginalPos;
    }

    void ResetMultiplierButtonVisuals()
    {
        SetButtonHighlight(halfXButton, defaultColor);
        SetButtonHighlight(oneXButton, defaultColor);
        SetButtonHighlight(twoXButton, defaultColor);
    }

    void SetButtonHighlight(Button button, Color color)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.selectedColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        button.colors = cb;
    }

    void FlipCoin(int playerChoice)
    {
        int bet;

        if (selectedMultiplier < 0f)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = "Please select a multiplier!";
            StartCoroutine(ShakeButtonsUntilSelected());
            return;
        }

        if (!int.TryParse(betInput.text, out bet) || bet <= 0 || bet > balance)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = "Invalid Bet!";
            return;
        }

        int result = Random.Range(0, 2); // 0 = Heads, 1 = Tails
        StartCoroutine(AnimateFlip(result, playerChoice, bet));
    }

    IEnumerator AnimateFlip(int result, int playerChoice, int bet)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = "Flipping...";

        float duration = 0.6f;
        float halfTime = duration / 2f;
        float elapsed = 0f;

        while (elapsed < halfTime)
        {
            float scale = Mathf.Lerp(1f, 0f, elapsed / halfTime);
            coinImage.rectTransform.localScale = new Vector3(scale, 1f, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        coinImage.sprite = (result == 0) ? headsSprite : tailsSprite;
        elapsed = 0f;

        while (elapsed < halfTime)
        {
            float scale = Mathf.Lerp(0f, 1f, elapsed / halfTime);
            coinImage.rectTransform.localScale = new Vector3(scale, 1f, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        int payout = Mathf.RoundToInt(bet * selectedMultiplier);

        if (playerChoice == result)
        {
            balance += payout;
            resultText.text = $"You Win! +${payout}";
        }
        else
        {
            balance -= payout;
            resultText.text = $"You Lose! -${payout}";
        }

        UpdateBalanceText();

        yield return new WaitForSeconds(2f);
        resultText.gameObject.SetActive(false);

        // Reset UI state for next round
        selectedMultiplier = -1f;
        ResetMultiplierButtonVisuals();
        headsButton.interactable = false;
        tailsButton.interactable = false;
    }

    void UpdateBalanceText()
    {
        balanceText.text = $"Balance: ${balance}";
    }

    IEnumerator ShakeButtonsUntilSelected()
    {
        float shakeAmount = 5f;
        float shakeSpeed = 15f;

        while (selectedMultiplier < 0f)
        {
            float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            multiplierGroup.localPosition = multiplierGroupOriginalPos + new Vector3(offsetX, 0f, 0f);
            yield return null;
        }

        multiplierGroup.localPosition = multiplierGroupOriginalPos;
    }
}
