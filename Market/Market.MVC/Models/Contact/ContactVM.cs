using System.ComponentModel.DataAnnotations;
using Market.Domain.Enums;

namespace Market.MVC.Models.Contact;

public class ContactVM
{
    [Required(ErrorMessage = "Имя обязательно")]
    [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Фамилия обязательна")]
    [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный email адрес")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Сообщение обязательно")]
    [StringLength(1000, ErrorMessage = "Сообщение не должно превышать 1000 символов")]
    public required string Message { get; set; }

    [Required(ErrorMessage = "Тип обращения обязателен")]
    public ContactType Type { get; set; }
}