using DataAccessLibrary.Data;
using DeclutterMeBlazorUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace DeclutterMeBlazorUI.Pages;

public partial class Product
{
    [Inject] DeclutterMeDbContext db { get; set; }
    [Inject] IWebHostEnvironment webHostEnvironment { get; set; }

    private IEnumerable<DataAccessLibrary.Entities.Product> _products;
    private IEnumerable<DataAccessLibrary.Entities.Category> _categories;
    private DataAccessLibrary.Entities.Product _product = new();
    private IBrowserFile _imageUpload;
    private Notification _notification;

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
            }

            _notification.ResetAlert();
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
        _categories = await db.Categories.ToListAsync();
    }

    private async Task LoadProducts()
    {
        _products = await db.Products
            .Include(p => p.Category)
            .ToListAsync();
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
            _notification.AlertError($"Upload failed: {ex.Message}");
        }

        await db.Products.AddAsync(_product);
        await db.SaveChangesAsync();
        _product = new();
        _notification.AlertSuccess("Product created successfully");
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
            _notification.AlertError($"Upload failed: {ex.Message}");
        }

        db.Products.Update(_product);
        await db.SaveChangesAsync();
        _product = new();
        _notification.AlertInfo("Product updated successfully");
        _editMode = false;
        await LoadProducts();
    }

    private async Task HandleDeleteProduct()
    {
        DataAccessLibrary.Entities.Product dbProduct = await db.Products.FirstOrDefaultAsync(p => p.Id == _product.Id);

        if (dbProduct == null)
        {
            _notification.AlertError("Product not found in database");
            return;
        }

        string oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, dbProduct.ImageUrl.TrimStart('\\'));

        if (File.Exists(oldImagePath))
        {
            File.Delete(oldImagePath);
        }

        db.Products.Remove(dbProduct);
        await db.SaveChangesAsync();
        _product = new();
        _notification.AlertSuccess("Product deleted successfully");
        await LoadProducts();
    }

    private void HandleInputFileChange(InputFileChangeEventArgs e)
    {
        _imageUpload = e.File;
    }
}
