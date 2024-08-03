namespace CampusCOIN.Pages;

public partial class WebViewPage : ContentPage
{
	public WebViewPage()
	{
		InitializeComponent();

		//URL to be loaded in the web view
		string source = "https://www.google.com";
		var webview = new WebView();

        // Set the content of the page to the WebView instance
        content.Content = webview;
		webview.Source = source;
		webview.Reload();
	}
}