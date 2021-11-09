var dataTable;
$(document).ready(function (data) {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Role/GetAll/"
        },
        "columns": [
            { "data": "id", "width": "40%" },
            { "data": "name", "width": "30%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <a class="btn btn-primary"style="cursor:pointer" href="/Admin/Role/Upsert/${data}"><i class="fas fa-edit"></i></a>
                                    <a class="btn btn-danger" onclick=Delete("/Admin/Role/Delete/${data}")><i class="fas fa-trash-alt"></i></a>
                            </div>`;
                },"width":"30%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure that you want to delete this data?",
        text: "If you once delete this you can not restore it",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}