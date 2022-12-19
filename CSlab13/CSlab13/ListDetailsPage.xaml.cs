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
    public partial class ListDetailsPage : ContentPage
    {
        public ListDetailsPage()
        {
            InitializeComponent();
        }
        // При открытии этой страницы инициализизуется список сборок из базы данных
        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AssemblyContext db = new AssemblyContext(dbPath))
            {
                DetailList.ItemsSource = db.Details.ToList();
            }
            base.OnAppearing();
        }
        // Обработка кнопки добавления сборки
        private async void CreateDetail(object sender, EventArgs e)
        {
            Detail detail = new Detail();
            DetailPage detailPage = new DetailPage();
            detailPage.BindingContext = detail;
            await Navigation.PushAsync(detailPage);
        }
        // Обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Detail selectedDetail = (Detail)e.SelectedItem;
            DetailPage detailPage = new DetailPage();
            detailPage.BindingContext = selectedDetail;
            await Navigation.PushAsync(detailPage);
        }
    }
}