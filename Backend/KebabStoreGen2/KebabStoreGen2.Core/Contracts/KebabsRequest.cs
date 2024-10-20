using KebabStoreGen2.Core.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KebabStoreGen2.Core.Contracts;

//public record KebabsRequest(
//    [Required][MaxLength(32)] string Name,
//    [Required][MaxLength(100)] string Description,
//    [Required][Range(0.01, decimal.MaxValue)] decimal Price,
//    IFormFile TitleImage);

public record KebabsRequest(
    [Required][MaxLength(Kebab.MAX_NAME_LENGTH)] string Name,
    [Required][MaxLength(Kebab.MAX_DESCRIPTION_LENGTH)] string Description,
    [Required] decimal Price,
    IFormFile TitleImage);