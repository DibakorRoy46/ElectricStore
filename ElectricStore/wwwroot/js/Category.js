var dataTable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a class="btn btn-success" href="/Admin/Category/Upsert/${data}" style="cursor:pointer;"><i class="fas fa-edit"></i></a>
                                <a class="btn btn-danger" onclick=Delete("/Admin/Category/Delete/${data}") style="cursor:pointer;"><i class="fas fa-trash-alt"></i></a>
                            </div>`;
                }, "width": "40%"
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