// ItemBox.cs
using UnityEngine;

[System.Serializable]
public class ItemProbability
{
    [Tooltip("アイテムの種類")]
    public ItemType itemType;
    [Tooltip("このアイテムの選出確率")]
    [Range(0f, 100f)]
    public float probability;
}

public class ItemBox : MonoBehaviour
{
    [Header("アイテムの確率設定")]
    [Tooltip("ここに設定したアイテムリストから抽選されます")]
    public ItemProbability[] itemProbabilities;

    [Header("アイテムボックスの設定")]
    [SerializeField] private bool hideOnPickup = true;
    [SerializeField] private float respawnTime = 10f;

    private void OnTriggerEnter(Collider other)
    {
        KartController kart = other.GetComponent<KartController>();
        if (kart != null && !kart.HasItem())
        {
            ItemType selectedItem = ChooseItem();
            kart.AcquireItem(selectedItem);
            Debug.Log(kart.name + " が " + selectedItem.ToString() + " を取得！");
            HandlePickup();
        }
    }

    private ItemType ChooseItem()
    {
        float totalProbability = 0f;
        foreach (var itemProb in itemProbabilities)
        {
            totalProbability += itemProb.probability;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        foreach (var itemProb in itemProbabilities)
        {
            cumulativeProbability += itemProb.probability;
            if (randomValue <= cumulativeProbability)
            {
                return itemProb.itemType;
            }
        }
        return ItemType.None;
    }

    private void HandlePickup()
    {
        if (hideOnPickup)
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    private void Respawn()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}