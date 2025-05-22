using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponsiveUI : MonoBehaviour
{
    [Header("Layout Elements")]
    public LayoutElement headsLayout;
    public LayoutElement tailsLayout;
    public LayoutElement betInputLayout;

    [Header("Coin Image")]
    public RectTransform coinRect;

    [Header("Text Elements")]
    public TMP_Text balanceText;
    public TMP_Text resultText;

    [Header("Sizing Multipliers")]
    public float buttonWidthPercent = 0.25f;
    public float buttonHeightPercent = 0.12f;

    public float inputWidthPercent = 0.4f;
    public float inputHeightPercent = 0.1f;

    public float coinSizePercent = 0.2f;

    public int fontBaseSize = 36;
    public float fontScaleFactor = 0.02f; // % of screen width

    private int lastScreenWidth;
    private int lastScreenHeight;

    void Start()
    {
        ResizeUI();
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }

    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            ResizeUI();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
        }
    }

    void ResizeUI()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Resize buttons
        float buttonWidth = screenWidth * buttonWidthPercent;
        float buttonHeight = screenHeight * buttonHeightPercent;

        headsLayout.preferredWidth = buttonWidth;
        headsLayout.preferredHeight = buttonHeight;

        tailsLayout.preferredWidth = buttonWidth;
        tailsLayout.preferredHeight = buttonHeight;

        // Resize input field
        float inputWidth = screenWidth * inputWidthPercent;
        float inputHeight = screenHeight * inputHeightPercent;

        betInputLayout.preferredWidth = inputWidth;
        betInputLayout.preferredHeight = inputHeight;

        // Resize coin image
        float coinSize = screenWidth * coinSizePercent;
        coinRect.sizeDelta = new Vector2(coinSize, coinSize);

        // Resize font size for text elements
        int calculatedFontSize = Mathf.RoundToInt(screenWidth * fontScaleFactor);
        balanceText.fontSize = calculatedFontSize;
        resultText.fontSize = calculatedFontSize;
    }
}
