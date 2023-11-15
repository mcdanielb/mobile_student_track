using bmcdani_academic_tracking.Pages;

namespace bmcdani_academic_tracking
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}