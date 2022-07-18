namespace Movies.Client.Models
{
    public class UserInfoViewModel:Dictionary<string, string>
    {
        public Dictionary<string, string> UserInfo { get; private set; }
        public UserInfoViewModel(Dictionary<string, string> userInfo)
        {
            UserInfo = userInfo;
        }
    }
}
