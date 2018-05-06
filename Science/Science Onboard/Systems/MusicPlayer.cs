using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using Scarlet.Components;
using Scarlet.Utilities;

namespace Science.Systems
{
    public class MusicPlayer : ISubsystem
    {
        private MidiFile MIDI;

        public void EmergencyStop()
        {
            
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            
        }



        public void Initialize()
        {
            this.MIDI = MidiFile.Read("music.mid");
            IEnumerable<Note> Notes = this.MIDI.GetNotes();
            Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Note count: " + Notes?.Count());
            int Position = 0;

            Notes = Notes.OrderByDescending((Note n) => n.NoteNumber).OrderBy((Note n) => n.Time);

            bool DownOne = false;

            Dictionary<string, Tuple<int, int>> NoteData = new Dictionary<string, Tuple<int, int>>();

            TempoMap TMap = this.MIDI.GetTempoMap();

            int Count = 0;
            foreach (Note Note in Notes)
            {
                int Start = (int)(Note.TimeAs<MetricTimeSpan>(TMap).TotalMicroseconds / 1000);
                int ms = (int)(Note.LengthAs<MetricTimeSpan>(TMap).TotalMicroseconds / 1000);
                if (NoteData.ContainsKey(NoteToStr(Note))) { NoteData[NoteToStr(Note)] = new Tuple<int, int>(Start, ms); }
                else { NoteData.Add(NoteToStr(Note), new Tuple<int, int>(Start, ms)); }
                Count++;
                if (Count % 100 == 0) { Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Now at " + Count); }
            }

            foreach (Note Note in Notes)
            {
                int Start = NoteData[NoteToStr(Note)].Item1;
                int ms = NoteData[NoteToStr(Note)].Item2;
                if (Start < Position) { continue; }
                RoverMain.IOHandler.DrillController.SetSpeed(NoteToPercent(Note.NoteNumber - (DownOne ? 12 : 0)), true);
                Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Outputting note " + Note.NoteNumber + " at freq " + NoteToPercent(Note.NoteNumber - (DownOne ? 12 : 0)) + " for " + ms + "ms.");
                Thread.Sleep(ms);
                RoverMain.IOHandler.DrillController.SetSpeed(0, false);
                Position = Start + ms;
            }
        }

        private static string NoteToStr(Note Note)
        {
            return Note.ToString() + Note.Time + "," + Note.Length;
        }

        private static float NoteToPercent(int Note)
        {
            double Frequency = 261.63 / Math.Pow((Math.Pow(2, 1F / 12)), 60 - Note);
            return (float)((Frequency - 1.1111F) / 711.67F);
        }

        public void UpdateState()
        {
            
        }
    }
}
