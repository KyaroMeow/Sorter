using System;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    
    public GameObject itemScanHUD;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void showItemScanHUD()
    {
        itemScanHUD.SetActive(true);
    }
    
    public void hideItemScanHUD()
    {
        itemScanHUD.SetActive(false);
    }
}