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
            iCollection.items.Add(new Item("CobbleStone", 3));
            iCollection.items.Add(new Item("Stone Pickaxe", 4));
            rCollection.recipes.Add(new Recipe(new List<AmountedItem>() { new AmountedItem(iCollection.items[0], 1) }, new AmountedItem(iCollection.items[1], 4)));
            rCollection.recipes.Add(new Recipe(new List<AmountedItem>() { new AmountedItem(iCollection.items[1], 2) }, new AmountedItem(iCollection.items[2], 4)));
            rCollection.recipes.Add(new Recipe(new List<AmountedItem>() { new AmountedItem(iCollection.items[2], 2), new AmountedItem(iCollection.items[3], 3) }, new AmountedItem(iCollection.items[4], 1)));
        }

        public void Craft(string CraftingItemName, int Amount)
        {
            AmountedItemCollection ToCraft = new();
            AmountedItemCollection Surplus = new();
            AmountedItemCollection CraftResult = new();
            try
            {
                ToCraft.Add(new AmountedItem(iCollection.Find(CraftingItemName), Amount));
            }
            catch(ItemNotExistingException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            foreach (AmountedItem ItemToCraft in ToCraft.amountedItems.ToList())
            {
                Console.WriteLine($"\nCrafting: {ItemToCraft.item.Name} in amount {ItemToCraft.amount}");

                Recipe recipe = GetRecipe(ItemToCraft);

                if (recipe == null)
                {
                    CraftResult.Add(ItemToCraft);
                    continue;
                }

                for (int i = ItemToCraft.amount; i > 0; i = i - recipe.ResultItem.amount)
                {
                    foreach (AmountedItem inputItem in recipe.InputItems)
                    {
                        CraftResult.Add(inputItem);
                    }

                    if (i - recipe.ResultItem.amount < 0)
                    {
                        Surplus.amountedItems.Add(new AmountedItem(ItemToCraft.item, recipe.ResultItem.amount - i));
                    }

                }
            }

            (AmountedItemCollection ItemsResult, AmountedItemCollection SuprplusResult) Result = Craft(CraftResult, Surplus);

            Console.WriteLine("\nCRAFTED!!!");

            Console.WriteLine("\nNeeded:");
            Result.ItemsResult.amountedItems.ForEach(x => Console.WriteLine($"{x.item.Name} in amount {x.amount}"));

            Console.WriteLine("\nSurplus:");
            Result.SuprplusResult.amountedItems.ForEach(x => Console.WriteLine($"{x.item.Name} in amount {x.amount}"));
        }

        public (AmountedItemCollection, AmountedItemCollection) Craft(AmountedItemCollection ToCraft, AmountedItemCollection Surplus)
        {
            if (ToCraft.amountedItems.All(i => GetRecipe(i) == null)) return (ToCraft, Surplus);

            AmountedItemCollection CraftResult = new();

            foreach (AmountedItem ItemToCraft in ToCraft.amountedItems.ToList())
            {
                Console.WriteLine($"\nCrafting: {ItemToCraft.item.Name} in amount {ItemToCraft.amount}");

                Recipe recipe = GetRecipe(ItemToCraft);

                if (recipe == null)
                {
                    CraftResult.Add(ItemToCraft);
                    continue;
                }

                for (int i = ItemToCraft.amount; i > 0; i = i - recipe.ResultItem.amount)
                {
                    foreach (AmountedItem inputItem in recipe.InputItems)
                    {
                        CraftResult.Add(inputItem);
                    }

                    if (i - recipe.ResultItem.amount < 0)
                    {
                        Surplus.amountedItems.Add(new AmountedItem(ItemToCraft.item, recipe.ResultItem.amount - i));
                    }

                }
            }

            return Craft(CraftResult, Surplus);
        }

        public Recipe GetRecipe(AmountedItem amountedItem)
        {
            if (rCollection.recipes.Any(r => r.ResultItem.item.ID == amountedItem.item.ID))
            {
                return rCollection.recipes.Find(r => r.ResultItem.item.ID == amountedItem.item.ID);
            }
            else
            {
                return null;
            }
        }
    }

    public class ItemCollection
    {
        public List<Item> items = new();

        public Item Find(string Name)
        {
            if (items.Any(i => i.Name == Name))
            {
                return items.Find(i => i.Name == Name);
            }
            else throw new ItemNotExistingException(Name);
        }
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

    public class ItemNotExistingException : Exception
    {
        public ItemNotExistingException() { }

        public ItemNotExistingException(string ItemName) : base($"Item {ItemName} does not exist in this program")
        {

        }
    }
}
