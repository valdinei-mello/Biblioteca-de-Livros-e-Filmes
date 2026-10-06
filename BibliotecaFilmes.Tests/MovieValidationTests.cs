using System.ComponentModel.DataAnnotations;
using BibliotecaFilmes.Api.DTOs;
using FluentAssertions;

namespace BibliotecaFilmes.Tests;

public class MovieValidationTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(5, true)]
    [InlineData(5.1, false)]
    public void CreateAndUpdateRequests_ValidateRatingRange(decimal rating, bool isValid)
    {
        var createRequest = new CreateMovieRequest { Rating = rating };
        var updateRequest = new UpdateMovieRequest { Rating = rating };

        ValidateRating(createRequest, createRequest.Rating).Should().Be(isValid);
        ValidateRating(updateRequest, updateRequest.Rating).Should().Be(isValid);
    }

    private static bool ValidateRating(object request, decimal rating)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(request)
        {
            MemberName = nameof(CreateMovieRequest.Rating)
        };

        return Validator.TryValidateProperty(rating, context, results);
    }
}
