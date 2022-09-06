namespace DeclutterMeBlazorUI.Pages;

public partial class Product
{
    [Inject] IUnitOfWork db { get; set; }
    [Inject] IWebHostEnvironment webHostEnvironment { get; set; }

    private IEnumerable<DataAccessLibrary.Entities.Product> _products;
    private IEnumerable<DataAccessLibrary.Entities.Category> _categories;
    private DataAccessLibrary.Entities.Product _product = new();
    private IBrowserFile _imageUpload;
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
                _product = new();
                EditMode = false;
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
            }

            ResetAlerts();
            _editMode = value;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadCategories();
        await LoadProducts();
    }

    private async Task LoadCategories()
    {
        _categories = await db.Category.GetAsync();
    }

    private async Task LoadProducts()
    {
        _products = await db.Product.GetWithCategoriesAsync();
    }

    private async Task HandleCreateProduct()
    {
        try
        {
            string wwwRootPath = webHostEnvironment.WebRootPath;

            if (_imageUpload is not null)
            {
                string fileName = $"{_product.Name}-{Guid.NewGuid()}";
                string uploads = Path.Combine(wwwRootPath, @"img\products");
                string extension = Path.GetExtension(_imageUpload.Name);

                if (_product.ImageUrl != null)
                {
                    string oldImagePath = Path.Combine(wwwRootPath, _product.ImageUrl.TrimStart('\\'));

                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                await using FileStream fs = new(Path.Combine(uploads, fileName + extension), FileMode.Create);
                await _imageUpload.OpenReadStream(maxAllowedSize: 5120000).CopyToAsync(fs);
                _product.ImageUrl = $@"\img\products\{fileName}{extension}";
            }
        }
        catch (Exception ex)
        {
            await InvokeAsync(StateHasChanged);
            _feedback = $"Upload failed: {ex.Message}";
            _showError = true;
        }

        await db.Product.AddAsync(_product);
        await db.SaveChangesAsync();
        _product = new();
        _feedback = "Product created successfully";
        _showSuccess = true;
        _createMode = false;
        await LoadProducts();
    }

    private async Task HandleEditProduct()
    {
        try
        {
            string wwwRootPath = webHostEnvironment.WebRootPath;

            if (_imageUpload is not null)
            {
                string fileName = $"{_product.Name}-{Guid.NewGuid()}";
                string uploads = Path.Combine(wwwRootPath, @"img\products");
                string extension = Path.GetExtension(_imageUpload.Name);

                if (_product.ImageUrl != null)
                {
                    string oldImagePath = Path.Combine(wwwRootPath, _product.ImageUrl.TrimStart('\\'));

                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                await using FileStream fs = new(Path.Combine(uploads, fileName + extension), FileMode.Create);
                await _imageUpload.OpenReadStream(maxAllowedSize: 10240000).CopyToAsync(fs);
                _product.ImageUrl = $@"\img\products\{fileName}{extension}";
            }
        }
        catch (Exception ex)
        {
            await InvokeAsync(StateHasChanged);
            _feedback = $"Upload failed: {ex.Message}";
            _showError = true;
        }

        await db.Product.UpdateAsync(_product);
        await db.SaveChangesAsync();
        _product = new();
        _feedback = "Product updated successfully";
        _showInfo = true;
        _editMode = false;
        await LoadProducts();
    }

    private async Task HandleDeleteProduct()
    {
        DataAccessLibrary.Entities.Product dbProduct = await db.Product.GetAsync(p => p.Id == _product.Id);

        if (dbProduct == null)
        {
            _feedback = "Product not found in database";
            _showError = true;
            return;
        }

        string oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, dbProduct.ImageUrl.TrimStart('\\'));

        if (File.Exists(oldImagePath))
        {
            File.Delete(oldImagePath);
        }

        db.Product.Remove(dbProduct);
        await db.SaveChangesAsync();
        _product = new();
        _feedback = "Product deleted successfully";
        _showSuccess = true;
        await LoadProducts();
    }

    private void ResetAlerts()
    {
        _showSuccess = false;
        _showError = false;
        _showInfo = false;
    }

    private void HandleInputFileChange(InputFileChangeEventArgs e)
    {
        _imageUpload = e.File;
    }
}
