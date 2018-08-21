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
        public static string MIDIFileName;
        public static int OctaveShift = int.MinValue;
        public static bool AutoOctaveDown = true;
        public static bool AutoOctaveUp = false;

        public bool TraceLogging { get; set; }

        private const float MOTOR_FREQUENCY_SLOPE = 730.48F;
        private const float MOTOR_FREQUENCY_INTERCEPT = -33.393F;

        private Thread PlayerThread;

        public void EmergencyStop() { this.PlayerThread.Abort(); }

        public void UpdateState() { }

        private void StartPlayer()
        {
            try
            {
                MidiFile MIDI = MidiFile.Read(MIDIFileName);
                IEnumerable<Note> Notes = MIDI.GetNotes();
                Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Will play MIDI file " + MIDIFileName + ", shifted " + OctaveShift + " octaves. Found " + Notes.Count() + " notes.");
                long PlayerPositionUS = 0;
                Notes = Notes.OrderByDescending((Note n) => n.NoteNumber).OrderBy((Note n) => n.Time); // Stable sorts. Sorts notes to be in high-to-low order, then back to by-time, so that for chords, only the highest note gets played.
                TempoMap TempoMap = MIDI.GetTempoMap();

                Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "MIDI playback starting...");
                foreach (Note Note in Notes)
                {
                    long StartUS = Note.TimeAs<MetricTimeSpan>(TempoMap).TotalMicroseconds;
                    long LengthUS = Note.LengthAs<MetricTimeSpan>(TempoMap).TotalMicroseconds;
                    if (StartUS < PlayerPositionUS) { continue; } // If we've missed the start of the note (due to overlap), skip it.
                    if (StartUS > (PlayerPositionUS + 1000)) { Thread.Sleep((int)((StartUS - PlayerPositionUS) / 1000)); } // If the start is in more than 1 ms, wait until then to allow for rests.

                    if (NoteToPercent(Note.NoteNumber + (OctaveShift * 12)) > 1 && AutoOctaveDown) { Note.NoteNumber = (SevenBitNumber)(Note.NoteNumber - 12); }
                    if (NoteToPercent(Note.NoteNumber + ((OctaveShift + 1) * 12)) <= 1 && AutoOctaveUp) { Note.NoteNumber = (SevenBitNumber)(Note.NoteNumber + 12); }
                    RoverMain.IOHandler.DrillController.SetSpeed(NoteToPercent(Note.NoteNumber + (OctaveShift * 12)), true);
                    if (this.TraceLogging) { Log.Trace(this, "Outputting note " + Note.NoteNumber + " at speed " + NoteToPercent(Note.NoteNumber + (OctaveShift * 12)) + " for " + (LengthUS / 1000) + "ms."); }
                    Thread.Sleep((int)(LengthUS / 1000));
                    RoverMain.IOHandler.DrillController.SetSpeed(0, true);
                    PlayerPositionUS = StartUS + LengthUS; // The player is now at the end of the note we just played.
                }
                RoverMain.IOHandler.DrillController.SetSpeed(0, false);
                Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "MIDI playback complete.");
            }
            catch(Exception Exc)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.MOTORS, "Failed to play MIDI file.");
                Log.Exception(Log.Source.MOTORS, Exc);
            }
        }

        public void Initialize()
        {
            if (MIDIFileName != null && OctaveShift != int.MinValue)
            {
                this.PlayerThread = new Thread(new ThreadStart(this.StartPlayer));
                this.PlayerThread.Start();
            }
        }

        private static float NoteToPercent(int Note)
        {
            float Frequency = 261.63F / (float)(Math.Pow(Math.Pow(2, (1.0 / 12)), 60 - Note));
            return (Frequency - MOTOR_FREQUENCY_INTERCEPT) / MOTOR_FREQUENCY_SLOPE;
        }

        public void Exit()
        {
            this.PlayerThread.Abort();
            RoverMain.IOHandler.DrillController.SetSpeed(0, true);
        }

    }
}
