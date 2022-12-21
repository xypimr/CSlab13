using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CSlab13
{
    public partial class PageListBuildings : ContentPage
    {
        public PageListBuildings()
        {
            InitializeComponent();
        }
        
        // При открытии этой страницы инициализизуется список сборок из базы данных
        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.Dbfilename);
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                AssemblyList.ItemsSource = db.Buildings.ToList();
            }
            base.OnAppearing();
        }
        // Обработка кнопки добавления сборки
        private async void CreateBuilding(object sender, EventArgs e)
        {
            Building building = new Building();
            PageBuilding pageBuilding = new PageBuilding();
            pageBuilding.BindingContext = building;
            await Navigation.PushAsync(pageBuilding);
        }
        // Обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Building building = (Building)e.SelectedItem;
            PageBuilding pageBuilding = new PageBuilding();
            pageBuilding.BindingContext = building;
            await Navigation.PushAsync(pageBuilding);
        }
    }
}