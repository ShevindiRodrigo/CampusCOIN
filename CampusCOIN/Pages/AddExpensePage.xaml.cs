using CampusCOIN.Data;
using CampusCOIN.Models;
namespace CampusCOIN.Pages;

// Bind query property to handle Expense data passed through navigation
[QueryProperty("Expense", "Expense")]
public partial class AddExpensePage : ContentPage
{
    private readonly ExpenseData data;
    private readonly BudgetData budgetData;
    private Expense Expense;
    private Budget Budget;
    public string userID;

    public AddExpensePage(ExpenseData expenseData, string userId)
    {
        InitializeComponent();
        data = expenseData;
        budgetData = new BudgetData();
        Expense = new Expense();
        Budget = new Budget();
        userID = userId;
        Date.Date = DateTime.Now.Date;
        BindingContext = Expense;
    }

    // Event handler for the Add Receipt button click
    private async void AddReceipt_Clicked(object sender, EventArgs e)
    {
        FileResult receipt = null;
        if (MediaPicker.Default.IsCaptureSupported && DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            // Choose between capture the receipt or access device media browser
            string action = await DisplayActionSheet("ADD RECEIPT FROM", "Cancel", null, "Camera", "File Storage");



            if (action == "Camera")
            {
                // Take photo
                receipt = await MediaPicker.Default.CapturePhotoAsync();
            }
            else if (action == "File Storage")
            {
                // Pick photo
                receipt = await MediaPicker.Default.PickPhotoAsync();
            }

            if (receipt != null)
            {
                // Save the image captured in the application.
                string receiptDir = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "receipts");
                System.IO.Directory.CreateDirectory(receiptDir);

                var newReceipt = Path.Combine(receiptDir, receipt.FileName);
                using Stream sourceStream = await receipt.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(newReceipt);
                await sourceStream.CopyToAsync(localFileStream);
                await DisplayAlert("Receipt Saved into Device", localFileStream.Name, "Ok");

                // Update the Expense model with the path to the receipt
                Expense.Receipt_path = newReceipt;
            }


        }
        else
        {
            // Display alert if the device is not supported
            await DisplayAlert("SORRY!!", "Your device isn't supported", "Ok");
        }

    }
    // Event handler for the Add button click
    public async void OnAddClicked(object sender, EventArgs e)
    {
        // Validate input fields
        if (string.IsNullOrWhiteSpace(Expense.Title) || string.IsNullOrEmpty(Expense.Category)
            || string.IsNullOrEmpty(Expense.Amount) || string.IsNullOrEmpty(Expense.Description))
        {
            await DisplayAlert("Cannot have empty space.Entry required!", "Please enter expense data", "OK!");
            return;
        }

        //await DisplayAlert("Details", Expense.Title + Expense.Category + Expense.Date +
        //Expense.Amount +Expense.Receipt_path, "OK");
        System.Diagnostics.Debug.WriteLine($"ExpenseData is null: {data == null}");
        System.Diagnostics.Debug.WriteLine($"userid: {userID}");

        // Save the expense data
        Expense.UserID = userID;
        System.Diagnostics.Debug.WriteLine($"user: {Expense.UserID}");

        int id = await data.SaveExpense(Expense);
        await DisplayAlert("YOU Have added", $"You have added expense successfully with ID: {id}", "OK!");
        //await Shell.Current.GoToAsync("//MainPage");

        // Reset the form for new entry
        Expense = new Expense();
        BindingContext = Expense;

    }
}