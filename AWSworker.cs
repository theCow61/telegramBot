using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;

namespace ShittyTea
{
    public class AWSworker
    {
        public string bucketName { get; set; }
        public RegionEndpoint bucketRegion { get; set; }
        private IAmazonS3 s3Client;
        public MemoryStream ms { get; set; }
        public string filePlacement { get; set; }
        public void assignS3()
        {
            s3Client = new AmazonS3Client(bucketRegion);
        }
        public async Task UploadFileAsync()
        {
            try
            {
                TransferUtility fileTransferUtility = new TransferUtility(s3Client);
                using (ms)
                {
                    await fileTransferUtility.UploadAsync(ms, bucketName, filePlacement);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public async Task DownloadFileAsync()
        {
            try
            {
                TransferUtility fileTransferUtility = new TransferUtility(s3Client);
                await fileTransferUtility.DownloadAsync(filePlacement, bucketName, filePlacement);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}