using System;
using System.Runtime.Serialization;

[Serializable()]
public abstract class Item : ISerializable
{
    protected string itemName = "null";
    protected string itemDesc = "null";
    protected bool isEquippable;

    public Item() {}

    public Item(SerializationInfo info, StreamingContext context) {
        itemName =     (string)info.GetValue("Item_Name", typeof(string));
        itemDesc =     (string)info.GetValue("Item_Desc", typeof(string));
        isEquippable = (bool)info.GetValue("Equippable", typeof(bool));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        info.AddValue("Item_Name",  itemName);
        info.AddValue("Item_Desc",  itemDesc);
        info.AddValue("Equippable", isEquippable);
    }

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
