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
    public partial class DetailPage : ContentPage
    {
        string dbPath;
        
        public DetailPage()
        {
            InitializeComponent();
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        }
        
        private void SaveDetail(object sender, EventArgs e)
        {
            var detail = (Detail)BindingContext;
            // if (!String.IsNullOrEmpty(detail.Name))
            {
                using (AssemblyContext db = new AssemblyContext(dbPath))
                {
                    if (detail.Id == 0)
                        db.Details.Add(detail);
                    else
                    {
                        db.Details.Update(detail);
                    }
                    db.SaveChanges();
                }
            }
            this.Navigation.PopAsync();
        }
        private void DeleteDetail(object sender, EventArgs e)
        {
            var detail = (Detail)BindingContext;
            using (AssemblyContext db = new AssemblyContext(dbPath))
            {
                db.Details.Remove(detail);
                db.SaveChanges();
            }
            this.Navigation.PopAsync();
        }
    }
}