using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Item Prefabs")]
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private GameObject anomalyItem;
    [SerializeField] private GameObject bomb;

    public void SpawnAnomalyItem()
    {
        Instantiate(anomalyItem, transform.position, Quaternion.identity);
    }
    public void SpawnBomb()
    {
        Instantiate(bomb, transform.position, Quaternion.identity);
    }
    public void SpawnItem()
    {
        if (itemPrefabs.Length == 0)
        {
            Debug.LogError("No item prefabs assigned!");
            return;
        }

        int randomIndex = Random.Range(0, itemPrefabs.Length);
        GameObject itemToSpawn = Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);

        SetupItem(itemToSpawn);
        GameManager.Instance.currentItem = itemToSpawn;
    }

    private void SetupItem(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        if (item == null) return;

        bool hasStain = false;
        bool hasBarcode = true;
        bool barcodeShowsGood = true;
        bool hasScratches = false;

        // Каждый дефект проверяется независимо
        float roll = Random.Range(0f, 1f);

        // Пятно
        if (roll <= SettingManager.Instance.defectChance)
        {
            hasStain = true;
        }

        // Отсутствие штрихкода
        roll = Random.Range(0f, 1f);
        if (roll <= SettingManager.Instance.noBarcodeChance)
        {
            hasBarcode = false;
        }

        // Неправильный штрихкод (только если штрихкод есть)
        if (hasBarcode)
        {
            roll = Random.Range(0f, 1f);
            if (roll <= SettingManager.Instance.wrongBarcodeChance)
            {
                barcodeShowsGood = false;
            }
        }

        // Царапины
        roll = Random.Range(0f, 1f);
        if (roll <= SettingManager.Instance.scratchesChance)
        {
            hasScratches = true;
        }

        // Предмет дефектный если есть хотя бы один дефект
        bool isDefective = hasStain || !hasBarcode || !barcodeShowsGood || hasScratches;

        item.InitializeItem(isDefective, hasBarcode, barcodeShowsGood, hasStain, hasScratches);
    }


}
    

