#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CSlab13
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageBuilding : ContentPage
    {
        string dbPath;

        public PageBuilding()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.Dbfilename);
            Console.WriteLine(dbPath);
        }

        protected override void OnAppearing()
        {
            Building v = (Building)this.BindingContext;
            if (v.Id != 0)
            {
                using (ApplicationContext db = new ApplicationContext(dbPath))
                {
                    var assembly =
                        db.AuditoriumsGroups.Include(x => x.Auditorium)
                            .Where(x => x.BuildingId == v.Id);
                    ListPartsInAssembly.ItemsSource = assembly;
                }
            }

            base.OnAppearing();
        }

        private void SaveAssembly(object sender, EventArgs e)
        {
            var assembly = (Building)BindingContext;
            if (!String.IsNullOrEmpty(assembly.Name))
            {
                using (ApplicationContext db = new ApplicationContext(dbPath))
                {
                    if (assembly.Id == 0)
                        db.Buildings.Add(assembly);
                    else
                    {
                        db.Buildings.Update(assembly);
                    }

                    db.SaveChanges();
                }
            }

            this.Navigation.PopAsync();
        }

        private void DeleteAssembly(object sender, EventArgs e)
        {
            var assembly = (Building)BindingContext;
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                db.Buildings.Remove(assembly);
                db.SaveChanges();
            }

            this.Navigation.PopAsync();
        }

        private async void AddPart(object sender, EventArgs e)
        {
            var building = (Building)BindingContext;
            var auditoriumName = await DisplayPromptAsync("Добавление детали в сборку",
                "Введите название детали",
                keyboard: Keyboard.Text);
            Console.WriteLine(auditoriumName);
            // Сделать выход после нажания отмены 
            if (auditoriumName == "" || auditoriumName == null)
                return;
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                var partL = db.AuditoriumsGroups.FirstOrDefault(x =>
                    (x.Auditorium.Name == auditoriumName) && (x.BuildingId == building.Id));
                if (partL != null)
                {
                    await DisplayAlert("Внимание", "Деталь уже есть в сборке", "Хорошо");
                    return;
                }

                // Проверка есть ли деталь с которой пытаемся создать часть сборки
                var auditorium = db.Auditoriums.FirstOrDefault(x => x.Name == auditoriumName);
                if (auditorium == null)
                {
                    bool flag = await DisplayAlert(
                        "Ошибочка",
                        "Похоже, нет такой детали(\nХотите создать?",
                        "Создать",
                        "Отмена");
                    // Если хотим то можно сразу создать ее
                    if (flag)
                    {
                        PageAuditorium pageAuditorium = new PageAuditorium();
                        Auditorium auditoriumNew = new Auditorium { Name = auditoriumName };
                        pageAuditorium.BindingContext = auditoriumNew;
                        await Navigation.PushAsync(pageAuditorium);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                string quantity = await DisplayPromptAsync("Добавление детали в сборку",
                    $"Введите небходимое в сборке количество \"{auditoriumName}\"",
                    keyboard: Keyboard.Numeric);
                if (quantity == "0" || quantity == "" || !int.TryParse(quantity, out var numericValue))
                    return;
                AuditoriumGroup temp = new AuditoriumGroup
                {
                    Quantity = Int32.Parse(quantity),
                    Auditorium = auditorium,
                    AuditoriumId = auditorium.Id
                };
                building.AuditoriumGroups.Add(temp);
                ListPartsInAssembly.ItemsSource = building.AuditoriumGroups;

                foreach (var VARIABLE in building.AuditoriumGroups)
                {
                    Console.WriteLine(VARIABLE.Auditorium.Name);
                }

                db.Buildings.Update(building);
                db.AuditoriumsGroups.Add(temp);
                db.SaveChanges();
                OnAppearing();
                await DisplayAlert("Внимание", "Деталь добавлена", "Хорошо");
            }
        }

        private async void EditPart(object sender, EventArgs e)
        {
            var detailName = ((MenuItem)sender).CommandParameter.ToString();
            string quantityNew = await DisplayPromptAsync("Редактирование части сборки",
                $"Введите новое количество \"{detailName}\"",
                keyboard: Keyboard.Numeric);
            if (quantityNew == "0" || quantityNew == "" || !int.TryParse(quantityNew, out var numericValue))
                return;
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                var part = db.AuditoriumsGroups.FirstOrDefault(x => x.Auditorium.Name == detailName);
                part.Quantity = int.Parse(quantityNew);
                db.AuditoriumsGroups.Update(part);
                db.SaveChanges();
            }

            OnAppearing();
        }

        private void DeletePart(object sender, EventArgs e)
        {
            var detailName = ((MenuItem)sender).CommandParameter.ToString();
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                var part = db.AuditoriumsGroups.FirstOrDefault(x => x.Auditorium.Name == detailName);
                db.AuditoriumsGroups.Remove(part);
                db.SaveChanges();
            }

            OnAppearing();
        }
    }
}