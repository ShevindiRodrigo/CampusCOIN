using CampusCOIN.Data;
using CampusCOIN.Models;

namespace CampusCOIN.Pages;

public partial class EditExpensePage : ContentPage
{
	private readonly ExpenseData data;
	private readonly Expense expense;

	public EditExpensePage(Expense exp)
	{
		InitializeComponent();
		expense = exp;

        

        Title.Text = expense.Title;
		Category.Text = expense.Category;
		Date.Date = expense.Date;
		Amount.Text = expense.Amount;	
		Description.Text = expense.Description;

		data = new ExpenseData();
		BindingContext = expense;

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
                expense.Receipt_path = newReceipt;
            }


        }
        else
        {
            // Display alert if the device is not supported
            await DisplayAlert("SORRY!!", "Your device isn't supported", "Ok");
        }

    }
    // Event handler for the Add button click
    public async void OnUpdateClicked(object sender, EventArgs e)
    {
        // Validate input fields
        if (string.IsNullOrWhiteSpace(expense.Title) || string.IsNullOrEmpty(expense.Category)
            || string.IsNullOrEmpty(expense.Amount) || string.IsNullOrEmpty(expense.Description))
        {
            await DisplayAlert("Cannot have empty space.Entry required!", "Please enter expense data", "OK!");
            return;
        }

        
        System.Diagnostics.Debug.WriteLine($"ExpenseData is null: {data == null}");



        // Update the expense data
        await data.UpdateExpenseData(expense);
        await DisplayAlert("YOU Have updated", $"You have updated expense successfully with ID: {expense.Id}", "OK!");

        await Navigation.PushAsync(new ReceiptViewPage(expense));

    }


}