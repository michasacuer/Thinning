namespace Thinning.Infrastructure
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using Thinning.Infrastructure.Interfaces;
    using Thinning.Infrastructure.Models;
    using static Thinning.Infrastructure.Models.Storage;

    public class WebService : IWebService
    {
        public void UpdatePcInfoStorage(string cpu, string gpu, string memory, string os)
        {
            StorageDto.PcInfo = new PcInfo
            {
                Cpu = cpu,
                Gpu = gpu,
                Memory = memory,
                Os = os
            };
        }

        public void UpdateStorage(List<string> algorithmNames)
        {
            StorageDto.TestLines = new List<TestLine>();
            foreach (string name in algorithmNames)
            {
                StorageDto.TestLines.Add(new TestLine
                {
                    AlgorithmName = name
                });
            }
        }

        public void UpdateStorage(TestResult testResult, string baseImageFilepath)
        {
            this.TestRunsToTestLines(testResult);
            this.SetResultBitmaps(testResult);
            this.SetTestImage(baseImageFilepath);
        }

        public async Task<bool> PublishResults()
        {
            const string endpoint = "https://localhost:5001/api/test/add";

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new
                {
                    pcInfo = StorageDto.PcInfo,
                    testLines = StorageDto.TestLines,
                    images = StorageDto.Images
                });

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }

        private void TestRunsToTestLines(TestResult testResult)
        {
            int algorithmCount = 0;
            foreach (var times in testResult.ResultTimes)
            {
                StorageDto.TestLines[algorithmCount].Iterations = times.Count;
                StorageDto.TestLines[algorithmCount].AlgorithmTestRuns = new List<TestRun>();

                int runCount = 0;
                foreach (double time in times)
                {
                    StorageDto.TestLines[algorithmCount].AlgorithmTestRuns.Add(new TestRun
                    {
                        Time = time,
                        RunCount = runCount
                    });

                    runCount++;
                }

                algorithmCount++;
            }
        }

        private void SetResultBitmaps(TestResult testResult)
        {
            StorageDto.Images = new List<Storage.Image>();
            int imageCount = 0;
            foreach (var bmp in testResult.ResultBitmaps)
            {
                StorageDto.Images.Add(new Storage.Image
                {
                    OriginalBpp = 0,
                    OriginalHeight = bmp.Height,
                    OriginalWidth = bmp.Width,
                    ImageContent = testResult.RawBitmaps[imageCount],
                    TestImage = false,
                    AlgorithmName = StorageDto.TestLines[imageCount].AlgorithmName
                });

                imageCount++;
            }
        }

        private void SetTestImage(string baseImageFilepath)
        {
            var testBitmap = new Bitmap(baseImageFilepath);
            var bitmapData = testBitmap.LockBits(
                new Rectangle(0, 0, testBitmap.Width, testBitmap.Height),
                ImageLockMode.ReadWrite,
                testBitmap.PixelFormat);

            int pixelsCount = bitmapData.Stride * testBitmap.Height;
            var imageContent = new byte[pixelsCount];
            Marshal.Copy(bitmapData.Scan0, imageContent, 0, pixelsCount);
            testBitmap.UnlockBits(bitmapData);

            StorageDto.Images.Add(new Storage.Image
            {
                OriginalBpp = 0,
                OriginalHeight = testBitmap.Height,
                OriginalWidth = testBitmap.Width,
                ImageContent = imageContent,
                TestImage = true,
                AlgorithmName = "TestImage"
            });
        }
    }
}
