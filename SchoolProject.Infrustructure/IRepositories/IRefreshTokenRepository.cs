using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrustructure.InfrustructureBases;

namespace SchoolProject.Infrustructure.IRepositories
{
    public interface IRefreshTokenRepository : IGenericRepositoryAsync<UserRefreshToken>
    {
    }
}
