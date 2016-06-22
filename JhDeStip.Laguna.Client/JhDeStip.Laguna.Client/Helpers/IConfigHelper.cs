using JhDeStip.Laguna.Dal;

namespace JhDeStip.Laguna.Client.Helpers
{
    public interface IConfigHelper
    {
        DalConfig BuildDalConfig();
    }
}