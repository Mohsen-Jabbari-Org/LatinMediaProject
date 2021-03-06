/**
 * Author: dyx1001
 * Email: dyx1001@126.com
 * License: MIT
 * Git URL: https://github.com/dyx1001/BigBlueButtonAPI.NET
 */
using System.Xml.Serialization;

namespace BigBlueButtonAPI.Core
{
    [XmlRoot("response")]
    public class PutRecordingTextTrackResponse:BaseResponse
    {
        public string recordID { get; set; }
    }
}