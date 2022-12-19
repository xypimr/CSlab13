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
    public partial class AssemblyPage : ContentPage
    {
        string dbPath;

        public AssemblyPage()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            Console.WriteLine(dbPath);
        }

        protected override void OnAppearing()
        {
            Assembly v = (Assembly)this.BindingContext;
            if (v.Id != 0)
            {
                using (AssemblyContext db = new AssemblyContext(dbPath))
                {
                    var assembly =
                        db.Parts.Include(x => x.Detail)
                            .Where(x => x.AssemblyId == v.Id);
                    ListPartsInAssembly.ItemsSource = assembly;
                }
            }

            base.OnAppearing();
        }

        private void SaveAssembly(object sender, EventArgs e)
        {
            var assembly = (Assembly)BindingContext;
            if (!String.IsNullOrEmpty(assembly.Name))
            {
                using (AssemblyContext db = new AssemblyContext(dbPath))
                {
                    if (assembly.Id == 0)
                        db.Assemblies.Add(assembly);
                    else
                    {
                        db.Assemblies.Update(assembly);
                    }

                    db.SaveChanges();
                }
            }

            this.Navigation.PopAsync();
        }

        private void DeleteAssembly(object sender, EventArgs e)
        {
            var assembly = (Assembly)BindingContext;
            using (AssemblyContext db = new AssemblyContext(dbPath))
            {
                db.Assemblies.Remove(assembly);
                db.SaveChanges();
            }

            this.Navigation.PopAsync();
        }

        private async void AddPart(object sender, EventArgs e)
        {
            var assembly = (Assembly)BindingContext;
            var detailName = await DisplayPromptAsync("Добавление детали в сборку",
                "Введите название детали",
                keyboard: Keyboard.Text);
            Console.WriteLine(detailName);
            // Сделать выход после нажания отмены 
            if (detailName == "" || detailName == null)
                return;
            using (AssemblyContext db = new AssemblyContext(dbPath))
            {
                var partL = db.Parts.FirstOrDefault(x =>
                    (x.Detail.Name == detailName) && (x.AssemblyId == assembly.Id));
                if (partL != null)
                {
                    await DisplayAlert("Внимание", "Деталь уже есть в сборке", "Хорошо");
                    return;
                }

                // Проверка есть ли деталь с которой пытаемся создать часть сборки
                var detail = db.Details.FirstOrDefault(x => x.Name == detailName);
                if (detail == null)
                {
                    bool flag = await DisplayAlert(
                        "Ошибочка",
                        "Похоже, нет такой детали(\nХотите создать?",
                        "Создать",
                        "Отмена");
                    // Если хотим то можно сразу создать ее
                    if (flag)
                    {
                        DetailPage detailPage = new DetailPage();
                        Detail detailNew = new Detail { Name = detailName };
                        detailPage.BindingContext = detailNew;
                        await Navigation.PushAsync(detailPage);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                string quantity = await DisplayPromptAsync("Добавление детали в сборку",
                    $"Введите небходимое в сборке количество \"{detailName}\"",
                    keyboard: Keyboard.Numeric);
                if (quantity == "0" || quantity == "" || !int.TryParse(quantity, out var numericValue))
                    return;
                Part temp = new Part
                {
                    Quantity = Int32.Parse(quantity),
                    Detail = detail,
                    DetailId = detail.Id
                };
                assembly.Parts.Add(temp);
                ListPartsInAssembly.ItemsSource = assembly.Parts;

                foreach (var VARIABLE in assembly.Parts)
                {
                    Console.WriteLine(VARIABLE.Detail.Name);
                }

                db.Assemblies.Update(assembly);
                db.Parts.Add(temp);
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
            using (AssemblyContext db = new AssemblyContext(dbPath))
            {
                var part = db.Parts.FirstOrDefault(x => x.Detail.Name == detailName);
                part.Quantity = int.Parse(quantityNew);
                db.Parts.Update(part);
                db.SaveChanges();
            }

            OnAppearing();
        }

        private void DeletePart(object sender, EventArgs e)
        {
            var detailName = ((MenuItem)sender).CommandParameter.ToString();
            using (AssemblyContext db = new AssemblyContext(dbPath))
            {
                var part = db.Parts.FirstOrDefault(x => x.Detail.Name == detailName);
                db.Parts.Remove(part);
                db.SaveChanges();
            }

            OnAppearing();
        }
    }
}