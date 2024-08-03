using CampusCOIN.Data;
using CampusCOIN.Models;

namespace CampusCOIN.Pages;

[QueryProperty("TuitionGoal", "TuitionGoal")]

public partial class TuitionGoalSummaryPage : ContentPage
{
    private Tuitiongoal tuitiongoal;

    public TuitionGoalSummaryPage(Tuitiongoal goalData)
	{
		InitializeComponent();
        tuitiongoal = goalData;

        //retrive and fill goal data
        Date.Text = "Due On: " + goalData.dueDate.ToShortDateString();
        Amount.Text = "$ "+ goalData.amount;
        Goal.Text = "$ "+ goalData.monthlyGoal;

	}

}