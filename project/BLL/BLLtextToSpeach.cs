using System;
using System.IO;
using Google.Cloud.TextToSpeech.V1;

public class BLLtextToSpeach
{
    public static string TextToSpeach(string text)
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\TextToSpeach-b2d8743c4197.json");
        // Instantiate a client
        TextToSpeechClient client = TextToSpeechClient.Create();

        // Set the text input to be synthesized.
        SynthesisInput input = new SynthesisInput
        {
            Text = text
        };

        // Build the voice request, select the language code ("en-US"),
        // and the SSML voice gender ("neutral").
        VoiceSelectionParams voice = new VoiceSelectionParams
        {
            LanguageCode = "en-US",
            SsmlGender = SsmlVoiceGender.Neutral
        };

        // Select the type of audio file you want returned.
        AudioConfig config = new AudioConfig
        {
            AudioEncoding = AudioEncoding.Mp3
        };

        // Perform the Text-to-Speech request, passing the text input
        // with the selected voice parameters and audio file type
        var response = client.SynthesizeSpeech(new SynthesizeSpeechRequest
        {
            Input = input,
            Voice = voice,
            AudioConfig = config
        });
        string url = "";
        // Write the binary AudioContent of the response to an MP3 file.
        using (FileStream output = File.Create("tmp.mp3"))
        {
            response.AudioContent.WriteTo(output);
            url = output.Name;
            Console.WriteLine($"Audio content written to file 'sample.mp3'");
        }
        return url;
    }
}