using FluentValidation;
using System.Net;
using Users.Models.RequestModels;

namespace Users.Models.ValidationModels
{
    public class UserConnectionRequestModelValidator : AbstractValidator<UserConnectionRequestModel>
    {
        public UserConnectionRequestModelValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0");

            RuleFor(x => x.IpAddress)
                .Must(BeValidIpAddress)
                .WithMessage("IpAddress must be a valid IPv4 or IPv6 address");
        }

        private bool BeValidIpAddress(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }
    }
}
