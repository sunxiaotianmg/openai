﻿using OpenAI.SDK.Interfaces;
using OpenAI.SDK.Models;
using OpenAI.SDK.Models.RequestModels;

namespace OpenAI.Playground
{
    internal static class SearchTestHelper
    {
        public static async Task UploadSampleFileAndGetSearchResponse(IOpenAISdk sdk)
        {
            const string fileName = "SearchSample.jsonl";

            try
            {
                Console.WriteLine($"Starting to read {fileName}");
                var searchSampleFile = await File.ReadAllBytesAsync($"SampleData/{fileName}");
                Console.WriteLine($"Uploading to read {fileName}");
                var uploadResult = await sdk.Files.UploadFiles(UploadFilePurposes.UploadFilePurpose.Search.EnumToString(), searchSampleFile, fileName);
                if (uploadResult?.Successful == true)
                {
                    Console.WriteLine($"Uploading is done.");
                    Console.WriteLine($"File name:{uploadResult.FileName}");
                    Console.WriteLine($"File id:{uploadResult.Id}");
                    Console.WriteLine($"File purpose:{uploadResult.Purpose}");
                }

                Console.WriteLine($"Fetching files.");
                var uploadedFiles = await sdk.Files.ListFiles();
                var uploadedFile = uploadedFiles!.Data.Single(r => r.Id == uploadResult.Id);
                Console.WriteLine($"File found.");
                var file = await sdk.Files.RetrieveFile(uploadedFile.Id);
                Console.WriteLine($"File retrieved.{file.CreatedAt}");
                //var deleteResponse = await sdk.Files.DeleteFile(uploadedFile.Id);
                //if (deleteResponse?.Successful == true && deleteResponse.Deleted)
                //{
                //    Console.WriteLine($"File deleted.");
                //}
                //else
                //{
                //    Console.WriteLine($"Something went wrong while deleting file.");
                //}
                var searchResponse = await sdk.Searches.CreateSearch(Engines.Engine.Ada, new CreateSearchRequest()
                {
                    File = uploadedFile.Id,
                    MaxRerank = 5,
                    Query = "happy",
                    SearchModel = Engines.Engine.Ada.EnumToString()
                });
                if (searchResponse?.Successful == true)
                {
                    Console.WriteLine(string.Join(",",searchResponse.Choices));
                }
                else
                {
                    Console.WriteLine($"Something went wrong while creating a search.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public static async Task UploadSampleFile(IOpenAISdk sdk)
        {
            const string fileName = "SearchSample.json";

            try
            {
                Console.WriteLine($"Starting to read {fileName}");
                var searchSampleFile = await File.ReadAllBytesAsync($"SampleData/{fileName}");
                Console.WriteLine($"Uploading to read {fileName}");
                var uploadResult = await sdk.Files.UploadFiles(UploadFilePurposes.UploadFilePurpose.Search.EnumToString(), searchSampleFile, fileName);
                if (uploadResult?.Successful == true)
                {
                    Console.WriteLine($"Uploading is done.");
                    Console.WriteLine($"File name:{uploadResult.FileName}");
                    Console.WriteLine($"File id:{uploadResult.Id}");
                    Console.WriteLine($"File purpose:{uploadResult.Purpose}");
                }

                Console.WriteLine($"Fetching files.");
                var uploadedFiles = await sdk.Files.ListFiles();
                var uploadedFile = uploadedFiles!.Data.Single(r => r.Id == uploadResult.Id);
                Console.WriteLine($"File found.");
                var file = await sdk.Files.RetrieveFile(uploadedFile.Id);
                Console.WriteLine($"File retrieved.{file.CreatedAt}");
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}