using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations;
using Unity.Mathematics;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Sprite[] _image;

    private bool _isHighlighted = false;

    public int pos_x;
    public int pos_y;

    private Grid_system gridSystem; // farther of all tile
    public int id;
    // Constructor
    public void set_pos(int x, int y){
        this.pos_x = x;
        this.pos_y = y;
     }
    public void get_pos()
    {
        Debug.Log(pos_x + " | " + pos_y);
    }
    public void set_nHightlighted(bool hight_light)
    {
            this._isHighlighted = hight_light;
    }
    public void _hight_light()
    {
        _highlight.SetActive(this._isHighlighted);
    }
    public void set_grid_system(Grid_system gridSystem)
    {
        this.gridSystem = gridSystem;
    }
    
    public void clear_tile()
    {
        this._renderer.gameObject.SetActive(false);
        Debug.Log("Clear: " + _renderer.gameObject.name +"(Hide)");
        get_pos();
    }
    //
    public void Init(bool isOffset,int random_num)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        _renderer.sprite = _image[random_num];
        this.id = random_num;
    }
    /// <summary>
    /// Hàm xử lý sự kiện nhấn chuột.
    /// </summary>

    private void OnMouseDown()
    {
        _isHighlighted = true;
        gridSystem.set_curr_tile(this);

        Debug.Log("right_mouse click: " + this._renderer.name);
        Vector3 mousePosition = Input.mousePosition; 

        Vector3 mousePosition_game = transform.InverseTransformPoint(mousePosition);

        
    }
    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(_isHighlighted);
    }
    
}