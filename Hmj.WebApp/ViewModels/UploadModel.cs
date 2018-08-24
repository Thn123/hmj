namespace Hmj.WebApp.ViewModels
{
    public class UploadModel
    {
        public string UploadId { get; set; }
        public string ShowText { get; set; }
        public int Type { get; set; }
        public string PostUrl { get; set; }

        public string OwnerId { get; set; }
    }

    /// <summary>
    /// 上传附件实体
    /// </summary>
    public class UploadInfo
    {
        public bool IsReady { get; set; }
        public int ContentLength { get; set; }
        public int UploadedLength { get; set; }
        public string FileName { get; set; }
    }
}