using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        
        public async Task<StudioUrlValidatorResult> Validate(Guid? studioId, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            
            if (studioId != null)
            {
                var entities = await _studioRepository.GetAsync();
                var urlAlreadyExists = entities.Any(s => s.Id != studioId && s.FriendlyUrl == url);

                if (urlAlreadyExists)
                {
                    throw new UriFormatException(string.Format("The given url is invalid: {0}", url));
                }
            }

            var doesUrlMatchPattern = Regex.IsMatch(url, ValidCharactersRegexPattern);
            if (!doesUrlMatchPattern)
            {
                var message =
                    string.Format(
                        "Friendly URL must start with a letter, and can only contain letters, numbers, hyphens and underscores",
                        url);

                throw new UriFormatException(message);
            }

            return StudioUrlValidatorResult.Success;
        }
    }
}