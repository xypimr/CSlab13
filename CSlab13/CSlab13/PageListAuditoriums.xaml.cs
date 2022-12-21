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
    public partial class PageListAuditoriums : ContentPage
    {
        public PageListAuditoriums()
        {
            InitializeComponent();
        }
        // При открытии этой страницы инициализизуется список сборок из базы данных
        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.Dbfilename);
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                ListAuditoriums.ItemsSource = db.Auditoriums.ToList();
            }
            base.OnAppearing();
        }
        // Обработка кнопки добавления сборки
        private async void CreateAuditorium(object sender, EventArgs e)
        {
            Auditorium auditorium = new Auditorium();
            PageAuditorium pageAuditorium = new PageAuditorium();
            pageAuditorium.BindingContext = auditorium;
            await Navigation.PushAsync(pageAuditorium);
        }
        // Обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Auditorium selectedAuditorium = (Auditorium)e.SelectedItem;
            PageAuditorium pageAuditorium = new PageAuditorium();
            pageAuditorium.BindingContext = selectedAuditorium;
            await Navigation.PushAsync(pageAuditorium);
        }
    }
}