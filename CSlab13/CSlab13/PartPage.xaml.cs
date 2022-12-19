#nullable enable
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
    public partial class PartPage : ContentPage
    {
        string dbPath;

        public PartPage()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        }

        private async void SavePart(object sender, EventArgs e)
        {
            var part = (Part)BindingContext;
            var detailName = Entry.Text;
            if (!string.IsNullOrEmpty(detailName))
            {
                using (AssemblyContext db = new AssemblyContext(dbPath))
                {
                    // Проверка есть ли деталь с которой пытаемся создать часть сборки
                    Detail? detail = db.Details.FirstOrDefault(x => x.Name == detailName);
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
                            detailPage.EntryNameDetail.Text = detailName;
                            Navigation.PushAsync(detailPage);
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }

                    part.DetailId = detail.Id;
                    part.Detail = detail;
                    if (part.Id == 0)
                        db.Parts.Add(part);
                    else
                    {
                        db.Parts.Update(part);
                    }

                    db.SaveChanges();
                }
            }

            Navigation.PopAsync();
        }

        private void DeletePart(object sender, EventArgs e)
        {
            var part = (Part)BindingContext;
            using (AssemblyContext db = new AssemblyContext(dbPath))
            {
                db.Parts.Remove(part);
                db.SaveChanges();
            }

            Navigation.PopAsync();
        }
    }
}
