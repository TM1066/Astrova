using System.Collections.Generic;

[System.Serializable]
public class LeaderboardWrapper
{
    public List<User> users;

    public LeaderboardWrapper(List<User> users)
    {
        this.users = users;
    }
}
