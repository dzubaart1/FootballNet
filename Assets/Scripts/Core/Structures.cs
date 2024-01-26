namespace FootBallNet
{
    public enum ETargetPlatform
    {
        Auto,
        VR,
        PC
    }

    public enum EScene
    {
        [StringValue("Пустыня")]
        Desert,
        [StringValue("Киберпанк")]
        Cyberpunk,
        [StringValue("Демо")]
        Demo,
        [StringValue("Лето")]
        Summer,
    }

    public enum EPlatformEffect
    {
        Wind = 0,
        Water = 1,
        Lighting = 2,
        Bubbles = 3,
        Snow = 4,
        Mouse = 5,
        WindHead = 6,
        Kick = 7
    }
}
