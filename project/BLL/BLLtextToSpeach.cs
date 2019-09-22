using System;
using System.IO;
using Google.Cloud.TextToSpeech.V1;
using BLL;
using Google.Cloud.Storage.V1;
using System.Collections.Generic;
using COMMON;

public class BLLtextToSpeach
{//Dictionary<string, int>
    public static string VoiceStorage(int userId,int catId, string URL, Dictionary<string, int> voicesCounter)
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\wordproject-29b2e0d3e0d5.json");
        // upload the image storage
        string voiceName;
        if(voicesCounter.Count>0)
        //voicesCounter[BLLcategory.GetCategoryById(catId).CategoryName]++
        voiceName = "voice" + BLLcategory.GetCategoryById(catId).CategoryName + voicesCounter[BLLcategory.GetCategoryById(catId).CategoryName]++ + ".mp3";
        else
        {
            List<COMimageObject> objs = new List<COMimageObject>();
            foreach (COMimage img in BLLimage.Getimages().FindAll(img => img.UserId == userId))
            {
                objs.AddRange(BLLobject.GetObjects().FindAll(obj => obj.ImageID == img.ImageID));
            }
            voiceName = "voice" + BLLcategory.GetCategoryById(catId).CategoryName + objs.Count + ".mp3";
        }
        string bucketName = "objectsound";
        var storage = StorageClient.Create();
        using (var f = File.OpenRead(URL))
        {
            try
            {
                var res = storage.UploadObject(bucketName, voiceName, null, f);
                URL = "https://storage.googleapis.com/" + bucketName + "/" + voiceName;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        return URL;
    }

    //get text return his temp voiceURL
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
            SsmlGender = SsmlVoiceGender.Neutral,
            Name = "en-US-Wavenet-F",
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
        string path = System.IO.Path.GetTempFileName();
      
        using (FileStream output = File.OpenWrite(path))
        {
            response.AudioContent.WriteTo(output);
            url = output.Name;
            Console.WriteLine($"Audio content written to file 'sample.mp3'");
        }
        return url;
    }
}
