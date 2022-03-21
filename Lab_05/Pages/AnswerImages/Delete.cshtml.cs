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

namespace Lab_05.Pages.AnswerImages
{
    public class DeleteModel : PageModel
    {

            private readonly BlobServiceClient _blobServiceClient;
            private BlobContainerClient _blobContainerClient;
            private readonly string earthContainerName = "earthimages";
            private readonly string computerContainerName = "computerimages";
            private string earthimagesOrComputerimages = "";


            private readonly Lab_05.Data.AnswerImageDataContext _context;

            public DeleteModel(BlobServiceClient blobServiceClient, Lab_05.Data.AnswerImageDataContext context)
            {
                _blobServiceClient = blobServiceClient;
                _context = context;
            }


            [BindProperty]
            public AnswerImage AnswerImage { get; set; }

            public async Task<IActionResult> OnGetAsync(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                AnswerImage = await _context.AnswerImage.FirstOrDefaultAsync(m => m.AnswerImageId == id);

                if (AnswerImage == null)
                {
                    return NotFound();
                }
                return Page();
            }

            public async Task<IActionResult> OnPostAsync(int? id)
            {

                if (id == null)
                {
                    return NotFound();
                }

                AnswerImage = await _context.AnswerImage.FindAsync(id);

                Question question = AnswerImage.Question;
                if (question == Question.Computer)
                    earthimagesOrComputerimages = earthContainerName;
                else
                    earthimagesOrComputerimages = computerContainerName;

                if (AnswerImage != null)
                {
                    try
                    {
                        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(earthimagesOrComputerimages);
                        if (await _blobContainerClient.GetBlobClient(AnswerImage.FileName).ExistsAsync())
                        {
                            await _blobContainerClient.GetBlobClient(AnswerImage.FileName).DeleteAsync();
                        }
                        _context.AnswerImage.Remove(AnswerImage);
                        await _context.SaveChangesAsync();


                    }
                    catch (RequestFailedException)
                    {
                        return RedirectToPage("/Error");
                    }

                }
                return RedirectToPage("./Index");
            }
        }
}
