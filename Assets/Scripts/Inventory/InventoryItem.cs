public class InventoryItem
{
    public bool isEmpty;
    public Item item;
    public int stack;

    public InventoryItem()
    {
        isEmpty = true;
        item = new Item(false);
        stack = 0;
    }

    public InventoryItem(Item i) 
    {
        isEmpty = false;
        item = i;
        stack = 1;
    }

    public InventoryItem(Item i, int s)
    {
        isEmpty = false;
        item = i;
        stack = s;
    }
}
