using Grpc.Core;
using System.IO;
using TestGrpc.FileService;
namespace GrpcServer.Services
{
    public class FileStreamerService : FileStreamer.FileStreamerBase
    {
        private const int BufferSize = 64 * 1024; // 64 KB chunks
        public override async Task DownloadFile(
            FileRequest request,
            IServerStreamWriter<FileChunk> responseStream,
            ServerCallContext context)
        {
            var filePath = Path.Combine("C:\\TestData", request.FileName);

            if (!File.Exists(filePath))
                throw new RpcException(new Status(StatusCode.NotFound, "File not found"));

            var totalSize = new FileInfo(filePath).Length;

            byte[] buffer = new byte[BufferSize];
            int read;
            int chunkNumber = 0;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                while ((read = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    chunkNumber++;

                    var chunk = new FileChunk
                    {
                        FileName = request.FileName,
                        TotalSize = totalSize,
                        Data = Google.Protobuf.ByteString.CopyFrom(buffer, 0, read),
                        ChunkNumber = chunkNumber,
                        IsLast = (fs.Position == totalSize)
                    };

                    await responseStream.WriteAsync(chunk);
                }
            }
        }
    }
}
