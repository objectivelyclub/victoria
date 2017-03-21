using System.Threading.Tasks;
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

        public async void newScan()
        {
            MainGrid.Children[2].BackgroundColor = Color.Red;
            await Task.Delay(200);
            MainGrid.Children[2].BackgroundColor = Color.Blue;            
        }

        void makeView()
        {
            MainGrid.RowDefinitions[0].Height = new GridLength(0.25, GridUnitType.Star);
            MainGrid.RowDefinitions[2].Height = new GridLength(0.25, GridUnitType.Star);
            /*
            TableView tableView = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot
                {
                    new TableSection
                    {
                        new SwitchCell
                        {
                            Text = "SwitchCell:"
                        }
                    }
                }
                
            };
            MainGrid.Children.Add(tableView,0,4);*/
//            MainGrid.Children.Insert(2, tableView);
        }
    }
}
