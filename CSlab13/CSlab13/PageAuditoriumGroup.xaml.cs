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
    public partial class PageAuditoriumGroups : ContentPage
    {
        string dbPath;

        public PageAuditoriumGroups()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.Dbfilename);
        }

        private async void SavePart(object sender, EventArgs e)
        {
            var auditoriumGroup = (AuditoriumGroup)BindingContext;
            var auditoriumName = Entry.Text;
            if (!string.IsNullOrEmpty(auditoriumName))
            {
                using (ApplicationContext db = new ApplicationContext(dbPath))
                {
                    // Проверка есть ли деталь с которой пытаемся создать часть сборки
                    Auditorium? auditorium = db.Auditoriums.FirstOrDefault(x => x.Name == auditoriumName);
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
                            pageAuditorium.EntryNameDetail.Text = auditoriumName;
                            Navigation.PushAsync(pageAuditorium);
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }

                    auditoriumGroup.AuditoriumId = auditorium.Id;
                    auditoriumGroup.Auditorium = auditorium;
                    if (auditoriumGroup.Id == 0)
                        db.AuditoriumsGroups.Add(auditoriumGroup);
                    else
                    {
                        db.AuditoriumsGroups.Update(auditoriumGroup);
                    }

                    db.SaveChanges();
                }
            }

            Navigation.PopAsync();
        }

        private void DeletePart(object sender, EventArgs e)
        {
            var part = (AuditoriumGroup)BindingContext;
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                db.AuditoriumsGroups.Remove(part);
                db.SaveChanges();
            }

            Navigation.PopAsync();
        }
    }
}
