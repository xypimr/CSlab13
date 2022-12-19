using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CSlab13
{
    public partial class ListAssembliesPage : ContentPage
    {
        public ListAssembliesPage()
        {
            InitializeComponent();
        }
        
        // При открытии этой страницы инициализизуется список сборок из базы данных
        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (AssemblyContext db = new AssemblyContext(dbPath))
            {
                AssemblyList.ItemsSource = db.Assemblies.ToList();
            }
            base.OnAppearing();
        }
        // Обработка кнопки добавления сборки
        private async void CreateAssembly(object sender, EventArgs e)
        {
            Assembly assembly = new Assembly();
            AssemblyPage assemblyPage = new AssemblyPage();
            assemblyPage.BindingContext = assembly;
            await Navigation.PushAsync(assemblyPage);
        }
        // Обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Assembly selectedassembly = (Assembly)e.SelectedItem;
            AssemblyPage assemblyPage = new AssemblyPage();
            assemblyPage.BindingContext = selectedassembly;
            await Navigation.PushAsync(assemblyPage);
        }
    }
}