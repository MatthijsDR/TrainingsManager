using DomainLibrary;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UITrainigsManager
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private TrainingManager _tm;
        public AddWindow(TrainingManager tm)
        {
            _tm = tm;
            InitializeComponent();
            bikeType.ItemsSource = Enum.GetValues(typeof(BikeType)).Cast<BikeType>();
            bikeTrainingType.ItemsSource = Enum.GetValues(typeof(TrainingType)).Cast<TrainingType>();
            runType.ItemsSource = Enum.GetValues(typeof(TrainingType)).Cast<TrainingType>();
        }

        private void btnAddBike_Click(object sender, RoutedEventArgs e)
        {
            bool speedFlag = double.TryParse(bikeAvgSpeed.Text, out double averageSpeed);
            float? nullableAverageSpeed = (averageSpeed == 0) ? null : (float?)averageSpeed;
            bool wattFlag = int.TryParse(avgWatt.Text, out int averageWatt);

            BikeType bikeType = BikeType.CityBike;
            bool typeFlag = false;
            if (this.bikeType.SelectedItem != null)
            {
                bikeType = (BikeType)this.bikeType.SelectedItem;
                typeFlag = true;
            }

            bool distanceFlag = double.TryParse(bikeAfstand.Text, out double distance);

            TrainingType trainingType = TrainingType.Endurance;
            bool trainigsFlag = false;
            if (bikeTrainingType.SelectedItem != null)
            {
                trainingType = (TrainingType)bikeTrainingType.SelectedItem;
                trainigsFlag = true;
            }
            var comment = bikeComment.Text;
            bool timeFlag = TimeSpan.TryParseExact(bikeTijdsduur.Text, "h\\:mm", CultureInfo.InvariantCulture, TimeSpanStyles.None, out var time);
            bool startTimeFlag = false;
            DateTime startTijd = DateTime.Now;
            if (dpStartTijd.Text != null)
            {
                startTijd = DateTime.Parse((dpStartTijd.Text));
                startTimeFlag = true;
            }


            if (typeFlag && trainigsFlag && timeFlag && startTimeFlag)
            {
                try
                {
                    _tm.AddCyclingTraining(startTijd, (float)distance, time,
                        nullableAverageSpeed, averageWatt, trainingType, comment, bikeType);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("An execption just occurred: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Record has been added","Succes", MessageBoxButton.OK);
                var window = new MainWindow();
                window.Show();
                this.Close();
            }
            else
            {
                if (!speedFlag) bikeAvgSpeed.Text = String.Empty;
                if (!distanceFlag) bikeAfstand.Text = String.Empty;
                if (!timeFlag) bikeTijdsduur.Text = String.Empty;
                MessageBox.Show("You didn't pass the right info", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnAddRun_Click(object sender, RoutedEventArgs e)
        {
            bool speedFlag = double.TryParse(runAvgSpeed.Text, out double averageSpeed);
            float? nullableAverageSpeed = (averageSpeed == 0) ? null : (float?)averageSpeed;

            bool distanceFlag = double.TryParse(runAfstand.Text, out double distance);

            TrainingType trainingType = TrainingType.Endurance;
            bool trainigsFlag = false;
            if (runType.SelectedItem != null)
            {
                trainingType = (TrainingType)runType.SelectedItem;
                trainigsFlag = true;
            }
            var comment = runComment.Text;
            bool timeFlag = TimeSpan.TryParseExact(runTijdsduur.Text, "h\\:mm", CultureInfo.InvariantCulture, TimeSpanStyles.None, out var time);
            bool startTimeFlag = false;
            DateTime startTijd = DateTime.Now;
            if (rundpStartTijd.Text != null)
            {
                startTijd = DateTime.Parse((rundpStartTijd.Text));
                startTimeFlag = true;
            }
            
            if (distanceFlag && trainigsFlag && timeFlag && startTimeFlag)
            {
                try
                {
                    _tm.AddRunningTraining(startTijd, (int)distance, time, nullableAverageSpeed, trainingType, comment);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An execption just occurred: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Record has been added", "Succes", MessageBoxButton.OK);
                var window = new MainWindow();
                window.Show();
                this.Close();
            }
            else
            {
                if (!distanceFlag) runAfstand.Text = String.Empty;
                if (!timeFlag) runTijdsduur.Text = String.Empty;
                MessageBox.Show("You didn't pass the right info", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
