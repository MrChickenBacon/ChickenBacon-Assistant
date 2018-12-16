//ChickenBacon Assistant Blank

//BEFORE YOU BEGIN:
//Add reference "System.Speech" manually

using System;
using System.Media;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace ChickenBaconAssistantTemplate
{
    class Program
    {
        //Default windows user path example C:\Users\Chris
        public static string Path = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}";

        //Test counter, start value 0
        public static int Test { get; set; }

        //Speech property
        private static readonly SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        static void Main(string[] args)
        {
            //Optional
            //synthesizer.SelectVoice("Voice Name");  <= example "Microsoft David". If nothing is set, => Default system voice will be used.

            SpeechRecoEngine();
            Thread.Sleep(1000);
            Console.WriteLine("Waiting for voice input.");
            Console.ReadKey();
        }

        private static void SpeechRecoEngine()
        {
            SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
            var gBuilder = CommandsGrammarBuilder();
            Grammar grammar = new Grammar(gBuilder);
            recEngine.LoadGrammarAsync(grammar);
            try
            {
                recEngine.SetInputToDefaultAudioDevice();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
                Console.WriteLine("Please plug in a microphone.");
                Console.ReadLine();
                throw;
            }
            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            recEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private static GrammarBuilder CommandsGrammarBuilder()
        {
            Choices commands = new Choices();

            //Add voicecommand. Split with comma.
            commands.Add(new string[]
            {
                "test", "number count test", "hello computer"
            });
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);
            return gBuilder;
        }

        private static void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Add command action with same name as voicecommand (They are binded together).
            switch (e.Result.Text)
            {
                case "test":
                    using (SoundPlayer player = new SoundPlayer($@"C:\Windows\media\tada.wav"))
                    {
                        player.Play();
                    }
                    Test++;
                    Console.WriteLine("Did you hear the tada sound?");
                    break;
                case "number count test":
                    synthesizer.SpeakAsync("Test has been said " + Test + " times.");
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