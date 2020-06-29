using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace jmeterSite.Pages
{
    public class FileUpload
    {

        [Required]
        [Display(Name="JMX文件")]
        public IFormFile UploadPublicSchedule { get; set; }
    }
}