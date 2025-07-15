﻿using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Web.Views.Account.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and confirmed password are not the same.")]
    public string ConfirmPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} length {2} to {1} characters.", MinimumLength = 3)]
    [DataType(DataType.Text)]
    public string UserName { set; get; }

}
