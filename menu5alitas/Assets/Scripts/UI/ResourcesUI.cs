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
        storageText.text = "Metal: " + resourcesStorage.AdjustedAmount(ResourceType.Metal) +
            "\nWater: " + resourcesStorage.AdjustedAmount(ResourceType.Water) +
            "\nWorker: " + resourcesStorage.AdjustedAmount(ResourceType.Worker) +
            "\nScience: " + resourcesStorage.AdjustedAmount(ResourceType.Science);
        happinessBar.fillAmount = happiness / 100;
    }
}
