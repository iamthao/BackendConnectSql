using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class PackageHistoryGridVo : ReadOnlyGridVo
    {
        public string OldPackage { get; set; }
        public string NewPackage { get; set; }
        public int RequestId { get; set; }
    }
}