using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/**
 * author : Reino Anthony, Bui Minh-Quân
 * last Update : 03/02/2020
 * 
 * Classe qui permet de définir le chemin à suivre pour les ennemis
 */

public class PathFinding
{
    private Position current;
    private Position start;
    private Position target;

    //Liste des positions en attente de traitement
    private List<Position> openList;
    //Liste des positions déjà traitées
    private List<Position> closedList;
    //Distance (cout) depuis la position de départ
    private int g;

    public PathFinding(Vector3 start, Vector3 target)
    {
        this.start = new Position((int)start.x, (int)start.y);
        this.target = new Position((int)target.x, (int)target.y);
        init();
        //On commence par ajouter la position d'origine dans l'openList
    }

    private void init()
    {
        this.current = null;
        this.openList = new List<Position>();
        this.closedList = new List<Position>();
        openList.Add(this.start);
        this.g = 0;
    }

    //Permet de connaitre les cases adjacentes disponibles
    private List<Position> GetWalkableAdjacentSquares(int x, int y, int[,] map, List<Position> openList)
    {
        List<Position> chemin = new List<Position>();

        
        //Si la cible est une case libre (0) ou l'objectif (2) on traite sinon s'il trouve l'obstacle (-1) il ne le prend pas en compte
        if (isInGrid(map, x, y-1) && (map[x, y - 1] == 0 || map[x, y - 1] == 2))
        {
            Position node = openList.Find(pos => pos.getX() == x && pos.getY() == y - 1);

            if (node == null)
            {
                chemin.Add(new Position(x, y - 1));
            }
            else
            {
                chemin.Add(node);
            }
        }

        if (isInGrid(map, x, y + 1) && (map[x, y + 1] == 0 || map[x, y + 1] == 2))
        {
            Position node = openList.Find(pos => pos.getX() == x && pos.getY() == y + 1);
            if (node == null)
                chemin.Add(new Position(x, y + 1));
            else
                chemin.Add(node);
        }

        if (isInGrid(map, x-1, y) && (map[x-1, y] == 0 || map[x-1, y] == 2))
        {
            Position node = openList.Find(pos => pos.getX() == x - 1 && pos.getY() == y);
            if (node == null)
                chemin.Add(new Position(x - 1, y));
            else
                chemin.Add(node);
        }

        if (isInGrid(map, x + 1, y) && (map[x + 1, y] == 0 ||  map[x + 1, y] == 2))
        {
            Position node = openList.Find(pos => pos.getX() == x + 1 && pos.getY() == y);
            if (node == null)
                chemin.Add(new Position(x + 1, y));
            else
                chemin.Add(node);
        }

        return chemin;
    }

    private bool isInGrid(int [,] map, int x, int y)
    {
        return (x >= 0 && y >= 0) && (x < map.GetLength(0) && y < map.GetLength(1));
    }


    //Calcul du cout pour H
    private int ComputeHScore(int x, int y, int targetX, int targetY)
    {
        return Math.Abs(targetX - x) + Math.Abs(targetY - y);
    }

    //Algo A*
    public List<Node> AStarPathFinding(int[,] matrice)
    {
        init();
        while (this.openList.Count > 0)
        {
            //On récupère la case avec le cout le plus faible (Si plusieurs case avec le meme cout F, on prend le 1er)
            var lowest = openList.Min(pos => pos.getF());
            this.current = openList.First(pos => pos.getF() == lowest);

            //Ajout de la case actuelle dans closeList
            closedList.Add(current);

            //On l'enlève aussi de l'openList pour pas qu'elle se fasse revisiter
            openList.Remove(current);

            //Se déclenche si on a trouvé le chemin 
            if (closedList.FirstOrDefault(pos => pos.getX() == target.getX() && pos.getY() == target.getY()) != null)
                break;

            var adjacentSquares = GetWalkableAdjacentSquares(current.getX(), current.getY(), matrice, openList);
            g = current.getG() + 1;

            foreach (var adjacentSquare in adjacentSquares)
            {
                //Si la case adjacente est déjà dans la closedList, on l'ignore
                if (closedList.FirstOrDefault(pos => pos.getX() == adjacentSquare.getX() && pos.getY() == adjacentSquare.Y) != null)
                    continue;

                //Si la case n'est pas dans la closedList on la traite  
                if (openList.FirstOrDefault(pos => pos.getX() == adjacentSquare.getX() && pos.getY() == adjacentSquare.getY()) == null)
                {
                    //Calcul des couts g, h et f
                    adjacentSquare.setG(g);
                    adjacentSquare.setH(ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y));
                    adjacentSquare.setF(adjacentSquare.getG() + adjacentSquare.getH());
                    adjacentSquare.Parent = current;

                    //On les ajoutes dans l'openList 
                    openList.Insert(0, adjacentSquare);
                }
                else
                {
                    //On test si le cout de G de la case actuelle rend le cout de F de la case adjacente plus faible
                    //si oui on update la case parent parce que cela signifie que le chemin est meilleur
                    if (g + adjacentSquare.getH() < adjacentSquare.getF())
                    {
                        adjacentSquare.setG(g);
                        adjacentSquare.setF(adjacentSquare.G + adjacentSquare.H);
                        adjacentSquare.Parent = current;
                    }
                }
            }
        }

        Position end = current;
        
        if(end.getX() != target.getX() || end.getY() != target.getY())
        {
            Debug.Log("Chemin impossible");
            return null;
        }

        //Le chemin a été trouvé, on l'affiche 
        List<Node> pathFound = new List<Node>();
        pathFound = createFoundPath(current, pathFound);
        for(int i = 0; i < pathFound.Count; ++i)
        {
            if(i > 0)
            {
                pathFound[i].setParent(pathFound[i - 1]);
            }
        }
        return pathFound;
    }

    List<Node> createFoundPath(Position current, List<Node> pathFound)
    {
        if(current.getParent() == null)
        {
            Node newNode = new Node(current.getX(), current.getY());
            pathFound.Insert(0,newNode);
            return pathFound;
        }
        else
        {
            Node newNode = new Node(current.getX(), current.getY());
            pathFound.Insert(0,newNode);
            return createFoundPath(current.getParent(), pathFound);
        }
    }
}
 
