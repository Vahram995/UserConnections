using FluentValidation.TestHelper;
using Users.Models.RequestModels;
using Users.Models.ValidationModels;

namespace Users.UnitTests.ValidatorTests
{
    public class UserConnectionRequestModelValidatorTests
    {
        private readonly UserConnectionRequestModelValidator _validator = new();

        [Theory]
        [InlineData(1, "127.0.0.1")]
        [InlineData(99, "2001:db8::1")]
        public void Valid_model_passes(long userId, string ip)
        {
            var model = new UserConnectionRequestModel { UserId = userId, IpAddress = ip };
            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0, "127.0.0.1")]
        [InlineData(-5, "8.8.8.8")]
        public void Invalid_userId_fails(long userId, string ip)
        {
            var model = new UserConnectionRequestModel { UserId = userId, IpAddress = ip };
            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Theory]
        [InlineData(1, "not-an-ip")]
        [InlineData(1, "999.999.999.999")]
        public void Invalid_ip_fails(long userId, string ip)
        {
            var model = new UserConnectionRequestModel { UserId = userId, IpAddress = ip };
            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.IpAddress);
        }
    }
}
