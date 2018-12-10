using System;
using System.Media;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace VoiceRecBlank
{ //ChickenBacon Assistant
    class Program
    {
        //Default windows user path example C:\Users\Chris
        public static string Path = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}";

        public static int Test { get; set; }

        //Add reference "System.Speech" manually
        private static readonly SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        static void Main(string[] args)
        {
            SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
            Choices commands = new Choices();

            //Add voicecommand. Split with comma.
            commands.Add(new string[]
            {
                "test", "number count test", "hello computer"
            });

            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);
            Grammar grammar = new Grammar(gBuilder);
            recEngine.LoadGrammarAsync(grammar);

            //Sets default input device to be used.
            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            recEngine.RecognizeAsync(RecognizeMode.Multiple);

            Thread.Sleep(1000);
            Console.WriteLine("Waiting for voice input.");
            Console.ReadKey();
        }

        private static void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Add command action with same name as voicecommand (They are binded together).
            switch (e.Result.Text)
            {
                case "test":
                    SoundPlayer player = new SoundPlayer($@"C:\Windows\media\tada.wav");
                    player.Play();
                    Test++;
                    Console.WriteLine("Did you hear the tada sound?");
                    break;
                case "number count test":
                    synthesizer.SpeakAsync("Test has been said " + Test + " times." );
                    Console.WriteLine($"{Test}");
                    break;
                case "hello computer":
                    synthesizer.SpeakAsync("Hello world");
                    Console.WriteLine("Hello world");
                    break;
                //case "add your own command":
                //    Action;
                //    Console.WriteLine("Test");
                //    break;
            }
        }
    }
}