namespace CampusCOIN
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCdkx3RHxbf1x0ZFZMY1pbR3JPMyBoS35RckVkWH1ec3dTRWZdV0dx");
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
