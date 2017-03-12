using Xamarin.Forms;

namespace victoria
{
    public partial class MainOverlay : ContentView
    {

        public MainOverlay()
        {
            InitializeComponent();
            makeView();
        }

        void makeView()
        {
            MainGrid.RowDefinitions[0].Height = new GridLength(0.5, GridUnitType.Star);
            MainGrid.RowDefinitions[2].Height = new GridLength(0.5, GridUnitType.Star);
        }
    }
}
