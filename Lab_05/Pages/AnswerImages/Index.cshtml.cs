using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab_05.Data;
using Lab_05.Models;

namespace Lab_05.Pages.AnswerImages
{
    public class IndexModel : PageModel
    {
        private readonly Lab_05.Data.AnswerImageDataContext _context;

        public IndexModel(Lab_05.Data.AnswerImageDataContext context)
        {
            _context = context;
        }

        public IList<AnswerImage> AnswerImage { get; set; }

        public async Task OnGetAsync()
        {
            AnswerImage = await _context.AnswerImage.ToListAsync();
        }
    }
}
