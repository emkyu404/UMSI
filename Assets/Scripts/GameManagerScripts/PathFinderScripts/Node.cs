using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * author : Bui Minh-Quân
 * last Update : 27/01/2020
 * 
 * Classe qui permet de définir un "noeud", utile pour définir le chemin que devront suivre les mobs
 */

public class Node 
{
    private int coordX;
    private int coordY;
    private DIRECTION direction;
    private Node previousNode;


    /**
     * Construteur de la classe Node
     * Prend en paramètre une position X, une position Y ainsi que la noeud Précédent
     */
    public Node(int posX, int posY, Node previousNode)
    {
        init(posX, posY);
        try
        {
            this.previousNode = previousNode;
        }
        catch (System.NullReferenceException ex)
        {
        }
        this.setDirection();
    }

    public Node(Node node)
    {
        init(node.getCoordX(), node.getCoordY());
    }

    public Node(int posX, int posY)
    {
        init(posX, posY);
        this.previousNode = null;
        this.direction = DIRECTION.IDLE;
    }

    private void init(int posX, int posY)
    {
        coordX = posX;
        coordY = posY;
    }


    /** 
     * Méthode qui regarde sur le noeud précédent les coordonnées et déduit la direction du mouvement (4 possibilités : Nord, Est, Sud, Ouest)
     *  - Nord : la coordonnée Y a été incrémenter de 1;
     *  - Est : la coordonnée X a été incrémenter de 1;
     *  - Sud : la coordonnée Y a été décrémenter de 1;
     *  - Ouest : la coordonnée X a été décrémenter de 1;
     */
    protected void setDirection()
    {
        if(direction != DIRECTION.EAST && coordX > (previousNode.getCoordX()))
        {
            direction = DIRECTION.EAST;
        }

        else if (direction != DIRECTION.NORTH && coordY > (previousNode.getCoordY()))
        {
            direction = DIRECTION.NORTH;
        }

        else if (direction != DIRECTION.WEST && coordX < (previousNode.getCoordX()))
        {
            direction = DIRECTION.WEST;
        }

        else if (direction != DIRECTION.SOUTH && coordY < (previousNode.getCoordY()))
        {
            direction = DIRECTION.SOUTH;
        }
    }

    /**
     * Méthode qui renvoit la coordonnée X du noeud
     * return : int
     */
    public int getCoordX()
    {
        return coordX;
    }

    /**
     * Méthode qui renvoit la coordonnée Y du noeud
     * return : int
     */
    public int getCoordY()
    {
        return coordY;
    }


    /**
     * Méthode qui renvoit la direction du mouvement
     * return : DIRECTION
     */
    public DIRECTION getDirection()
    {
        return direction;
    }
    
    public void setParent(Node previousNode)
    {
        this.previousNode = previousNode;
        setDirection();
    }
}
