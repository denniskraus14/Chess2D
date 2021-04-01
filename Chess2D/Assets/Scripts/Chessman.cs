using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    //References 
    public GameObject controller;
    public GameObject movePlate;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //var to keep track of turn
    private string player;

    //var to keep track of check
    private bool check = false;

    //refs for all the sprites of chess pieces
    public Sprite black_king, black_queen, black_knight, black_bishop, black_rook, black_pawn;
    public Sprite white_king, white_queen, white_knight, white_bishop, white_rook, white_pawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        
        //take the instantiated location and adjust the transform
        SetCoords();

        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= .66f;
        y *= .66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x,y,-2.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer().Equals(player))
        {
            DestroyMovePlates();  //remove previous move plates
            InitiateMovePlates(); //initiate new move plates
            if (player.Equals("white"))
            {
                //check = isInCheck("black"); //is this the right place to call this?
            }
            else
            {
                //check = isInCheck("white");
            }
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for(int i=0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        if (!check)
        {
            switch (this.name)
            {
                case "black_queen":
                case "white_queen":
                    LineMovePlate(1, 0); //create a line of move plates. handles all possible move cases
                    LineMovePlate(1, 1);
                    LineMovePlate(0, 1);
                    LineMovePlate(-1, 0);
                    LineMovePlate(-1, -1);
                    LineMovePlate(0, -1);
                    LineMovePlate(-1, 1);
                    LineMovePlate(1, -1);
                    break;
                case "black_knight":
                case "white_knight":
                    LMovePlate();
                    break;
                case "black_bishop":
                case "white_bishop":
                    LineMovePlate(1, 1);
                    LineMovePlate(1, -1);
                    LineMovePlate(-1, 1);
                    LineMovePlate(-1, -1);
                    break;
                case "black_king":
                case "white_king":
                    SurroundMovePlate();
                    break;
                case "black_rook":
                case "white_rook":
                    LineMovePlate(1, 0);
                    LineMovePlate(0, 1);
                    LineMovePlate(-1, 0);
                    LineMovePlate(0, -1);
                    break;
                case "black_pawn":
                    PawnMovePlate(xBoard, yBoard - 1);
                    break;
                case "white_pawn":
                    PawnMovePlate(xBoard, yBoard + 1);
                    break;
            }
        }
        else
        {
            ;//figure out how to display the boxes that get the player out of check
        }
    }

    public List<(int,int)> LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;
        List<(int, int)> acc = new List<(int, int)>();

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null) 
        {
            MovePlateSpawn(x, y);
            acc.Add((x, y));
            x += xIncrement;
            y += yIncrement;
        }
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateSpawn(x, y, true);
            acc.Add((x, y));
        }
        return acc;
    }

    public List<(int, int)> LMovePlate()
    {
        List<(int, int)> acc = new List<(int, int)>();
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
        acc.Add((xBoard+1, yBoard+2));
        acc.Add((xBoard-1, yBoard+2));
        acc.Add((xBoard+2, yBoard+1));
        acc.Add((xBoard+2, yBoard-1));
        acc.Add((xBoard+1, yBoard-2));
        acc.Add((xBoard-1, yBoard-2));
        acc.Add((xBoard-2, yBoard+1));
        acc.Add((xBoard-2, yBoard-1));
        return acc;
    }

    public List<(int, int)> SurroundMovePlate()
    {
        List<(int, int)> acc = new List<(int, int)>();
        PointMovePlate(xBoard,yBoard+1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard-1, yBoard);
        PointMovePlate(xBoard-1, yBoard - 1);
        PointMovePlate(xBoard-1, yBoard + 1);
        PointMovePlate(xBoard+1, yBoard - 1);
        PointMovePlate(xBoard+1, yBoard);
        PointMovePlate(xBoard+1, yBoard + 1);
        acc.Add((xBoard, yBoard-1));
        acc.Add((xBoard, yBoard+1));
        acc.Add((xBoard-1, yBoard));
        acc.Add((xBoard-1, yBoard-1));
        acc.Add((xBoard-1, yBoard+1));
        acc.Add((xBoard+1, yBoard-1));
        acc.Add((xBoard+1, yBoard));
        acc.Add((xBoard+1, yBoard+1));
        return acc;
    }

    public List<(int, int)> PointMovePlate(int x, int y)
    {
        List<(int, int)> acc = new List<(int, int)>();
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);
            if (cp == null)
            {
                MovePlateSpawn(x, y);
                acc.Add((x,y));
            }
            else if(cp.GetComponent<Chessman>().player!=player){
                MovePlateSpawn(x, y, true);
                acc.Add((x, y));
            }
        }
        return acc;
    }

    public List<(int, int)> PawnMovePlate(int x, int y)
    {
        List<(int, int)> acc = new List<(int, int)>();
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            //not an attack (forward)
            if (sc.GetPosition(x, y) == null)
            {
                //check the color and row for each pawn and call this twice if they are in starting position ONLY
                if ((player.Equals("white")&&y==2) && sc.GetPosition(x, y+1)==null)
                {
                    MovePlateSpawn(x, y); //one space up
                    MovePlateSpawn(x, y+1); //two spaces up
                }
                else if((player.Equals("black") && y == 5) && sc.GetPosition(x, y-1)==null)
                {
                    MovePlateSpawn(x, y); //one space up
                    MovePlateSpawn(x, y-1); //two spaces up
                }
                else
                {
                    MovePlateSpawn(x, y); //one space up
                }
                
            }
            //two attack possibilities (left and right diagonal)
            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateSpawn(x + 1, y, true);
                acc.Add((x + 1, y));
            }
            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateSpawn(x - 1, y, true);
                acc.Add((x - 1, y));
            }
        }
        return acc;
    }

    public void MovePlateSpawn(int matrixX, int matrixY, bool isAttack=false)
    {
        //List<(int, int)> acc = new List<(int, int)>();
        float x = matrixX;
        float y = matrixY;

        x *= .66f;
        y *= .66f;
        x += -2.3f;
        y -= 2.3f;

        GameObject map = Instantiate(movePlate, new Vector3(x,y,-3),Quaternion.identity);
        MovePlate mpScript = map.GetComponent<MovePlate>();
        if (isAttack)
        {
            mpScript.attack = true;
        }
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);

    }

    public void MovePlateCheck(int x, int y)
    {
        List<(int, int)> acc = new List<(int, int)>();
        //this should only allow you to move to places that would take you out of check. if there are none for any piece, then checkmate
        if (check)
        {
            //the valid moves are the ones that get you out of check
        }
    }
    /*
    public bool isInCheck(string player)
    {
        GameObject[] opponentPieces;
        GameObject king;
        Chessman cm;
        if (player.Equals("white")) {
            king = controller.GetComponent<Game>().playerWhite[4];
            cm = king.GetComponent<Chessman>();
            opponentPieces = controller.GetComponent<Game>().playerBlack;
        }
        else
        {
            king = controller.GetComponent<Game>().playerBlack[4];
            cm = king.GetComponent<Chessman>();
            opponentPieces = controller.GetComponent<Game>().playerWhite;
        }
        foreach(GameObject piece in opponentPieces){
            //if this piece can attack king, check=True. else continue
            List < List < (int, int) >> landingSpots = null ;
            if (piece.name.Contains("queen"))
            {
                landingSpots.Add(LineMovePlate(1, 0));
                landingSpots.Add(LineMovePlate(1, 1));
                landingSpots.Add(LineMovePlate(0, 1));
                landingSpots.Add(LineMovePlate(-1, 0));
                landingSpots.Add(LineMovePlate(-1, -1));
                landingSpots.Add(LineMovePlate(0, -1));
                landingSpots.Add(LineMovePlate(-1, 1));
                landingSpots.Add(LineMovePlate(1, -1));
                DestroyMovePlates();
            }
            else if (piece.name.Contains("knight"))
            {
                landingSpots.Add(PointMovePlate(xBoard - 1, yBoard + 2));
                landingSpots.Add(PointMovePlate(xBoard + 2, yBoard + 1));
                landingSpots.Add(PointMovePlate(xBoard + 2, yBoard - 1));
                landingSpots.Add(PointMovePlate(xBoard + 1, yBoard - 2));
                landingSpots.Add(PointMovePlate(xBoard - 1, yBoard - 2));
                landingSpots.Add(PointMovePlate(xBoard - 2, yBoard + 1));
                landingSpots.Add(PointMovePlate(xBoard - 2, yBoard - 1));
                DestroyMovePlates();

            }
            else if (piece.name.Contains("bishop"))
            {
                landingSpots.Add(LineMovePlate(1, 1));
                landingSpots.Add(LineMovePlate(1, -1));
                landingSpots.Add(LineMovePlate(-1, 1));
                landingSpots.Add(LineMovePlate(-1, -1));
                DestroyMovePlates();

            }
            else if (piece.name.Contains("rook"))
            {
                landingSpots.Add(LineMovePlate(1, 0));
                landingSpots.Add(LineMovePlate(0, 1));
                landingSpots.Add(LineMovePlate(-1, 0));
                landingSpots.Add(LineMovePlate(0, -1));
                DestroyMovePlates();
            }
            else if (piece.name.Contains("white_pawn"))
            {
                landingSpots.Add(PawnMovePlate(xBoard, yBoard + 1));
                DestroyMovePlates();
            }
            else if (piece.name.Contains("black_pawn"))
            {
                landingSpots.Add(PawnMovePlate(xBoard, yBoard - 1));
                DestroyMovePlates();
            }
            foreach (List<(int, int)> spots in landingSpots)
            {
                if (spots.Contains((cm.GetXBoard(), cm.GetYBoard())))
                {
                    check = true;
                    return check;
                }
            }
        }
        return check;
    }*/
}
