public class FieldInfosDto
{
    public float[] player1Pos { get; set; } = new float[2];
    public int player1Lives { get; set; } = 0;
    public float[] player2Pos { get; set; } = new float[2];
    public int player2Lives { get; set; } = 0;
    public float[] ballPos { get; set; } = new float[2];
    public float[] ballLightPos { get; set; } = new float[2];
}

public class KeyP2InfoDto
{
    // 0 - UP | 1 - DOWN | 2 - LEFT | 3 - RIGHT | 4 - DASH
    public bool[] player2Keys { get; set; } = new bool[5];
}