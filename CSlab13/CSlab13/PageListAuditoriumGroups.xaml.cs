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
    public partial class PageListAuditoriumGroup : ContentPage
    {
        public PageListAuditoriumGroup()
        {
            InitializeComponent();
        }
        // При открытии этой страницы инициализизуется список сборок из базы данных
        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.Dbfilename);
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                PartList.ItemsSource = db.AuditoriumsGroups.ToList();
            }
            base.OnAppearing();
        }
        // Обработка кнопки добавления сборки
        private async void CreateAuditoriumGroup(object sender, EventArgs e)
        {
            AuditoriumGroup auditoriumGroup = new AuditoriumGroup();
            PageAuditoriumGroups pageAuditoriumGroups = new PageAuditoriumGroups();
            pageAuditoriumGroups.BindingContext = auditoriumGroup;
            await Navigation.PushAsync(pageAuditoriumGroups);
        }
        // Обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            AuditoriumGroup selectedAuditoriumGroup = (AuditoriumGroup)e.SelectedItem;
            PageAuditoriumGroups pageAuditoriumGroups = new PageAuditoriumGroups();
            pageAuditoriumGroups.BindingContext = selectedAuditoriumGroup;
            await Navigation.PushAsync(pageAuditoriumGroups);
        }
    }
}