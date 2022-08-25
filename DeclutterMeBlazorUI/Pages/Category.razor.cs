namespace DeclutterMeBlazorUI.Pages;

public partial class Category
{
    [Inject] ICategoryRepository db { get; set; }

    private IEnumerable<DataAccessLibrary.Entities.Category> _categories;
    private DataAccessLibrary.Entities.Category _category = new();
    private bool _showError = false;
    private bool _showSuccess = false;
    private bool _showInfo = false;
    private string _feedback = "";
    
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

            ResetAlerts();
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

            ResetAlerts();
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

            ResetAlerts();
            _deleteMode = value;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadCategories();
    }

    private async Task LoadCategories()
    {
        _categories = await db.GetAsync();
    }

    private async Task HandleCreateCategory()
    {
        if (_category.Name == _category.DisplayOrder.ToString())
        {
            _feedback = "The DisplayOrder cannot match the Name";
            _showError = true;
            return;
        }

        await db.AddAsync(_category);
        await db.SaveChangesAsync();
        _category = new();
        _feedback = "Category created successfully";
        _showSuccess = true;
        _createMode = false;
        await LoadCategories();
    }

    private async Task HandleEditCategory()
    {
        if (_category.Name == _category.DisplayOrder.ToString())
        {
            _feedback = "The DisplayOrder cannot match the Name";
            _showError = true;
            return;
        }

        db.Update(_category);
        await db.SaveChangesAsync();
        _category = new();
        _feedback = "Category updated successfully";
        _showInfo = true;
        _editMode = false;
        await LoadCategories();
    }

    private async Task HandleDeleteCategory()
    {
        db.Remove(_category);
        await db.SaveChangesAsync();
        _category = new();
        _feedback = "Category deleted successfully";
        _showSuccess = true;
        _deleteMode = false;
        await LoadCategories();
    }

    private void ResetAlerts()
    {
        _showSuccess = false;
        _showError = false;
        _showInfo = false;
    }
}
