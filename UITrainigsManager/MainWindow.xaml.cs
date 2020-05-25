using DataLayer;
using DataLayer.Repositories;
using DomainLibrary.Domain;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UITrainigsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TrainingManager tm = new TrainingManager(new UnitOfWork(new TrainingContext()));

        public MainWindow()
        {
            InitializeComponent();
            setBestSessions();

        }
        private void setBestSessions()
        {
            var report = tm.GenerateMonthlyTrainingsReport(1994,9);

            var longestCycling = report.MaxDistanceSessionCycling;
            var highestSpeedCycling = report.MaxSpeedSessionCycling;
            var highestWattCycling = report.MaxWattSessionCycling;
            var longestRun = report.MaxDistanceSessionRunning;
            var highestSpeedRun = report.MaxSpeedSessionRunning;

            var list = new[]
            {
                new {Title = "Longest Ride", session = (object)longestCycling},
                new {Title = "Highest Speed Ride", session = (object)highestSpeedCycling},
                new {Title = "Highest Watt Ride", session = (object)highestWattCycling},
                new {Title = "Longest Run", session = (object)longestRun},
                new {Title = "Highest Speed Run", session = (object)highestSpeedRun}
            };
            lvBestSessions.ItemsSource = list;

        }

        private void btnGetMonthlyOverview_Click(object sender, RoutedEventArgs e)
        {

            var yearMonthArray = tbNumberOfMonthAndYear.Text.Split("/").ToList();
            bool monthOk = int.TryParse(yearMonthArray.FirstOrDefault(), out int month);
            bool yearOk = int.TryParse(yearMonthArray.LastOrDefault(), out int year);
            monthOk = (monthOk) ? (month > 0 && month < 13) : false;
            yearOk = (yearOk) ? (year > 0 && year < 100000) : false;
            if (!monthOk || !yearOk) { MessageBox.Show("Give a acceptable date please...", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); tbNumberOfMonthAndYear.Text = String.Empty; return; }

            
            if (cbBycicle.IsChecked == true && cbRunning.IsChecked == false)
            {
                var report = tm.GenerateMonthlyCyclingReport(year, month);
                lvOverview.ItemsSource = report.Rides;
            }
            else if (cbRunning.IsChecked == true && cbBycicle.IsChecked == false)
            {
                var report = tm.GenerateMonthlyRunningReport(year, month);
                lvOverview.ItemsSource = report.Runs;
            }
            else if (cbRunning.IsChecked == true && cbBycicle.IsChecked == true)
            {
                var report = tm.GenerateMonthlyTrainingsReport(year, month);
                var timeLine = report.TimeLine;
                var objects = new List<Object>();
                foreach(var t in timeLine)
                {
                    objects.Add(t.Item2);
                }
                lvOverview.ItemsSource = objects;
            }
            else
            {
                    MessageBox.Show("Please select Running or Biking or both...","ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var w = new AddWindow(tm);
            w.Show();
            this.Close();
        }
        private List<Object> SortSessions(List<CyclingSession> cycSes, List<RunningSession> runSes)
        {
            List<Object> outputList = new List<object>();
            SortedList<DateTime, Object> list = new SortedList<DateTime, object>();
            foreach (RunningSession s in runSes)
            {
                if (!list.ContainsKey(s.When))
                    list.Add(s.When, s);
            }
            foreach (CyclingSession s in cycSes)
            {
                if (!list.ContainsKey(s.When))
                    list.Add(s.When, s);
            }
            list.OrderBy(s => s.Key);
            foreach (KeyValuePair<DateTime, Object> pair in list)
            {
                outputList.Add(pair.Value);
            }
            return outputList;

        }

        private void btnGetLatetsSessions_Click(object sender, RoutedEventArgs e)
        {
            bool isNumber = int.TryParse(numberOfLatetsSessions.Text, out int amount);
            if (!isNumber) { MessageBox.Show("Give a number", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            if (cbBycicle.IsChecked == false && cbRunning.IsChecked == false) { MessageBox.Show("Select a type", "ERROR", MessageBoxButton.OK); return; }
            List<CyclingSession> biSes = new List<CyclingSession>();
            if (cbBycicle.IsChecked == true)
            {
                biSes.AddRange(tm.GetPreviousCyclingSessions(amount));
                lbTitle.Content = $"The last {amount} bike-sessions";
            }
            List<RunningSession> runSes = new List<RunningSession>();
            if (cbRunning.IsChecked == true)
            {
                runSes.AddRange(tm.GetPreviousRunningSessions(amount));
                lbTitle.Content = $"The last {amount} run-sessions";
            }
            if(cbRunning.IsChecked == true && cbBycicle.IsChecked == true)
            {
                lbTitle.Content = $"The last {amount} sessions";
            }

            List<Object> sessions = SortSessions(biSes, runSes).Take(amount).ToList();

            lvOverview.ItemsSource = sessions;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var trainigsmanager = new TrainingManager(new UnitOfWork(new TrainingContext()));
            if(lvOverview.SelectedItems != null)
            {
                List<int> cyclingIds = new List<int>();
                List<int> runningIds = new List<int>();
                foreach(var item in lvOverview.SelectedItems)
                {
                    if (item is CyclingSession)
                        cyclingIds.Add((item as CyclingSession).Id);
                    if (item is RunningSession)
                        runningIds.Add((item as RunningSession).Id);
                    trainigsmanager.RemoveTrainings(cyclingIds, runningIds);
                }
            }
            var window = new MainWindow();
            window.Show();
            this.Close();

        }
    }
}
