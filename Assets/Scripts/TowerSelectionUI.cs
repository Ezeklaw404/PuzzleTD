using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class TowerSelectionUI : MonoBehaviour
{
    [Tooltip("Parent RectTransform where buttons will be spawned")]
    [SerializeField] private RectTransform buttonContainer;

    [Tooltip("A simple Button prefab with a Text child")]
    [SerializeField] private Button buttonPrefab;

    private Action<GameObject> onSelected;
    /// <summary>
    /// Call this to initialize the menu.
    /// </summary>
    public void Show(List<GameObject> towerOptions, Action<GameObject> onSelectedCallback)
    {
        Debug.Log($"[TowerSelectionUI] Show() called – towerOptions.Count = {towerOptions?.Count}");

        onSelected = onSelectedCallback;

        // create one button per tower prefab
        foreach (var prefab in towerOptions)
        {
            var btn = Instantiate(buttonPrefab, buttonContainer);
            var txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (txt == null)
            {
                Debug.LogError("No TMP text component found on buttonPrefab!");
            }
            else
            {
                txt.text = prefab.name;

                // force the layout to rebuild so LayoutUtility can read the new preferred size
                LayoutRebuilder.ForceRebuildLayoutImmediate(txt.rectTransform);

                float textWidth = LayoutUtility.GetPreferredSize(txt.rectTransform, 0);
                float padding = 20f;  // pick whatever horizontal padding you want
                RectTransform btnRT = btn.GetComponent<RectTransform>();
                btnRT.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    textWidth + padding
                );
            }
            btn.onClick.AddListener(() => Select(prefab));
        }
    }

    private void Select(GameObject prefab)
    {
        onSelected?.Invoke(prefab);
        Destroy(gameObject);
    }
}
