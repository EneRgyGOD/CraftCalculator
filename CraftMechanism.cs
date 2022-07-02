namespace CraftCalculator
{
    internal class CraftMechanism
    {
        public ItemCollection iCollection = new();
        public RecipeCollection rCollection = new();

        public void GenerateResources()
        {
            iCollection.items.Add(new Item("Oak", 0));
            iCollection.items.Add(new Item("Planks", 1));
            iCollection.items.Add(new Item("Stick", 2));
            rCollection.recipes.Add(new Recipe(new List<AmountedItem>() { new AmountedItem(iCollection.items[0], 1) }, new AmountedItem(iCollection.items[1], 4)));
            rCollection.recipes.Add(new Recipe(new List<AmountedItem>() { new AmountedItem(iCollection.items[1], 2) }, new AmountedItem(iCollection.items[2], 4)));
        }

        public void Craft()
        {
            int requestedAmount = 9;

            AmountedItemCollection ToCraft = new();
            AmountedItemCollection Surplus = new();

            for (int i = requestedAmount; i > 0; i = i - rCollection.recipes[1].ResultItem.amount)
            {
                foreach (AmountedItem inputItem in rCollection.recipes[1].InputItems)
                {
                    ToCraft.Add(inputItem);
                }
            }
            Console.WriteLine(ToCraft.amountedItems[0].amount);
        }
    }

    public class ItemCollection
    {
        public List<Item> items = new();
    }

    public class RecipeCollection
    {
        public List<Recipe> recipes = new();
    }

    public class AmountedItemCollection
    {
        public List<AmountedItem> amountedItems = new();

        public void Add(AmountedItem amountedItem)
        {
            if (amountedItems.Any(x => x.item.ID == amountedItem.item.ID))
            {
                amountedItems.Find(x => x.item.ID == amountedItem.item.ID)!.amount += amountedItem.amount;
            }
            else
            {
                amountedItems.Add(new AmountedItem(amountedItem.item, amountedItem.amount));
            }
        }

        public void Add(Item item)
        {
            if (amountedItems.Any(x => x.item.ID == item.ID))
            {
                amountedItems.Find(x => x.item.ID == item.ID)!.amount++;
            }
            else
            {
                amountedItems.Add(new AmountedItem(item, 1));
            }
        }

        public int Count()
        {
            return amountedItems.Count();
        }
    }

    public class Item
    {
        public string Name;
        public int ID;

        public Item(string Name, int ID)
        {
            this.Name = Name;
            this.ID = ID;
        }
    }
    public class AmountedItem
    {

        public Item item;
        public int amount;

        public AmountedItem(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }

    public class Recipe
    {
        public List<AmountedItem> InputItems = new();
        public AmountedItem ResultItem;

        public Recipe(List<AmountedItem> Input, AmountedItem Output)
        {
            InputItems = Input;
            ResultItem = Output;
        }
    }
}
