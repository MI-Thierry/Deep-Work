using DeepWork.Infrastructure.Data;

namespace DeepWork.IntegrationTests;
public class BaseDeepWorkRepoTest
{
    private readonly DeepWorkRepository _deepWorkRepo;
    private readonly string _dbPath = "deepWorkDbTest.db";
    public BaseDeepWorkRepoTest()
    {
        _deepWorkRepo = new DeepWorkRepository(_dbPath);
    }

    protected DeepWorkRepository GetDeepWorkRepo()
    {
        return _deepWorkRepo;
    }
}
