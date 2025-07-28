using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class TowerSelectionUI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The RectTransform with the HorizontalLayoutGroup")]
    [SerializeField] private RectTransform buttonContainer;

    [Tooltip("A prefab whose root has a Button component and a TextMeshProUGUI child")]
    [SerializeField] private GameObject buttonPrefab;

    [Tooltip("Your PlayerUI script instance (for reading current Money)")]
    [SerializeField] private Player  player;

    private Action<GameObject> onSelected;

    // track each tower entry so we can update its interactable state
    private class Entry
    {
        public Button btn;
        public TextMeshProUGUI label;
        public int cost;
        public Color defaultColor;
    }
    private readonly List<Entry> entries = new();

    void Awake()
    {
        player = Player.Instance;
    }

    void OnEnable()
    {
        if (player != null)
            player.OnStatsChanged += RefreshAffordability;
    }

    void OnDisable()
    {
        if (player != null)
            player.OnStatsChanged -= RefreshAffordability;
    }

    void RefreshAffordability()
    {
        if (entries.Count == 0 || player == null) return;

        int money = player.Money;
        foreach (var e in entries)
        {
            bool canAfford = money >= e.cost;
            e.btn.interactable = canAfford;
            e.label.color = canAfford ? e.defaultColor : Color.gray;
        }
    }

    public void Show(List<GameObject> towerOptions, Action<GameObject> onSelectedCallback)
    {
        onSelected = onSelectedCallback;
        entries.Clear();

        // clear any previous buttons
        foreach (Transform t in buttonContainer)
            Destroy(t.gameObject);

        // helper to size a button to its TMP label
        void SizeButton(GameObject go, float padX, float padY)
        {
            var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(tmp.rectTransform);
            float w = LayoutUtility.GetPreferredSize(tmp.rectTransform, 0);
            float h = LayoutUtility.GetPreferredSize(tmp.rectTransform, 1);

            var rt = go.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w + padX);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h + padY);
        }

        // helper to create each entry (tower or cancel)
        void CreateEntry(string text, Action clickAction, int? cost = null)
        {
            var entryGO = new GameObject(text + "Entry", typeof(RectTransform));
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

            // button
            var btnGO = Instantiate(buttonPrefab, entryGO.transform, false);
            var btn = btnGO.GetComponent<Button>();
            var tmp = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = text;
            SizeButton(btnGO, padX: 20f, padY: 10f);

            // if this is a tower (cost != null), set interactable based on current money
            if (cost.HasValue)
            {
                int c = cost.Value;
                bool canAfford = player.Money >= c;

                // track it for dynamic updates
                entries.Add(new Entry
                {
                    btn = btn,
                    label = tmp,
                    cost = cost.Value,
                    defaultColor = tmp.color
                });

                // only hook up click if affordable right now
                btn.interactable = canAfford;
                if (!canAfford) tmp.color = Color.gray;
                btn.onClick.AddListener(() => { clickAction(); Destroy(gameObject); });
            }
            else
            {
                // this is the Cancel entry
                btn.onClick.AddListener(() => { clickAction(); Destroy(gameObject); });
            }
        }

        // build one entry per tower
        foreach (var prefab in towerOptions)
        {
            int c = prefab.GetComponent<Tower>().Cost;
            CreateEntry(
                text: prefab.name,
                clickAction: () => 
                {
                    player.Purchase(c);
                    onSelected(prefab);
                },
                cost: c
            );
        }

        // and finally a Cancel entry
        CreateEntry(
            text: "Cancel",
            clickAction: () => onSelected(null),
            cost: null
        );
    }
}
