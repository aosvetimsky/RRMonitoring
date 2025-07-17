using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using Nomium.Core.FileStorage.Providers.S3;
using Nomium.Core.FileStorage.Providers.S3.Models;
using Nomium.Core.FileStorage.Utils;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Constants;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhoto;

public class UpdateCurrentUserPhotoRequest : IRequest
{
	public IFormFile Photo { get; set; }

	public UpdateCurrentUserPhotoRequest(IFormFile profilePhoto)
	{
		Photo = profilePhoto;
	}
}

public class UpdateCurrentUserPhotoHandler : IRequestHandler<UpdateCurrentUserPhotoRequest>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly IS3FileProvider _s3FileProvider;

	private readonly string _userPhotoFolder;

	public UpdateCurrentUserPhotoHandler(
		IAccountService accountService,
		IdentityUserManager userManager,
		IS3FileProvider s3FileProvider,
		IOptions<IdentityS3FileProviderConfiguration> options)
	{
		_accountService = accountService;
		_userManager = userManager;
		_s3FileProvider = s3FileProvider;

		_userPhotoFolder = options.Value.UserPhotoFolder;
	}

	public async Task<Unit> Handle(UpdateCurrentUserPhotoRequest request, CancellationToken cancellationToken)
	{
		var currentUserId = _accountService.GetCurrentUserId();
		if (!currentUserId.HasValue)
		{
			throw new UnauthorizedAccessException();
		}

		var user = await _userManager.FindByIdAsync(currentUserId.Value.ToString());
		if (user is null)
		{
			throw new ValidationException($"No user with ID: {currentUserId}");
		}

		await using var namedStream = await ImageCompressor.Compress(request.Photo, maxWidth: 300);
		var uploadFileModel = new UploadFileModel
		{
			FilePath = $"{_userPhotoFolder}/{user.Id}{KnownFileExtensions.Jpg}", Stream = namedStream
		};

		await _s3FileProvider.UploadFile(uploadFileModel, cancellationToken);

		return Unit.Value;
	}
}
