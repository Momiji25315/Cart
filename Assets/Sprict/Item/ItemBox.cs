// ItemBox.cs (修正版)
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

    // ★★★【変更点】ここから ★★★
    private void OnTriggerEnter(Collider other)
    {
        KartController kart = other.GetComponent<KartController>();

        // 接触したのがカートなら処理を開始
        if (kart != null)
        {
            // アイテムを持っていない場合のみ、アイテムを抽選して付与する
            if (!kart.HasItem())
            {
                ItemType selectedItem = ChooseItem();
                kart.AcquireItem(selectedItem);
                Debug.Log(kart.name + " が " + selectedItem.ToString() + " を取得！");
            }
            else
            {
                // 既にアイテムを持っている場合は、メッセージだけ表示する
                Debug.Log(kart.name + " は既にアイテムを持っています。");
            }

            // カートが触れたら（アイテム所持状況に関わらず）ボックスは消える
            HandlePickup();
        }
    }
    // ★★★【変更点】ここまで ★★★

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
            // コライダーを先に無効化して、複数回触れるのを防ぐ
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    private void Respawn()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}