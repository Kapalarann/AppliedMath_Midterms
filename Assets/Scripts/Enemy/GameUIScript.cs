using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

public class GameUIScript : MonoBehaviour
{
    [Header("Currency")]
    public float currency;
    private float displayedCurrency;
    public float currencyLerpSpeed = 8f;
    public TextMeshProUGUI currencyUI;

    [Header("Wave")]
    public int Wave;
    public TextMeshProUGUI waveUI;

    [Header("Player HP")]
    public float playerHP;
    public float maxHP = 100f;

    private float displayedHP;
    public float hpLerpSpeed = 10f;

    public Slider playerSlider;

    public float shakeDuration = 0.2f;
    public float shakeStrength = 10f;

 
    private Vector3 currencyOriginalPos;

    private float lastCurrency;

    public static GameUIScript instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        displayedCurrency = currency;
        displayedHP = playerHP;

        playerSlider.maxValue = maxHP;
        playerSlider.value = playerHP;

        playerHP = maxHP;

        currencyOriginalPos = currencyUI.rectTransform.anchoredPosition;
        lastCurrency = currency;

        currency = Economy.instance.money;

        UpdateWaveUI();
        UpdateCurrencyInstant();
    }


    void Update()
    {
        currency = Economy.instance.money;
        AnimateCurrency();
        AnimateHP();

        if (currency != lastCurrency)
        {
            StartCoroutine(ShakeCurrency());
            lastCurrency = currency;
        }
    }

    void AnimateCurrency()
    {
        displayedCurrency = Mathf.Lerp(displayedCurrency, currency, Time.deltaTime * currencyLerpSpeed);

        bool isLerping = Mathf.Abs(displayedCurrency - currency) > 0.01f;

        if (!isLerping)
            displayedCurrency = currency;

        currencyUI.text = "₱" + displayedCurrency.ToString("0");
    }

    void AnimateHP()
    {
        displayedHP = Mathf.Lerp(displayedHP, playerHP, Time.deltaTime * hpLerpSpeed);

        if (Mathf.Abs(displayedHP - playerHP) < 0.01f)
            displayedHP = playerHP;

        playerSlider.value = displayedHP;
    }


    IEnumerator ShakeCurrency()
    {
        float time = 0f;

        while (time < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitCircle * shakeStrength;

            currencyUI.rectTransform.anchoredPosition = currencyOriginalPos + randomOffset;
            time += Time.deltaTime;
            yield return null;
        }

        currencyUI.rectTransform.anchoredPosition = currencyOriginalPos;
    }

    

    void UpdateCurrencyInstant()
    {
        displayedCurrency = currency;
        currencyUI.text = currency.ToString("0");
    }

    void UpdateWaveUI()
    {
        waveUI.text = "Wave " + Wave.ToString();
    }

    public void SetHP(float hp)
    {
        playerHP = hp;
    }

    public void dmgHP(float dmg)
    {
        playerHP -= dmg;
        CameraShake.instance.Shake(0.2f, 0.5f);
    }

    public void AddCurrency(float amount)
    {
        Economy.instance.money += amount;
    }

    public void SetWave(int newWave)
    {
        Wave = newWave;
        UpdateWaveUI();
    }
}
