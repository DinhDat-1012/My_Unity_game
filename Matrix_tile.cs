using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix_tile : MonoBehaviour
{
    private Tile[,] tile_array = new Tile[10,16];
    
    public Matrix_tile()
    {
        for (int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 14; j++)
            {
                if (i == 0 || j == 0 || i == 9 || j == 13)
                {
                    tile_array[i, j] = null;
                }
            }
        }
    }
    public void set_matrix(int x,int y,Tile new_tile)
    {
        this.tile_array[8-x,y+1] = new_tile;
    }
    public Tile get_matrix_tile(int x,int y)
    {
        if (tile_array[8-x,y+1] == null)
        {
            return null;
        }
        else
        {
            return tile_array[8-x,y+1];
        }
    }
}
