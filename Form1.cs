namespace CraftCalculator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void CalculateItem(object sender, EventArgs e)
        {
            CraftMechanism CraftMechanism = new();

            CraftMechanism.GenerateResources();

            CraftMechanism.Craft("bebra", 1);
            CraftMechanism.Craft("Stone Pickaxe", 2);
        }
    }
}