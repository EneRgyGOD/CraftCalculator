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
            CraftMechanism mechanism = new();

            mechanism.GenerateResources();

            mechanism.Craft();
        }
    }
}