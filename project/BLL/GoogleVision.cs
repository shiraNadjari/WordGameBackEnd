using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace BLL
{
    public class GoogleVision
    {
        
       public void ReadJasonFile()
        {
            var file = File.ReadLines(@"C:\Users\shira_000\Desktop");

        }










            //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Users\shira_000\Downloads\wordproject-29b2e0d3e0d5.json");
            //// Instantiates a client
            //var client = ImageAnnotatorClient.Create();
            
            //// Load the image file into memory
            //var image = Image.FromFile(@"C:\Users\shira_000\Downloads\download (4).jpg");
            //// Performs label detection on the image file
            //var response = client.DetectLabels(image);
            //foreach (var annotation in response)
            //{
            //    if (annotation.Description != null)
            //        Console.WriteLine(annotation.Description);
            //}
        
    }
}
