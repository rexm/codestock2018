using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace submission_api
{
    public class Submission
    {
        public string Number {get;set;}
        public string SubmitterName {get;set;}
        public SubmissionState State {get;set;}

        public bool IsValid {get;set;}
        public int PayoutAmount {get;set;}
    }

    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubmissionState
    {
        Initialized = 0,
        Pending = 1,
        Hold = 2,
        Released = 3,
        Deleted = 4
    }
}