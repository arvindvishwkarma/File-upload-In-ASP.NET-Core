using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadDemo.Controllers
{
    public class FileController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private string _baseFolderPath;


        public FileController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _baseFolderPath = _hostingEnvironment.WebRootPath + "\\files\\";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadSingleFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = new FileStream(_baseFolderPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files)
        {

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = new FileStream(Path.Combine(_baseFolderPath, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileWithModel(FileUploadModel model)
        {
            if (model.File != null && model.File.Length > 0)
            {
                using (var stream = new FileStream(_baseFolderPath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }

    public class FileUploadModel
    {
        public IFormFile File { get; set; }
        public string FolderName { get; set; }
    }
}