using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public enum RespiratoryEffort { 
        None,
        Gasping,
        Weak,
        Regular
    }

    public enum HeartRate  {
        LessSixty,
        SixtyToEighty,
        EightyToHundred,
        GreaterHundred
    }

    public enum ChestMovement { 
        Absent,
        Present
    }
    public class Reassessment : Event
    {
        private HeartRate hr;
        private ChestMovement movement;
        private RespiratoryEffort effort;
        private string time;

        public HeartRate Hr { get => hr; set => hr = value; }
        public ChestMovement Movement { get => movement; set => movement = value; }
        public RespiratoryEffort Effort { get => effort; set => effort = value; }
        public string Time { get => time; set => time = value; }



        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("Reassessment at " + Time + " :\n");
            sb.Append('\t' + "Heart Rate (bpm): " + HeartRateToString() + '\n');
            sb.Append('\t' + "Chest Movement: " + Movement.ToString() + '\n');
            sb.Append('\t' + "Respiratory Effor: " + RespEffortToString() + "\n");
            sb.Append('\t' + "Timespan: " + Time);

            return sb.ToString();
        }

        private String HeartRateToString() {
            switch (Hr) {
                case HeartRate.LessSixty:
                    return "<60";
                case HeartRate.SixtyToEighty:
                    return "60 to 80";
                case HeartRate.EightyToHundred:
                    return "80 to 100";
                case HeartRate.GreaterHundred:
                    return ">100";                   
                default:
                    return "";

            }
        }

        private String RespEffortToString()
        {
            switch (Effort)
            {
                case RespiratoryEffort.None:
                    return "No Effort";
                case RespiratoryEffort.Gasping:
                    return "Gasping";
                case RespiratoryEffort.Weak:
                    return "Weak Effort";
                case RespiratoryEffort.Regular:
                    return "Regular Respirations";
                default:
                    return "";
            }

        }
    }
}
