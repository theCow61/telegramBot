using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Amazon.Runtime.Internal.Util;
using ByteSizeLib;

namespace TelegramBot
{
    public class AWSworker
    {
        public string bucketName { get; set; }
        public RegionEndpoint bucketRegion { get; set; }
        private IAmazonS3 s3Client;
        public MemoryStream ms { get; set; }
        public string filePlacement { get; set; }
        public string AWSfoldRoot { get; set; }
        public string lsSpit;
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
        public async Task ListingObjectsAsync()
        {
            try
            {
                lsSpit = "";
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = $"{bucketName}",
                    MaxKeys = 50,
                    Prefix = $"{filePlacement}"
                };
                ListObjectsV2Response response;
                do
                {
                    response = await s3Client.ListObjectsV2Async(request);
                    //proccess it tho
                    foreach(S3Object entry in response.S3Objects)
                    {
                        string size = ByteSize.FromBytes(entry.Size).ToString();

                        string temKey = entry.Key.Remove(0, AWSfoldRoot.Length);
                        lsSpit += String.Format("{0,-15}        {1,15}\n", temKey, size);
                    }
                    //Console.WriteLine($"Next continue token: {response.NextContinuationToken}");
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);
                Console.WriteLine(lsSpit);
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
        public async Task DeleteObjectBucketAsync()
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = $"{bucketName}",
                    Key = $"{filePlacement}"
                };
                await s3Client.DeleteObjectAsync(deleteObjectRequest);
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
