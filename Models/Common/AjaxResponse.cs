namespace GCMS.Models.Common
{
    public class AjaxResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public long CaseId { get; set; }

        public object Data { get; set; }
    }
}