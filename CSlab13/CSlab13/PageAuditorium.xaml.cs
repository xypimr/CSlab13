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
    public partial class PageAuditorium : ContentPage
    {
        string dbPath;
        
        public PageAuditorium()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.Dbfilename);
        }
        
        private void SaveAuditorium(object sender, EventArgs e)
        {
            var auditorium = (Auditorium)BindingContext;
            // if (!String.IsNullOrEmpty(detail.Name))
            {
                using (ApplicationContext db = new ApplicationContext(dbPath))
                {
                    if (auditorium.Id == 0)
                        db.Auditoriums.Add(auditorium);
                    else
                    {
                        db.Auditoriums.Update(auditorium);
                    }
                    db.SaveChanges();
                }
            }
            this.Navigation.PopAsync();
        }
        private void DeleteAuditorium(object sender, EventArgs e)
        {
            var auditorium = (Auditorium)BindingContext;
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                db.Auditoriums.Remove(auditorium);
                db.SaveChanges();
            }
            this.Navigation.PopAsync();
        }
    }
}