using CQRSExample.Data;
using CQRSExample.Features.GuestRegistrations.Commands;
using CQRSExample.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSExample.Features.GuestRegistrations.Handlers
{
    /// <summary>
    /// Handles the creation of a new guest registration.
    /// </summary>
    public class CreateGuestRegistrationHandler : IRequestHandler<CreateGuestRegistrationCommand, int>
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateGuestRegistrationHandler(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<int> Handle(CreateGuestRegistrationCommand request, CancellationToken cancellationToken)
        {
            var attachedFileUrls = new List<string>();
            if (request.AttachedFiles != null && request.AttachedFiles.Count > 0)
            {
                var attachmentsPath = Path.Combine(_webHostEnvironment.WebRootPath ?? "wwwroot", "attachments");
                if (!Directory.Exists(attachmentsPath))
                {
                    Directory.CreateDirectory(attachmentsPath);
                }

                foreach (var file in request.AttachedFiles)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(attachmentsPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream, cancellationToken);
                    }
                    attachedFileUrls.Add($"/attachments/{fileName}");
                }
            }

            var guestRegistration = new GuestRegistration
            {
                Name = request.Name,
                DateOfBirth = request.DateOfBirth,
                PassportNumber = request.PassportNumber,
                Nationality = request.Nationality,
                Nrc = request.Nrc,
                CurrentAddress = request.CurrentAddress,
                PermanentAddress = request.PermanentAddress,
                AttachedFileUrls = JsonSerializer.Serialize(attachedFileUrls)
            };

            _context.GuestRegistrations.Add(guestRegistration);
            await _context.SaveChangesAsync(cancellationToken);

            return guestRegistration.Id;
        }
    }
}
