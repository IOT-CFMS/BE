using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IAuthServices
    {
        Task<BaseResponseForLogin<LoginResponseModel>> AuthenticateAsync(string email, string password);
        Task<BaseResponse<TokenModel>> RegisterAsync(RegisterRequestModel registerModel);
        Task<BaseResponse<TokenModel>> AdminGenAcc(AdminCreateAccountModel adminCreateAccountModel);
        Task<BaseResponse> SendAccount(int userId);
        Task<BaseResponse> ForgotPassword(ForgotPasswordRequest request);
        Task<BaseResponse<TokenModel>> RefreshTokenAsync(string refreshToken);
        Task<BaseResponse> SendVerificationEmailAsync(string email);
        Task<BaseResponse<TokenModel>> VerifyAccountAsync(EmailVerificationModel model);
        Task<BaseResponse> SetEmailVerified(string email);
    }
}