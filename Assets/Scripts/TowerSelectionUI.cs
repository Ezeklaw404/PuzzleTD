using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class TowerSelectionUI : MonoBehaviour
{
    [SerializeField] private RectTransform buttonContainer;  // with a HorizontalLayoutGroup on it
    [SerializeField] private GameObject buttonPrefab;     // root has Button + TextMeshProUGUI child

    private Action<GameObject> onSelected;

    public void Show(List<GameObject> towerOptions, Action<GameObject> onSelectedCallback)
    {
        onSelected = onSelectedCallback;

        // Clear out any old entries
        foreach (Transform t in buttonContainer)
            Destroy(t.gameObject);

        // Helper to size a button to its TMP child
        void SizeButton(GameObject btnGO, float padX, float padY)
        {
            var tmp = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(tmp.rectTransform);

            float w = LayoutUtility.GetPreferredSize(tmp.rectTransform, 0);
            float h = LayoutUtility.GetPreferredSize(tmp.rectTransform, 1);

            var rt = btnGO.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w + padX);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h + padY);
        }

        // Helper to create one “entry” with optional cost label
        void CreateEntry(string label, Action clickAction, int? cost = null)
        {
            // 1) Wrapper with VerticalLayoutGroup + ContentSizeFitter
            var entryGO = new GameObject(label + "Entry", typeof(RectTransform));
            entryGO.transform.SetParent(buttonContainer, false);

            var vlg = entryGO.AddComponent<VerticalLayoutGroup>();
            vlg.childControlWidth = false;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = false;
            vlg.childForceExpandHeight = false;
            vlg.spacing = 4;

            var csf = entryGO.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // 2) The button itself
            var btnGO = Instantiate(buttonPrefab, entryGO.transform);
            var btn = btnGO.GetComponent<Button>();
            var tmp = btnGO.GetComponentInChildren<TextMeshProUGUI>();

            tmp.text = label;
            SizeButton(btnGO, padX: 20f, padY: 10f);

            btn.onClick.AddListener(() => {
                clickAction();
                Destroy(gameObject);
            });

            // 3) Optional cost label under the button
            if (cost.HasValue)
            {
                var costGO = new GameObject("CostLabel", typeof(RectTransform));
                costGO.transform.SetParent(entryGO.transform, false);

                var costTMP = costGO.AddComponent<TextMeshProUGUI>();
                costTMP.alignment = TextAlignmentOptions.Center;
                costTMP.fontSize = tmp.fontSize * 0.8f;
                costTMP.text = $"Cost: {cost.Value}";

                var costCSF = costGO.AddComponent<ContentSizeFitter>();
                costCSF.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                costCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }

        // 4) Create one entry per tower type
        foreach (var prefab in towerOptions)
        {
            int cost = prefab.GetComponent<Tower>().Cost;
            CreateEntry(
                label: prefab.name,
                clickAction: () => onSelected?.Invoke(prefab),
                cost: prefab.GetComponent<Tower>().Cost
            );
        }

        // 5) Finally, a Cancel entry (no cost)
        CreateEntry(
            label: "Cancel",
            clickAction: () => onSelected?.Invoke(null),
            cost: null
        );
    }
}
