using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_05.Data;
using Lab_05.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab_05.Pages.AnswerImages
{
    public class CreateModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;
        BlobContainerClient _blobContainerClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";
        private string earthimagesOrComputerimages = "";
        private readonly Lab_05.Data.AnswerImageDataContext _context;

        public CreateModel(BlobServiceClient blobServiceClient, Lab_05.Data.AnswerImageDataContext context)
        {
            _blobServiceClient = blobServiceClient;
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["AnswerImageId"] = new SelectList(_context.AnswerImage, "FileName", "Question", "Url");
            return Page();
        }

        [BindProperty]
        public AnswerImage AnswerImage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IEnumerable<IFormFile> files)
        {

            Question question = AnswerImage.Question;
            if (question == Question.Computer)
                earthimagesOrComputerimages = earthContainerName;
            else
                earthimagesOrComputerimages = computerContainerName;


            try
            {
                _blobContainerClient = await _blobServiceClient.CreateBlobContainerAsync(earthimagesOrComputerimages);
                _blobContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                _blobContainerClient = _blobServiceClient.GetBlobContainerClient(earthimagesOrComputerimages);
            }
            try
            {
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        //var filePath = Path.Combine(_config["StoredFilesPath"],Path.GetRandomFileName());
                        var filePath = Path.GetRandomFileName();
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                            stream.Position = 0;
                            await _blobContainerClient.GetBlobClient(filePath).UploadAsync(stream);
                            stream.Close();
                        }
                        var image = new AnswerImage
                        {
                            Url = _blobContainerClient.GetBlobClient(filePath).Uri.AbsoluteUri,
                            FileName = filePath,
                            Question = AnswerImage.Question
                        };
                        _context.AnswerImage.Add(image);
                        await _context.SaveChangesAsync();

                    }

                }

            }
            catch (RequestFailedException)
            {

                return RedirectToPage("/Error");
            }

            return RedirectToPage("./Index");
        }
    }
}
