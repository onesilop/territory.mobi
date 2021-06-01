namespace territory.mobi.Models
{
    public partial class CongUser
    {
        public AspNetUsers User { get; set; }
        public AspNetRoles Role { get; set; }
        public AspNetUserClaims Claims { get; set; }
    }
}
