using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public class StudioUrlValidator : IStudioUrlValidator
    {
        private readonly IStudioRepository _studioRepository;

        private static readonly int MinLength = 5;
        private static readonly int MaxLength = 25;
        private static readonly string ValidCharactersRegexPattern = @"^[a-zA-Z]([a-zA-Z0-9\-_]+)?$";

        public StudioUrlValidator(
            IStudioRepository studioRepository)
        {
            _studioRepository = studioRepository;
        }
        
        public async Task<StudioUrlValidatorResult> ValidateAsync(Guid? studioId, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (url.Length < MinLength || url.Length > MaxLength)
            {
                throw new ArgumentException($"URL must be between {MinLength} and {MaxLength} characters in length");
            }
            
            var entities = await _studioRepository.GetAsync();
            var urlAlreadyExists =
                studioId == null
                ? entities.Any(s => s.FriendlyUrl == url)
                : entities.Any(s => s.Id != studioId && s.FriendlyUrl == url);
                
            if (urlAlreadyExists)
            {
                throw new StudioUrlInUseException(string.Format("The given url is already in use: {0}", url));
            }

            var doesUrlMatchPattern = Regex.IsMatch(url, ValidCharactersRegexPattern);
            if (!doesUrlMatchPattern)
            {
                var message =
                    string.Format(
                        "URL must start with a letter, and can only contain letters, numbers, hyphens and underscores",
                        url);

                throw new UriFormatException(message);
            }

            return StudioUrlValidatorResult.Success;
        }
    }
}