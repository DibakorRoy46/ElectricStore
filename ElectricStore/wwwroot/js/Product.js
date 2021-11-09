var dataTable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "code", "width": "12%" },
            { "data": "price", "width": "10%" },
            { "data": "category.name", "width": "12%" },
            { "data": "brand.name", "width": "12%" },
            { "data": "quantity", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a class="btn btn-success" href="/Admin/Product/Upsert/${data}" style="cursor:pointer;"><i class="fas fa-edit"></i></a>
                                <a class="btn btn-danger" onclick=Delete("/Admin/Product/Delete/${data}") style="cursor:pointer;"><i class="fas fa-trash-alt"></i></a>
                            </div>`;
                }, "width": "24%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure that you want to delete this data?",
        text: "If you want delete this you can not retrive that",
        icon: "warning",
        dangerMode: true,
        buttons: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
