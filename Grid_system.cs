
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;

public class Grid_system : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int width;

    [SerializeField] private Tile _tilePrelab;

    [SerializeField] private Transform _camera;

    private int[] ran_position = new int[112];

    public GameObject scoreMg;
    //Variable for tile manager

    private Tile last_tile;
    private Tile current_tile;
    
    private Matrix_tile t_matrix = new Matrix_tile();
    // algorithm for find path.




    //constructor//

    void Start() {

        random_pos();
        GenerateGrid();
        
    }
    private void random_pos()
    {
        System.Random r = new System.Random();
        for (int i = 0; i < 112; i++)
        {
            this.ran_position[i] = i % 14;
            Debug.Log(i % 14);
        }
        for (int i = 0; i < 500; i++)
        {
            int swap_1 = r.Next(0, 111);
            int swap_2 = r.Next(0, 111);
            ShuffleArray(ran_position, swap_1, swap_2);
        }
    }
    void ShuffleArray(int[] array, int swap_1, int swap_2)
    {

        int temp = array[swap_1];
        array[swap_1] = array[swap_2];
        array[swap_2] = temp;

    }
    void GenerateGrid()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawned_tile = Instantiate(_tilePrelab, new Vector3(x, y), Quaternion.identity);
                spawned_tile.name = $"Tile block {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawned_tile.Init(isOffset, ran_position[x + y * width]);
                Debug.Log(x + y * width + " -> var: " + this.ran_position[x + y * width]);
                spawned_tile.set_grid_system(this);
                spawned_tile.set_pos(y, x);
                //
                t_matrix.set_matrix(spawned_tile.pos_x,spawned_tile.pos_y, spawned_tile);

            }
        }
        _camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

    }



    public void set_curr_tile(Tile new_tile)
    {
        this.last_tile = current_tile;
        this.current_tile = new_tile;
        if (current_tile != null && last_tile != null && last_tile.id == current_tile.id && last_tile != current_tile && check_two_point() ) {

            scoreMg = GameObject.Find("Score");   // Lấy gameobject
            score_manager scoreManager = scoreMg.GetComponent<score_manager>();  // lấy script của gameObject
            scoreManager.add_score(1);

            last_tile.clear_tile();
            current_tile.clear_tile();
            t_matrix.set_matrix(current_tile.pos_x, current_tile.pos_y, null);
            t_matrix.set_matrix(last_tile.pos_x, last_tile.pos_y, null);
            last_tile = null;
            current_tile = null;
        }
        if (current_tile != null && last_tile != null)
        {
            current_tile.set_nHightlighted(false);
            last_tile.set_nHightlighted(false);
            current_tile._hight_light();
            last_tile._hight_light();
            current_tile = null;
            last_tile = null;
        }
        Debug.Log("curr tile change to be:" + this.current_tile);
    }

    // gamecontroler.

    public bool check_two_point()
    {
        if (check_neighbor()){
            return true;
        }else if (check_line_X())
        {
            return true;
        }else if (check_line_Y())
        {
            return true;
        }else if (check_lineX_up())
        {
            return true;
        }else if (check_lineX_down())
        {
            return true;
        }else if (check_lineY_left())
        {
            return true;
        }else if (check_lineY_right())
        {
            return true;
        }else if (check_sqareL_up())
        {
            return true;
        }else if (check_squareL_down())
        {
            return true;
        }else if (check_squareL_left())
        {
            return true;
        }else if (check_squareL_right())
        {
            return true;
        }
        else{
            return false;
        }
    }


        public bool check_neighbor()
    {
        if (current_tile != null && last_tile!=null) {
            if(current_tile.pos_x == last_tile.pos_x)
            {
                if(current_tile.pos_y == last_tile.pos_y-1 || current_tile.pos_y == last_tile.pos_y + 1) {
                    return true;
                }
            }
            if (current_tile.pos_y == last_tile.pos_y)
            {
                if (current_tile.pos_x == last_tile.pos_x - 1 || current_tile.pos_x == last_tile.pos_x + 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
        public bool check_line_X()
    {
        if(current_tile.pos_x != last_tile.pos_x)
        {
            return false;
        }
        int min_posY = Math.Min(current_tile.pos_y, last_tile.pos_y);
        int max_posY = Math.Max(current_tile.pos_y, last_tile.pos_y);

        for(int i = min_posY+1;i<max_posY;i++)
        {
            if (t_matrix.get_matrix_tile(current_tile.pos_x, i) != null) { 
            return false;
            };
        }
        return true;
    }
    public bool check_line_Y()
    {
        if (current_tile.pos_y != last_tile.pos_y)
        {
            return false;
        }
        int min_posX = Math.Min(current_tile.pos_x, last_tile.pos_x);
        int max_posX = Math.Max(current_tile.pos_x, last_tile.pos_x);

        for (int i = min_posX + 1; i < max_posX; i++)
        {
            if (t_matrix.get_matrix_tile(i, current_tile.pos_y) != null){
                return false;
            };
        }
        return true;
    }
    public bool check_empty_line(int left,int right,int num_row)
    {
        int min_value = Math.Min(left, right);
        int max_value = Math.Max(left, right);
        for(int i = min_value ;i<= max_value ;i++) {
            if (t_matrix.get_matrix_tile(num_row,i) != null)
            {
                return false;
            }                           
        }
        return true;
    }

    public bool check_empty_col(int upper, int under, int num_col)
    {
        int min_value = Math.Min(under, upper);
        int max_value = Math.Max(under, upper);
        for (int i = min_value ; i <= max_value; i++)
        {
            if (t_matrix.get_matrix_tile(i, num_col) != null)
            {
                return false;
            }
        }
        return true;
    }
    public bool checK_empty_col(int upper,int under,int num_col)
    {
        int min_value = Math.Min(under, upper);
        int max_value = Math.Max(under, upper);
        for(int i = min_value+1 ; i<= max_value ;i++)
        {
            if (t_matrix.get_matrix_tile(i, num_col)!= null)
            {
                return false;
            }
        }
        return true;
    }
    public bool check_lineX_up()
    {
        if (current_tile.pos_x != last_tile.pos_x)
        {
            return false;
        }
        for(int i = current_tile.pos_x ; i <=8 ;i++)
        {
            if (check_empty_line(current_tile.pos_y, last_tile.pos_y, i))
            {
                if (checK_empty_col(current_tile.pos_x, i, current_tile.pos_y) && checK_empty_col(last_tile.pos_x, i, last_tile.pos_y))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool check_lineX_down()
    {
        if (current_tile.pos_x != last_tile.pos_x)
        {
            return false;
        }
        for(int i = current_tile.pos_x ; i >= -1; i--)
        {
            if(check_empty_line(current_tile.pos_x,last_tile.pos_x, i))
            {
                if (checK_empty_col(i, current_tile.pos_x-1, current_tile.pos_y) && checK_empty_col(i, last_tile.pos_x-1, last_tile.pos_y))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool check_lineY_left() {
    if(current_tile.pos_y != last_tile.pos_y)
        {
            return false;
        }
    for(int i=current_tile.pos_y ; i >= -1; i--)
        {
            if (check_empty_col(current_tile.pos_x, last_tile.pos_x, i))
            {
                if (check_empty_line(current_tile.pos_y - 1, i, current_tile.pos_x) && check_empty_line(last_tile.pos_y - 1, i, last_tile.pos_x))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool check_lineY_right()
    {
        if (current_tile.pos_y != last_tile.pos_y)
        {
            return false;
        }
        for (int i = current_tile.pos_y; i <= 14; i++)
        {
            if (check_empty_col(current_tile.pos_x, last_tile.pos_x, i))
            {
                if (check_empty_line(current_tile.pos_y + 1, i, current_tile.pos_x) && check_empty_line(last_tile.pos_y + 1, i, last_tile.pos_x))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool check_sqareL_up()
    {
        int min_pos_x  = Math.Min(current_tile.pos_x,last_tile.pos_x);
        int max_pos_x = Math.Max(current_tile.pos_x, last_tile.pos_x);
        if (check_empty_col(min_pos_x + 1, max_pos_x, current_tile.pos_y)|| check_empty_col(min_pos_x + 1, max_pos_x, last_tile.pos_y))
        {
            for (int i = max_pos_x; i <= 8; i++)
            {
                if (check_empty_line(current_tile.pos_y, last_tile.pos_y, i))
                {
                    if (checK_empty_col(current_tile.pos_x, i, current_tile.pos_y) && checK_empty_col(last_tile.pos_x, i, last_tile.pos_y))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public bool check_squareL_down()
    {
        for (int i = current_tile.pos_x; i >= -1; i--)
        {
            if (check_empty_line(current_tile.pos_y, last_tile.pos_y, i))
            {
                if (checK_empty_col(i, current_tile.pos_x - 1, current_tile.pos_y) && checK_empty_col(i, last_tile.pos_x - 1, last_tile.pos_y))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool check_squareL_left()
    {
        for (int i = current_tile.pos_y; i >= -1; i--)
        {
            if (check_empty_col(current_tile.pos_x, last_tile.pos_x, i))
            {
                if (check_empty_line(current_tile.pos_y - 1, i, current_tile.pos_x) && check_empty_line(last_tile.pos_y - 1, i, last_tile.pos_x))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool check_squareL_right()
    {
        for (int i = current_tile.pos_y; i <= 14; i++)
        {
            if (check_empty_col(current_tile.pos_x, last_tile.pos_x, i))
            {
                if (check_empty_line(current_tile.pos_y + 1, i, current_tile.pos_x) && check_empty_line(last_tile.pos_y + 1, i, last_tile.pos_x))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool check_on_square()
    {

        return false;
    }
}

