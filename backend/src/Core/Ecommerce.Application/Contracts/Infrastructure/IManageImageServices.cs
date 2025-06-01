using Ecommerce.Application.Models.ImageData;
using Ecommerce.Application.Models.ImageManagement;

namespace Ecommerce.Application.Contracts.Infrastructure;

public interface IManageImageServices
{
    Task<ImageResponse> UploadImage(ImageData imageStream);
    
}