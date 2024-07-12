using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    public static ResourcesUI Instance;

    [SerializeField] TMP_Text storageText;
    [SerializeField] Image happinessBar;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateUI(ResourceCounterList resourcesStorage, float happiness)
    {
        storageText.text = "Metal: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Metal) +
            "\nWater: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Water) +
            "\nWorker: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Worker) +
            "\nScience: " + resourcesStorage.AdjustedAmountOfResource(ResourceType.Science);
        happinessBar.fillAmount = happiness / 100;
    }
}
