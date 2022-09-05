using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * author : Reino Anthony
 * last Update : 03/02/2020
 * 
 * Classe qui permet de définir des données sur chaque node qui serviront à l'algo du pathfinding
 */

public class Position
{
    public int X;
    public int Y;

    
    //Le cout G : distance à partir du départ
    public int G;
    //Le cout H : distance estimé de l'arrivée
    public int H;
    //Le cout F : simplement G + H
    public int F;
    //Stock la case précédente
    public Position Parent;

    public Position(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public int getF()
    {
        return this.F;
    }

    public void setF(int f)
    {
        this.F = f;
    }

    public int getX()
    {
        return this.X;
    }

    public int getY()
    {
        return this.Y;
    }

    public int getG()
    {
        return this.G;
    }

    public void setG(int g)
    {
        this.G = g;
    }

    public int getH()
    {
        return this.H;
    }

    public void setH(int h)
    {
        this.H = h;
    }

    public Position getParent()
    {
        return Parent;
    }
}
