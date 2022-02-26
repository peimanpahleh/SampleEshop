using Products.Domain.Images;

namespace Products.Application.Products.User.Queries;

public record UserGetImageQuery(string Id) : IQuery<Image>;

public class UserGetImageQueryHandler : IQueryHandler<UserGetImageQuery, Image>
{
    private readonly IImageRepository _imageRepos;

    public UserGetImageQueryHandler(IImageRepository imageRepos)
    {
        _imageRepos = imageRepos;
    }

    public async Task<Image> Handle(UserGetImageQuery request, CancellationToken cancellationToken)
    {
        var res = await _imageRepos.GetByIdAsync(request.Id);
        return res;
    }
}