using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CSlab13
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPartsPage : ContentPage
    {
        public ListPartsPage()
        {
            InitializeComponent();
        }
        // При открытии этой страницы инициализизуется список сборок из базы данных
        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AssemblyContext db = new AssemblyContext(dbPath))
            {
                PartList.ItemsSource = db.Parts.ToList();
            }
            base.OnAppearing();
        }
        // Обработка кнопки добавления сборки
        private async void CreatePart(object sender, EventArgs e)
        {
            Part part = new Part();
            PartPage partPage = new PartPage();
            partPage.BindingContext = part;
            await Navigation.PushAsync(partPage);
        }
        // Обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Part selectedPart = (Part)e.SelectedItem;
            PartPage partPage = new PartPage();
            partPage.BindingContext = selectedPart;
            await Navigation.PushAsync(partPage);
        }
    }
}