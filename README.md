# CampusCOIN Expense Tracker

## Overview

Being an international student, I understand the challenge of managing financial matters along with studies and work. Managing expenses and saving for upcoming tuition fees is a critical task. CampusCOIN is a technical solution designed to address this problem. It is an expense tracker app that allows university students to set tuition fee goals for upcoming semesters and track and manage their progress. Additionally, the app provides the functionality to log and manage daily expenses and incomes, set and manage monthly budgets, and send important reminders through push notifications.

## Features

- **Set Tuition Fee Goals**: Users can set tuition fee goals for upcoming semesters along with their due dates. The app will automatically calculate the monthly saving amount needed to achieve the total goal.
- **Track Tuition Fee Goals**: Users can track their previously set tuition fee goals along with their monthly achievements through user-friendly progress bars.
- **Log Expenses and Incomes**: Students can log their daily expenses and incomes in the app and view them as necessary.
- **Budget Management**: Users can set monthly budgets and monitor their spending and earnings to achieve their tuition fee goals.
- **Expense View, Edit, and Delete**: Students can view their individual expenses along with their receipts, which can be uploaded when adding an expense. They can also edit and delete expenses when necessary.
- **Push Notifications**: This feature allows users to receive reminders about upcoming tuition fees and other financial tasks and achievements.

## Technology Stack

- **Platform**: .NET MAUI (v8.0) for cross-platform support.
- **Local Storage**: SQLite database for storing all financial information including expenses, incomes, budgets, tuition fee goals, and user details.
- **Authentication**: Firebase Authentication for secure user authentication and authorization.
- **UI Components**: Syncfusion.Maui.Core for enhanced UI components, including circular progress bars.
- **Notifications**: Microsoft.Toolkit.Uwp.Notifications for local push notifications.
- **Media Integration**: Microsoft.Maui.Media for camera access to capture and upload expense receipts.

## Libraries and Packages Used

- **Xamarin.AndroidX packages**
- **Xamarin.GooglePlayServices packages**
- **Sqlite-net-pcl**
- **Microsoft Maui**
- **Firebase.Authentication**
- **Microsoft.Toolkit.Uwp.Notifications**
- **CommunityToolkit.Maui**
- **Syncfusion.Maui.Core**

## Real World Problem Solves

There are many international university students facing challenges in managing their finances while focusing on their studies. This often leads to stress, debts, and academic underperformance. CampusCOIN provides a user-friendly solution for budgeting and tracking expenses and incomes. The app’s simple interface helps students keep an overview of their financial situation without the need for complex graphs and maps, reducing anxiety and stress. Additionally, the tuition fee tracker feature helps students monitor their savings for upcoming tuition fees and receive notifications for important milestones.

## Installation

To get started with CampusCOIN, follow these steps:

1. Clone the repository:
   ```sh
   git clone https://github.com/your-username/CampusCOIN.git
2. Navigate to the project directory:
   ```sh
   cd CampusCOIN
3. Restore the necessary packages:
   ```sh
   dotnet restore
4. Build the project:
   ```sh
   dotnet build
5. Run the project:
   ```sh
   dotnet run

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgements

Thanks to the contributors of the following libraries:

- Xamarin.AndroidX
- Xamarin.GooglePlayServices
- Sqlite-net-pcl
- Microsoft Maui
- Firebase.Authentication
- Microsoft.Toolkit.Uwp.Notifications
- CommunityToolkit.Maui
- Syncfusion.Maui.Core

## Contact

For any inquiries or feedback, please reach out to [shevi.rodrigo@gmail.com](mailto:shevi.rodrigo@gmail.com).

---

Happy budgeting and saving with CampusCOIN!
