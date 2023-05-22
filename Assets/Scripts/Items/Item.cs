using System;

[Serializable()]
public abstract class Item
{
    protected string itemName = "null";
    protected string itemDesc = "null";
    protected bool isEquippable;

    public Item() {}

    public string GetItemName() {
        return itemName;
    }

    public string GetItemDesc() {
        return itemDesc;
    }

    public bool GetIsEquippable() {
        return isEquippable;
    }
}
