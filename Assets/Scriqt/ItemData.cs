using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Manager", menuName = "ItemList")]
public class ItemData : ScriptableObject
{
    public List<ItemList> items;
}
[System.Serializable]
public class ItemList 
{
    public string itemName;
    public string itemType;
    public int itemID;
    public string itemdescription;
    public float weight;
    public Sprite icon;
}



