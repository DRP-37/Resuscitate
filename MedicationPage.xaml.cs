using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MedicationPage : Page
    {
        private static int NUM_MEDICATIONS = 8;
        
        public Timing TimingCount { get; set; }
        private TextBlock[] NameViews;

        // Doses:
        // 0: Adrenaline 1 in 10,000 (0.1 ml/kg) IV  
        // 1: Adrenaline 1 in 10,000 (0.3 ml/kg) IV  
        // 2: Sodium Bicarbonate 4.2% (2 to 4 mls/kg) IV 
        // 3: Dextrose (2.5mls/kg) IV 
        // 4: Red cell transfusion 
        // 5: Adrenaline via ETT 
        // 6: Surfactant via ETT 120mg 
        // 7: Surfactant via ETT 240mg
        private Button[] Medications;
        private TextBlock[] DoseViews;
        private int[] NumDoses = new int[NUM_MEDICATIONS];
        private bool[] DoseGiven = new bool[NUM_MEDICATIONS];
        private StatusEvent[] MedicationEvents;

        private Medication medication;

        public MedicationPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Medications = new Button[] { ADR1Button, ADR2Button, SodBicarbButton, DextroseButton,
                CellTransfusionButton, ADRviaETTButton, Surfactant120Button, Surfactant240Button };
            DoseViews = new TextBlock[] { ADR1Dose, ADR2Dose, SodBicarbDose, DextroseDose,
                CellTransfusionDose, ADRviaETTDose, Surfactant120Dose, Surfactant240Dose };
            NameViews = new TextBlock[] { ADR1View, ADR2View, SodBicarbView, DextroseView, 
                CellTransfusionView, ADRviaETTView, Surfactant120View, Surfactant240View};
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            base.OnNavigatedTo(e);

            medication = new Medication();
            MedicationEvents = new StatusEvent[NUM_MEDICATIONS];

            // Reset DoseGiven and buttons' colours
            foreach (Button Medication in Medications)
            {
                Medication.Background = new SolidColorBrush(Colors.White);
            }

            DoseGiven = new bool[NUM_MEDICATIONS];
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            medication.Time = TimingCount.Time;
            medication.setData(DoseGiven);

            List<Event> Events = new List<Event>();
            Events.Add(medication);

            string Time = TimingCount.Time;
            List<StatusEvent> StatusEvents = new List<StatusEvent>();

            foreach (StatusEvent Event in MedicationEvents)
            {
                if (Event != null)
                {
                    StatusEvents.Add(Event);
                }
            }

            if (StatusEvents.Count > 0)
            {
                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
            ResetDoses();
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void DoseGiven_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button == null)
                return;

            int reference = Array.IndexOf(Medications, button);

            SolidColorBrush Colour = button.Background as SolidColorBrush;

            if (Colour.Color == Colors.White)
            {
                NumDoses[reference]++;
                DoseGiven[reference] = true;
                MedicationEvents[reference] = GenerateStatusEvent(reference);
                button.Background = new SolidColorBrush(Colors.LightGreen);
            } else
            {
                NumDoses[reference]--;
                DoseGiven[reference] = false;
                MedicationEvents[reference] = null;
                button.Background = new SolidColorBrush(Colors.White);
            }

            DoseViews[reference].Text = NumDoses[reference].ToString();
        }

        private void ResetDoses()
        {
            for (int i = 0; i < NUM_MEDICATIONS; i++)
            {
                if (DoseGiven[i])
                {
                    NumDoses[i]--;
                    DoseViews[i].Text = NumDoses[i].ToString();
                }
            }
        }


        // int index refers to the index of a Button in Button[] Medications
        private StatusEvent GenerateStatusEvent(int index)
        {
            return new StatusEvent("Medication Given", NameViews[index].Text + " (Dose " + NumDoses[index] + ")", TimingCount.Time, medication);
        }

        // Move to its own class later on - needed it many classes
        private bool SelectionMade(Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
