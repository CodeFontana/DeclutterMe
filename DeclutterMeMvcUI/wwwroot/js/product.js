var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#productData').DataTable({
        "ajax": {
            "url": "/Admin/Product/Get"
        },
        "columns": [
            {"data": "name", "width": "20%"},
            {"data": "listPrice", "width": "20%"},
            {"data": "actualPrice", "width": "20%"},
            {"data": "category.name", "width": "20%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary">Edit</a>
                        <a onClick=deleteProduct('/Admin/Product/Delete/+${data}') class="btn btn-danger">Delete</a>
                        `
                },
                "width": "20%"
            }
        ]
    });
}

function deleteProduct(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
