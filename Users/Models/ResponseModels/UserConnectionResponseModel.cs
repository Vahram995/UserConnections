namespace Users.Models.ResponseModels
{
    public class UserConnectionResponseModel
    {
        public long UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime ConnectedAt { get; set; }
    }
}