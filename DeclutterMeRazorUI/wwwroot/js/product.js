var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#productData').DataTable({
        "ajax": {
            url: "/Admin/Product/List",
            type: 'GET'
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "listPrice", "width": "20%" },
            { "data": "actualPrice", "width": "20%" },
            { "data": "category.name", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Admin/Product/Edit?id=${data}" class="btn btn-primary">Edit</a>
                        <a href="/Admin/Product/Delete?id=${data}" class="btn btn-danger">Delete</a>
                        `
                },
                "width": "20%"
            }
        ]
    });
}
