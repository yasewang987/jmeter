using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;

namespace jmeterSite.Pages
{
    public class IndexModel : PageModel
    {
        public List<FileList> FileResult = new List<FileList>();
        [BindProperty]
        public FileUpload FileUpload {get;set;}
        private readonly IFileProvider _fileProvider;
        public IndexModel(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public void OnGet()
        {
            var directoryContents = _fileProvider.GetDirectoryContents("/jmx");
            var csvDirectoryContents = _fileProvider.GetDirectoryContents("/csv");
            var htmlDirectoryContents = _fileProvider.GetDirectoryContents("/html");
            foreach(var item in directoryContents) {
                var name = item.Name.Remove(item.Name.Length-4);
                var file = new FileList{
                    FileName = name
                };
                if(csvDirectoryContents.Any(i => $"{name}.csv".Equals(i.Name)))
                {
                    file.haveCsv = true;
                }

                if(htmlDirectoryContents.Any(i => name.Equals(i.Name)))
                {
                    file.haveHtml = true;
                }

                FileResult.Add(file);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // var file = Request.Form.Files[0];
            var filePath = $"/j/jmx/{FileUpload.UploadPublicSchedule.FileName}";

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await FileUpload.UploadPublicSchedule.CopyToAsync(fileStream);
            }

            return RedirectToPage("./Index");
        }

        public ActionResult OnGetCsv(string filename)
        {
            var result = _ExecBash("csv.sh", filename);
            return new JsonResult(result);
        }

        public ActionResult OnGetHtml(string filename)
        {
            var result = _ExecBash("html.sh", filename);
            return new JsonResult(result);
        }

        private string _ExecBash(string filename, string arguments)
        {
            var psi = new ProcessStartInfo($"docker", $"exec jclient sh/{filename} {arguments}")
            {
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            var sb = new StringBuilder();
            //启动
            var proc=Process.Start(psi);
            if (proc == null)
            {
                sb.Append("无法执行！");
            }
            else
            {
                proc.WaitForExit();
                //开始读取
                using (var sr = proc.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        sb.AppendLine($"{sr.ReadLine()}");
                    }

                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                }
                sb.AppendLine($"Exited Code ： {proc.ExitCode}");
            }
            return sb.ToString();
        }
    }
}
