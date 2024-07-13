using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text storageText;
    [SerializeField] Image happinessBar;

    [SerializeField] TMP_Text turnText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateResourcesUI(ResourceCounterList resourcesStorage, float happiness)
    {
        storageText.text = "Metal: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Metal) +
            "\nWater: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Water) +
            "\nWorker: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Worker) +
            "\nScience: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Science);
        happinessBar.fillAmount = happiness / 100;
    }

    public void updateTurnUI(uint turn)
    {
        turnText.text = turn + "/100";
    }
}
