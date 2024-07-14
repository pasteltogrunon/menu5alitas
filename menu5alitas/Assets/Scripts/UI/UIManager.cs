using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text storageText;
    [SerializeField] TMP_Text productionText;
    [SerializeField] TMP_Text costText;
    [SerializeField] Image happinessBar;
    [SerializeField] TMP_Text happinessFactorText;

    [SerializeField] TMP_Text catastropheText;

    [SerializeField] TMP_Text turnText;

    [SerializeField] GameObject GolemScreen;

    [SerializeField] TMP_Text[] golemSliderTexts;
    [SerializeField] TMP_Text golemScoreText;

    int[] spentResourcesAux = new int[4];

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateResourcesUI(ResourceCounterList resourcesStorage, ResourceCounterList production, ResourceCounterList cost, float happiness)
    {
        storageText.text = "Metal: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Metal) +
            "\nWater: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Water) +
            "\nWorker: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Worker) +
            "\nScience: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Science);

        productionText.text = "Metal: " + production.AdjustedAmountOfResource(ResourceType.Metal) +
            "\nWater: " + production.AdjustedAmountOfResource(ResourceType.Water) +
            "\nWorker: " + production.AdjustedAmountOfResource(ResourceType.Worker) +
            "\nScience: " + production.AdjustedAmountOfResource(ResourceType.Science);

        costText.text = "Metal: " + cost.AdjustedAmountOfResource(ResourceType.Metal) +
            "\nWater: " + cost.AdjustedAmountOfResource(ResourceType.Water) +
            "\nWorker: " + cost.AdjustedAmountOfResource(ResourceType.Worker) +
            "\nScience: " + cost.AdjustedAmountOfResource(ResourceType.Science);

        happinessBar.fillAmount = happiness / 100;
        happinessFactorText.text = "x" + Mathf.Pow(happiness / 100, 2);
    }

    public void updateTurnUI(uint turn)
    {
        turnText.text = turn + "/50";
    }

    public void updateCatastropheText(string text)
    {
        catastropheText.text = text;
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
}
