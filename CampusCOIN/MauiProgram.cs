using CampusCOIN.Data;
using CampusCOIN.Pages;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace CampusCOIN
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("K2D-Bold.ttf", "K2DBold");
                    fonts.AddFont("K2D-Regular.ttf", "K2D");
                    fonts.AddFont("K2D-SemiBold.ttf", "K2DSemiBold");
                    fonts.AddFont("K2D-Light.ttf", "K2DThin");

                });

            //Dependency injection
            builder.Services.AddSingleton<ExpenseData>();
            builder.Services.AddSingleton<BudgetData>();
            builder.Services.AddSingleton<UserData>();
            builder.Services.AddSingleton<TuitionGoalData>();
            builder.Services.AddTransient<ExpenseListPage>();
            builder.Services.AddSingleton<IncomeData>();
            builder.Services.AddTransient<IncomeListPage>();
            builder.Services.AddTransient<AddExpensePage>();
            builder.Services.AddTransient<AddIncomePage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddTuitionGoalPage>();
            builder.Services.AddTransient<AddGoalForm>();
            builder.Services.AddTransient<TuitionGoalSummaryPage>();
            builder.Services.AddTransient<ExpenseSearchPage>();
            builder.Services.AddTransient<ReceiptViewPage>();
            builder.Services.AddTransient<SetBudgetPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<LoginForm>();
            builder.Services.AddTransient<GoalTrackerPage>();
            builder.Services.AddTransient<Settings>();
            builder.Services.AddTransient<UpdateUserDetailsPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
