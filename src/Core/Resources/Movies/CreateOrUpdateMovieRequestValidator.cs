using System.Linq;
using FluentValidation;

namespace WebApiMovieRest.Core.Resources.Movies
{
    public class CreateOrUpdateMovieRequestValidator<T> : AbstractValidator<T> where T : ICreateOrUpdateMovieRequest
    {
        public CreateOrUpdateMovieRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(r => r.ImdbId)
                .NotEmpty()
                .Length(9);

            RuleFor(r => r.Title)
                .NotEmpty()
                .Length(1, 128);

            RuleFor(r => r.Rating)
                .InclusiveBetween(0, 10);

            RuleFor(r => r.ReleaseDate)
                .NotNull();

            RuleFor(r => r.Director)
                .NotEmpty()
                .Length(1, 64);

            RuleFor(r => r.Plot)
                .NotEmpty()
                .Length(1, 256);

            RuleFor(r => r.Genres)
                .NotEmpty()
                .Must(BeNonEmptyAndLessThan64Characters)
                .WithMessage("'Genre' must not be empty and between 1 and 64 characters.");
        }

        private bool BeNonEmptyAndLessThan64Characters(string[] genres)
        {
            return genres.All(genre => !string.IsNullOrWhiteSpace(genre) && genre.Length <= 64);
        }
    }
}