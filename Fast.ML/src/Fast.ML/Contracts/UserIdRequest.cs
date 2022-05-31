using System.ComponentModel.DataAnnotations;

namespace Fast.ML.Contracts;

public class UserIdRequest
{
    [Required]
    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}