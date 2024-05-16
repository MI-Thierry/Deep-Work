using DeepWork.Infrastructure.Data;

namespace DeepWork.IntegrationTests;
public class BaseDeepWorkRepoTest
{
    private readonly DeepWorkRepositories _deepWorkRepo;
    private readonly string _dbPath = "deepWorkDbTest.db";
    public BaseDeepWorkRepoTest()
    {
        _deepWorkRepo = new DeepWorkRepositories(_dbPath);
    }

    protected DeepWorkRepositories GetDeepWorkRepo()
    {
        return _deepWorkRepo;
    }
}
