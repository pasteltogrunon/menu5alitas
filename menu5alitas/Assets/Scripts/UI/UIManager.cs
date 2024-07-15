using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text[] storageText;
    [SerializeField] TMP_Text[] productionText;
    [SerializeField] TMP_Text[] costText;
    [SerializeField] TMP_Text[] cardCostText;
    [SerializeField] Image happinessBar;
    [SerializeField] TMP_Text happinessFactorText;

    [SerializeField] TMP_Text catastropheText;

    [SerializeField] TMP_Text turnText;

    [SerializeField] GameObject GolemScreen;

    [SerializeField] TMP_Text[] golemSliderTexts;
    [SerializeField] TMP_Text golemScoreText;

    [SerializeField] GameObject winGolemScreen;
    [SerializeField] TMP_Text winText;
    [TextArea(3,10)] [SerializeField] string winString;
    [SerializeField] GameObject defeatGolemScreen;
    [SerializeField] TMP_Text defeatText;
    [TextArea(3, 10)] [SerializeField] string defeatString;
    [SerializeField] GameObject picketsScreen;
    [SerializeField] TMP_Text picketText;
    [TextArea(3, 10)] [SerializeField] string picketString;

    int[] spentResourcesAux = new int[4];

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateResourcesUI(ResourceCounterList resourcesStorage, ResourceCounterList production, ResourceCounterList cost, float happiness)
    {
        int i = 0;
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            storageText[i].text = formatString(resourcesStorage.AdjustedAmountOfResource(resource));
            i++;
        }

        i = 0;
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            productionText[i].text = "+" + formatString(production.AdjustedAmountOfResource(resource));
            i++;
        }

        i = 0;
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            costText[i].text = "-" + formatString(cost.AdjustedAmountOfResource(resource));
            i++;
        }
        happinessBar.fillAmount = happiness / 100;
        happinessFactorText.text = "x" + Mathf.Pow(happiness / 100, 2);
    }

    public string formatString(int amount)
    {
        if(amount >= 1000)
        {
            return String.Format("{0:0.00}", (float)amount / 1000)  + "K";
        }

        return amount.ToString();
    }

    public void updateTurnUI(uint turn)
    {
        turnText.text = turn + "/50";
    }

    public void updateCatastropheText(string text)
    {
        catastropheText.text = text;
    }
    
    public void endGameByGolem(bool win)
    {
        (win ? winGolemScreen : defeatGolemScreen).SetActive(true);
        StartCoroutine(showTextInTextbox(0.03f, (win ? winString : defeatString), (win ? winText : defeatText)));
    }

    public void endGameByPickets()
    {
        picketsScreen.SetActive(true);
        StartCoroutine(showTextInTextbox(0.03f, picketString, picketText));
    }

    public void updateCostText(ResourceCounterList cost)
    {
        int i = 0;
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            cardCostText[i].text = "-" + formatString(cost.AdjustedAmountOfResource(resource));

            if (cost.AdjustedAmountOfResource(resource) > ResourceManager.Instance.storedResources.AdjustedAmountOfResource(resource))
                cardCostText[i].color = Color.red;
            else
                cardCostText[i].color = Color.white;

            i++;
        }
        
    }


    public void ShowGolemScreen()
    {
        GolemScreen.SetActive(true);
        ChangeMetalSlider(0);
        ChangeWaterSlider(0);
        ChangeWorkerSlider(0);
        ChangeScienceSlider(0);
    }

    public void ChangeMetalSlider(float fillAmount)
    {
        spentResourcesAux[0] = Mathf.CeilToInt(ResourceManager.Instance.storedResources.AdjustedAmountOfResource(ResourceType.Metal) * fillAmount);
        golemSliderTexts[0].text = spentResourcesAux[0] + " / " + ResourceManager.Instance.storedResources.AdjustedAmountOfResource(ResourceType.Metal);
    }

    public void ChangeWaterSlider(float fillAmount)
    {
        spentResourcesAux[1] = Mathf.CeilToInt(ResourceManager.Instance.storedResources.AdjustedAmountOfResource(ResourceType.Water) * fillAmount);
        golemSliderTexts[1].text = spentResourcesAux[1] + " / " + ResourceManager.Instance.storedResources.AdjustedAmountOfResource(ResourceType.Water);
    }

    public void ChangeWorkerSlider(float fillAmount)
    {
        spentResourcesAux[2] = Mathf.CeilToInt(ResourceManager.Instance.storedResources.AdjustedAmountOfResource(ResourceType.Worker) * fillAmount);
        golemSliderTexts[2].text = spentResourcesAux[2] + " / " + ResourceManager.Instance.storedResources.AdjustedAmountOfResource(ResourceType.Worker);
    }

    public void ChangeScienceSlider(float fillAmount)
    {
        spentResourcesAux[3] = Mathf.CeilToInt(ResourceManager.Instance.storedResources.AdjustedAmountOfResource(ResourceType.Science) * fillAmount);
        golemSliderTexts[3].text = spentResourcesAux[3] + " / " + ResourceManager.Instance.storedResources.AdjustedAmountOfResource(ResourceType.Science);
    }

    public void DoneGolemScreen()
    {
        ResourceCounterList spentResources = new ResourceCounterList(ResourceCounterType.AlreadyAdjusted);

        spentResources.metalResourceCounter.amount = spentResourcesAux[0];
        spentResources.waterResourceCounter.amount = spentResourcesAux[1];
        spentResources.workerResourceCounter.amount = spentResourcesAux[2];
        spentResources.scienceResourceCounter.amount = spentResourcesAux[3];

        golemScoreText.text = GameManager.Instance.GolemScore(spentResources) + "/5";
    }

    public void EndGolemScreen()
    {
        HandManager.Instance.isInGolemScreen = false;
    }

    public IEnumerator showTextInTextbox(float interval, string text, TMP_Text textbox)
    {
        int character = 0;
        while(textbox.text != text)
        {
            textbox.text = text.Substring(0, character);
            yield return new WaitForSeconds(interval);
            character++;
        }
    }
}
