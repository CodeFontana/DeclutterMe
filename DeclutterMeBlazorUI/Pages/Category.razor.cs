using DataAccessLibrary.Data;
using DeclutterMeBlazorUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeBlazorUI.Pages;

public partial class Category
{
    [Inject] DeclutterMeDbContext db { get; set; }
    
    private IEnumerable<DataAccessLibrary.Entities.Category> _categories;
    private DataAccessLibrary.Entities.Category _category = new();
    private Notification _notification;
    
    private bool _createMode = false;
    public bool CreateMode
    {
        get { return _createMode; }
        set 
        {
            if (value)
            {
                _category = new();
                EditMode = false;
                DeleteMode = false;
            }

            _notification.ResetAlert();
            _createMode = value; 
        }
    }

    private bool _editMode = false;
    public bool EditMode
    {
        get { return _editMode; }
        set
        {
            if (value)
            {
                CreateMode = false;
                DeleteMode = false;
            }

            _notification.ResetAlert();
            _editMode = value;
        }
    }

    private bool _deleteMode = false;
    public bool DeleteMode
    {
        get { return _deleteMode; }
        set
        {
            if (value)
            {
                CreateMode = false;
                EditMode = false;
            }

            _notification.ResetAlert();
            _deleteMode = value;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadCategories();
    }

    private async Task LoadCategories()
    {
        _categories = await db.Categories.ToListAsync();
    }

    private async Task HandleCreateCategory()
    {
        if (_category.Name == _category.DisplayOrder.ToString())
        {
            _notification.AlertError("The DisplayOrder cannot match the Name");
            return;
        }

        await db.Categories.AddAsync(_category);
        await db.SaveChangesAsync();
        _category = new();
        _notification.AlertSuccess("Category created successfully");
        _createMode = false;
        await LoadCategories();
    }

    private async Task HandleEditCategory()
    {
        if (_category.Name == _category.DisplayOrder.ToString())
        {
            _notification.AlertError("The DisplayOrder cannot match the Name");
            return;
        }

        await db.SaveChangesAsync();
        _category = new();
        _notification.AlertInfo("Category updated successfully");
        _editMode = false;
        await LoadCategories();
    }

    private async Task HandleDeleteCategory()
    {
        db.Categories.Remove(_category);
        await db.SaveChangesAsync();
        _category = new();
        _notification.AlertSuccess("Category deleted successfully");
        _deleteMode = false;
        await LoadCategories();
    }
}
